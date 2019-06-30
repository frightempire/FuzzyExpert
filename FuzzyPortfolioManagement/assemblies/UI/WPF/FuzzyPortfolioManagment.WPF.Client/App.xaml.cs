using System.Windows;
using FuzzyPortfolioManagment.WPF.Client.DependencyInjection;
using ProductionRuleSelectorAction.Panels;
using SimpleInjector;

namespace FuzzyPortfolioManagment.WPF.Client
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Current.DispatcherUnhandledException += OnDispatcherUnhandledException;

            SimpleInjectorContainerFactory containerFactory = new SimpleInjectorContainerFactory();
            Container container = containerFactory.CreateSimpleInjectorContainer();
            SimpleInjectorResolver resolver = new SimpleInjectorResolver(container);

            var startUpWindow = (ImplicationRuleSelectorAction) resolver.Resolve(typeof(ImplicationRuleSelectorAction));
            startUpWindow.Show();
        }

        void OnDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            string errorMessage = $"An unhandled exception occurred: {e.Exception.Message}";
            MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true;
            Current.Shutdown();
        }
    }
}
