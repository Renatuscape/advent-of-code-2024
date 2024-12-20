namespace AoC_24_12
{
    internal partial class Program
    {
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
    }
}
