using System;
using SimpleInjector;

namespace FuzzyExpert.ConsoleClient.DependencyInjection
{
    public class SimpleInjectorResolver
    {
        private readonly Container _simpleInjectorContainer;

        public SimpleInjectorResolver(Container simpleInjectorContainer)
        {
            _simpleInjectorContainer = simpleInjectorContainer ?? throw new ArgumentNullException(nameof(simpleInjectorContainer));
        }

        public object Resolve(Type itemType)
        {
            try
            {
                return _simpleInjectorContainer.GetInstance(itemType);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void Dispose()
        {
            _simpleInjectorContainer.Dispose();
        }
    }
}