using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using FuzzyExpert.Application.Common.Interfaces;
using FuzzyExpert.Application.Entities;
using FuzzyExpert.CommonUILogic.Implementations;
using FuzzyExpert.Core.Entities;
using FuzzyExpert.Core.Extensions;
using FuzzyExpert.Infrastructure.KnowledgeManager.Interfaces;
using FuzzyExpert.Infrastructure.LinguisticVariableParsing.Interfaces;
using FuzzyExpert.Infrastructure.ProductionRuleParsing.Interfaces;
using FuzzyExpert.Infrastructure.ProfileManaging.Entities;
using FuzzyExpert.Infrastructure.ProfileManaging.Interfaces;
using FuzzyExpert.WpfClient.Annotations;

namespace FuzzyExpert.WpfClient.ViewModels
{
    public class AddImplicationRuleActionModel : INotifyPropertyChanged
    {
        private readonly IProfileRepository _profileRepository;
        private readonly IImplicationRuleValidator _ruleValidator;
        private readonly ILinguisticVariableValidator _variableValidator;
        private readonly IImplicationRuleCreator _ruleCreator;
        private readonly ILinguisticVariableCreator _variableCreator;
        private readonly IKnowledgeBaseValidator _knowledgeValidator;
        private readonly IFileOperations _fileOperations;

        public string ProfileName { get; }

        public ObservableCollection<string> Rules { get; set; }

        public ObservableCollection<string> Variables { get; set; }

        public AddImplicationRuleActionModel(
            string profileName,
            IProfileRepository profileRepository,
            IImplicationRuleValidator ruleValidator,
            ILinguisticVariableValidator variableValidator,
            IImplicationRuleCreator ruleCreator,
            ILinguisticVariableCreator variableCreator,
            IKnowledgeBaseValidator knowledgeValidator,
            IFileOperations fileOperations)
        {
            if (string.IsNullOrEmpty(profileName))
            {
                throw new ArgumentNullException(nameof(profileName));
            }
            ProfileName = profileName;

            _profileRepository = profileRepository ?? throw new ArgumentNullException(nameof(profileRepository));
            _ruleValidator = ruleValidator ?? throw new ArgumentNullException(nameof(ruleValidator));
            _variableValidator = variableValidator ?? throw new ArgumentNullException(nameof(variableValidator));
            _ruleCreator = ruleCreator ?? throw new ArgumentNullException(nameof(ruleCreator));
            _variableCreator = variableCreator ?? throw new ArgumentNullException(nameof(variableCreator));
            _knowledgeValidator = knowledgeValidator ?? throw new ArgumentNullException(nameof(knowledgeValidator));
            _fileOperations = fileOperations ?? throw new ArgumentNullException(nameof(fileOperations));

            InitializeBindingProperties();
            InitializeCollectionValues();
        }

        private void InitializeBindingProperties()
        {
            AddRuleButtonEnable = false;
            AddVariableButtonEnable = false;
            RemoveRuleButtonEnable = false;
            RemoveVariableButtonEnable = false;
            CommitButtonEnable = false;

            InitializeButtonsVisibility();

            Rules = new ObservableCollection<string>();
            Variables = new ObservableCollection<string>();
        }

        private void InitializeButtonsVisibility()
        {
            ImportButtonVisible = Visibility.Visible;
            ImportRuleBoxVisible = Visibility.Hidden;
        }

        private void RevertButtonsVisibility()
        {
            ImportButtonVisible = Visibility.Hidden;
            ImportRuleBoxVisible = Visibility.Visible;
        }

        private void InitializeCollectionValues()
        {
            var profile = _profileRepository.GetProfileByName(ProfileName);
            if (profile.Value.Rules == null || profile.Value.Variables == null)
            {
                return;
            }

            foreach (var rule in profile.Value.Rules)
            {
                Rules.Add(rule);
            }
            foreach (var variable in profile.Value.Variables)
            {
                Variables.Add(variable);
            }
            UpdateCommitButtonStatus();
        }

        private void UpdateAddButtonsStatus()
        {
            if (string.IsNullOrEmpty(UserInput))
            {
                AddRuleButtonEnable = false;
                AddVariableButtonEnable = false;
            }
            else
            {
                AddRuleButtonEnable = true;
                AddVariableButtonEnable = true;
            }
        }

        private void UpdateCommitButtonStatus()
        {
            CommitButtonEnable = Rules.Any() && Variables.Any();
        }

        private void UpdateStartImportButtonStatus()
        {
            if (!string.IsNullOrEmpty(RuleFilePath) &&
                !string.IsNullOrEmpty(VariableFilePath))
            {
                StartImportButtonEnable = true;
            }
        }

        private bool _addRuleButtonEnable;
        public bool AddRuleButtonEnable
        {
            get => _addRuleButtonEnable;
            set
            {
                _addRuleButtonEnable = value;
                OnPropertyChanged(nameof(AddRuleButtonEnable));
            }
        }

        private bool _addVariableButtonEnable;
        public bool AddVariableButtonEnable
        {
            get => _addVariableButtonEnable;
            set
            {
                _addVariableButtonEnable = value;
                OnPropertyChanged(nameof(AddVariableButtonEnable));
            }
        }

        private bool _removeRuleButtonEnable;
        public bool RemoveRuleButtonEnable
        {
            get => _removeRuleButtonEnable;
            set
            {
                _removeRuleButtonEnable = value;
                OnPropertyChanged(nameof(RemoveRuleButtonEnable));
            }
        }

        private bool _removeVariableButtonEnable;
        public bool RemoveVariableButtonEnable
        {
            get => _removeVariableButtonEnable;
            set
            {
                _removeVariableButtonEnable = value;
                OnPropertyChanged(nameof(RemoveVariableButtonEnable));
            }
        }

        private bool _commitButtonEnable;
        public bool CommitButtonEnable
        {
            get => _commitButtonEnable;
            set
            {
                _commitButtonEnable = value;
                OnPropertyChanged(nameof(CommitButtonEnable));
            }
        }

        private bool _startImportButtonEnable;
        public bool StartImportButtonEnable
        {
            get => _startImportButtonEnable;
            set
            {
                _startImportButtonEnable = value;
                OnPropertyChanged(nameof(StartImportButtonEnable));
            }
        }

        private string _selectedRule;
        public string SelectedRule
        {
            get => _selectedRule;
            set
            {
                if (value == _selectedRule)
                {
                    return;
                }

                _selectedRule = value;
                RemoveRuleButtonEnable = true;
                OnPropertyChanged(nameof(SelectedRule));
            }
        }

        private string _selectedVariable;
        public string SelectedVariable
        {
            get => _selectedVariable;
            set
            {
                if (value == _selectedVariable)
                {
                    return;
                }

                _selectedVariable = value;
                RemoveVariableButtonEnable = true;
                OnPropertyChanged(nameof(SelectedVariable));
            }
        }

        private Visibility _importButtonVisible;
        public Visibility ImportButtonVisible
        {
            get => _importButtonVisible;
            set
            {
                _importButtonVisible = value;
                OnPropertyChanged(nameof(ImportButtonVisible));
            }
        }

        private Visibility _importRuleBoxVisible;
        public Visibility ImportRuleBoxVisible
        {
            get => _importRuleBoxVisible;
            set
            {
                _importRuleBoxVisible = value;
                OnPropertyChanged(nameof(ImportRuleBoxVisible));
            }
        }

        private string _userInput;
        public string UserInput
        {
            get => _userInput;
            set
            {
                _userInput = value;
                UpdateAddButtonsStatus();
                OnPropertyChanged(nameof(UserInput));
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

        private RelayCommand _addRuleCommand;
        public RelayCommand AddRuleCommand
        {
            get
            {
                return _addRuleCommand ??
                       (_addRuleCommand = new RelayCommand(obj =>
                       {
                           string preProcessedImplicationRule = PreprocessString(UserInput);
                           var validationResult = _ruleValidator.ValidateImplicationRule(preProcessedImplicationRule);
                           if (validationResult.IsSuccess)
                           {
                               Rules.Add(preProcessedImplicationRule);
                               UpdateCommitButtonStatus();
                               UserInput = string.Empty;
                           }
                           else
                           {
                               MessageBox.Show(string.Join("\n",validationResult.Messages), "Implication rule", MessageBoxButton.OK);
                           }
                       }));
            }
        }

        private RelayCommand _addVariableCommand;
        public RelayCommand AddVariableCommand
        {
            get
            {
                return _addVariableCommand ??
                       (_addVariableCommand = new RelayCommand(obj =>
                       {
                           string preProcessedLinguisticVariable = PreprocessString(UserInput);
                           var validationResult = _variableValidator.ValidateLinguisticVariables(preProcessedLinguisticVariable);
                           if (validationResult.IsSuccess)
                           {
                               Variables.Add(preProcessedLinguisticVariable);
                               UpdateCommitButtonStatus();
                               UserInput = string.Empty;
                           }
                           else
                           {
                               MessageBox.Show(string.Join("\n", validationResult.Messages), "Linguistic variable", MessageBoxButton.OK);
                           }
                       }));
            }
        }

        private RelayCommand _removeRuleCommand;
        public RelayCommand RemoveRuleCommand
        {
            get
            {
                return _removeRuleCommand ??
                       (_removeRuleCommand = new RelayCommand(obj =>
                       {
                           Rules.Remove(SelectedRule);
                           RemoveRuleButtonEnable = false;
                       }));
            }
        }

        private RelayCommand _removeVariableCommand;
        public RelayCommand RemoveVariableCommand
        {
            get
            {
                return _removeVariableCommand ??
                       (_removeVariableCommand = new RelayCommand(obj =>
                       {
                           Variables.Remove(SelectedVariable);
                           RemoveVariableButtonEnable = false;
                       }));
            }
        }

        private RelayCommand _importCommand;
        public RelayCommand ImportCommand
        {
            get
            {
                return _importCommand ?? (_importCommand = new RelayCommand(obj => { RevertButtonsVisibility(); }));
            }
        }

        private RelayCommand _getRulesFromFileCommand;
        public RelayCommand GetRulesFromFileCommand
        {
            get
            {
                return _getRulesFromFileCommand ??
                       (_getRulesFromFileCommand = new RelayCommand(obj =>
                       {
                           var dialog = new FileDialogInteractor();
                           if (!dialog.OpenFileDialog())
                           {
                               return;
                           }
                           RuleFilePath = dialog.FilePath;
                           UpdateStartImportButtonStatus();
                       }));
            }
        }

        private RelayCommand _getVariablesFromFileCommand;
        public RelayCommand GetVariablesFromFileCommand
        {
            get
            {
                return _getVariablesFromFileCommand ??
                       (_getVariablesFromFileCommand = new RelayCommand(obj =>
                       {
                           var dialog = new FileDialogInteractor();
                           if (!dialog.OpenFileDialog())
                           {
                               return;
                           }
                           VariableFilePath = dialog.FilePath;
                           UpdateStartImportButtonStatus();
                       }));
            }
        }

        private RelayCommand _startImportCommand;
        public RelayCommand StartImportCommand
        {
            get
            {
                return _startImportCommand ?? (_startImportCommand = new RelayCommand(obj =>
                {
                    var rulesFromFile = _fileOperations.ReadFileByLines(RuleFilePath);
                    List<ValidationOperationResult> failedRulesValidations = new List<ValidationOperationResult>();
                    foreach (var ruleFromFile in rulesFromFile)
                    {
                        string preProcessedImplicationRule = PreprocessString(ruleFromFile);
                        var validationResult = _ruleValidator.ValidateImplicationRule(preProcessedImplicationRule);
                        if (!validationResult.IsSuccess)
                        {
                            failedRulesValidations.Add(validationResult);
                        }
                    }

                    var variablesFromFile = _fileOperations.ReadFileByLines(VariableFilePath);
                    List<ValidationOperationResult> failedVariablesValidations = new List<ValidationOperationResult>();
                    foreach (var variableFromFile in variablesFromFile)
                    {
                        string preProcessedLinguisticVariable = PreprocessString(variableFromFile);
                        var validationResult = _variableValidator.ValidateLinguisticVariables(preProcessedLinguisticVariable);
                        if (!validationResult.IsSuccess)
                        {
                            failedVariablesValidations.Add(validationResult);
                        }
                    }

                    if (failedRulesValidations.Any() || failedVariablesValidations.Any())
                    {
                        MessageBox.Show(
                            $"Rules :\n{string.Join("\n", failedRulesValidations.SelectMany(r => r.Messages))}" +
                            $"\n\nVariables :\n{string.Join("\n", failedVariablesValidations.SelectMany(r => r.Messages))}", 
                            "Knowledge base from file", MessageBoxButton.OK);
                    }
                    else
                    {
                        foreach (var rule in rulesFromFile)
                        {
                            Rules.Add(PreprocessString(rule));
                        }
                        foreach (var variable in variablesFromFile)
                        {
                            Variables.Add(PreprocessString(variable));
                        }
                        UpdateCommitButtonStatus();
                    }
                }));
            }
        }

        private RelayCommand _commitCommand;
        public RelayCommand CommitCommand
        {
            get
            {
                return _commitCommand ??
                       (_commitCommand = new RelayCommand(obj =>
                       {
                           var rules = new List<ImplicationRule>();
                           foreach (var rule in Rules)
                           {
                               rules.Add(_ruleCreator.CreateImplicationRuleEntity(rule));
                           }

                           var variables = new List<LinguisticVariable>();
                           foreach (var variable in Variables)
                           {
                               variables.AddRange(_variableCreator.CreateLinguisticVariableEntities(variable));
                           }

                           var validationResult = _knowledgeValidator.ValidateLinguisticVariablesNames(rules, variables);

                           if (validationResult.IsSuccess)
                           {
                               var profile = new InferenceProfile
                               {
                                   ProfileName = ProfileName,
                                   Rules = Rules.ToList(),
                                   Variables = Variables.ToList()
                               };
                               _profileRepository.SaveProfile(profile);
                               MessageBox.Show("Knowledge base is valid.\nUpdate successful!", "Knowledge", MessageBoxButton.OK);
                           }
                           else
                           {
                               MessageBox.Show(string.Join("\n", validationResult.Messages), "Knowledge", MessageBoxButton.OK);
                           }
                       }));
            }
        }

        private string PreprocessString(string input)
        {
            return input.RemoveUnwantedCharacters(new List<char> {' '});
        }

        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}