using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using FuzzyExpert.Application.Contracts;
using FuzzyExpert.Application.InferenceExpert.Entities;
using FuzzyExpert.Application.InferenceExpert.Interfaces;
using FuzzyExpert.Core.Entities;
using FuzzyExpert.ImplicationRuleSelectorAction.Panels;
using FuzzyExpert.ImplicationRuleSelectorAction.Properties;
using FuzzyExpert.Infrastructure.ResultLogging.Interfaces;

namespace FuzzyExpert.ImplicationRuleSelectorAction.ViewModels
{
    public class InferenceActionModel : INotifyPropertyChanged
    {
        private readonly DataSelectorAction _dataSelectorAction;
        private readonly IExpert _expert;
        private readonly IKnowledgeBaseManager _knowledgeBaseManager;
        private readonly IInferenceResultLogger _inferenceResultLogger;

        public ObservableCollection<string> ImplicationRules { get; set; }

        private ExpertOpinion ExpertOpinion { get; set; }

        public InferenceActionModel(
            DataSelectorAction dataSelectorAction,
            IExpert expert,
            IKnowledgeBaseManager knowledgeBaseManager,
            IInferenceResultLogger inferenceResultLogger)
        {
            _dataSelectorAction = dataSelectorAction ?? throw new ArgumentNullException(nameof(dataSelectorAction));
            _expert = expert ?? throw new ArgumentNullException(nameof(expert));
            _knowledgeBaseManager = knowledgeBaseManager ?? throw new ArgumentNullException(nameof(knowledgeBaseManager));
            _inferenceResultLogger = inferenceResultLogger ?? throw new ArgumentNullException(nameof(inferenceResultLogger));

            InitializeBindingProperties();
        }

        private void InitializeBindingProperties()
        {
            StartInferenceButtonEnable = "False";
            OpenResultFileButtonEnable = "False";
            ImplicationRules = new ObservableCollection<string>();
            ExpertOpinion = new ExpertOpinion();
        }

        private void ResetBindingProperties()
        {
            StartInferenceButtonEnable = "False";
            OpenResultFileButtonEnable = "False";
            ImplicationRules.Clear();
            ExpertOpinion = new ExpertOpinion();
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
                           ExpertOpinion = _expert.GetResult();
                           if (ExpertOpinion.IsSuccess)
                           {
                               OpenResultFileButtonEnable = "True";
                           }
                       }));
            }
        }

        private RelayCommand _openResultFileCommand;
        public RelayCommand OpenResultFileCommand
        {
            get
            {
                return _openResultFileCommand ??
                       (_openResultFileCommand = new RelayCommand(obj =>
                       {
                           File.Delete(_inferenceResultLogger.LogPath);
                           _inferenceResultLogger.LogImplicationRules(_knowledgeBaseManager.GetKnowledgeBase().Value.ImplicationRules);
                           if (ExpertOpinion.IsSuccess)
                           {
                               _inferenceResultLogger.LogInferenceResult(ExpertOpinion.Result);
                           }
                           else
                           {
                               _inferenceResultLogger.LogInferenceErrors(ExpertOpinion.ErrorMessages);
                           }
                           Process.Start(_inferenceResultLogger.LogPath);
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

        private string _openResultFileButtonEnable;
        public string OpenResultFileButtonEnable
        {
            get => _openResultFileButtonEnable;
            set
            {
                _openResultFileButtonEnable = value;
                OnPropertyChanged(nameof(OpenResultFileButtonEnable));
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