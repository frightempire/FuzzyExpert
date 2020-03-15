using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using FuzzyExpert.Infrastructure.ProfileManaging.Interfaces;
using FuzzyExpert.Profiling.Properties;
using FuzzyExpert.Profiling.Views;

namespace FuzzyExpert.Profiling.ViewModels
{
    public class ProfilingActionsModel : INotifyPropertyChanged
    {
        private readonly AddProfileAction _addProfileAction;
        private readonly IProfileRepository _profileRepository;

        public ObservableCollection<string> Profiles { get; set; }

        public ProfilingActionsModel(
            AddProfileAction addProfileAction,
            IProfileRepository profileRepository)
        {
            _addProfileAction = addProfileAction ?? throw new ArgumentNullException(nameof(addProfileAction));
            _profileRepository = profileRepository ?? throw new ArgumentNullException(nameof(profileRepository));

            InitializeBindingProperties();
            InitializeCollectionValues();
        }

        private void InitializeBindingProperties()
        {
            RemoveButtonEnable = false;
            Profiles = new ObservableCollection<string>();
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
                Profiles.Add(profile.ProfileName);
            }
        }

        private void ResetBindingProperties()
        {
            RemoveButtonEnable = false;
            Profiles.Clear();
        }

        private bool _removeButtonEnable;
        public bool RemoveButtonEnable
        {
            get => _removeButtonEnable;
            set
            {
                _removeButtonEnable = value;
                OnPropertyChanged(nameof(RemoveButtonEnable));
            }
        }

        private string _selectedProfile;
        public string SelectedProfile
        {
            get => _selectedProfile;
            set
            {
                if (value == _selectedProfile)
                {
                    return;
                }

                _selectedProfile = value;
                RemoveButtonEnable = true;
                OnPropertyChanged(nameof(SelectedProfile));
            }
        }

        private RelayCommand _addProfileCommand;
        public RelayCommand AddProfileCommand
        {
            get
            {
                return _addProfileCommand ??
                       (_addProfileCommand = new RelayCommand(obj =>
                       {
                           _addProfileAction.ShowDialog();
                           RefreshProfiles();
                       }));
            }
        }

        private RelayCommand _removeProfileCommand;
        public RelayCommand RemoveProfileCommand
        {
            get
            {
                return _removeProfileCommand ??
                       (_removeProfileCommand = new RelayCommand(obj =>
                           {
                               _profileRepository.DeleteProfile(SelectedProfile);
                               Profiles.Remove(SelectedProfile);
                               RemoveButtonEnable = false;
                           }));
            }
        }

        private void RefreshProfiles()
        {
            ResetBindingProperties();
            var profiles = _profileRepository.GetProfiles();
            if (!profiles.IsPresent)
            {
                return;
            }

            foreach (var profile in profiles.Value)
            {
                Profiles.Add(profile.ProfileName);
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