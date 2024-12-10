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

            var diagonalPatterns = WordSearcher.FindDiagonalPatterns(grid);
            foreach (var pattern in diagonalPatterns)
            {
                score += WordSearcher.LineReader(pattern);
            }

            Console.WriteLine("Part 1 found " + score + " matches.");

            // PART 2
            List<XMas> xMaslist = new List<XMas>();

            for (int i = 0; i < input.Length; i++) {

                if (i + 2 < input.Length)
                {
                    for (int j = 0; j < input[i].Length; j++)
                    {
                        if (j + 2 < input[i].Length)
                        {
                            var xMas = new XMas();
                            xMaslist.Add(xMas);

                            xMas.row1 = input[i][j].ToString() + "." + input[i][j + 2].ToString();
                            xMas.row2 = "." + input[i+1][j + 1].ToString() + ".";
                            xMas.row3 = input[i + 2][j].ToString() + "." + input[i + 2][j + 2].ToString();
                        }
                    }
                }
            }

            xMaslist = WordSearcher.CleanXMasList(xMaslist);
            Console.WriteLine("Part 2 found " + xMaslist.Count + " matches.");
        }

        internal static class WordSearcher
        {
            internal static List<XMas> CleanXMasList(List<XMas> xMaslist)
            {
                var cleanList = new List<XMas>();

                foreach (var xMas in xMaslist)
                {
                    if (xMas.row2.Length < 3)
                    {
                        xMas.Print();
                    }
                    else if (xMas.row2[1] == 'A')
                    {
                        if ((xMas.row1[0] == 'M' && xMas.row3[2] == 'S')||
                            (xMas.row1[0] == 'S' && xMas.row3[2] == 'M'))
                        {
                            if ((xMas.row1[2] == 'S' && xMas.row3[0] == 'M')||
                                (xMas.row1[2] == 'M' && xMas.row3[0] == 'S'))
                            {
                                cleanList.Add(xMas);
                                xMas.Print();
                            }
                        }
                    }
                }

                return cleanList;
            }

            internal static int LineReader(string input)
            {
                int xmasCount = input.Split(new[] { "XMAS" }, StringSplitOptions.None).Length - 1;
                int samxCount = input.Split(new[] { "SAMX" }, StringSplitOptions.None).Length - 1;

                return xmasCount + samxCount;
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

            internal static List<string> FindDiagonalPatterns(char[,] grid)
            {
                var patterns = new List<string>();
                int rows = grid.GetLength(0);
                int cols = grid.GetLength(1);

                // Down-right diagonals
                for (int startRow = 0; startRow < rows; startRow++)
                {
                    for (int startCol = 0; startCol < cols; startCol++)
                    {
                        string xmasDiagonal = GetDiagonalPattern(grid, startRow, startCol, 1, 1, "XMAS");
                        string samxDiagonal = GetDiagonalPattern(grid, startRow, startCol, 1, 1, "SAMX");

                        if (xmasDiagonal != null) patterns.Add(xmasDiagonal);
                        if (samxDiagonal != null) patterns.Add(samxDiagonal);
                    }
                }

                // Down-left diagonals
                for (int startRow = 0; startRow < rows; startRow++)
                {
                    for (int startCol = 0; startCol < cols; startCol++)
                    {
                        string xmasDiagonal = GetDiagonalPattern(grid, startRow, startCol, 1, -1, "XMAS");
                        string samxDiagonal = GetDiagonalPattern(grid, startRow, startCol, 1, -1, "SAMX");

                        if (xmasDiagonal != null) patterns.Add(xmasDiagonal);
                        if (samxDiagonal != null) patterns.Add(samxDiagonal);
                    }
                }

                return patterns;
            }

            private static string GetDiagonalPattern(char[,] grid, int startRow, int startCol, int rowStep, int colStep, string pattern)
            {
                int rows = grid.GetLength(0);
                int cols = grid.GetLength(1);

                for (int i = 0; i <= pattern.Length - 1; i++)
                {
                    int r = startRow + i * rowStep;
                    int c = startCol + i * colStep;

                    if (r < 0 || r >= rows || c < 0 || c >= cols || grid[r, c] != pattern[i])
                    {
                        return null;
                    }
                }

                return pattern;
            }
        }

        internal class XMas
        {
            public string row1 = "";
            public string row2 = "";
            public string row3 = "";

            public void Print()
            {
                Console.WriteLine($"|{row1}|");
                Console.WriteLine($"|{row2}|");
                Console.WriteLine($"|{row3}|\n");
            }
        }
    }
}
