using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using CommonLogic;
using CommonLogic.Interfaces;
using KnowledgeManager.Interfaces;
using ProductionRuleParser.Entities;
using ProductionRuleSelectorAction.Annotations;
using UILogic.Common.Interfaces;

namespace ProductionRuleSelectorAction.ViewModels
{
    public class ImplicationRuleSelectorActionModel: INotifyPropertyChanged
    {
        private readonly IFileDialogInteractor _fileDialogInteractor;
        private readonly IImplicationRuleFilePathProvider _filePathProvider;
        private readonly IImplicationRuleManager _implicationRuleManager;

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

        public ImplicationRuleSelectorActionModel(
            IFileDialogInteractor fileDialogInteractor,
            IImplicationRuleFilePathProvider filePathProvider,
            IImplicationRuleManager implicationRuleManager)
        {
            ExceptionAssert.IsNull(fileDialogInteractor);
            ExceptionAssert.IsNull(filePathProvider);
            ExceptionAssert.IsNull(implicationRuleManager);

            _fileDialogInteractor = fileDialogInteractor;
            _filePathProvider = filePathProvider;
            _implicationRuleManager = implicationRuleManager;

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
                           _filePathProvider.FilePath = FilePath;

                           List<ImplicationRule> implicationRules = _implicationRuleManager.ImplicationRules.Value.Values.ToList();
                           ImplicationRules.Clear();
                           implicationRules.ForEach(ir => ImplicationRules.Add(ir.ToString()));
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
