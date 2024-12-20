namespace AoC_24_12
{
    internal partial class Program
    {
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

            public int GetArea()
            {
                return Tiles.Count;
            }

            public int GetCircumference()
            {
                int fences = 0;

                foreach (Tile t in Tiles)
                {
                    fences += t.GetFencing();
                }

                return fences;
            }

            public int GetScore()
            {
                return GetArea() * GetCircumference();
            }
        }
    }
}
