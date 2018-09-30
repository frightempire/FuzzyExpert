using System.Windows;
using FuzzyPortfolioManagment.Client.DependencyInjection;
using ProductionRuleSelectorAction.Panels;
using SimpleInjector;

namespace FuzzyPortfolioManagment.Client
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            SimpleInjectorContainerFactory containerFactory = new SimpleInjectorContainerFactory();
            Container container = containerFactory.CreateSimpleInjectorContainer();
            SimpleInjectorResolver resolver = new SimpleInjectorResolver(container);

            var startUpWindow = (ImplicationRuleSelectorAction) resolver.Resolve(typeof(ImplicationRuleSelectorAction));
            startUpWindow.Show();
        }
    }
}
