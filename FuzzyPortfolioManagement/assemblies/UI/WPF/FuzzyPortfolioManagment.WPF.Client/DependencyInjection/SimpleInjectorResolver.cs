using System;
using CommonLogic;
using SimpleInjector;

namespace FuzzyPortfolioManagment.WPF.Client.DependencyInjection
{
    public class SimpleInjectorResolver
    {
        private readonly Container _simpleInjectorContainer;

        public SimpleInjectorResolver(Container simpleInjectorContainer)
        {
            ExceptionAssert.IsNull(simpleInjectorContainer);

            _simpleInjectorContainer = simpleInjectorContainer;
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
