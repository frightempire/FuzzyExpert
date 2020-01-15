using System.Windows;
using FuzzyExpert.WpfClient.DependencyInjection;
using SimpleInjector;

namespace FuzzyExpert.WpfClient
{
    public partial class App : System.Windows.Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Current.DispatcherUnhandledException += OnDispatcherUnhandledException;

            SimpleInjectorContainerFactory containerFactory = new SimpleInjectorContainerFactory();
            Container container = containerFactory.CreateSimpleInjectorContainer();
            SimpleInjectorResolver resolver = new SimpleInjectorResolver(container);

            var startUpWindow = (FuzzyExpertActions) resolver.Resolve(typeof(FuzzyExpertActions));
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