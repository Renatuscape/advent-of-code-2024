using System.Text;

namespace AoC_24_9
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string input = File.ReadAllText("input-aoc-24-9.txt");
            AmphipodComputer.ObjectifyDisk(input);
            Console.WriteLine("DISK MAPPED: " + AmphipodComputer.diskMap + "\n");
            AmphipodComputer.SortDisk();
            Console.WriteLine("\nDISK SORTED: " + AmphipodComputer.diskMap.ToString());

            Console.WriteLine($"\nInput length: {input.Length}\n" +
                $"Disk map length: {AmphipodComputer.diskMap.Length}");

            Console.WriteLine("\nFINAL CHECKSUM: " + AmphipodComputer.CalculateCheckSum());
        }
    }

    public static class AmphipodComputer
    {
        public static string diskMap = "";

        public static void ObjectifyDisk(string input)
        {
            Console.WriteLine("Objectifying disk: " + input);
            int id = 0;

            for (int i = 0; i < input.Length; i += 2)
            {
                for (int j = 0; j < input[i] - '0'; j++)
                {
                    diskMap += id;
                }

                id++;

                if (i + 1 < input.Length - 1)
                {
                    for (int j = 0; j < input[i +1] - '0'; j++)
                    {
                        diskMap += ".";
                    }
                }
            }
        }

        public static void SortDisk()
        {
            char[] diskArray = diskMap.ToCharArray();

            for (int i = 0; i < diskArray.Length; i++)
            {
                if (diskArray[i] == '.')
                {
                    // Find the rightmost non-period character
                    for (int j = diskArray.Length - 1; j > i; j--)
                    {
                        if (diskArray[j] != '.')
                        {
                            // Swap the first period with the rightmost non-period
                            diskArray[i] = diskArray[j];
                            diskArray[j] = '.';

                            //Console.WriteLine(new string(diskArray));
                            break;
                        }
                    }
                }
            }

            diskMap = new string(diskArray);
        }

        public static long CalculateCheckSum()
        {
            long sum = 0;

            // Iterate through the entire diskMap
            for (int i = 0; i < diskMap.Length; i++)
            {
                // Only calculate for non-period characters
                if (diskMap[i] != '.')
                {
                    // Multiply the current position by the file ID
                    long id = diskMap[i] - '0';
                    long toAdd = (long)i * id;
                    sum += toAdd;

                    //Console.WriteLine($"Position {i}: ID {id}, Contribution {toAdd}, Running Sum {sum}");
                }
            }

            return sum;
        }
    }
}