using System.Collections.Generic;
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

        private List<string> _rules;
        public List<string> Rules
        {
            get => _rules;
            set
            {
                _rules = value;
                OnPropertyChanged(nameof(Rules));
            }
        }

        private List<string> _variables;
        public List<string> Variables
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
