using System.Windows;
using FuzzyExpert.WpfClient.DependencyInjection;
using FuzzyExpert.WpfClient.Views;

namespace FuzzyExpert.WpfClient
{
    public partial class App : System.Windows.Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Current.DispatcherUnhandledException += OnDispatcherUnhandledException;

            var containerFactory = new SimpleInjectorContainerFactory();
            var container = containerFactory.CreateSimpleInjectorContainer();
            var resolver = new SimpleInjectorResolver(container);

            var startUpWindow = (FuzzyExpertActions) resolver.Resolve(typeof(FuzzyExpertActions));
            startUpWindow.Show();
        }

        void OnDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            var errorMessage = $"An unhandled exception occurred: {e.Exception.Message}";
            MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true;
            Current.Shutdown();
        }
    }
}