using System.Text;

namespace AoC_24_9
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string input = File.ReadAllText("input-aoc-24-8.txt");
            //Console.WriteLine(input + "\n");
            AmphipodComputer.ObjectifyDisk(input);
            AmphipodComputer.MapDisk();
            AmphipodComputer.SortDisk();
            Console.WriteLine(AmphipodComputer.diskMap.ToString());
        }
    }

    public static class AmphipodComputer
    {
        public static List<MemoryBlock> memoryObjects = new();
        public static List<MemoryBlock> sortedMemoryObjects = new();
        public static StringBuilder diskMap = new();

        public static void ObjectifyDisk(string input)
        {
            memoryObjects.Clear();

            int id = 0;

            for (int i = 0; i < input.Length; i += 2)
            {
                MemoryBlock file = new() { id = id, size = int.Parse(input[i].ToString()) };
                memoryObjects.Add(file);
                id++;

                if (i + 1 < input.Length - 1)
                {
                    MemoryBlock space = new() { id = -1, size = int.Parse(input[i + 1].ToString()) };
                    memoryObjects.Add(space);

                }
            }

            foreach (MemoryBlock file in memoryObjects)
            {
                Console.Write(file.size);
            }
            Console.WriteLine();
        }

        public static StringBuilder MapDisk()
        {
            diskMap.Clear();

            foreach (MemoryBlock file in memoryObjects)
            {
                if (file.id < 0)
                {
                    for (int i = 0; i < file.size; i++)
                    {

                        diskMap.Append(".");
                    }
                }
                else
                {
                    for (int i = 0; i < file.size; i++)
                    {

                        diskMap.Append(file.id);
                    }
                }
            }

            return diskMap;
        }

        public static void SortDisk()
        {
            int filesToMove = 0;

            foreach (var file in diskMap.ToString())
            {
                if (file != '.')
                {
                    filesToMove++;
                }
            }

            for (int i = 0; i < filesToMove; i++)
            {
                if (diskMap[i] == '.')
                {
                    int spaceIndex = i;
                    int fileIndex = -1;

                    for (int j = diskMap.Length -1; j > 0; j--)
                    {
                        if (diskMap[j] != '.')
                        {
                            fileIndex = j;
                            break;
                        }
                    }

                    if (fileIndex >= 0)
                    {
                        string file = diskMap[fileIndex].ToString();
                        diskMap.Append('.');
                        diskMap.Remove(fileIndex, 1);
                        diskMap.Remove(spaceIndex, 1);
                        diskMap.Insert(spaceIndex, file);
                        //Console.WriteLine(diskMap.ToString());
                    }
                }
            }
        }

        public static int CalculateCheckSum()
        {
            string diskMapString = diskMap.ToString();
            int sum = 0;

            for (int i = 0; i < diskMapString.Length; i++)
            {

            }

            return sum;
        }
    }

    public class MemoryBlock
    {
        public int id;
        public int size;
    }
}
