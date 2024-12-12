using System.Drawing;
using static AoC_24_8.Program;

namespace AoC_24_8
{
    internal class Program
    {
        public class Antenna
        {
            public char frequency;
            public int x;
            public int y;
        }

        public class Antinode
        {
            public int x;
            public int y;
        }

        static void Main(string[] args)
        {
            string[] input = File.ReadAllLines("input-aoc-24-8.txt");

            foreach (var line in input)
            {
                Controller.map.Add(new(line.ToList()));
            }

            Controller.PrintMap();
            Controller.MapAntennae(true);
            Controller.MapAntinodesPart1(true);
            Controller.MapAntinodesPart2(true);
        }

        public static class Controller
        {
            public static readonly List<List<char>> map = new();
            public static List<Antenna> antennae = new();
            public static List<Antinode> antinodes = new();

            public static void PrintMap()
            {
                foreach (var col in map)
                {
                    foreach (var row in col)
                    {
                        Console.Write(row.ToString() + " ");
                    }
                    Console.Write("\n");
                }
            }

            public static void PrintMapWithAntinodes(bool overwriteAntennae = false)
            {
                Console.WriteLine("\nMAP OF ANTINODES");
                for (int y = 0; y < map.Count; y++)
                {
                    for (int x = 0; x < map[y].Count; x++)
                    {
                        var antinode = antinodes.FirstOrDefault(a => a.x == x && a.y == y);

                        if (antinode != null)
                        {
                            if (!overwriteAntennae && map[y][x] != '.')
                            {
                                Console.Write(map[y][x].ToString() + " ");
                            }
                            else if (overwriteAntennae && map[y][x] != '.')
                            {
                                Console.Write("#_");
                            }
                            else
                            {
                                Console.Write("# ");
                            }
                        }
                        else
                        {
                            Console.Write(map[y][x].ToString() + " ");
                        }
                    }
                    Console.Write("\n");
                }
            }

            public static void MapAntennae(bool printList = false)
            {
                antennae.Clear();

                for (int y = 0; y < map.Count; y++)
                {
                    for (int x = 0; x < map[y].Count; x++)
                    {
                        if (map[y][x] != '.')
                        {
                            antennae.Add(new() { frequency = map[y][x], x = x, y = y });
                        }
                    }
                }

                if (printList)
                {
                    Console.WriteLine("\nANTENNAE FOUND");
                    foreach (var antenna in antennae)
                    {
                        Console.WriteLine($"{antenna.frequency}: {antenna.y},{antenna.x}");
                    }
                }
            }

            public static void MapAntinodesPart1(bool printMap = false)
            {
                antinodes.Clear();

                foreach (var antenna in antennae)
                {
                    foreach (var otherAntenna in antennae)
                    {
                        if (antenna.frequency == otherAntenna.frequency
                            && otherAntenna != antenna)
                        {
                            int antinodeX = otherAntenna.x + (otherAntenna.x - antenna.x);
                            int antinodeY = otherAntenna.y + (otherAntenna.y - antenna.y);

                            // Check if coordinates are within map bounds
                            if (antinodeY >= 0 && antinodeY < map.Count &&
                                antinodeX >= 0 && antinodeX < map[antinodeY].Count)
                            {
                                // Check if node overlaps with existing antinode
                                if (antinodes.FirstOrDefault(a => antinodeX == a.x && antinodeY == a.y) == null)
                                {
                                    antinodes.Add(new() { x = antinodeX, y = antinodeY });
                                }
                            }
                        }
                    }
                }

                if (printMap)
                {
                    PrintMapWithAntinodes(false);
                }

                Console.WriteLine("Total antinodes: " + antinodes.Count);
            }


            public static void MapAntinodesPart2(bool printMap = false)
            {
                antinodes.Clear();
                Console.WriteLine("\nANTINODES PART 2");

                foreach (var antenna in antennae)
                {
                    foreach (var otherAntenna in antennae)
                    {
                        if (antenna.frequency == otherAntenna.frequency && antenna != otherAntenna)
                        {
                            int x = otherAntenna.x + (otherAntenna.x - antenna.x);
                            int y = otherAntenna.y + (otherAntenna.y - antenna.y);

                            AddAntinode(antenna.x, antenna.y);

                            while (y >= 0 && y < map.Count && x >= 0 && x < map[y].Count)
                            {
                                AddAntinode(x, y);

                                x += otherAntenna.x - antenna.x;
                                y += otherAntenna.y - antenna.y;
                            }
                        }
                    }
                }

                if (printMap)
                {
                    PrintMapWithAntinodes(true);
                }

                Console.WriteLine("Total antinodes: " + antinodes.Count);
            }

            public static void AddAntinode(int x, int y)
            {
                if (antinodes.FirstOrDefault(a => x == a.x && y == a.y) == null)
                {
                    antinodes.Add(new() { x = x, y = y });
                }
            }
        }
    }
}
