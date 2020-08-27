using System;
using System.Windows;
using System.Windows.Controls;

namespace FuzzyExpert.WpfClient.Views
{
    public partial class FuzzyExpertActions : Window
    {
        private readonly LoginActions _loginActions;
        private readonly ProfilingActions _profilingActions;
        private readonly InferencingActions _inferencingActions;

        public FuzzyExpertActions(
            ProfilingActions profilingActions,
            InferencingActions inferencingActions,
            LoginActions loginActions)
        {
            _loginActions = loginActions ?? throw new ArgumentNullException(nameof(loginActions));
            _profilingActions = profilingActions ?? throw new ArgumentNullException(nameof(profilingActions));
            _inferencingActions = inferencingActions ?? throw new ArgumentNullException(nameof(inferencingActions));

            InitializeComponent();
            InitializeHeaderState();

            _loginActions.LoggedIn += OnLoggedIn;
        }

        private void InitializeHeaderState()
        {
            ProfilingButton.IsEnabled = false;
            InferencingButton.IsEnabled = false;
            LogoutButton.Visibility = Visibility.Hidden;
            LoginButton.Visibility = Visibility.Visible;
        }

        private void OnLoggedIn(object sender, EventArgs e)
        {
            ProfilingButton.IsEnabled = true;
            InferencingButton.IsEnabled = true;
            LogoutButton.Visibility = Visibility.Visible;
            LoginButton.Visibility = Visibility.Hidden;
        }

        private void LoadAction(object sender, RoutedEventArgs e)
        {
            var origin = (Button) sender;
            ContentArea.Children.Clear();
            switch (origin.Name)
            {
                case "ProfilingButton":
                    //var res = _loginActions.LoggedInUserName;
                    _profilingActions.RefreshProfiles();
                    ContentArea.Children.Add(_profilingActions);
                    break;
                case "InferencingButton":
                    _inferencingActions.RefreshProfiles();
                    ContentArea.Children.Add(_inferencingActions);
                    break;
                case "LoginButton":
                    ContentArea.Children.Add(_loginActions);
                    break;
                case "LogoutButton":
                    InitializeHeaderState();
                    _loginActions.InitializeState();
                    ContentArea.Children.Add(_loginActions);
                    break;
            }
        }
    }
}