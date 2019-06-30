using System.ComponentModel;
using System.Runtime.CompilerServices;
using CommonLogic;
using DataProvider.Interfaces;
using KnowledgeManager.Interfaces;
using ProductionRuleSelectorAction.Annotations;
using UILogic.Common.Implementations;

namespace ProductionRuleSelectorAction.ViewModels
{
    public class DataSelectorActionModel : INotifyPropertyChanged
    {
        private readonly IImplicationRuleFilePathProvider _implicationRuleFilePathProvider;
        private readonly ILinguisticVariableFilePathProvider _linguisticVariableFilePathProvider;
        private readonly IDataFilePathProvider _dataFilePathProvider;

        public DataSelectorActionModel(
            IImplicationRuleFilePathProvider implicationRuleFilePathProvider,
            ILinguisticVariableFilePathProvider linguisticVariableFilePathProvider,
            IDataFilePathProvider dataFilePathProvider)
        {
            ExceptionAssert.IsNull(implicationRuleFilePathProvider);
            ExceptionAssert.IsNull(linguisticVariableFilePathProvider);
            ExceptionAssert.IsNull(dataFilePathProvider);

            _implicationRuleFilePathProvider = implicationRuleFilePathProvider;
            _linguisticVariableFilePathProvider = linguisticVariableFilePathProvider;
            _dataFilePathProvider = dataFilePathProvider;

            InitializeBindingProperties();
        }

        private void InitializeBindingProperties()
        {
            CloseButtonEnable = "False";
        }

        private string _initialDataFilePath;
        public string InitialDataFilePath
        {
            get => _initialDataFilePath;
            set
            {
                _initialDataFilePath = value;
                OnPropertyChanged(nameof(InitialDataFilePath));
            }
        }

        private string _linguisticVariableFilePath;
        public string LinguisticVariableFilePath
        {
            get => _linguisticVariableFilePath;
            set
            {
                _linguisticVariableFilePath = value;
                OnPropertyChanged(nameof(LinguisticVariableFilePath));
            }
        }

        private string _implicationRuleFilePath;
        public string ImplicationRuleFilePath
        {
            get => _implicationRuleFilePath;
            set
            {
                _implicationRuleFilePath = value;
                OnPropertyChanged(nameof(ImplicationRuleFilePath));
            }
        }

        private RelayCommand _getDataCommand;
        public RelayCommand GetDataCommand
        {
            get
            {
                return _getDataCommand ??
                       (_getDataCommand = new RelayCommand(obj =>
                       {
                           var dialog = new FileDialogInteractor();
                           if (!dialog.OpenFileDialog()) return;
                           _dataFilePathProvider.FilePath = dialog.FilePath;
                           InitialDataFilePath = dialog.FilePath;
                           UpdateCloseButtonStatus();
                       }));
            }
        }

        private RelayCommand _getRulesCommand;
        public RelayCommand GetRulesCommand
        {
            get
            {
                return _getRulesCommand ??
                       (_getRulesCommand = new RelayCommand(obj =>
                       {
                           var dialog = new FileDialogInteractor();
                           if (!dialog.OpenFileDialog()) return;
                           _implicationRuleFilePathProvider.FilePath = dialog.FilePath;
                           ImplicationRuleFilePath = dialog.FilePath;
                           UpdateCloseButtonStatus();
                       }));
            }
        }

        private RelayCommand _getVariablesCommand;
        public RelayCommand GetVariablesCommand
        {
            get
            {
                return _getVariablesCommand ??
                       (_getVariablesCommand = new RelayCommand(obj =>
                       {
                           var dialog = new FileDialogInteractor();
                           if (!dialog.OpenFileDialog()) return;
                           _linguisticVariableFilePathProvider.FilePath = dialog.FilePath;
                           LinguisticVariableFilePath = dialog.FilePath;
                           UpdateCloseButtonStatus();
                       }));
            }
        }

        private void UpdateCloseButtonStatus()
        {
            if (!string.IsNullOrEmpty(InitialDataFilePath) &&
                !string.IsNullOrEmpty(ImplicationRuleFilePath) &&
                !string.IsNullOrEmpty(LinguisticVariableFilePath))
            {
                CloseButtonEnable = "True";
            }
        }

        private string _closeButtonEnable;
        public string CloseButtonEnable
        {
            get => _closeButtonEnable;
            set
            {
                _closeButtonEnable = value;
                OnPropertyChanged(nameof(CloseButtonEnable));
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