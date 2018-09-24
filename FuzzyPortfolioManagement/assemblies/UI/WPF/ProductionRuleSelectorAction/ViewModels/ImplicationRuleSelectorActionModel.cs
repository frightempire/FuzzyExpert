using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CommonLogic;
using CommonLogic.Interfaces;
using ProductionRuleSelectorAction.Annotations;
using UILogic.Common.Interfaces;

namespace ProductionRuleSelectorAction.ViewModels
{
    public class ImplicationRuleSelectorActionModel: INotifyPropertyChanged
    {
        private readonly IFileDialogInteractor _fileDialogInteractor;
        private readonly IFileReader _fileReader;

        private string _filePath;
        public string FilePath
        {
            get => _filePath;
            set
            {
                _filePath = value;
                OnPropertyChanged(nameof(FilePath));
            }
        }

        public ObservableCollection<string> ImplicationRules { get; set; }

        public ImplicationRuleSelectorActionModel(IFileDialogInteractor fileDialogInteractor, IFileReader fileReader)
        {
            ExceptionAssert.IsNull(fileDialogInteractor);
            ExceptionAssert.IsNull(fileReader);

            _fileDialogInteractor = fileDialogInteractor;
            _fileReader = fileReader;

            InitializeBindingProperties();
        }

        private void InitializeBindingProperties()
        {
            ImplicationRules = new ObservableCollection<string>();
            FilePath = string.Empty;
        }

        private RelayCommand _openCommand;
        public RelayCommand OpenCommand
        {
            get
            {
                return _openCommand ??
                       (_openCommand = new RelayCommand(obj =>
                       {
                           if (!_fileDialogInteractor.OpenFileDialog()) return;

                           FilePath = _fileDialogInteractor.FilePath;

                           var implicationRules = _fileReader.ReadFileByLines(FilePath);
                           ImplicationRules.Clear();
                           implicationRules.ForEach(ir => ImplicationRules.Add(ir));
                       }));
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
