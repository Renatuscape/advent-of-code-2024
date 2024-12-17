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

            Console.WriteLine("Score equals: " + SolvePart1(MapArchive.originalMap));
            Console.WriteLine("Goal was: " + (useExampleInput ? "36" : "???"));
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

            Console.WriteLine("TRAILHEADS");
            List<Trail> foundTrails = new();
            //GET ALL TRAILS FROM TRAILHEADS
            foreach (Trailhead trailhead in trailheads)
            {
                var trails = trailhead.GetAllTrails();
                foundTrails.AddRange(trails);

                Console.WriteLine($"\nTotal of {trails.Count} trails found for trailhead {trailhead.startingCoordinates}:");

                foreach (var trail in trails)
                {
                    Console.Write("\n");
                    foreach (var step in trail.Steps)
                    {
                        Console.Write(step.coordinates + $" E{step.elevation}  ");
                    }
                }
                Console.Write("\n\n");
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
                    Console.WriteLine("\n------------------------------" +
                        "\nSEARCHING AT ELEVATION " + desiredElevation);
                    (int x, int y) curCoords = trail.Steps[desiredElevation - 1].coordinates;
                    List<(int x, int y)> searchArea = GetViableCoordinates(curCoords, desiredElevation);

                    Console.WriteLine("Search area found with starting coordinates " + curCoords + ": ");
                    foreach (var coord in searchArea)
                    {
                        Console.Write(coord + " ");
                    }
                    Console.Write("\n");

                    if (searchArea.Count == 1) // One new step was found. Expand existing trail
                    {
                        Console.WriteLine("Found one step for elevation " + desiredElevation);
                        trail.AddStep(desiredElevation, searchArea[0]);
                    }
                    else if (searchArea.Count > 1) // More than one step was found. Expand trail and add copies with new steps
                    {
                        Console.WriteLine($"Found {searchArea.Count} steps for elevation " + desiredElevation);

                        for (int i = 1; i < searchArea.Count; i++)// Create new trail for each coordinate
                        {
                            Console.WriteLine("Adding new trail for new step " + searchArea[i]);
                            Trail newTrail = new Trail();

                            // Deep copy ALL existing steps from the original trail
                            foreach (var step in trail.Steps)
                            {
                                newTrail.Steps.Add(new TrailStep
                                {
                                    coordinates = step.coordinates,
                                    elevation = step.elevation
                                });
                            }

                            // Add the new step at the current elevation
                            newTrail.AddStep(desiredElevation, searchArea[i]);

                            newTrails.Add(newTrail);
                        }

                        trail.AddStep(desiredElevation, searchArea[0]);
                    }
                    else
                    {
                        deadEndTrails.Add(trail);
                        Console.WriteLine("NO VIABLE NEXT STEP found for elevation " + desiredElevation);
                    }
                }

                // Remove dead ends
                foreach (Trail deadEnd in deadEndTrails)
                {
                    Console.WriteLine("Removing dead end: " + deadEnd.PrintSteps());
                    foundTrails.Remove(deadEnd);
                }
                deadEndTrails.Clear();

                foreach (Trail newTrail in newTrails)
                {
                    Console.WriteLine("Adding new trail: " + newTrail.PrintSteps());
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
            TrailStep firstStep = new TrailStep() { elevation = 0, coordinates = startingCoords };
            newTrail.Steps.Add(firstStep);

            return newTrail;
        }

        public static List<(int x, int y)> GetViableCoordinates((int x, int y) curCoords, int desiredElevation)
        {
            var searchArea = GetSearchArea(curCoords, MapArchive.originalMap.Count, MapArchive.originalMap[0].Count);
            var filteredArea = FilterSearchAreaForElevation(searchArea, desiredElevation);
            Console.WriteLine($"FILTERED SEARCH AREA: {string.Join(" ", filteredArea)} e{desiredElevation}");
            return filteredArea;
        }

        public static List<(int x, int y)> FilterSearchAreaForElevation(List<(int x, int y)> searchArea, int desiredElevation)
        {
            List<(int x, int y)> validCoords = new();
            foreach (var coords in searchArea)
            {
                Console.WriteLine($"Checking coordinates: {coords}, Desired Elevation: {desiredElevation}");

                if (coords.y >= 0 &&
                    coords.y < MapArchive.originalMap.Count &&
                    coords.x >= 0 &&
                    coords.x < MapArchive.originalMap[0].Count)
                {
                    int actualElevation = MapArchive.originalMap[coords.y][coords.x];
                    Console.WriteLine($"Actual Elevation at {coords}: {actualElevation}");

                    if (actualElevation == desiredElevation)
                    {
                        validCoords.Add(coords);
                        Console.WriteLine($"Valid coordinate added: {coords}");
                    }
                }
                else
                {
                    Console.WriteLine($"Coordinate {coords} out of bounds");
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
            public List<TrailStep> Steps { get; private set; } = new();

            public void AddStep(int elevation, (int x, int y) coordinates)
            {
                Steps.Add(new() { coordinates = coordinates, elevation = elevation });
            }

            public string PrintSteps()
            {
                return string.Join(" ", Steps.Select(step => $"{step.coordinates}"));
            }
        }

        public class TrailStep
        {
            public int elevation;
            public (int x, int y) coordinates;
        }
    }
}
