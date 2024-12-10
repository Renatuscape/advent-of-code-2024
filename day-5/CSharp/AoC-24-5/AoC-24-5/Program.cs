
namespace AoC_24_5
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] input = File.ReadAllLines("input-aoc-24-5.txt");
            int sum = 0;

            UpdateValidator.CleanInput(input);

            // Run pages through rules and collect all updates that pass
            var passingRules = UpdateValidator.CheckUpdates();

            Console.WriteLine("Found passing rules: " + passingRules.Count);

            // Get the middle page number from all passing updates and add to sum
            foreach (string passingRule in passingRules)
            {
                var pageNumbers = passingRule.Split(',');
                var middleIndex = pageNumbers.Length / 2;
                var middlePage = int.Parse(pageNumbers[middleIndex]);
                sum += middlePage;
            }

            Console.WriteLine("Final sum: " + sum);
        }

        internal static class UpdateValidator
        {
            internal static List<Rule> rules = new();
            internal static List<string> updates = new();
            internal static void CleanInput(string[] input)
            {
                List<string> rulesRaw = new();

                bool foundBreak = false;

                foreach (string line in input)
                {
                    if (!foundBreak)
                    {
                        if (line == "")
                        {
                            foundBreak = true;
                        }
                        else
                        {
                            rulesRaw.Add(line);
                        }
                    }
                    else
                    {
                        updates.Add(line);
                    }
                }

                BuildRules(rulesRaw);

                Console.WriteLine("\nUPDATES");
                foreach (var line in updates)
                {
                    Console.WriteLine(line);
                }
            }

            internal static void BuildRules(List<string> rulesRaw)
            {
                foreach (var line in rulesRaw)
                {
                    Rule rule = new();
                    rules.Add(rule);

                    var ruleNumbers = line.Split('|');
                    rule.ruleX = ruleNumbers[0];
                    rule.ruleY = ruleNumbers[1];
                }
            }

            internal static List<string> CheckUpdates()
            {
                List<string> validUpdates = new();

                foreach (var update in updates)
                {
                    bool failedCheck = false;

                    foreach (var rule in rules)
                    {
                        if (rule.CheckRuleValid(update))
                        {
                            if (!rule.CheckRule(update))
                            {
                                failedCheck = true;
                                break;
                            }
                        }
                    }

                    if (!failedCheck)
                    {
                        validUpdates.Add(update);
                    }
                }

                return validUpdates;
            }
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
                    Console.WriteLine($"Passed rule {ruleX}|{ruleY}: " + update);
                    return true;
                }
                Console.WriteLine($"Failed rule {ruleX}|{ruleY}: " + update);
                return false;
            }
        }
    }
}
