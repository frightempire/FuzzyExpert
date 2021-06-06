using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using FuzzyExpert.Infrastructure.DatabaseManagement.Entities;
using FuzzyExpert.Infrastructure.DatabaseManagement.Interfaces;
using FuzzyExpert.WpfClient.Annotations;
using FuzzyExpert.WpfClient.Models;

namespace FuzzyExpert.WpfClient.ViewModels
{
    public class SettingsActionsModel: INotifyPropertyChanged
    {
        private readonly ISettingsRepository _settingsRepository;
        private readonly IDefaultSettingsProvider _defaultSettingsProvider;

        public SettingsActionsModel(
            ISettingsRepository settingsRepository,
            IDefaultSettingsProvider defaultSettingsProvider)
        {
            _settingsRepository = settingsRepository ?? throw new ArgumentNullException(nameof(settingsRepository));
            _defaultSettingsProvider = defaultSettingsProvider ?? throw new ArgumentNullException(nameof(defaultSettingsProvider));

            SaveSettingsCommand = new RelayCommand(obj => SaveSettings(), obj => true);
        }

        public void InitializeState()
        {
            Settings = new SettingsModel();
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

        public RelayCommand SaveSettingsCommand { get; }

        private void SaveSettings()
        {
            _settingsRepository.SaveSettings(new Settings
            {
                Id = Settings.Id,
                UserName = Settings.UserName,
                ConfidenceFactorLowerBoundary = Settings.ConfidenceFactorLowerBoundary
            });
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

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}