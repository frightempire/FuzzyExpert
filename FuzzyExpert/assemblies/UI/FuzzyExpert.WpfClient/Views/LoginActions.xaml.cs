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
            if (model.Login())
            {
                LoggedIn?.Invoke(this, new EventArgs());
            }
        }

        private void Register(object sender, RoutedEventArgs e)
        {
            var model = (LoginActionsModel)DataContext;
            if (model.Register())
            {
                LoggedIn?.Invoke(this, new EventArgs());
            }
        }

        public void InitializeState() => ((LoginActionsModel)DataContext).InitializeState();

        public string LoggedInUserName => ((LoginActionsModel)DataContext).User.UserName;
    }
}