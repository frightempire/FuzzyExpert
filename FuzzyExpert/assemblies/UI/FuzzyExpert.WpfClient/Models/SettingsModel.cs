using System.ComponentModel;
using System.Runtime.CompilerServices;
using FuzzyExpert.WpfClient.Annotations;

namespace FuzzyExpert.WpfClient.Models
{
    public class SettingsModel : INotifyPropertyChanged
    {
        private int _id;
        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged(nameof(Id));
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

        private double _confidenceFactorLowerBoundary;
        public double ConfidenceFactorLowerBoundary
        {
            get => _confidenceFactorLowerBoundary;
            set
            {
                _confidenceFactorLowerBoundary = value;
                OnPropertyChanged(nameof(ConfidenceFactorLowerBoundary));
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