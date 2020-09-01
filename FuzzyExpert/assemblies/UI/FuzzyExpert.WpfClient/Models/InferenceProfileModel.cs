using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using FuzzyExpert.WpfClient.Annotations;

namespace FuzzyExpert.WpfClient.Models
{
    public class InferenceProfileModel : INotifyPropertyChanged
    {
        private string _profileName;
        public string ProfileName
        {
            get => _profileName;
            set
            {
                _profileName = value;
                OnPropertyChanged(nameof(ProfileName));
            }
        }

        private string _userName;
        public string UserName
        {
            get => _userName;
            set
            {
                _userName = value;
                OnPropertyChanged(nameof(UserName));
            }
        }

        private string _description;
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        private ObservableCollection<ContentModel> _rules;
        public ObservableCollection<ContentModel> Rules
        {
            get => _rules;
            set
            {
                _rules = value;
                OnPropertyChanged(nameof(Rules));
            }
        }

        private ObservableCollection<ContentModel> _variables;
        public ObservableCollection<ContentModel> Variables
        {
            get => _variables;
            set
            {
                _variables = value;
                OnPropertyChanged(nameof(Variables));
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
