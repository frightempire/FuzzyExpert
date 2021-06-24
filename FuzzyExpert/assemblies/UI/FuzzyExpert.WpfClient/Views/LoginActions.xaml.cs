using System;
using System.Windows;
using System.Windows.Controls;
using FuzzyExpert.WpfClient.ViewModels;

namespace FuzzyExpert.WpfClient.Views
{
    public partial class LoginActions : UserControl
    {
        public LoginActions(LoginActionsModel model)
        {
            DataContext = model ?? throw new ArgumentNullException(nameof(model));
            InitializeComponent();
        }

        public event EventHandler LoggedIn;

        private void Login(object sender, RoutedEventArgs e)
        {
            var model = (LoginActionsModel) DataContext;
            if (!model.Login())
            {
                return;
            }

            LoggedIn?.Invoke(this, new EventArgs());
            Password.Clear();
        }

        private void Register(object sender, RoutedEventArgs e)
        {
            var model = (LoginActionsModel)DataContext;
            if (!model.Register())
            {
                return;
            }

            LoggedIn?.Invoke(this, new EventArgs());
            Password.Clear();
        }

        public void InitializeState() => ((LoginActionsModel)DataContext).InitializeState();

        public string LoggedInUserName => ((LoginActionsModel)DataContext).User.UserName;

        private void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext != null)
            {
                ((dynamic)DataContext).User.Password = ((PasswordBox)sender).Password;
            }
        }
    }
}