using System;
using System.Windows.Controls;
using System.Windows.Input;
using FuzzyExpert.Application.Common.Interfaces;
using FuzzyExpert.Infrastructure.KnowledgeManager.Interfaces;
using FuzzyExpert.Infrastructure.LinguisticVariableParsing.Interfaces;
using FuzzyExpert.Infrastructure.ProductionRuleParsing.Interfaces;
using FuzzyExpert.Infrastructure.ProfileManaging.Interfaces;
using FuzzyExpert.Profiling.ViewModels;

namespace FuzzyExpert.Profiling.Views
{
    public partial class ProfilingActions : UserControl
    {
        private readonly IProfileRepository _profileRepository;
        private readonly IImplicationRuleValidator _ruleValidator;
        private readonly ILinguisticVariableValidator _variableValidator;
        private readonly IImplicationRuleCreator _ruleCreator;
        private readonly ILinguisticVariableCreator _variableCreator;
        private readonly IKnowledgeBaseValidator _knowledgeValidator;
        private readonly IFileOperations _fileOperations;

        public ProfilingActions(
            ProfilingActionsModel model,
            IProfileRepository profileRepository,
            IImplicationRuleValidator ruleValidator,
            ILinguisticVariableValidator variableValidator,
            IImplicationRuleCreator ruleCreator,
            ILinguisticVariableCreator variableCreator,
            IKnowledgeBaseValidator knowledgeValidator,
            IFileOperations fileOperations)
        {
            InitializeComponent();
            DataContext = model ?? throw new ArgumentNullException(nameof(model));

            _profileRepository = profileRepository ?? throw new ArgumentNullException(nameof(profileRepository));
            _ruleValidator = ruleValidator ?? throw new ArgumentNullException(nameof(ruleValidator));
            _variableValidator = variableValidator ?? throw new ArgumentNullException(nameof(variableValidator));
            _ruleCreator = ruleCreator ?? throw new ArgumentNullException(nameof(ruleCreator));
            _variableCreator = variableCreator ?? throw new ArgumentNullException(nameof(variableCreator));
            _knowledgeValidator = knowledgeValidator ?? throw new ArgumentNullException(nameof(knowledgeValidator));
            _fileOperations = fileOperations ?? throw new ArgumentNullException(nameof(fileOperations));
        }

        // figure out how to move it to ViewModel
        // issue : no command binding for double click event
        private void OnProfileDoubleClicked(object sender, MouseButtonEventArgs e)
        {
            var action = new AddImplicationRuleAction(new AddImplicationRuleActionModel(
                ((ProfilingActionsModel)DataContext).SelectedProfile,
                _profileRepository,
                _ruleValidator,
                _variableValidator,
                _ruleCreator,
                _variableCreator,
                _knowledgeValidator,
                _fileOperations));

            action.ShowDialog();
        }
    }
}