using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
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

            GetDataCommand = new RelayCommand(obj => GetData(), obj => true);
            StartInferenceCommand = new RelayCommand(obj => StartInference(), obj => !string.IsNullOrEmpty(DataFilePath) && SelectedProfile != null && SelectedProfile.Rules.Count != 0);
            GetPartialResultCommand = new RelayCommand(obj => GetPartialResult(), obj => true);
            OpenResultFileCommand = new RelayCommand(obj => OpenResultFile(), obj => ExpertOpinion.IsSuccess);

            OpenResultViewCommand = new RelayCommand(obj => OpenResultView(), obj => true);
            CloseResultViewCommand = new RelayCommand(obj => CloseResultView(), obj => true);

            InitializeState();
        }

        public void InitializeState()
        {
            Profiles = new ObservableCollection<InferenceProfileModel>();
            PartialResult = new ObservableCollection<ContentModel>();
            Variables = new ObservableCollection<ContentModel>();
            ExpertOpinion = new ExpertOpinion();
            DataFilePath = string.Empty;
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

        private void ResetBindingProperties()
        {
            PartialResult.Clear();
            Variables.Clear();
            ExpertOpinion = new ExpertOpinion();
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

        public RelayCommand CloseResultViewCommand { get; }

        public RelayCommand OpenResultViewCommand { get; }


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
            if (!ExpertOpinion.IsSuccess)
            {
                return;
            }

            PartialResult.Clear();
            var selectedVariable = SelectedVariable.Content;
            var lastVariableUsage = ExpertOpinion.Result.LastIndexOf(ExpertOpinion.Result.Last(r => r.Item1.Contains(selectedVariable)));
            var previousResults = ExpertOpinion.Result.GetRange(0, lastVariableUsage+1);
            foreach (var previousResult in previousResults)
            {
                PartialResult.Add(new ContentModel
                {
                    Content = $"Node {previousResult.Item1} was enabled with confidence factor {previousResult.Item2}"
                });
            }
            OnPropertyChanged(nameof(PartialResult));
        }

        private void OpenResultFile()
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
        }

        private void OpenResultView()
        {
            ResultViewVisible = true;
            PopUpVisible = true;
        }

        private void CloseResultView()
        {
            ResultViewVisible = false;
            PopUpVisible = false;
        }

        private bool _resultViewVisible;
        public bool ResultViewVisible
        {
            get => _resultViewVisible;
            set
            {
                _resultViewVisible = value;
                OnPropertyChanged(nameof(ResultViewVisible));
            }
        }

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

        public void RefreshProfiles(string userName)
        {
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