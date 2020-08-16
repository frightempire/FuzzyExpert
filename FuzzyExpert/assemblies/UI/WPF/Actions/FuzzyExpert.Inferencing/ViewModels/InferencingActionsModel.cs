using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using FuzzyExpert.Application.Contracts;
using FuzzyExpert.Application.InferenceExpert.Entities;
using FuzzyExpert.Application.InferenceExpert.Interfaces;
using FuzzyExpert.CommonUILogic.Implementations;
using FuzzyExpert.Inferencing.Properties;
using FuzzyExpert.Infrastructure.InitialDataProviding.Interfaces;
using FuzzyExpert.Infrastructure.ProfileManaging.Entities;
using FuzzyExpert.Infrastructure.ProfileManaging.Interfaces;
using FuzzyExpert.Infrastructure.ResultLogging.Interfaces;

namespace FuzzyExpert.Inferencing.ViewModels
{
    public class InferencingActionsModel : INotifyPropertyChanged
    {
        private readonly IProfileRepository _profileRepository;
        private readonly IExpert _expert;
        private readonly IKnowledgeBaseManager _knowledgeBaseManager;
        private readonly IDataFilePathProvider _dataFilePathProvider;
        private readonly IResultLogger _resultLogger;

        public InferencingActionsModel(
            IProfileRepository profileRepository,
            IExpert expert,
            IKnowledgeBaseManager knowledgeBaseManager,
            IDataFilePathProvider dataFilePathProvider,
            IResultLogger resultLogger)
        {
            _profileRepository = profileRepository ?? throw new ArgumentNullException(nameof(profileRepository));
            _expert = expert ?? throw new ArgumentNullException(nameof(expert));
            _knowledgeBaseManager = knowledgeBaseManager ?? throw new ArgumentNullException(nameof(knowledgeBaseManager));
            _dataFilePathProvider = dataFilePathProvider ?? throw new ArgumentNullException(nameof(dataFilePathProvider));
            _resultLogger = resultLogger ?? throw new ArgumentNullException(nameof(resultLogger));

            InitializeBindingProperties();
            InitializeCollectionValues();
        }

        private ExpertOpinion ExpertOpinion { get; set; }

        public ObservableCollection<InferenceProfile> Profiles { get; set; }

        public ObservableCollection<string> ImplicationRules { get; set; }

        public ObservableCollection<string> Results { get; set; }

        private void InitializeBindingProperties()
        {
            StartInferenceButtonEnable = false;
            OpenResultFileButtonEnable = false;
            Profiles = new ObservableCollection<InferenceProfile>();
            ImplicationRules = new ObservableCollection<string>();
            Results = new ObservableCollection<string>();
            ExpertOpinion = new ExpertOpinion();
        }

        private void ResetBindingProperties()
        {
            StartInferenceButtonEnable = false;
            OpenResultFileButtonEnable = false;
            ImplicationRules.Clear();
            Results.Clear();
            ExpertOpinion = new ExpertOpinion();
        }

        private void InitializeCollectionValues()
        {
            var profiles = _profileRepository.GetProfiles();
            if (!profiles.IsPresent)
            {
                return;
            }

            foreach (var profile in profiles.Value)
            {
                Profiles.Add(profile);
            }
        }

        private bool _startInferenceButtonEnable;
        public bool StartInferenceButtonEnable
        {
            get => _startInferenceButtonEnable;
            set
            {
                _startInferenceButtonEnable = value;
                OnPropertyChanged(nameof(StartInferenceButtonEnable));
            }
        }

        private bool _openResultFileButtonEnable;
        public bool OpenResultFileButtonEnable
        {
            get => _openResultFileButtonEnable;
            set
            {
                _openResultFileButtonEnable = value;
                OnPropertyChanged(nameof(OpenResultFileButtonEnable));
            }
        }

        private string _dataFilePath;
        public string DataFilePath
        {
            get => _dataFilePath;
            set
            {
                _dataFilePath = value;
                OnPropertyChanged(nameof(DataFilePath));
            }
        }

        private InferenceProfile _selectedProfile;
        public InferenceProfile SelectedProfile
        {
            get => _selectedProfile;
            set
            {
                if (value == _selectedProfile)
                {
                    return;
                }

                _selectedProfile = value;
                ResetBindingProperties();
                UpdateStartInferenceButtonStatus();
                UpdateImplicationRulesStatus();
                OnPropertyChanged(nameof(SelectedProfile));
            }
        }

        private RelayCommand _getDataCommand;
        public RelayCommand GetDataCommand
        {
            get
            {
                return _getDataCommand ??
                       (_getDataCommand = new RelayCommand(obj =>
                       {
                           var dialog = new FileDialogInteractor();
                           if (!dialog.OpenFileDialog())
                           {
                               return;
                           }
                           DataFilePath = dialog.FilePath;
                           _dataFilePathProvider.FilePath = dialog.FilePath;
                           UpdateStartInferenceButtonStatus();
                       }));
            }
        }

        private RelayCommand _startInferenceCommand;
        public RelayCommand StartInferenceCommand
        {
            get
            {
                return _startInferenceCommand ??
                       (_startInferenceCommand = new RelayCommand(obj =>
                       {
                           ExpertOpinion = _expert.GetResult(SelectedProfile.ProfileName);
                           if (!ExpertOpinion.IsSuccess)
                           {
                               return;
                           }

                           OpenResultFileButtonEnable = true;
                           foreach (var result in ExpertOpinion.Result)
                           {
                               Results.Add($"Node {result.Key} was enabled with confidence factor {result.Value}");
                           }
                       }));
            }
        }

        private RelayCommand _openResultFileCommand;
        public RelayCommand OpenResultFileCommand
        {
            get
            {
                return _openResultFileCommand ??
                       (_openResultFileCommand = new RelayCommand(obj =>
                       {
                           File.Delete(_resultLogger.ResultLogPath);

                           var rules = _knowledgeBaseManager.GetKnowledgeBase(SelectedProfile.ProfileName).Value.ImplicationRules;
                           _resultLogger.LogImplicationRules(rules);

                           if (ExpertOpinion.IsSuccess)
                           {
                               _resultLogger.LogInferenceResult(ExpertOpinion.Result);
                           }
                           else
                           {
                               _resultLogger.LogInferenceErrors(ExpertOpinion.ErrorMessages);
                           }
                           Process.Start(_resultLogger.ResultLogPath);
                       }));
            }
        }

        private void UpdateStartInferenceButtonStatus()
        {
            StartInferenceButtonEnable = !string.IsNullOrEmpty(DataFilePath) && SelectedProfile != null;
        }

        private void UpdateImplicationRulesStatus()
        {
            if (SelectedProfile == null)
            {
                return;
            }

            var knowledgeBase = _knowledgeBaseManager.GetKnowledgeBase(SelectedProfile.ProfileName);
            if (!knowledgeBase.IsPresent &&
                (knowledgeBase.Value.ImplicationRules.Count == 0 || knowledgeBase.Value.LinguisticVariables.Count == 0))
            {
                return;
            }

            var rules = knowledgeBase.Value.ImplicationRules;
            foreach (var rule in rules)
            {
                ImplicationRules.Add(rule.Value.ToString());
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