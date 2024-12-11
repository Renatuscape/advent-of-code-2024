using static System.Runtime.InteropServices.JavaScript.JSType;
using static AoC_24_7.Program;

namespace AoC_24_7
{
    internal class Program
    {
        public class Equation
        {
            public int result;
            public List<int> values = new();
        }
        static void Main(string[] args)
        {
            string[] input = File.ReadAllLines("input-aoc-24-7.txt");
            List<Equation> equations = new();

            foreach (var line in input)
            {
                Equation newEquation = new();

                var numbers = line.Split(' ');
                int result = int.Parse(numbers[0].Replace(":", ""));

                newEquation.result = result;

                for (int i = 1; i < numbers.Length; i++)
                {
                    newEquation.values.Add(int.Parse(numbers[i]));
                }

                equations.Add(newEquation);
            }

            foreach (var equation in equations)
            {
                Console.WriteLine($"\nDesired result: {equation.result}");
                foreach (var number in equation.values)
                {
                    Console.Write(number.ToString() + " ");
                }
            }

            Console.WriteLine("\n\nAttempting to solve equation\n");

            foreach (var equation in equations)
            {
                List<List<string>> sequences = new();

                foreach (var opr in MathsMachine.operators)
                {
                    var sequence = MathsMachine.BuildOperationSequence(equation, opr);
                    sequences.Add(sequence);
                }

                foreach (var sequence in sequences)
                {
                    Console.WriteLine("\nRESULT: " + MathsMachine.SolveEquation(equation, sequence));
                }
            }
        }

        public static class MathsMachine
        {
            public static List<string> operators = new()
            {
                "*",
                "+"
            };

            public static List<string> BuildOperationSequence(Equation equation, string opr)
            {
                List<string> operatorSequences = new();
                int operatorSpaces = equation.values.Count - 1;

                for (int i = 0; i < operatorSpaces; i++)
                {
                    operatorSequences.Add(opr);
                }

                return operatorSequences;
            }

            public static int SolveEquation(Equation equation, List<string> operatorSequence)
            {
                int result = equation.values[0]; // int operatorSpaces = values.Count - 1;
                Console.WriteLine($"\nStarting value is {result}");

                for (int i = 1; i < equation.values.Count; i++)
                {
                    Console.WriteLine($"Attempting to solve {result} {operatorSequence[i-1]} {equation.values[i]}");
                    int output = EquationSwitch(result, equation.values[i], operatorSequence[i-1]);
                    result = output;
                    Console.WriteLine($"Output was {output} and current result was {result}");
                }

                return result;
            }

            public static int EquationSwitch(int x, int y, string mathOperator)
            {
                if (mathOperator == "*")
                {
                    return Multiply(x, y);
                }
                else
                {
                    return Add(x, y);
                }
            }

            static int Multiply(int x, int y)
            {
                Console.WriteLine("\tMultiplying " + x + " * " + y + " = " + (x * y));
                return x * y;
            }

            static int Add(int x, int y)
            {
                Console.WriteLine("\tAdding " + x + " + " + y + " = " + (x + y));
                return x + y;
            }
        }
    }
}
