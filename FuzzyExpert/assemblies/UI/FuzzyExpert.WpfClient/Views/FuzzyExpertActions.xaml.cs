using System;
using System.Windows;
using System.Windows.Controls;
using FuzzyExpert.WpfClient.Annotations;

namespace FuzzyExpert.WpfClient.Views
{
    public partial class FuzzyExpertActions : Window
    {
        private readonly SettingsActions _settingsActions;
        private readonly LoginActions _loginActions;
        private readonly ProfilingActions _profilingActions;
        private readonly InferencingActions _inferencingActions;

        public FuzzyExpertActions(
            [NotNull] SettingsActions settingsActions,
            ProfilingActions profilingActions,
            InferencingActions inferencingActions,
            LoginActions loginActions)
        {
            _settingsActions = settingsActions ?? throw new ArgumentNullException(nameof(settingsActions));
            _loginActions = loginActions ?? throw new ArgumentNullException(nameof(loginActions));
            _profilingActions = profilingActions ?? throw new ArgumentNullException(nameof(profilingActions));
            _inferencingActions = inferencingActions ?? throw new ArgumentNullException(nameof(inferencingActions));

            InitializeComponent();
            InitializeHeaderState();

            _loginActions.LoggedIn += OnLoggedIn;
        }

        private double BasicMinHeight = 130;
        private double BasicMinWidth = 1200;

        private void InitializeHeaderState()
        {
            SettingsButton.IsEnabled = false;
            ProfilingButton.IsEnabled = false;
            InferencingButton.IsEnabled = false;
            LogoutButton.Visibility = Visibility.Hidden;
            LoginButton.Visibility = Visibility.Visible;
        }

        private void OnLoggedIn(object sender, EventArgs e)
        {
            SettingsButton.IsEnabled = true;
            ProfilingButton.IsEnabled = true;
            InferencingButton.IsEnabled = true;
            LogoutButton.Visibility = Visibility.Visible;
            LoginButton.Visibility = Visibility.Hidden;
        }

        private void LoadAction(object sender, RoutedEventArgs e)
        {
            var origin = (Button) sender;
            ContentArea.Children.Clear();
            MinHeight = BasicMinHeight;
            MinWidth = BasicMinWidth;

            switch (origin.Name)
            {
                case "SettingsButton":
                    _settingsActions.InitializeState(_loginActions.LoggedInUserName);
                    UpdateMainWindowsSize(_settingsActions);
                    ContentArea.Children.Add(_settingsActions);
                    break;
                case "ProfilingButton":
                    _profilingActions.InitializeState(_loginActions.LoggedInUserName);
                    UpdateMainWindowsSize(_profilingActions);
                    ContentArea.Children.Add(_profilingActions);
                    break;
                case "InferencingButton":
                    _inferencingActions.InitializeState(_loginActions.LoggedInUserName);
                    UpdateMainWindowsSize(_inferencingActions);
                    ContentArea.Children.Add(_inferencingActions);
                    break;
                case "LoginButton":
                    UpdateMainWindowsSize(_loginActions);
                    ContentArea.Children.Add(_loginActions);
                    break;
                case "LogoutButton":
                    InitializeHeaderState();
                    _loginActions.InitializeState();
                    UpdateMainWindowsSize(_loginActions);
                    ContentArea.Children.Add(_loginActions);
                    break;
            }
        }

        private void UpdateMainWindowsSize(UserControl control)
        {
            MinHeight += control.MinHeight;
            Height = MinHeight;
            Width = MinWidth;
        }
    }
}