namespace AoC_24_11
{
    internal class Program
    {
        // RULES
        // Every blink, each stone changes according to the first applicable rule:
        // 0 becomes 1
        // Even number digits becomes two stones with half of each number, not keeping leading zeroes (1000 = 10 0)
        // If no other rule applies, stone's old number is multiplied by 2024

        // Order is preserved and they stay in a perfectly straight line

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

            // Use array instead of list for better performance
            PlutonianRock[] rocks = input.Split(" ")
                .Select(x => new PlutonianRock(long.Parse(x)))
                .ToArray();

            Console.WriteLine("Number of rocks: " + rocks.Length);
            Console.WriteLine(string.Join(" ", rocks.Select(r => r.mark)) + "\n\n");

            for (int i = 1; i <= blinks; i++)
            {
                rocks = Blink(rocks);
                Console.WriteLine("#" + i + " Blink");
            }

            Console.WriteLine("Number of rocks: " + rocks.Length);
        }

        public static PlutonianRock[] Blink(PlutonianRock[] rocks)
        {
            //Console.WriteLine(string.Join(" ", rocks.Select(r => r.mark)));

            // Pre-allocate a list to avoid multiple resizes
            var newRocks = new List<PlutonianRock>(rocks.Length * 2);

            foreach (var rock in rocks)
            {
                if (rock.mark == 0)
                {
                    newRocks.Add(new PlutonianRock(1));
                }
                else
                {
                    string markStr = rock.mark.ToString();
                    if (markStr.Length % 2 == 0)
                    {
                        int middleIndex = markStr.Length / 2;

                        // Split exactly in half, removing leading zeros
                        long leftMark = long.Parse(markStr.Substring(0, middleIndex));
                        long rightMark = long.Parse(markStr.Substring(middleIndex));

                        newRocks.Add(new PlutonianRock(leftMark));
                        newRocks.Add(new PlutonianRock(rightMark));
                    }
                    else
                    {
                        newRocks.Add(new PlutonianRock(rock.mark * 2024));
                    }
                }
            }

            return newRocks.ToArray();
        }

        public record PlutonianRock(long mark);
    }
}
