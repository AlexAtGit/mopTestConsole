using System;
using Autofac;
using mop.BaseCalculator.Interfaces;

namespace mop.ConsoleApp
{
    /// <summary>
    /// Console app to test the calculator engine
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Check that we have the right number of arguments
                if (args.Length < 3 || args.Length > 4)
                {
                    Console.WriteLine("Usage: add number1 number2 <-c>, where");
                    Console.WriteLine("where -c is an optional parameters to carry out a checked calculation operation");
                    return;
                }

                // Get the various arguments
                var operation = args[0];
                var argument1 = args[1];
                var argument2 = args[2];
                var isChecked = (args.Length == 4 && args[3].Trim().Equals("-c", StringComparison.InvariantCultureIgnoreCase));
                   
                // Perform the specified calculation
                PerformCalculation(operation.ToLower().Trim(), argument1.Trim(), argument2.Trim(), isChecked);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Calculator Error: " + ex.Message);
            }
        }

        private static void PerformCalculation(string operation, string argument1, string argument2, bool isChecked)
        {
            // Get the actual calculator that would perform teh requested operation
            var container = ContainerConfig.BuildCalculator(operation, isChecked);
            if (container == null) throw new Exception($"Unsuported operation: {operation}");

            // Do the calculation and output the result

            // Try the case of perform a calculation on two integers
            if (TryCalculateInt(container, argument1, argument2)) return;

            // OK, the above did not work, now try the case of performing a calculation on two double values
            if (TryCalculateDouble(container, argument1, argument2)) return;
        }
        private static bool TryCalculateInt(IContainer container, string argument1, string argument2)
        {
            // Ensure that we have a valid first argument
            if (!int.TryParse(argument1, out var argValue1)) return false;

            // Ensure that we have a valid second argument
            if (int.TryParse(argument2, out var argValue2))
            {
                var calculator1 = container.Resolve<ICalculator<int>>();
                var result1 = calculator1.Calculate(argValue1, argValue2);
                Console.WriteLine($"{argValue1} + {argValue2} = {result1}");
                return true;
            }

            if (!double.TryParse(argument2, out var argValue3)) return false;

            var calculator2 = container.Resolve<ICalculator<double>>();
            var result2 = calculator2.Calculate(argValue1, argValue3);
            Console.WriteLine($"{argValue1:0.000} + {argValue3:0.0000} = {result2:0.0000}");
            return true;
        }
        private static bool TryCalculateDouble(IContainer container, string argument1, string argument2)
        {
            if (!double.TryParse(argument1, out var argValue1)) return false;

            if (!double.TryParse(argument2, out var argValue2)) return false;
            
            var calculator1 = container.Resolve<ICalculator<double>>();
            var result1 = calculator1.Calculate(argValue1, argValue2);
            Console.WriteLine($"{argValue1:0.000} + {argValue2:0.000} = {result1:0.000}");
            return true;
        }
    }
}
