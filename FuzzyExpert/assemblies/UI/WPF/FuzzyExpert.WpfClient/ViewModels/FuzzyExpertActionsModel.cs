using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using FuzzyExpert.Inferencing.Views;
using FuzzyExpert.Infrastructure.ProfileManaging.Interfaces;
using FuzzyExpert.WpfClient.Annotations;
using FuzzyExpert.WpfClient.Models;
using FuzzyExpert.WpfClient.Views;

namespace FuzzyExpert.WpfClient.ViewModels
{
    public class FuzzyExpertActionsModel : INotifyPropertyChanged
    {
        public ProfilingActions ProfilingActions { get; }

        public InferencingActions InferencingActions { get; }

        private readonly IProfileRepository _profileRepository;

        public FuzzyExpertActionsModel(
            ProfilingActions profilingActions,
            InferencingActions inferencingActions,
            IProfileRepository profileRepository)
        {
            ProfilingActions = profilingActions ?? throw new ArgumentNullException(nameof(profilingActions));
            InferencingActions = inferencingActions ?? throw new ArgumentNullException(nameof(inferencingActions));
            _profileRepository = profileRepository ?? throw new ArgumentNullException(nameof(profileRepository));

            ShowProfilingCommand = new RelayCommand(obj => ShowProfiling());
            ShowInferencingCommand = new RelayCommand(obj => ShowInferencing());

            ShowProfiling();
        }

        private ObservableCollection<InferenceProfileModel> InferenceProfiles { get; set; }

        #region Commands

        public RelayCommand ShowProfilingCommand { get; }

        public RelayCommand ShowInferencingCommand { get; }

        private void ShowProfiling()
        {
            UpdateInferenceProfiles();
            ProfilingActions.SetProfiles(InferenceProfiles);
            ProfilingEnabled = true;
        }

        private void ShowInferencing()
        {
            UpdateInferenceProfiles();
            ProfilingActions.SetProfiles(InferenceProfiles);
            ProfilingEnabled = false;
        }

        #endregion

        #region Properties

        private bool _profilingEnabled;
        public bool ProfilingEnabled
        {
            get => _profilingEnabled;
            set
            {
                _profilingEnabled = value;
                OnPropertyChanged(nameof(ProfilingEnabled));
            }
        }

        #endregion

        #region State change

        private void UpdateInferenceProfiles()
        {
            var profiles = _profileRepository.GetProfiles();
            if (!profiles.IsPresent)
            {
                return;
            }

            InferenceProfiles = new ObservableCollection<InferenceProfileModel>(
                profiles.Value.Select(p => new InferenceProfileModel
                {
                    ProfileName = p.ProfileName,
                    Description = p.Description,
                    Rules = p.Rules,
                    Variables = p.Variables
                }));
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}