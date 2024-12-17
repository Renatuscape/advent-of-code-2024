using static AoC_24_10.Program;

namespace AoC_24_10
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool useExampleInput = true;

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

            SolvePart1(MapArchive.originalMap);
        }

        public static class MapArchive
        {
            public static List<List<int>> originalMap = new();
        }

        public static long SolvePart1(List<List<int>> map)
        {
            // Hiking trail must be as long as possible and have an even, gradual, uphill slope
            // A hiking trail starts at 0 and ends at 9
            // Always increases by exaqctly 1 at each step
            // No diagonal steps, only up, down, left, right (map pespective)

            // Trailhead: any position that starts one or more hiking trails, always height 0
            // Trailhead score: the number of 9-height positions that are reachable from the trailhead

            long score = 0L;
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

            Console.WriteLine("TRAILHEADS");

            foreach (Trailhead trailhead in trailheads)
            {
                List<List<int>> mapCopy = new();

                foreach (var line in map)
                {
                    mapCopy.Add(line.Select(c => int.Parse(c.ToString())).ToList());
                }

                FindTrail(trailhead, mapCopy);
            }

            return score;
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

        public static void FindAllTrails(Trailhead trailhead, List<List<int>> map)
        {
            int desiredElevation = 1;
        }

        public static void FindTrail(Trailhead trailhead, List<List<int>> map)
        {
            Console.WriteLine("\nSTARTING NEW TRAILFINDER\nAttempting to find trail for " + trailhead.startingCoordinates);
            (int x, int y) curCoords = trailhead.startingCoordinates;
            Trail newTrail = new();
            TrailStep firstStep = new TrailStep() { elevation = 0, coordinates = curCoords };
            newTrail.Steps.Add(firstStep);

            bool pathBlocked = false;
            bool trailFound = false;
            int desiredElevation = 1;

            while (!pathBlocked)
            {
                Console.WriteLine("Attempting to find next step");
                pathBlocked = !FindNextStep(); // If next step was found, path is not blocked.
                PrintMap(map);

                if (trailFound)
                {
                    var addTrail = trailhead.AddTrail(newTrail);

                    if (addTrail)
                    {
                        Console.WriteLine("Successfully added trail: ");
                        foreach (var step in newTrail.Steps)
                        {
                            Console.Write("(" + step.coordinates.y.ToString() + "," + step.coordinates.x.ToString() + " e" + MapArchive.originalMap[step.coordinates.y][step.coordinates.x] + ") ");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Trail already exists");
                    }
                    break;
                }

                if (pathBlocked)
                {
                    Console.WriteLine("Path was blocked.");
                }
            }

            bool FindNextStep()
            {
                var searchArea = GetSearchArea(curCoords, map.Count, map[0].Count);
                map[curCoords.y][curCoords.x] = -1;

                for (int i = 0; i < searchArea.Count; i++)
                {
                    var coords = searchArea[i];

                    if (map[coords.y][coords.x] == desiredElevation)
                    {
                        Console.WriteLine($"Found step: {coords}");
                        curCoords = coords;
                        newTrail.AddStep(desiredElevation, coords);
                        desiredElevation++;

                        if (map[coords.y][coords.x] >= 9)
                        {
                            Console.WriteLine("Found FINAL step.");
                            trailFound = true;
                            break;
                        }

                        return true;
                    }
                }

                Console.WriteLine("Could not find a next step");
                return false;
            }
        }

        public static List<(int x, int y)> GetSearchArea((int x, int y) curCoords, int maxY, int maxX)
        {
            List<(int x, int y)> searchArea = new List<(int x, int y)>();
            List<(int x, int y)> potentialCoordinates = new()
                {
                    (curCoords.x, curCoords.y - 1), // North
                    (curCoords.x, curCoords.y + 1), // South
                    (curCoords.x + 1, curCoords.y), // East
                    (curCoords.x - 1, curCoords.y), // West
                };

            // Filter out invalid coordinates
            foreach (var coords in potentialCoordinates)
            {
                if (coords.y < maxY && coords.y > -1 && coords.x < maxX && coords.x > -1)
                {
                    searchArea.Add(coords);
                }
            }

            return searchArea;
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
        }

        public class Trail
        {
            public List<TrailStep> Steps { get; private set; } = new();

            public void AddStep(int elevation, (int x, int y) coordinates)
            {
                Steps.Add(new() { coordinates = coordinates, elevation = elevation });
            }
        }

        public class TrailStep
        {
            public int elevation;
            public (int x, int y) coordinates;
        }
    }
}
