using System.Collections.Generic;
using static AoC_24_10.Program;

namespace AoC_24_10
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool useExampleInput = false;

            string[] input;

            if (useExampleInput)
            {
                input = File.ReadAllLines("example-aoc-24-10.txt");
            }
            else
            {
                input = File.ReadAllLines("input-aoc-24-10.txt");
            }

            foreach (string line in input)
            {
                MapArchive.originalMap.Add(line.Select(c => int.Parse(c.ToString())).ToList());
            }

            Console.WriteLine("LAVA ISLAND HIKING MAP");
            foreach (var line in MapArchive.originalMap)
            {
                Console.WriteLine(string.Join(" ", line.ToArray()));
            }
            Console.WriteLine();

            Console.WriteLine("Score equals: " + SolvePart1(MapArchive.originalMap));
            Console.WriteLine("Goal was: " + (useExampleInput ? "36" : "582"));
        }

        public static class MapArchive
        {
            public static List<List<int>> originalMap = new();
        }

        public static long SolvePart1(List<List<int>> map)
        {
            // FIND ALL TRAILHEADS
            List<Trailhead> trailheads = new();

            for (int col = 0; col < map.Count; col++)
            {
                for (int row = 0; row < map[col].Count; row++)
                {
                    if (map[col][row] == 0)
                    {
                        trailheads.Add(new() { startingCoordinates = (row, col) });
                    }
                }
            }

            Console.WriteLine("Found trailheads: " + trailheads.Count);

            //GET ALL TRAILS FROM TRAILHEADS
            List<Trail> foundTrails = new();

            foreach (Trailhead trailhead in trailheads)
            {
                var trails = trailhead.GetAllTrails();
                Console.WriteLine($"\n----------------------------------------------------------------" +
                    $"\nTrailhead {trailhead.startingCoordinates} has {trails.Count} trails\n");

                List<Trail> uniqueTrailEnds = new();

                foreach (var trail in trails)
                {
                    if (uniqueTrailEnds.FirstOrDefault(t => t.Coordinates[9].x == trail.Coordinates[9].x && t.Coordinates[9].y == trail.Coordinates[9].y) == null)
                    {
                        uniqueTrailEnds.Add(trail);
                    }
                }

                foundTrails.AddRange(uniqueTrailEnds);

                foreach (var trail in trails)
                {
                    Console.WriteLine($"\t{trail.PrintSteps()}");
                }

                Console.WriteLine($"\n\tTrailhead {trailhead.startingCoordinates} has {uniqueTrailEnds.Count} unique trail ends.\n");
            }


            return foundTrails.Count;
        }

        public static void PrintMap(List<List<int>> map, int delay = 0)
        {
            Thread.Sleep(delay);
            Console.Write("\n");
            foreach (var line in map)
            {
                foreach (var num in line)
                {
                    if (num < 0)
                    {
                        Console.Write("# ");
                    }
                    else
                    {
                        Console.Write(num.ToString() + " ");
                    }
                }
                Console.Write("\n");
            }
            Console.Write("\n");
        }

        public static List<Trail> FindAllTrails(Trailhead trailhead)
        {
            List<Trail> foundTrails = new List<Trail>()  // Index will equal elevation
            {
                CreateNewTrail(trailhead.startingCoordinates)
            };

            List<Trail> newTrails = new();
            List<Trail> deadEndTrails = new();

            int desiredElevation = 1;

            while (desiredElevation <= 9)
            {
                foreach (Trail trail in foundTrails)
                {
                    (int x, int y) curCoords = trail.Coordinates[desiredElevation - 1];
                    List<(int x, int y)> searchArea = GetViableCoordinates(curCoords, desiredElevation);

                    if (searchArea.Count == 1) // One new step was found. Expand existing trail
                    {
                        trail.AddStep(searchArea[0]);
                    }
                    else if (searchArea.Count > 1) // More than one step was found. Expand trail and add copies with new steps
                    {
                        for (int i = 1; i < searchArea.Count; i++)// Create new trail for each coordinate
                        {
                            Trail newTrail = new Trail();

                            // Deep copy ALL existing steps from the original trail
                            foreach (var step in trail.Coordinates)
                            {
                                newTrail.Coordinates.Add(step);
                            }

                            // Add the new step at the current elevation
                            newTrail.AddStep(searchArea[i]);

                            newTrails.Add(newTrail);
                        }

                        trail.AddStep(searchArea[0]);
                    }
                    else
                    {
                        deadEndTrails.Add(trail);
                    }
                }

                // Remove dead ends
                foreach (Trail deadEnd in deadEndTrails)
                {
                    foundTrails.Remove(deadEnd);
                }
                deadEndTrails.Clear();

                foreach (Trail newTrail in newTrails)
                {
                    foundTrails.Add(newTrail);
                }
                newTrails.Clear();

                // Increase elevation
                desiredElevation++;
            }

            return foundTrails;
        }

        public static Trail CreateNewTrail((int x, int y) startingCoords)
        {
            Trail newTrail = new();
            newTrail.Coordinates.Add(startingCoords);

            return newTrail;
        }

        public static List<(int x, int y)> GetViableCoordinates((int x, int y) curCoords, int desiredElevation)
        {
            var searchArea = GetSearchArea(curCoords, MapArchive.originalMap.Count, MapArchive.originalMap[0].Count);
            var filteredArea = FilterSearchAreaForElevation(searchArea, desiredElevation);
            return filteredArea;
        }

        public static List<(int x, int y)> FilterSearchAreaForElevation(List<(int x, int y)> searchArea, int desiredElevation)
        {
            List<(int x, int y)> validCoords = new();
            foreach (var coords in searchArea)
            {
                if (coords.y >= 0 &&
                    coords.y < MapArchive.originalMap.Count &&
                    coords.x >= 0 &&
                    coords.x < MapArchive.originalMap[0].Count)
                {
                    int actualElevation = MapArchive.originalMap[coords.y][coords.x];

                    if (actualElevation == desiredElevation)
                    {
                        validCoords.Add(coords);
                    }
                }
            }
            return validCoords;
        }

        public static List<(int x, int y)> GetSearchArea((int x, int y) curCoords, int maxY, int maxX)
        {
            return new()
                {
                    (curCoords.x, curCoords.y - 1), // North
                    (curCoords.x, curCoords.y + 1), // South
                    (curCoords.x + 1, curCoords.y), // East
                    (curCoords.x - 1, curCoords.y), // West
                };
        }

        public class Trailhead
        {
            public (int x, int y) startingCoordinates;
            public List<Trail> Trails { get; private set; } = new(); // Value tuple

            public bool AddTrail(Trail incomingTrail)
            {
                Trails.Add(incomingTrail);
                return true;
            }

            public List<Trail> GetAllTrails()
            {
                return FindAllTrails(this);
            }
        }

        public class Trail
        {
            public List<(int x, int y)> Coordinates { get; private set; } = new();

            public void AddStep((int x, int y) coordinates)
            {
                Coordinates.Add(coordinates);
            }

            public string PrintSteps()
            {
                return string.Join(" ", Coordinates);
            }
        }
    }
}
