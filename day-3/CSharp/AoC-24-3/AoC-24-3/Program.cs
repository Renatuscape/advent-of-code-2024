using System.Text.RegularExpressions;

namespace AoC_24_3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string input = File.ReadAllText("input-coa-24-3.txt");
            string pattern = @"(mul\(([0-9,]+)\)|do\(\)|don't\(\))";
            int sum = 0;
            bool doMul = true;

            MatchCollection matches = Regex.Matches(input, pattern);

            foreach (Match match in matches)
            {
                string contents = match.Groups[1].Value;
                var instructions = contents.Split('\n');

                foreach (var instruction in instructions)
                {
                    Console.WriteLine(instruction); // Outputs: data1, data2

                    if (instruction == "do()")
                    {
                        doMul = true;
                    }
                    else if (instruction == "don't()")
                    {
                        doMul = false;
                    }
                    else if (doMul)
                    {
                        var numbers = instruction.Replace("mul(", "").Replace(")", "").Split(',');
                        sum += int.Parse(numbers[0]) * int.Parse(numbers[1]);
                    }
                }
            }

            Console.WriteLine("\nFINALY SUM: " + sum);
        }
    }
}
