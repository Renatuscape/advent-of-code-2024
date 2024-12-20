using static AoC_24_12.Program;

namespace AoC_24_12
{
    internal class Program
    {
        public static List<List<char>> map = new();
        public static List<Region> regions = new();
        public static List<Tile> tiles = new();
        public static List<char> types = new();

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
            
            ConstructMap(input);
            GenerateTiles();

            // Divide into regions
            FindRegions();

            // Erect fencing in each region
            foreach (var region in regions)
            {
                region.SetFencing();
            }

            //PrintRegionCells(regions[0]);
            // Draw the regions for visualisation
            //foreach (var region in regions)
            //{
            //    PrintRegionCells(region);
            //}
            PrintMapCells();

            // Count up tiles and fences

            foreach (var region in regions)
            {

            }
        }

        public static void ConstructMap(string[] input)
        {
            // Construct map
            foreach (var line in input)
            {
                map.Add(line.ToCharArray().ToList());
            }
        }

        public static void GenerateTiles()
        {
            // Generate tiles
            for (int y = 0; y < map.Count; y++)
            {
                for (int x = 0; x < map[y].Count; x++)
                {
                    Tile newTile = new() { type = map[y][x], coords = (x, y) };
                    tiles.Add(newTile);
                    types.Add(newTile.type);
                }
            }
        }
        public static void FindRegions()
        {
            types = types.Distinct().ToList();
            HashSet<(int x, int y)> visitedCoordinates = new();

            foreach (var tile in tiles)
            {
                // Skip if we've already processed this tile
                if (visitedCoordinates.Contains(tile.coords))
                    continue;

                // Create a new region for each unvisited tile we find
                Region newRegion = new();
                regions.Add(newRegion);

                // Use flood fill to find all connected tiles of the same type
                FloodFillRegion(tile, newRegion, visitedCoordinates);
            }
        }

        private static void FloodFillRegion(Tile startTile, Region region, HashSet<(int x, int y)> visitedCoordinates)
        {
            // If we've visited this tile or it's the wrong type, stop
            if (visitedCoordinates.Contains(startTile.coords))
                return;

            // Add the tile to the region and mark as visited
            if (!region.AddTile(startTile))  // If we couldn't add the tile, stop
                return;

            visitedCoordinates.Add(startTile.coords);

            // Get all adjacent coordinates
            var adjacentCoordinates = GetAdjacentCoordinates(startTile);

            // Check each adjacent coordinate
            foreach (var adjCoord in adjacentCoordinates)
            {
                // Skip if out of bounds
                if (adjCoord.x < 0 || adjCoord.y < 0 ||
                    adjCoord.y >= map.Count || adjCoord.x >= map[0].Count)
                    continue;

                // Find the tile at these coordinates
                var adjacentTile = tiles.FirstOrDefault(t => t.coords == adjCoord);
                if (adjacentTile != null && adjacentTile.type == startTile.type)
                {
                    FloodFillRegion(adjacentTile, region, visitedCoordinates);
                }
            }
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
            public List<Tile> Tiles { get; set; } = new();

            public bool AddTile(Tile tile)
            {
                // If this is the first tile, set region type
                if (Tiles.Count == 0)
                {
                    type = tile.type;
                }

                // Only add if tile matches region type
                if (tile.type == type)
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

        public static (int x, int y)[] GetFilteredCoordinates(Tile tile)
        {
            return GetAdjacentCoordinates(tile).Where(c =>
            {
                if (c.x < 0 || c.y < 0 || c.x > map[0].Count || c.y >= map.Count)
                {
                    Console.WriteLine("\tFilterC: Filtered out coordinate " + c);
                    return false;
                }
                Console.WriteLine("\tFilterC: Kept coordinate " + c);
                return true;
            }).ToArray();
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

        public static void PrintRegion(Region region)
        {
            Tile? prevTile = null;
            var orderedTiles = region.Tiles.OrderBy(t => t.coords.y).ThenBy(t => t.coords.x).ToList();

            Console.WriteLine("\n------------------------\nREGION: " + region.type + region.startingCoords);
            Console.Write("\n");

            Console.Write(new string(' ', orderedTiles[0].coords.x));
            foreach (var tile in orderedTiles)
            {
                string spacing = new string(' ', tile.coords.x);

                if (prevTile != null && prevTile.coords.y != tile.coords.y)
                {
                    Console.Write("\n" + spacing);
                }
                else if (prevTile != null && prevTile.coords.x + 1 != tile.coords.x)
                {
                    Console.Write(new string(' ', (tile.coords.x - prevTile.coords.x) - 1));
                }

                // Draw tile in a 3x3 grid later, to display fencing
                Console.Write(tile.type);

                prevTile = tile;
            }
            Console.Write("\n");
        }

        public static void PrintRegionCells(Region region)
        {
            var orderedTiles = region.Tiles.OrderBy(t => t.coords.y).ThenBy(t => t.coords.x).ToList();
            int maxY = map.Count;
            Dictionary<(int y, int x), string[]> cells = new();

            var emptyCell = new[] {
                $"     ",
                $"     ",
                $"     "
            };

            // Populate dictionary
            for (int y = 0; y < maxY; y++) {
                for (int x = 0; x < map[0].Count; x++) {

                    var foundTile = orderedTiles.FirstOrDefault(t => t.coords.x == x && t.coords.y == y);

                    if (foundTile != null)
                    {
                        cells.Add((y, x), CreateTileCell(foundTile));
                    }
                    else
                    {
                        cells.Add((y, x), emptyCell);
                    }
                }
            }

            // Print from dictionary
            for (int y = 0; y < maxY; y++)
            {
                string line0 = "";
                string line1 = "";
                string line2 = "";
                for (int x = 0; x < map[0].Count; x++)
                {
                    if (cells.TryGetValue((y, x), out var cell))
                    {
                        line0 += cell[0];
                        line1 += cell[1];
                        line2 += cell[2];
                    }
                }
                Console.Write(line0 + "\n");
                Console.Write(line1 + "\n");
                Console.Write(line2 + "\n");
            }

            Console.Write("\n");
        }

        public static void PrintMapCells()
        {
            var orderedTiles = tiles.OrderBy(t => t.coords.y).ThenBy(t => t.coords.x).ToList();
            int maxY = map.Count;
            Dictionary<(int y, int x), string[]> cells = new();

            var emptyCell = new[] {
                $"     ",
                $"     ",
                $"     "
            };

            // Populate dictionary
            for (int y = 0; y < maxY; y++)
            {
                for (int x = 0; x < map[0].Count; x++)
                {

                    var foundTile = orderedTiles.FirstOrDefault(t => t.coords.x == x && t.coords.y == y);

                    if (foundTile != null)
                    {
                        cells.Add((y, x), CreateTileCell(foundTile));
                    }
                    else
                    {
                        cells.Add((y, x), emptyCell);
                    }
                }
            }

            // Print from dictionary
            for (int y = 0; y < maxY; y++)
            {
                string line0 = "";
                string line1 = "";
                string line2 = "";
                for (int x = 0; x < map[0].Count; x++)
                {
                    if (cells.TryGetValue((y, x), out var cell))
                    {
                        line0 += cell[0];
                        line1 += cell[1];
                        line2 += cell[2];
                    }
                }
                Console.Write(line0 + "\n");
                Console.Write(line1 + "\n");
                Console.Write(line2 + "\n");
            }

            Console.Write("\n");
        }


        public static string[] CreateTileCell(Tile tile)
        {
            var t = tile.type;
            var fencing = GetFence(tile);
            var N = fencing[0];
            var S = fencing[1];
            var E = fencing[2];
            var W = fencing[3];
            var A = (N != '.' && W != '.') ? '+' : '.';
            var B = (N != '.' && E != '.') ? '+' : '.';
            var C = (S != '.' && W != '.') ? '+' : '.';
            var D = (S != '.' && E != '.') ? '+' : '.';


            if (N == '-' && W == '.')
            {
                A = '-';
            }
            if (N == '-' && E == '.')
            {
                B = '-';
            }
            if (S == '-' && W == '.')
            {
                C = '-';
            }
            if (S == '-' && E == '.')
            {
                D = '-';
            }

            if (N == '.' && W == '|')
            {
                A = '|';
            }
            if (N == '.' && E == '|')
            {
                B = '|';
            }
            if (S == '.' && W == '|')
            {
                C = '|';
            }
            if (S == '.' && E == '|')
            {
                D = '|';
            }

            return new[] {
                $"{A} {N} {B}",
                $"{W} {t} {E}",
                $"{C} {S} {D}"
            };
        }

        public static char[] GetFence(Tile tile)
        {
            return new[]
            {
                tile.fencing.N ? '-' : '.',
                tile.fencing.S ? '-' : '.',
                tile.fencing.E ? '|' : '.',
                tile.fencing.W ? '|' : '.'
            };
        }
    }
}
