using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using FuzzyExpert.Infrastructure.DatabaseManagement.Entities;
using FuzzyExpert.Infrastructure.DatabaseManagement.Interfaces;
using FuzzyExpert.WpfClient.Annotations;
using FuzzyExpert.WpfClient.Models;

namespace FuzzyExpert.WpfClient.ViewModels
{
    public class LoginActionsModel : INotifyPropertyChanged
    {
        private readonly IUserRepository _userRepository;

        public LoginActionsModel(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            InitializeState();
        }

        public void InitializeState()
        {
            User = new UserModel();
            ValidationMessage = string.Empty;
        }

        private UserModel _user;
        public UserModel User
        {
            get => _user;
            set
            {
                _user = value;
                OnPropertyChanged(nameof(User));
            }
        }

        private string _validationMessage;
        public string ValidationMessage
        {
            get => _validationMessage;
            set
            {
                _validationMessage = value;
                OnPropertyChanged(nameof(ValidationMessage));
            }
        }
        
        public bool Login()
        {
            var user = _userRepository.GetUserByName(User.UserName);
            if (user.IsPresent && user.Value.Password == User.Password)
            {
                ValidationMessage = "Log in successful";
                return true;
            }

            ValidationMessage = "User name or password are incorrect";
            return false;
        }

        public bool Register()
        {
            var user = _userRepository.GetUserByName(User.UserName);
            if (user.IsPresent)
            {
                ValidationMessage = "Such user already exists";
                return false;
            }

            var saveResult = _userRepository.SaveUser(
                new User
                {
                    UserName = User.UserName,
                    Password = User.Password
                });

            if (saveResult)
            {
                ValidationMessage = "Registration and log in successful";
                return true;
            }

            ValidationMessage = "Registration failed";
            return false;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}