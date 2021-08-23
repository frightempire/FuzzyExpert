using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using FuzzyExpert.Application.Contracts;
using FuzzyExpert.Application.InferenceExpert.Entities;
using FuzzyExpert.Application.InferenceExpert.Interfaces;
using FuzzyExpert.Infrastructure.DatabaseManagement.Interfaces;
using FuzzyExpert.Infrastructure.InitialDataProviding.Interfaces;
using FuzzyExpert.Infrastructure.ResultLogging.Interfaces;
using FuzzyExpert.WpfClient.Annotations;
using FuzzyExpert.WpfClient.Helpers;
using FuzzyExpert.WpfClient.Models;

namespace FuzzyExpert.WpfClient.ViewModels
{
    public class InferencingActionsModel : INotifyPropertyChanged
    {
        private readonly IProfileRepository _profileRepository;
        private readonly ISettingsRepository _settingsRepository;
        private readonly IDefaultSettingsProvider _defaultSettingsProvider;
        private readonly IExpert _expert;
        private readonly IKnowledgeBaseManager _knowledgeBaseManager;
        private readonly IDataFilePathProvider _dataFilePathProvider;
        private readonly IResultLogger _resultLogger;

        public InferencingActionsModel(
            IProfileRepository profileRepository,
            ISettingsRepository settingsRepository,
            IDefaultSettingsProvider defaultSettingsProvider,
            IExpert expert,
            IKnowledgeBaseManager knowledgeBaseManager,
            IDataFilePathProvider dataFilePathProvider,
            IResultLogger resultLogger)
        {
            _profileRepository = profileRepository ?? throw new ArgumentNullException(nameof(profileRepository));
            _settingsRepository = settingsRepository ?? throw new ArgumentNullException(nameof(settingsRepository));
            _defaultSettingsProvider = defaultSettingsProvider ?? throw new ArgumentNullException(nameof(defaultSettingsProvider));
            _expert = expert ?? throw new ArgumentNullException(nameof(expert));
            _knowledgeBaseManager = knowledgeBaseManager ?? throw new ArgumentNullException(nameof(knowledgeBaseManager));
            _dataFilePathProvider = dataFilePathProvider ?? throw new ArgumentNullException(nameof(dataFilePathProvider));
            _resultLogger = resultLogger ?? throw new ArgumentNullException(nameof(resultLogger));

            GetDataCommand = new RelayCommand(obj => GetData(), obj => true);
            StartInferenceCommand = new RelayCommand(obj => StartInference(), obj => !string.IsNullOrEmpty(DataFilePath) && SelectedProfile != null && SelectedProfile.Rules.Count != 0);
            GetPartialResultCommand = new RelayCommand(obj => GetPartialResult(), obj => true);
            OpenResultFileCommand = new RelayCommand(obj => OpenResultFile(), obj => ExpertOpinion != null);

            InitializeState();
        }

        public void InitializeState()
        {
            Profiles = new ObservableCollection<InferenceProfileModel>();
            PartialResult = new ObservableCollection<ContentModel>();
            OnPropertyChanged(nameof(PartialResult));
            Variables = new ObservableCollection<ContentModel>();
            OnPropertyChanged(nameof(Variables));
            ExpertOpinion = null;
            DataFilePath = string.Empty;
            Settings = new SettingsModel();
            ConfidenceResultMessage = string.Empty;
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

        private SettingsModel _settings;
        public SettingsModel Settings
        {
            get => _settings;
            set
            {
                if (value == _settings)
                {
                    return;
                }

                _settings = value;
                OnPropertyChanged(nameof(Settings));
            }
        }

        private ExpertOpinion ExpertOpinion { get; set; }

        public ObservableCollection<InferenceProfileModel> Profiles { get; set; }

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
                ResetBindingProperties();
                OnPropertyChanged(nameof(SelectedProfile));
            }
        }

        public ObservableCollection<ContentModel> PartialResult { get; set; }

        public ObservableCollection<ContentModel> Variables { get; set; }

        private ContentModel _selectedVariable;
        public ContentModel SelectedVariable
        {
            get => _selectedVariable;
            set
            {
                if (value == _selectedVariable)
                {
                    return;
                }

                _selectedVariable = value;
                OnPropertyChanged(nameof(SelectedVariable));
            }
        }

        private string _confidenceResultMessage;
        public string ConfidenceResultMessage
        {
            get => _confidenceResultMessage;
            set
            {
                _confidenceResultMessage = value;
                OnPropertyChanged(nameof(ConfidenceResultMessage));
            }
        }

        private void ResetBindingProperties()
        {
            PartialResult.Clear();
            ConfidenceResultMessage = string.Empty;
            Variables.Clear();
            ExpertOpinion = null;
        }

        public void RefreshSettings(string userName)
        {
            var settings = _settingsRepository.GetSettingsForUser(userName);
            if (!settings.IsPresent)
            {
                Settings = new SettingsModel
                {
                    Id = _settingsRepository.GetMaxSettingsId() + 1,
                    UserName = userName,
                    ConfidenceFactorLowerBoundary = _defaultSettingsProvider.Settings.ConfidenceFactorLowerBoundary
                };
            }
            else
            {
                Settings = new SettingsModel
                {
                    Id = settings.Value.Id,
                    UserName = settings.Value.UserName,
                    ConfidenceFactorLowerBoundary = settings.Value.ConfidenceFactorLowerBoundary
                };
            }
            OnPropertyChanged(nameof(Settings));
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

        public RelayCommand GetDataCommand { get; }

        public RelayCommand StartInferenceCommand { get; }

        public RelayCommand GetPartialResultCommand { get; }

        public RelayCommand OpenResultFileCommand { get; }

        private void GetData()
        {
            var dialog = new FileDialogInteractor();
            if (!dialog.OpenFileDialog())
            {
                return;
            }
            DataFilePath = dialog.FilePath;
            _dataFilePathProvider.FilePath = dialog.FilePath;
        }

        private void StartInference()
        {
            ExpertOpinion = _expert.GetResult(SelectedProfile.ProfileName);

            if (!ExpertOpinion.IsSuccess)
            {
                ConfidenceResultMessage = "Inference failed. Consult result log for more information.";
                return;
            }

            Variables.Clear();
            foreach (var variable in SelectedProfile.Variables.SelectMany(v => v.Content.Substring(1, v.Content.IndexOf(']') - 1).Split(',')))
            {
                Variables.Add(new ContentModel { Content = variable });
            }
            OnPropertyChanged(nameof(Variables));
        }

        private void GetPartialResult()
        {
            if (!ExpertOpinion.IsSuccess || SelectedVariable == null)
            {
                return;
            }

            PartialResult.Clear();

            var selectedVariable = SelectedVariable.Content;
            var lastVariableUsage = ExpertOpinion.Result.LastOrDefault(
                r => r.Item1.Split(new[] {" = "}, StringSplitOptions.RemoveEmptyEntries)[0] == selectedVariable);

            if (lastVariableUsage == null)
            {
                FillUnreachedInferenceResult(selectedVariable);
            }
            else
            {
                FillReachedInferenceResult(lastVariableUsage);
            }
        }

        private void FillUnreachedInferenceResult(string selectedVariable)
        {
            PartialResult.Add(new ContentModel
            {
                Content = $"Variable {selectedVariable} was not used during inference"
            });
            ConfidenceResultMessage = string.Empty;

            OnPropertyChanged(nameof(PartialResult));
            OnPropertyChanged(nameof(ConfidenceResultMessage));
        }

        private void FillReachedInferenceResult(Tuple<string, double> lastVariableUsage)
        {
            var lastVariableUsageIndex = ExpertOpinion.Result.LastIndexOf(lastVariableUsage);
            var previousResults = ExpertOpinion.Result.GetRange(0, lastVariableUsageIndex + 1);

            foreach (var previousResult in previousResults)
            {
                PartialResult.Add(new ContentModel
                {
                    Content = $"Node {previousResult.Item1} was enabled with confidence factor {previousResult.Item2}"
                });
            }

            ConfidenceResultMessage = lastVariableUsage.Item2 < Settings.ConfidenceFactorLowerBoundary
                ? $"Confidence for {lastVariableUsage.Item1} is lower then {Settings.ConfidenceFactorLowerBoundary}. " +
                  "It's not advisable to proceed."
                : $"Confidence for {lastVariableUsage.Item1} is greater then {Settings.ConfidenceFactorLowerBoundary}. " +
                  "It's advisable to proceed.";

            OnPropertyChanged(nameof(ConfidenceResultMessage));
            OnPropertyChanged(nameof(PartialResult));
        }

        private void OpenResultFile()
        {
            var rules = _knowledgeBaseManager.GetKnowledgeBase(SelectedProfile.ProfileName).Value.ImplicationRules;
            var resultLogPath = _resultLogger.LogInferenceResult(rules, ExpertOpinion, UserName);
            Process.Start(resultLogPath);
        }

        public void RefreshProfiles(string userName)
        {
            UserName = userName;

            var profiles = _profileRepository.GetProfilesForUser(userName);
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
                        new ObservableCollection<ContentModel>(),
                    UserName = userName
                });
            }
            OnPropertyChanged(nameof(Profiles));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}