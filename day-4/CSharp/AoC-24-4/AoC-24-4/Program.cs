using System.Text;

namespace AoC_24_4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] input = File.ReadAllLines("input-aoc-24-4.txt");
            int score = 0;

            char[,] grid = new char[input.Length, input[0].Length];

            for (int r = 0; r < input.Length; r++)
            {
                for (int c = 0; c < input[r].Length; c++)
                {
                    grid[r, c] = input[r][c];
                }
            }

            //WordSearcher.PrintGrid(grid);

            var rows = WordSearcher.GetRowStrings(grid);
            foreach (var row in rows)
            {
                score += WordSearcher.LineReader(row);
            }

            var columns = WordSearcher.GetColumnStrings(grid);
            foreach (var column in columns)
            {
                score += WordSearcher.LineReader(column);
            }

            var diagonals = WordSearcher.GetColumnStrings(grid);
            foreach (var diagonal in diagonals)
            {
                score += WordSearcher.LineReader(diagonal);
            }

            Console.WriteLine("Found " + score + " matches.");
        }

        internal static class WordSearcher
        {
            internal static int LineReader(string input)
            {
                int score = input.Split(new[] { "XMAS" }, StringSplitOptions.None).Length - 1;
                score += input.Split(new[] { "SAMX" }, StringSplitOptions.None).Length - 1;

                return score;
            }

            internal static void PrintGrid(char[,] grid)
            {
                for (int r = 0; r < grid.GetLength(0); r++)
                {
                    for (int c = 0; c < grid.GetLength(1); c++)
                    {
                        Console.Write(grid[r, c]);
                    }
                    Console.WriteLine(); // New line after each row
                }
            }

            internal static List<string> GetRowStrings(char[,] grid)
            {
                var rows = new List<string>();
                for (int r = 0; r < grid.GetLength(0); r++)
                {
                    rows.Add(string.Concat(Enumerable.Range(0, grid.GetLength(1))
                        .Select(c => grid[r, c])));
                }
                return rows;
            }

            internal static List<string> GetColumnStrings(char[,] grid)
            {
                var columns = new List<string>();
                for (int c = 0; c < grid.GetLength(1); c++)
                {
                    columns.Add(string.Concat(Enumerable.Range(0, grid.GetLength(0))
                        .Select(r => grid[r, c])));
                }
                return columns;
            }

            internal static List<string> GetDiagonalStrings(char[,] grid)
            {
                var diagonals = new List<string>();
                int rows = grid.GetLength(0);
                int cols = grid.GetLength(1);

                // Main diagonals
                for (int r = 0; r < rows; r++)
                {
                    var diagonal = new StringBuilder();
                    for (int i = 0; r + i < rows && i + r < cols; i++)
                    {
                        diagonal.Append(grid[r + i, i]);
                    }
                    if (diagonal.Length > 1) diagonals.Add(diagonal.ToString());
                }

                for (int c = 1; c < cols; c++)
                {
                    var diagonal = new StringBuilder();
                    for (int i = 0; i + c < cols && i < rows; i++)
                    {
                        diagonal.Append(grid[i, c + i]);
                    }
                    if (diagonal.Length > 1) diagonals.Add(diagonal.ToString());
                }

                return diagonals;
            }
        }
    }
}
