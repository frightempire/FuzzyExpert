using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using FuzzyExpert.Infrastructure.ProfileManaging.Entities;
using FuzzyExpert.Infrastructure.ProfileManaging.Interfaces;
using FuzzyExpert.Profiling.Properties;

namespace FuzzyExpert.Profiling.ViewModels
{
    public class AddProfileActionModel : INotifyPropertyChanged
    {
        private readonly IProfileRepository _profileRepository;

        public AddProfileActionModel(IProfileRepository profileRepository)
        {
            _profileRepository = profileRepository ?? throw new ArgumentNullException(nameof(profileRepository));

            InitializeBindingProperties();
        }

        public Action CloseAction { get; set; }

        private void InitializeBindingProperties()
        {
            ProfileName = string.Empty;
            ProfileDescription = string.Empty;
            AddButtonEnable = false;
        }

        private bool _addButtonEnable;
        public bool AddButtonEnable
        {
            get => _addButtonEnable;
            set
            {
                _addButtonEnable = value;
                OnPropertyChanged(nameof(AddButtonEnable));
            }
        }

        private string _profileName;
        public string ProfileName
        {
            get => _profileName;
            set
            {
                _profileName = value;
                UpdateAddButtonStatus();
                OnPropertyChanged(nameof(ProfileName));
            }
        }

        private string _profileDescription;
        public string ProfileDescription
        {
            get => _profileDescription;
            set
            {
                _profileDescription = value;
                UpdateAddButtonStatus();
                OnPropertyChanged(nameof(ProfileDescription));
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
                           _profileRepository.SaveProfile(new InferenceProfile
                           {
                               ProfileName = ProfileName,
                               Description = ProfileDescription
                           });
                           InitializeBindingProperties();
                           CloseAction();
                       }));
            }
        }

        private void UpdateAddButtonStatus()
        {
            if (string.IsNullOrEmpty(ProfileName) || string.IsNullOrEmpty(ProfileDescription))
            {
                AddButtonEnable = false;
            }
            else
            {
                AddButtonEnable = true;
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