
using static AoC_24_5.Program;

namespace AoC_24_5
{
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

        internal static List<string> CheckUpdates(out List<string> invalidUpdates)
        {
            List<string> validUpdates = new();
            invalidUpdates = new();

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
                            invalidUpdates.Add(update);
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

        internal static List<string> FixUpdates(List<string> updates)
        {
            List<string> fixedUpdates = new();

            foreach (var update in updates)
            {
                //Console.WriteLine("Attempting to fix: " + update);
                List<string> splitNumbers = update.Split(',').ToList();

                foreach (var rule in rules)
                {
                    if (rule.CheckRuleValid(update))

                        if (!rule.CheckRule(update))
                        {
                            int indexX = splitNumbers.IndexOf(rule.ruleX);
                            int indexY = splitNumbers.IndexOf(rule.ruleY);

                            var elementX = splitNumbers[indexX];
                            splitNumbers.Remove(elementX);
                            splitNumbers.Insert(0, elementX);
                        }
                }

                string sortedNumbers = string.Join(',', splitNumbers);
                fixedUpdates.Add(sortedNumbers);
                //Console.WriteLine("Result of fixed: " + sortedNumbers);
            }

            return fixedUpdates;
        }
    }
}
