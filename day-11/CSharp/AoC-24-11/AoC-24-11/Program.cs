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

            // Use array instead of list for better performance
            long[] rocks = input.Split(" ")
                .Select(long.Parse)
                .ToArray();

            Console.WriteLine("Number of rocks: " + rocks.Length);
            Console.WriteLine(string.Join(" ", rocks) + "\n\n");

            for (int i = 1; i <= blinks; i++)
            {
                rocks = Blink(rocks);
                // Uncomment for verbose output
                Console.WriteLine("#" + i + " Blink");
            }

            Console.WriteLine("Number of rocks: " + rocks.Length);
        }

        public static long[] Blink(long[] rocks)
        {
            // Pre-allocate maximum possible size to reduce reallocations
            var newRocks = new long[rocks.Length * 2];
            int newRocksCount = 0;

            foreach (var rock in rocks)
            {
                if (rock == 0)
                {
                    newRocks[newRocksCount++] = 1;
                }
                else
                {
                    // Use mathematical approach instead of string conversion
                    long currentRock = rock;
                    int digitCount = currentRock == 0 ? 1 : (int)Math.Floor(Math.Log10(currentRock) + 1);

                    if (digitCount % 2 == 0)
                    {
                        // Split into two parts
                        long divisor = (long)Math.Pow(10, digitCount / 2);
                        long leftMark = currentRock / divisor;
                        long rightMark = currentRock % divisor;

                        newRocks[newRocksCount++] = leftMark;
                        newRocks[newRocksCount++] = rightMark;
                    }
                    else
                    {
                        // Multiply by 2024
                        newRocks[newRocksCount++] = currentRock * 2024;
                    }
                }
            }

            // Create a new array with exactly the number of rocks
            return newRocks[..newRocksCount];
        }
    }
}