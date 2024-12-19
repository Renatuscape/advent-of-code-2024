using static AoC_24_12.Program;

namespace AoC_24_12
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool useExampleInput = true;
            string[] input;

            if (useExampleInput)
            {
                input = File.ReadAllLines("example.txt");
            }
            else
            {
                input = File.ReadAllLines("input.txt");
            }


            // PART 1
            List<List<char>> map = new();
            List<(int x, int y)> usedCoords = new();

            foreach (var line in input)
            {
                map.Add(line.ToCharArray().ToList());
            }

            List<Region> regions = new() { new() { type = map[0][0] } };
            List<Tile> tiles = new();

            // Generate tiles
            for (int y = 0; y < map.Count; y++)
            {
                for (int x = 0; x < map[y].Count; x++)
                {
                    Tile newTile = new() { type = map[y][x], coords = (x, y) };
                    tiles.Add(newTile);
                }
            }

            // Divide into regions
            // TODO: Fix tile division. Currently does not catch all tiles belonging to the same region
            // Split tiles based on type and sort types into regions based on whether they touch any tile in that region
            foreach (var tile in tiles)
            {
                bool isAdded = false;
                Console.WriteLine("\n" + new string('-', 30) + "\nChecking " + tile.ToString());
                for (int r = 0; r < regions.Count; r++)
                {
                    if (regions[r].type == tile.type)
                    {
                        // Check if an adjacent tile exists
                        var adjacentCoordinates = GetAdjacentCoordinates(tile);

                        for (int i = 0; i < adjacentCoordinates.Length; i++)
                        {
                            // Check if tile matches with tiles that have been added to region
                            if (regions[r].Tiles.Count == 0 || regions[r].Tiles.FirstOrDefault(t => t.coords == adjacentCoordinates[i]) != null)
                            {
                                // Check if tile has adjectent tile on map
                                if (regions[r].AddTile(tile))
                                {
                                    isAdded = true;
                                    break;
                                }
                            }
                            // Check if a tile that has yet to be added might connect to the region
                            //else if (tile.coords.x + 1 < map[0].Count && map[tile.coords.y][tile.coords.x + 1] == tile.type)
                            //{
                            //    // Check if tile has adjectent tile on map
                            //    if (regions[r].AddTile(tile))
                            //    {
                            //        isAdded = true;
                            //        break;
                            //    }
                            //}
                        }
                    }
                }

                if (!isAdded)
                {
                    Region newRegion = new();
                    regions.Add(newRegion);
                    if (!newRegion.AddTile(tile))
                    {
                        Console.WriteLine("Could not add tile to newly created region.");
                    }
                }
            }

            // Erect fencing in each region

            // Draw the regions for visualisation
            foreach (var region in regions)
            {
                Console.WriteLine("\n------------------------\nREGION: " + region.type + region.startingCoords);
                DrawRegion(region);
            }

            // Count up tiles and fences
        }

        public class Tile
        {
            public (int x, int y) coords;
            public char type;
            public (bool N, bool S, bool E, bool W) fencing;

            public int GetFencing()
            {
                int count = new[] { fencing.N, fencing.S, fencing.E, fencing.W }.Count(b => b);
                Console.WriteLine($"Number of fenced sides: {count}");
                return count;
            }

            public override string ToString()
            {
                return type + coords.ToString();
            }
        }

        public class Region
        {
            public char type;
            public (int x, int y) startingCoords;
            public List<Tile> Tiles { get; private set; } = new();

            public bool AddTile(Tile tile)
            {
                // If this is the first tile, add and set region data
                if (Tiles.Count == 0)
                {
                    startingCoords = tile.coords;
                    type = tile.type;
                    Tiles.Add(tile);
                    return true;
                }
                else if (tile.type == type)
                {
                    Tiles.Add(tile);
                    return true;
                }

                return false;
            }

            public void SetFencing()
            {
                foreach (Tile tile in Tiles)
                {
                    tile.fencing = (false, false, false, false);

                    // Get all surrounding coordinates for the tile
                    var adjacentCoordinates = GetAdjacentCoordinates(tile);

                    for (int i = 0; i < adjacentCoordinates.Length; i++)
                    {
                        // Check if an adjacent tile exists in the region
                        if (Tiles.FirstOrDefault(t => t.coords == adjacentCoordinates[i]) == null)
                        {
                            //If no adjacent tile exists, turn on fence for the corresponding direction
                            if (i == 0)
                            { tile.fencing.N = true; }
                            else if (i == 1)
                            { tile.fencing.S = true; }
                            else if (i == 2)
                            { tile.fencing.E = true; }
                            else if (i == 3)
                            { tile.fencing.W = true; }
                        }
                    }
                }
            }
        }

        public static (int x, int y)[] GetAdjacentCoordinates(Tile tile)
        {
            return [
                (tile.coords.x, tile.coords.y -1), // N (index 0)
                (tile.coords.x, tile.coords.y +1), // S (index 1)
                (tile.coords.x +1, tile.coords.y), // E (index 2)
                (tile.coords.x -1, tile.coords.y), // W (index 3)
            ];
        }

        public static void DrawRegion(Region region)
        {
            Tile? prevTile = null;
            Console.Write("\n");

            Console.Write(new string(' ', region.Tiles[0].coords.x));

            foreach (var tile in region.Tiles)
            {
                string spacing = new string(' ', tile.coords.x);

                if (prevTile != null && prevTile.coords.y != tile.coords.y)
                {
                    Console.Write("\n" + spacing);
                }
                else if (prevTile != null && prevTile.coords.x +1 != tile.coords.x)
                {
                    Console.Write(new string(' ', tile.coords.x - prevTile.coords.x -1));
                }

                // Draw tile in a 3x3 grid later, to display fencing
                Console.Write(tile.type);

                prevTile = tile;
            }
            Console.Write("\n");

            //foreach (var tile in region.Tiles)
            //{
            //    Console.WriteLine(tile.coords);
            //}
        }
    }
}
