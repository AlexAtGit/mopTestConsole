using Autofac;
using mop.BaseCalculator.Interfaces;

namespace mop.ConsoleApp
{
    public class ContainerConfig
    {
        public static IContainer BuildCalculator(string operation, bool isChecked)
        {
            var builder = new ContainerBuilder();

            switch (operation)
            {
                // Case of the add operation
                case "add": builder.RegisterGeneric(isChecked ? typeof(Calculator.AddCalculatorChecked<>) : typeof(Calculator.AddCalculator<>)).As(typeof(ICalculator<>)); break;

                // unsuported operation
                default: return null;
            }

            return builder.Build();
        }
    }
}
