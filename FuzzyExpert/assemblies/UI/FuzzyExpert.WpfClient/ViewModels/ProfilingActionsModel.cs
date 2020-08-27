using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using FuzzyExpert.Application.Common.Interfaces;
using FuzzyExpert.Core.Entities;
using FuzzyExpert.Core.Extensions;
using FuzzyExpert.Infrastructure.DatabaseManagement.Entities;
using FuzzyExpert.Infrastructure.DatabaseManagement.Interfaces;
using FuzzyExpert.Infrastructure.KnowledgeManager.Interfaces;
using FuzzyExpert.Infrastructure.LinguisticVariableParsing.Interfaces;
using FuzzyExpert.Infrastructure.ProductionRuleParsing.Interfaces;
using FuzzyExpert.Infrastructure.ResultLogging.Interfaces;
using FuzzyExpert.WpfClient.Annotations;
using FuzzyExpert.WpfClient.Helpers;
using FuzzyExpert.WpfClient.Models;

namespace FuzzyExpert.WpfClient.ViewModels
{
    public class ProfilingActionsModel : INotifyPropertyChanged
    {
        private readonly IProfileRepository _profileRepository;
        private readonly IFileOperations _fileOperations;
        private readonly IImplicationRuleValidator _ruleValidator;
        private readonly ILinguisticVariableValidator _variableValidator;
        private readonly IImplicationRuleCreator _ruleCreator;
        private readonly ILinguisticVariableCreator _variableCreator;
        private readonly IKnowledgeBaseValidator _knowledgeValidator;
        private readonly IResultLogger _resultLogger;

        public ProfilingActionsModel(
            IProfileRepository profileRepository,
            IFileOperations fileOperations,
            IImplicationRuleValidator ruleValidator,
            ILinguisticVariableValidator variableValidator,
            IImplicationRuleCreator ruleCreator,
            ILinguisticVariableCreator variableCreator,
            IKnowledgeBaseValidator knowledgeValidator,
            IResultLogger resultLogger)
        {
            _profileRepository = profileRepository ?? throw new ArgumentNullException(nameof(profileRepository));
            _fileOperations = fileOperations ?? throw new ArgumentNullException(nameof(fileOperations));
            _ruleValidator = ruleValidator ?? throw new ArgumentNullException(nameof(ruleValidator));
            _variableValidator = variableValidator ?? throw new ArgumentNullException(nameof(variableValidator));
            _ruleCreator = ruleCreator ?? throw new ArgumentNullException(nameof(ruleCreator));
            _variableCreator = variableCreator ?? throw new ArgumentNullException(nameof(variableCreator));
            _knowledgeValidator = knowledgeValidator ?? throw new ArgumentNullException(nameof(knowledgeValidator));
            _resultLogger = resultLogger ?? throw new ArgumentNullException(nameof(resultLogger));

            AddProfileCommand = new RelayCommand(obj => AddProfile(), obj => true);
            RemoveProfileCommand = new RelayCommand(obj => RemoveProfile(), obj => SelectedProfile != null);
            CreateProfileCommand = new RelayCommand(obj => CreateProfile(), obj => !string.IsNullOrEmpty(NewProfileName));
            CloseCreateProfileCommand = new RelayCommand(obj => CloseCreateProfile(), obj => true);

            UpdateProfileCommand = new RelayCommand(obj => UpdateProfile(), obj => true);
            CloseUpdateProfileCommand = new RelayCommand(obj => CloseUpdateProfile(), obj => true);
            ImportFromFilesCommand = new RelayCommand(obj => ImportFromFiles(), obj => true);
            GetRulesFromFileCommand = new RelayCommand(obj => GetRulesFromFile(), obj => true);
            GetVariablesFromFileCommand = new RelayCommand(obj => GetVariablesFromFile(), obj => true);
            StartImportFromFilesCommand = new RelayCommand(obj => StartImportFromFiles(), obj => !string.IsNullOrEmpty(RuleFilePath) && !string.IsNullOrEmpty(VariableFilePath));
            ImportRuleFromInputCommand = new RelayCommand(obj => ImportRuleFromInput(), obj => !string.IsNullOrEmpty(UpdatingInput));
            ImportVariableFromInputCommand = new RelayCommand(obj => ImportVariableFromInput(), obj => !string.IsNullOrEmpty(UpdatingInput));
            CommitProfileCommand = new RelayCommand(obj => CommitProfile(), obj => true);

            Profiles = new ObservableCollection<InferenceProfileModel>();
        }

        #region Collections

        private ObservableCollection<InferenceProfileModel> _profiles;
        public ObservableCollection<InferenceProfileModel> Profiles
        {
            get => _profiles;
            set
            {
                _profiles = value;
                OnPropertyChanged(nameof(Profiles));
            }
        }

        private InferenceProfileModel _selectedProfile;
        public InferenceProfileModel SelectedProfile
        {
            get => _selectedProfile;
            set
            {
                if (value == _selectedProfile)
                {
                    return;
                }

                _selectedProfile = value;
                OnPropertyChanged(nameof(SelectedProfile));
            }
        }

        #endregion

        #region Updating profiles

        public RelayCommand UpdateProfileCommand { get; }

        public RelayCommand CloseUpdateProfileCommand { get; }

        public RelayCommand ImportFromFilesCommand { get; }

        public RelayCommand GetRulesFromFileCommand { get; }

        public RelayCommand GetVariablesFromFileCommand { get; }

        public RelayCommand StartImportFromFilesCommand { get; }

        public RelayCommand ImportRuleFromInputCommand { get; }

        public RelayCommand ImportVariableFromInputCommand { get; }

        public RelayCommand CommitProfileCommand { get; }

        private void UpdateProfile()
        {
            UpdatingInput = string.Empty;
            RuleFilePath = string.Empty;
            VariableFilePath = string.Empty;
            UpdateProfileValidationMessage = string.Empty;
            ImportButtonVisible = true;
            UpdateProfileVisible = true;
            PopUpVisible = true;
        }

        private void CloseUpdateProfile()
        {
            UpdateProfileVisible = false;
            PopUpVisible = false;
        }

        private void ImportFromFiles()
        {
            ImportButtonVisible = false;
        }

        private void GetRulesFromFile()
        {
            var dialog = new FileDialogInteractor();
            if (!dialog.OpenFileDialog())
            {
                return;
            }
            RuleFilePath = dialog.FilePath;
        }

        private void GetVariablesFromFile()
        {
            var dialog = new FileDialogInteractor();
            if (!dialog.OpenFileDialog())
            {
                return;
            }
            VariableFilePath = dialog.FilePath;
        }

        private void StartImportFromFiles()
        {
            var rulesFromFile = _fileOperations.ReadFileByLines(RuleFilePath).ToList();
            var preProcessedImplicationRules = rulesFromFile.Select(PreprocessString).ToList();
            var failedRulesValidations = preProcessedImplicationRules
                .Select(ruleFromFile => _ruleValidator.ValidateImplicationRule(ruleFromFile))
                .Where(validationResult => !validationResult.IsSuccess).ToList();

            var variablesFromFile = _fileOperations.ReadFileByLines(VariableFilePath).ToList();
            var preProcessedLinguisticVariables = variablesFromFile.Select(PreprocessString).ToList();
            var failedVariablesValidations = preProcessedLinguisticVariables
                .Select(variableFromFile => _variableValidator.ValidateLinguisticVariables(variableFromFile))
                .Where(validationResult => !validationResult.IsSuccess).ToList();

            if (failedRulesValidations.Any() || failedVariablesValidations.Any())
            {
                UpdateProfileValidationMessage = $"{failedVariablesValidations.Count + failedRulesValidations.Count} validation errors were found." +
                                                 "Consult log file for more information.";
                File.Delete(_resultLogger.ValidationLogPath);
                if (failedRulesValidations.Any())
                {
                    _resultLogger.LogValidationErrors(failedRulesValidations.SelectMany(f => f.Messages).ToList());
                }
                if (failedVariablesValidations.Any())
                {
                    _resultLogger.LogValidationErrors(failedVariablesValidations.SelectMany(f => f.Messages).ToList());
                }
                Process.Start(_resultLogger.ValidationLogPath);
            }
            else
            {
                UpdateProfileValidationMessage = "Validation passed successfully";
                foreach (var rule in preProcessedImplicationRules)
                {
                    SelectedProfile.Rules.Add(new ContentModel { Content = rule });
                }
                foreach (var variable in preProcessedLinguisticVariables)
                {
                    SelectedProfile.Variables.Add(new ContentModel { Content = variable });
                }
            }
        }

        private void ImportRuleFromInput()
        {
            var preProcessedRule = PreprocessString(UpdatingInput);
            var validationResult = _ruleValidator.ValidateImplicationRule(preProcessedRule);
            if (validationResult.IsSuccess)
            {
                UpdateProfileValidationMessage = "Validation passed successfully";
                SelectedProfile.Rules.Add(new ContentModel {Content = preProcessedRule});
            }
            else
            {
                UpdateProfileValidationMessage = $"{validationResult.Messages.Count} validation errors were found.";
                File.Delete(_resultLogger.ValidationLogPath);
                _resultLogger.LogValidationErrors(validationResult.Messages);
                Process.Start(_resultLogger.ValidationLogPath);
            }
        }

        private void ImportVariableFromInput()
        {
            var preProcessedVariable = PreprocessString(UpdatingInput);
            var validationResult = _variableValidator.ValidateLinguisticVariables(preProcessedVariable);
            if (validationResult.IsSuccess)
            {
                UpdateProfileValidationMessage = "Validation passed successfully";
                SelectedProfile.Variables.Add(new ContentModel {Content = preProcessedVariable});
            }
            else
            {
                UpdateProfileValidationMessage = $"{validationResult.Messages.Count} validation errors were found.";
                File.Delete(_resultLogger.ValidationLogPath);
                _resultLogger.LogValidationErrors(validationResult.Messages);
                Process.Start(_resultLogger.ValidationLogPath);
            }
        }

        private void CommitProfile()
        {
            var preProcessedImplicationRules = SelectedProfile.Rules.Select(x => PreprocessString(x.Content)).ToList();
            var failedRulesValidations = preProcessedImplicationRules
                .Select(ruleFromFile => _ruleValidator.ValidateImplicationRule(ruleFromFile))
                .Where(validationResult => !validationResult.IsSuccess).ToList();

            var preProcessedLinguisticVariables = SelectedProfile.Variables.Select(x => PreprocessString(x.Content)).ToList();
            var failedVariablesValidations = preProcessedLinguisticVariables
                .Select(variableFromFile => _variableValidator.ValidateLinguisticVariables(variableFromFile))
                .Where(validationResult => !validationResult.IsSuccess).ToList();

            if (failedRulesValidations.Any() || failedVariablesValidations.Any())
            {
                CommitProfileValidationMessage = $"{failedVariablesValidations.Count + failedRulesValidations.Count} validation errors were found." +
                                                 "Consult log file for more information.";
                File.Delete(_resultLogger.ValidationLogPath);
                _resultLogger.LogValidationErrors(failedRulesValidations.SelectMany(f => f.Messages).ToList());
                _resultLogger.LogValidationErrors(failedVariablesValidations.SelectMany(f => f.Messages).ToList());
                Process.Start(_resultLogger.ValidationLogPath);
            }
            else
            {
                var rules = new List<ImplicationRule>();
                var variables = new List<LinguisticVariable>();
                foreach (var rule in preProcessedImplicationRules)
                {
                    rules.Add(_ruleCreator.CreateImplicationRuleEntity(rule));
                }
                foreach (var variable in preProcessedLinguisticVariables)
                {
                    variables.AddRange(_variableCreator.CreateLinguisticVariableEntities(variable));
                }
                var knowledgeValidationResult = _knowledgeValidator.ValidateLinguisticVariablesNames(rules, variables);

                if (knowledgeValidationResult.IsSuccess)
                {
                    var profile = new InferenceProfile
                    {
                        ProfileName = SelectedProfile.ProfileName,
                        Rules = preProcessedImplicationRules,
                        Variables = preProcessedLinguisticVariables
                    };
                    _profileRepository.SaveProfile(profile);
                    CommitProfileValidationMessage = "Knowledge base is valid. Update successful!";
                }
                else
                {
                    CommitProfileValidationMessage = $"{knowledgeValidationResult.Messages.Count} validation errors were found." +
                                                     "Consult log file for more information.";
                    File.Delete(_resultLogger.ValidationLogPath);
                    _resultLogger.LogValidationErrors(knowledgeValidationResult.Messages);
                    Process.Start(_resultLogger.ValidationLogPath);
                }
            }
        }

        private string _updatingInput;
        public string UpdatingInput
        {
            get => _updatingInput;
            set
            {
                _updatingInput = value;
                OnPropertyChanged(nameof(UpdatingInput));
            }
        }

        private string _ruleFilePath;
        public string RuleFilePath
        {
            get => _ruleFilePath;
            set
            {
                _ruleFilePath = value;
                OnPropertyChanged(nameof(RuleFilePath));
            }
        }

        private string _variableFilePath;
        public string VariableFilePath
        {
            get => _variableFilePath;
            set
            {
                _variableFilePath = value;
                OnPropertyChanged(nameof(VariableFilePath));
            }
        }

        private string _updateProfileValidationMessage;
        public string UpdateProfileValidationMessage
        {
            get => _updateProfileValidationMessage;
            set
            {
                _updateProfileValidationMessage = value;
                OnPropertyChanged(nameof(UpdateProfileValidationMessage));
            }
        }

        private bool _updateProfileVisible;
        public bool UpdateProfileVisible
        {
            get => _updateProfileVisible;
            set
            {
                _updateProfileVisible = value;
                OnPropertyChanged(nameof(UpdateProfileVisible));
            }
        }

        private bool _importButtonVisible;
        public bool ImportButtonVisible
        {
            get => _importButtonVisible;
            set
            {
                _importButtonVisible = value;
                OnPropertyChanged(nameof(ImportButtonVisible));
            }
        }

        private string _commitProfileValidationMessage;
        public string CommitProfileValidationMessage
        {
            get => _commitProfileValidationMessage;
            set
            {
                _commitProfileValidationMessage = value;
                OnPropertyChanged(nameof(CommitProfileValidationMessage));
            }
        }

        private string PreprocessString(string input)
        {
            return input.RemoveUnwantedCharacters(new List<char> { ' ', '\r', '\n' });
        }

        #endregion

        #region Creating\removing profiles

        public RelayCommand AddProfileCommand { get; }

        public RelayCommand RemoveProfileCommand { get; }

        public RelayCommand CloseCreateProfileCommand { get; }

        public RelayCommand CreateProfileCommand { get; }

        private void AddProfile()
        {
            NewProfileName = string.Empty;
            NewProfileDescription = string.Empty;
            CreateProfileVisible = true;
            PopUpVisible = true;
        }

        private void CloseCreateProfile()
        {
            CreateProfileVisible = false;
            PopUpVisible = false;
        }

        private void RemoveProfile()
        {
            _profileRepository.DeleteProfile(SelectedProfile.ProfileName);
            Profiles.Remove(SelectedProfile);
        }

        private void CreateProfile()
        {
            _profileRepository.SaveProfile(new InferenceProfile
            {
                ProfileName = NewProfileName,
                Description = NewProfileDescription
            });

            CreateProfileVisible = false;
            PopUpVisible = false;
            RefreshProfiles();
        }

        private bool _createProfileVisible;
        public bool CreateProfileVisible
        {
            get => _createProfileVisible;
            set
            {
                _createProfileVisible = value;
                OnPropertyChanged(nameof(CreateProfileVisible));
            }
        }

        private string _newProfileName;
        public string NewProfileName
        {
            get => _newProfileName;
            set
            {
                _newProfileName = value;
                OnPropertyChanged(nameof(NewProfileName));
            }
        }

        private string _newProfileDescription;
        public string NewProfileDescription
        {
            get => _newProfileDescription;
            set
            {
                _newProfileDescription = value;
                OnPropertyChanged(nameof(NewProfileDescription));
            }
        }

        public void RefreshProfiles()
        {
            var profiles = _profileRepository.GetProfiles();
            if (!profiles.IsPresent)
            {
                return;
            }

            Profiles.Clear();
            foreach (var profile in profiles.Value)
            {
                Profiles.Add(new InferenceProfileModel
                {
                    ProfileName = profile.ProfileName,
                    Description = profile.Description,
                    Rules = profile.Rules != null ?
                        new ObservableCollection<ContentModel>(profile.Rules.Select(r => new ContentModel { Content = r })) :
                        new ObservableCollection<ContentModel>(),
                    Variables = profile.Variables != null ?
                        new ObservableCollection<ContentModel>(profile.Variables.Select(v => new ContentModel { Content = v })) :
                        new ObservableCollection<ContentModel>()
                });
            }
            OnPropertyChanged(nameof(Profiles));
        }

        #endregion

        private bool _popUpVisible;
        public bool PopUpVisible
        {
            get => _popUpVisible;
            set
            {
                _popUpVisible = value;
                OnPropertyChanged(nameof(PopUpVisible));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}