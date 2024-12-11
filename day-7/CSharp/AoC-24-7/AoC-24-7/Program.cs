namespace AoC_24_7
{
    internal class Program
    {
        public class Equation
        {
            public long result;
            public List<long> values = new();
        }
        static void Main(string[] args)
        {
            string[] input = File.ReadAllLines("input-aoc-24-7.txt");
            List<Equation> equations = new();

            foreach (var line in input)
            {
                if (line != "")
                {
                    Equation newEquation = new();

                    var numbers = line.Split(' ');
                    if (numbers[0].Contains(":"))
                    {
                        //Console.WriteLine(numbers[0]);
                        long result = long.Parse(numbers[0].Replace(":", ""));

                        newEquation.result = result;

                        for (int i = 1; i < numbers.Length; i++)
                        {
                            newEquation.values.Add(int.Parse(numbers[i]));
                        }

                        equations.Add(newEquation);
                    }
                }
            }

            foreach (var equation in equations)
            {
                Console.WriteLine($"\nDesired result: {equation.result}");
                foreach (var number in equation.values)
                {
                    Console.Write(number.ToString() + " ");
                }
            }

            Console.WriteLine("\n\nAttempting to solve equations\n");
            long totalSum = 0;
            foreach (var equation in equations)
            {

                var sequences = MathsMachine.GenerateOperatorCombinations(equation.values.Count - 1);

                foreach (var sequence in sequences)
                {
                    long result = MathsMachine.SolveEquation(equation, sequence);
                    //Console.WriteLine("\tResult: " + result);
                    if (result == equation.result)
                    {
                        //Console.WriteLine("\t\tMATCH on equation result.");
                        totalSum += result;
                        break;
                    }
                    else
                    {
                        //Console.WriteLine("\t\tNOT match on equation result.");
                    }
                }
            }
            Console.WriteLine("\n TOTAL SUM IS: " + totalSum);
        }

        public static class MathsMachine
        {
            public static List<string> operators = new()
            {
                "*",
                "+",
                "||"
            };

            public static List<List<string>> GenerateOperatorCombinations(int length)
            {
                var combinations = new List<List<string>>();

                // Helper method to generate combinations recursively
                void GenerateCombinationsRecursive(List<string> currentCombination)
                {
                    // If we've reached the desired length, add the combination to results
                    if (currentCombination.Count == length)
                    {
                        combinations.Add(new List<string>(currentCombination));
                        return;
                    }

                    // Try adding each operator and recursively generate further combinations
                    foreach (var op in operators)
                    {
                        currentCombination.Add(op);
                        GenerateCombinationsRecursive(currentCombination);
                        currentCombination.RemoveAt(currentCombination.Count - 1);
                    }
                }

                // Start the recursive generation
                GenerateCombinationsRecursive(new List<string>());

                return combinations;
            }

            public static long SolveEquation(Equation equation, List<string> operatorSequence)
            {
                long result = equation.values[0];
                //Console.WriteLine($"\nAttempting to solve {equation.result}: {string.Join(" ", equation.values.ToArray())}");

                for (int i = 1; i < equation.values.Count; i++)
                {
                    long output = EquationSwitch(result, equation.values[i], operatorSequence[i - 1]);
                    result = output;
                }

                return result;
            }

            public static long EquationSwitch(long x, long y, string mathOperator)
            {
                if (mathOperator == "*")
                {
                    return Multiply(x, y);
                }
                else if (mathOperator == "||")
                {
                    return Concatenate(x, y);
                }
                else
                {
                    return Add(x, y);
                }
            }

            static long Multiply(long x, long y)
            {
                //Console.WriteLine("\tMultiplying " + x + " * " + y + " = " + (x * y));
                return x * y;
            }

            static long Add(long x, long y)
            {
                //Console.WriteLine("\tAdding " + x + " + " + y + " = " + (x + y));
                return x + y;
            }

            static long Concatenate(long x, long y)
            {
                return long.Parse(x.ToString() + y.ToString());
            }
        }
    }
}
