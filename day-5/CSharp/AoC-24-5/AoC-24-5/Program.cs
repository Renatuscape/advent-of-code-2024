
namespace AoC_24_5
{
    internal partial class Program
    {
        static void Main(string[] args)
        {
            string[] input = File.ReadAllLines("input-aoc-24-5.txt");
            int sum = 0;

            UpdateValidator.CleanInput(input);

            // Run pages through rules and collect all updates that pass
            var validUpdates = UpdateValidator.CheckUpdates(out var invalidUpdates);

            // Get the middle page number from all passing updates and add to sum
            foreach (string passingRule in validUpdates)
            {
                var pageNumbers = passingRule.Split(',');
                var middleIndex = pageNumbers.Length / 2;
                var middlePage = int.Parse(pageNumbers[middleIndex]);
                sum += middlePage;
            }

            Console.WriteLine("\n\tCleared updates: " + validUpdates.Count +
                "\n\tFailed updates: " + invalidUpdates.Count +
                "\n\tTotal updates: " + UpdateValidator.updates.Count +
                "\n\tSCORE: " + sum);

            int totalFails = invalidUpdates.Count;
            var fixedUpdates = invalidUpdates;
            List<string> validatedFixedUpdates = new();

            while (totalFails > 0) {
                Console.WriteLine("\n\nRUNNING UPDATE FIXER\n");
                fixedUpdates = UpdateValidator.FixUpdates(fixedUpdates);
                UpdateValidator.updates = fixedUpdates;
                validatedFixedUpdates = UpdateValidator.CheckUpdates(out var invalidUpdatesB);
                totalFails = invalidUpdatesB.Count;
                Console.WriteLine("\n\tFixed updates: " + fixedUpdates.Count +
                    "\n\tPassing fixed updates: " + validatedFixedUpdates.Count +
                    "\n\tFailed fixed updates: " + invalidUpdatesB.Count);
            }

            sum = 0;

            foreach (string passingRule in validatedFixedUpdates)
            {
                var pageNumbers = passingRule.Split(',');
                var middleIndex = pageNumbers.Length / 2;
                var middlePage = int.Parse(pageNumbers[middleIndex]);
                sum += middlePage;
            }

            Console.WriteLine("\n\n\t** FINAL SUM: " + sum + " **");
        }

        internal class Rule
        {
            public string ruleX = string.Empty;
            public string ruleY = string.Empty;

            internal bool CheckRuleValid(string update)
            {
                if (update.Contains(ruleX) && update.Contains(ruleY))
                {
                    return true;
                }
                return false;
            }

            internal bool CheckRule(string update)
            {
                var splitNumbers = update.Split(ruleY);

                if (splitNumbers[0].Contains(ruleX))
                {
                    //Console.WriteLine($"Passed rule {ruleX}|{ruleY}: " + update);
                    return true;
                }
                //Console.WriteLine($"Failed rule {ruleX}|{ruleY}: " + update);
                return false;
            }
        }
    }
}
