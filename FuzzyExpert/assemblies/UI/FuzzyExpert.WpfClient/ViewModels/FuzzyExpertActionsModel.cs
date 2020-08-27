using System.ComponentModel;
using System.Runtime.CompilerServices;
using FuzzyExpert.WpfClient.Annotations;

namespace FuzzyExpert.WpfClient.ViewModels
{
    public class FuzzyExpertActionsModel : INotifyPropertyChanged
    {
        public FuzzyExpertActionsModel()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}