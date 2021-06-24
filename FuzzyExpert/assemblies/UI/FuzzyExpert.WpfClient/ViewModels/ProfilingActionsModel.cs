using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

        public ProfilingActionsModel(
            IProfileRepository profileRepository,
            IFileOperations fileOperations,
            IImplicationRuleValidator ruleValidator,
            ILinguisticVariableValidator variableValidator,
            IImplicationRuleCreator ruleCreator,
            ILinguisticVariableCreator variableCreator,
            IKnowledgeBaseValidator knowledgeValidator)
        {
            _profileRepository = profileRepository ?? throw new ArgumentNullException(nameof(profileRepository));
            _fileOperations = fileOperations ?? throw new ArgumentNullException(nameof(fileOperations));
            _ruleValidator = ruleValidator ?? throw new ArgumentNullException(nameof(ruleValidator));
            _variableValidator = variableValidator ?? throw new ArgumentNullException(nameof(variableValidator));
            _ruleCreator = ruleCreator ?? throw new ArgumentNullException(nameof(ruleCreator));
            _variableCreator = variableCreator ?? throw new ArgumentNullException(nameof(variableCreator));
            _knowledgeValidator = knowledgeValidator ?? throw new ArgumentNullException(nameof(knowledgeValidator));

            AddProfileCommand = new RelayCommand(obj => AddProfile(), obj => true);
            RemoveProfileCommand = new RelayCommand(obj => RemoveProfile(), obj => SelectedProfile != null);
            CreateProfileCommand = new RelayCommand(obj => CreateProfile(), obj => !string.IsNullOrEmpty(NewProfileName));
            CloseCreateProfileCommand = new RelayCommand(obj => CloseCreateProfile(), obj => true);

            UpdateProfileCommand = new RelayCommand(obj => OpenUpdateProfileForm(updateMode: true), obj => true);
            CloseUpdateProfileCommand = new RelayCommand(obj => CloseUpdateProfileForm(), obj => true);
            GetRulesFromFileCommand = new RelayCommand(obj => GetRulesFromFile(), obj => true);
            GetVariablesFromFileCommand = new RelayCommand(obj => GetVariablesFromFile(), obj => true);
            StartImportFromFilesCommand = new RelayCommand(obj => StartImportFromFiles(), obj => !string.IsNullOrEmpty(RuleFilePath) && !string.IsNullOrEmpty(VariableFilePath));
            ImportRuleFromInputCommand = new RelayCommand(obj => ImportRuleFromInput(), obj => !string.IsNullOrEmpty(UpdatingInput));
            ImportVariableFromInputCommand = new RelayCommand(obj => ImportVariableFromInput(), obj => !string.IsNullOrEmpty(UpdatingInput));
            CommitProfileCommand = new RelayCommand(obj => CommitProfile(), obj => true);

            ValidationResults = new ObservableCollection<string>();
            Profiles = new ObservableCollection<InferenceProfileModel>();
        }

        public void InitializeState()
        {
            ValidationResults = new ObservableCollection<string>();
            Profiles = new ObservableCollection<InferenceProfileModel>();
        }

        #region Collections

        private ObservableCollection<string> _validationResults;
        public ObservableCollection<string> ValidationResults
        {
            get => _validationResults;
            set
            {
                _validationResults = value;
                OnPropertyChanged(nameof(ValidationResults));
            }
        }

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

        private string _userName;
        public string UserName
        {
            get => _userName;
            set
            {
                _userName = value;
                OnPropertyChanged(nameof(UserName));
            }
        }

        #endregion

        #region Updating profiles

        public RelayCommand UpdateProfileCommand { get; }

        public RelayCommand CloseUpdateProfileCommand { get; }

        public RelayCommand GetRulesFromFileCommand { get; }

        public RelayCommand GetVariablesFromFileCommand { get; }

        public RelayCommand StartImportFromFilesCommand { get; }

        public RelayCommand ImportRuleFromInputCommand { get; }

        public RelayCommand ImportVariableFromInputCommand { get; }

        public RelayCommand CommitProfileCommand { get; }

        private void OpenUpdateProfileForm(bool updateMode)
        {
            UpdatingInput = string.Empty;
            RuleFilePath = string.Empty;
            VariableFilePath = string.Empty;
            UpdateProfileVisible = true;
            PopUpVisible = true;

            if (updateMode)
            {
                ValidationResults.Clear();
            }
        }

        private void CloseUpdateProfileForm()
        {
            UpdateProfileVisible = false;
            PopUpVisible = false;
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
            ValidateImplicationRules(preProcessedImplicationRules);

            var variablesFromFile = _fileOperations.ReadFileByLines(VariableFilePath).ToList();
            var preProcessedLinguisticVariables = variablesFromFile.Select(PreprocessString).ToList();
            ValidateLinguisticVariables(preProcessedLinguisticVariables);

            if (ValidationResults.Any())
            {
                return;
            }

            ValidationResults.Add("Validation passed successfully");
            foreach (var rule in preProcessedImplicationRules)
            {
                SelectedProfile.Rules.Add(new ContentModel { Content = rule });
            }
            foreach (var variable in preProcessedLinguisticVariables)
            {
                SelectedProfile.Variables.Add(new ContentModel { Content = variable });
            }
        }

        private void ImportRuleFromInput()
        {
            var preProcessedRule = PreprocessString(UpdatingInput);
            var validationResult = _ruleValidator.ValidateImplicationRule(preProcessedRule);
            if (validationResult.Successful)
            {
                ValidationResults.Add("Validation passed successfully");
                SelectedProfile.Rules.Add(new ContentModel {Content = preProcessedRule});
            }
            else
            {
                ValidationResults.Add($"{validationResult.Messages.Count} validation errors were found");
                validationResult.Messages.ForEach(v => ValidationResults.Add(v));
            }
        }

        private void ImportVariableFromInput()
        {
            var preProcessedVariable = PreprocessString(UpdatingInput);
            var validationResult = _variableValidator.ValidateLinguisticVariables(preProcessedVariable);
            if (validationResult.Successful)
            {
                ValidationResults.Add("Validation passed successfully");
                SelectedProfile.Variables.Add(new ContentModel {Content = preProcessedVariable});
            }
            else
            {
                ValidationResults.Add($"{validationResult.Messages.Count} validation errors were found");
                validationResult.Messages.ForEach(v => ValidationResults.Add(v));
            }
        }

        private void CommitProfile()
        {
            var preProcessedImplicationRules = SelectedProfile.Rules.Select(x => PreprocessString(x.Content)).ToList();
            ValidateImplicationRules(preProcessedImplicationRules);

            var preProcessedLinguisticVariables = SelectedProfile.Variables.Select(x => PreprocessString(x.Content)).ToList();
            ValidateLinguisticVariables(preProcessedLinguisticVariables);

            if (ValidationResults.Any())
            {
                OpenUpdateProfileForm(updateMode: false);
                return;
            }

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

            if (knowledgeValidationResult.Successful)
            {
                var profile = new InferenceProfile
                {
                    ProfileName = SelectedProfile.ProfileName,
                    UserName = UserName,
                    Description = SelectedProfile.Description,
                    Rules = preProcessedImplicationRules,
                    Variables = preProcessedLinguisticVariables
                };
                _profileRepository.SaveProfile(profile);
                RefreshProfiles(UserName);
                ProfileValidationMessage = "Knowledge base is valid - update successful";
            }
            else
            {
                ProfileValidationMessage = $"{knowledgeValidationResult.Messages.Count} validation errors were found";
                knowledgeValidationResult.Messages.ForEach(v => ValidationResults.Add(v));
                OpenUpdateProfileForm(updateMode: false);
            }
        }

        private void ValidateImplicationRules(List<string> preProcessedImplicationRules)
        {
            var ruleErrorsFound = false;
            foreach (var implicationRule in preProcessedImplicationRules)
            {
                var validationResult = _ruleValidator.ValidateImplicationRule(implicationRule);
                if (validationResult.Successful)
                {
                    continue;
                }

                ValidationResults.Add(implicationRule);
                validationResult.Messages.ForEach(v => ValidationResults.Add(v));
                ruleErrorsFound = true;
            }

            if (ruleErrorsFound)
            {
                ValidationResults.Add($"Rule example : {RuleFormatExample}");
            }
        }

        private string RuleFormatExample => "IF (Pressure = HIGH & Danger = HIGH) THEN (Evacuate = TRUE)";

        private void ValidateLinguisticVariables(List<string> preProcessedLinguisticVariables)
        {
            var variableErrorsFound = false;
            foreach (var linguisticVariable in preProcessedLinguisticVariables)
            {
                var validationResult = _variableValidator.ValidateLinguisticVariables(linguisticVariable);
                if (validationResult.Successful)
                {
                    continue;
                }

                ValidationResults.Add(linguisticVariable);
                validationResult.Messages.ForEach(v => ValidationResults.Add(v));
                variableErrorsFound = true;
            }

            if (variableErrorsFound)
            {
                ValidationResults.Add($"Rule example : {VariableFormatExample}");
            }
        }

        private string VariableFormatExample => "[WaterTemperature,AirTemperature]:Initial:" +
                                                "[Cold:Trapezoidal:(0,20,20,30)|Hot:Trapezoidal:(50,60,60,80)]";

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

        private string _profileValidationMessage;
        public string ProfileValidationMessage
        {
            get => _profileValidationMessage;
            set
            {
                _profileValidationMessage = value;
                OnPropertyChanged(nameof(ProfileValidationMessage));
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
                Description = NewProfileDescription,
                UserName = UserName
            });

            CreateProfileVisible = false;
            PopUpVisible = false;
            RefreshProfiles(UserName);
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

        public void RefreshProfiles(string userName)
        {
            Profiles = new ObservableCollection<InferenceProfileModel>();

            UserName = userName;
            var profiles = _profileRepository.GetProfilesForUser(userName);
            if (!profiles.IsPresent)
            {
                return;
            }

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
                        new ObservableCollection<ContentModel>(),
                    UserName = UserName
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