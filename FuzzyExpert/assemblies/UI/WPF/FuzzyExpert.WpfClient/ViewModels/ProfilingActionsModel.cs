using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using FuzzyExpert.Infrastructure.ProfileManaging.Entities;
using FuzzyExpert.Infrastructure.ProfileManaging.Interfaces;
using FuzzyExpert.WpfClient.Annotations;
using FuzzyExpert.WpfClient.Models;

namespace FuzzyExpert.WpfClient.ViewModels
{
    public class ProfilingActionsModel : INotifyPropertyChanged
    {
        private readonly IProfileRepository _profileRepository;

        public ProfilingActionsModel(IProfileRepository profileRepository)
        {
            _profileRepository = profileRepository ?? throw new ArgumentNullException(nameof(profileRepository));

            AddProfileCommand = new RelayCommand(obj => AddProfile());
            RemoveProfileCommand = new RelayCommand(obj => RemoveProfile());
            CloseCreateProfileCommand = new RelayCommand(obj => CloseCreateProfile(), obj => true);
            CreateProfileCommand = new RelayCommand(obj => CreateProfile(), obj => !string.IsNullOrEmpty(NewProfileName));
        }


        private ICollectionView _ruleView;
        private ICollectionView _variableView;
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
                RemoveButtonEnable = true;
                _ruleView = CollectionViewSource.GetDefaultView(SelectedProfile.Rules ?? new List<string>());
                _ruleView.Filter = o => string.IsNullOrEmpty(RuleFilter) || ((string)o).Contains(RuleFilter);
                _variableView = CollectionViewSource.GetDefaultView(SelectedProfile.Variables ?? new List<string>());
                _variableView.Filter = o => string.IsNullOrEmpty(VariableFilter) || ((string)o).Contains(VariableFilter);
                OnPropertyChanged(nameof(SelectedProfile));
            }
        }

        private string _ruleFilter;
        public string RuleFilter
        {
            get => _ruleFilter;
            set
            {
                if (value == _ruleFilter)
                {
                    return;
                }

                _ruleFilter = value;
                _ruleView.Refresh();
                OnPropertyChanged(nameof(RuleFilter));
            }
        }

        private string _variableFilter;
        public string VariableFilter
        {
            get => _variableFilter;
            set
            {
                if (value == _variableFilter)
                {
                    return;
                }

                _variableFilter = value;
                _variableView.Refresh();
                OnPropertyChanged(nameof(VariableFilter));
            }
        }

        #region Working with profiles

        public RelayCommand AddProfileCommand { get; }

        public RelayCommand RemoveProfileCommand { get; }

        public RelayCommand CloseCreateProfileCommand { get; }

        private void AddProfile()
        {
            NewProfileName = string.Empty;
            NewProfileDescription = string.Empty;
            CreateProfileVisible = true;
        }

        private void CloseCreateProfile()
        {
            CreateProfileVisible = false;
        }

        private void RemoveProfile()
        {
            _profileRepository.DeleteProfile(SelectedProfile.ProfileName);
            Profiles.Remove(SelectedProfile);
            RemoveButtonEnable = false;
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

        #endregion

        #region Creating new profile

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

        public RelayCommand CreateProfileCommand { get; }

        private void CreateProfile()
        {
            _profileRepository.SaveProfile(new InferenceProfile
            {
                ProfileName = NewProfileName,
                Description = NewProfileDescription
            });

            CreateProfileVisible = false;
            RefreshProfiles();
        }

        private void RefreshProfiles()
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
                    Rules = profile.Rules,
                    Variables = profile.Variables
                });
            }
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