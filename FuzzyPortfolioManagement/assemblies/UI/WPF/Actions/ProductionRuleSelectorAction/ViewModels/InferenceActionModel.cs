using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CommonLogic;
using InferenceExpert.Entities;
using InferenceExpert.Interfaces;
using KnowledgeManager.Interfaces;
using ProductionRuleParser.Entities;
using ProductionRuleSelectorAction.Annotations;
using ProductionRuleSelectorAction.Panels;

namespace ProductionRuleSelectorAction.ViewModels
{
    public class InferenceActionModel : INotifyPropertyChanged
    {
        private readonly DataSelectorAction _dataSelectorAction;
        private readonly IExpert _expert;
        private readonly IKnowledgeBaseManager _knowledgeBaseManager;

        public ObservableCollection<string> ImplicationRules { get; set; }

        public ObservableCollection<string> ExpertResult { get; set; }

        public InferenceActionModel(
            DataSelectorAction dataSelectorAction,
            IExpert expert,
            IKnowledgeBaseManager knowledgeBaseManager)
        {
            ExceptionAssert.IsNull(dataSelectorAction);
            ExceptionAssert.IsNull(expert);
            ExceptionAssert.IsNull(knowledgeBaseManager);

            _dataSelectorAction = dataSelectorAction;
            _expert = expert;
            _knowledgeBaseManager = knowledgeBaseManager;

            InitializeBindingProperties();
        }

        private void InitializeBindingProperties()
        {
            StartInferenceButtonEnable = "False";
            ImplicationRules = new ObservableCollection<string>();
            ExpertResult = new ObservableCollection<string>();
        }

        private void ResetBindingProperties()
        {
            StartInferenceButtonEnable = "False";
            ImplicationRules.Clear();
            ExpertResult.Clear();
        }

        private RelayCommand _getDataCommand;
        public RelayCommand GetDataCommand
        {
            get
            {
                return _getDataCommand ??
                       (_getDataCommand = new RelayCommand(obj =>
                       {
                           ResetBindingProperties();
                           if (_dataSelectorAction.ShowDialog() != false) return;

                           Dictionary<int, ImplicationRule> implicationRules = _knowledgeBaseManager.GetKnowledgeBase().Value.ImplicationRules;
                           ImplicationRules.Clear();
                           foreach (var rule in implicationRules)
                           {
                               ImplicationRules.Add(rule.Value.ToString());
                           }
                           StartInferenceButtonEnable = "True";
                       }));
            }
        }

        private RelayCommand _startInferenceCommand;
        public RelayCommand StartInferenceCommand
        {
            get
            {
                return _startInferenceCommand ??
                       (_startInferenceCommand = new RelayCommand(obj =>
                       {
                           ExpertOpinion expertOpinion = _expert.GetResult();
                           ExpertResult.Clear();
                           if (expertOpinion.IsSuccess)
                           {
                               foreach (var result in expertOpinion.Result)
                               {
                                   ExpertResult.Add(result.Key);
                               }
                           }
                           else
                           {
                               foreach (var error in expertOpinion.ErrorMessages)
                               {
                                   ExpertResult.Add(error);
                               }
                           }
                       }));
            }
        }

        private string _startInferenceButtonEnable;
        public string StartInferenceButtonEnable
        {
            get => _startInferenceButtonEnable;
            set
            {
                _startInferenceButtonEnable = value;
                OnPropertyChanged(nameof(StartInferenceButtonEnable));
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
