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
            int blinks = 25;

            string input;

            if (useExampleInput)
            {
                input = File.ReadAllText("example.txt");
            }
            else
            {
                input = File.ReadAllText("input.txt");
            }

            List<PlutonianRock> rocks = new();

            foreach (var rock in input.Split(" "))
            {
                rocks.Add(new PlutonianRock() { mark = int.Parse(rock) });
            }

            Console.WriteLine("Number of rocks: " + rocks.Count);
            Console.WriteLine(string.Join(" ", rocks.Select(r => r.mark)));

            for (int i = 0; i < blinks; i++)
            {
                Blink(rocks);
            }

            Console.WriteLine("Number of rocks: " + rocks.Count);
            //Console.WriteLine(string.Join(" ", rocks.Select(r => r.mark)));
        }

        public static void Blink(List<PlutonianRock> rocks)
        {
            Dictionary<PlutonianRock, PlutonianRock> rockClones = new();
            
            //Console.WriteLine(string.Join(" ", rocks.Select(r => r.mark)));
            for (int i = 0; i < rocks.Count; i++)
            {
                var rock = rocks[i];

                if (rock.mark == 0)
                {
                    rock.mark = 1;
                }
                else if (rock.mark.ToString().Length % 2 == 0)
                {
                    char[] mark = rock.mark.ToString().ToCharArray();
                    int middleIndex = mark.Length / 2;

                    string leftMark = new string(mark.Take(middleIndex).ToArray());
                    string rightMark = new string(mark.Skip(middleIndex).ToArray());

                    //Console.WriteLine($"{leftMark} - {rightMark}");
                    rock.mark = long.Parse(leftMark);
                    PlutonianRock newRock = new() { mark = long.Parse(rightMark) };
                    rockClones.Add(rock, newRock);
                }
                else
                {
                    rock.mark *= 2024;
                }
            }

            foreach (var rockPair in rockClones)
            {
                int rockIndex = rocks.IndexOf(rockPair.Key) +1;
                rocks.Insert(rockIndex, rockPair.Value);
            }
        }

        public class PlutonianRock
        {
            public long mark;
        }
    }
}
