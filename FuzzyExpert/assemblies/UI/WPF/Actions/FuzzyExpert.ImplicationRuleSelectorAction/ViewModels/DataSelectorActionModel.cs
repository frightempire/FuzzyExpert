using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using FuzzyExpert.CommonUILogic.Implementations;
using FuzzyExpert.ImplicationRuleSelectorAction.Properties;
using FuzzyExpert.Infrastructure.InitialDataProviding.Interfaces;
using FuzzyExpert.Infrastructure.KnowledgeManager.Interfaces;

namespace FuzzyExpert.ImplicationRuleSelectorAction.ViewModels
{
    public class DataSelectorActionModel : INotifyPropertyChanged
    {
        private readonly IDataFilePathProvider _dataFilePathProvider;

        public DataSelectorActionModel(
            IDataFilePathProvider dataFilePathProvider)
        {
            _dataFilePathProvider = dataFilePathProvider ?? throw new ArgumentNullException(nameof(dataFilePathProvider));

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