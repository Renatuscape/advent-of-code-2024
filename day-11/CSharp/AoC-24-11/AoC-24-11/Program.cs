namespace AoC_24_11
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool useExampleInput = false;
            int blinks = 75;
            string input;
            if (useExampleInput)
            {
                input = File.ReadAllText("example.txt");
            }
            else
            {
                input = File.ReadAllText("input.txt");
            }

            // Parse initial rocks
            var initialRocks = input.Split(" ")
                .Select(long.Parse)
                .ToList();

            // Use a dictionary to track rock distributions
            var rockDistribution = new Dictionary<long, long>();
            foreach (var rock in initialRocks)
            {
                if (!rockDistribution.ContainsKey(rock))
                    rockDistribution[rock] = 0;
                rockDistribution[rock]++;
            }

            Console.WriteLine("Initial number of rocks: " + initialRocks.Count);
            PrintDistribution(rockDistribution);

            // Perform blinks
            for (int i = 1; i <= blinks; i++)
            {
                rockDistribution = Blink(rockDistribution);
                Console.WriteLine($"#Blink {i} - Rocks: {rockDistribution.Sum(x => x.Value)}");
                // Uncomment to see distribution each blink
                // PrintDistribution(rockDistribution);
            }

            Console.WriteLine("Final number of rocks: " + rockDistribution.Sum(x => x.Value));
        }

        static Dictionary<long, long> Blink(Dictionary<long, long> currentRocks)
        {
            var newRocks = new Dictionary<long, long>();

            foreach (var rock in currentRocks)
            {
                long rockValue = rock.Key;
                long rockCount = rock.Value;

                if (rockValue == 0)
                {
                    // 0 becomes 1
                    AddToDistribution(newRocks, 1, rockCount);
                }
                else
                {
                    string rockStr = rockValue.ToString();
                    if (rockStr.Length % 2 == 0)
                    {
                        // Split in half
                        int middleIndex = rockStr.Length / 2;
                        long leftMark = long.Parse(rockStr.Substring(0, middleIndex));
                        long rightMark = long.Parse(rockStr.Substring(middleIndex));

                        AddToDistribution(newRocks, leftMark, rockCount);
                        AddToDistribution(newRocks, rightMark, rockCount);
                    }
                    else
                    {
                        // Multiply by 2024
                        AddToDistribution(newRocks, rockValue * 2024, rockCount);
                    }
                }
            }

            return newRocks;
        }

        // Helper method to add to rock distribution
        static void AddToDistribution(Dictionary<long, long> distribution, long rock, long count)
        {
            if (!distribution.ContainsKey(rock))
                distribution[rock] = 0;
            distribution[rock] += count;
        }

        // Helper method to print rock distribution
        static void PrintDistribution(Dictionary<long, long> distribution)
        {
            Console.WriteLine("Rock Distribution:");
            foreach (var rock in distribution.OrderBy(x => x.Key))
            {
                Console.WriteLine($"Rock {rock.Key}: {rock.Value} times");
            }
            Console.WriteLine("Total Rocks: " + distribution.Sum(x => x.Value));
        }
    }
}