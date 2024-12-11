using System;

namespace AoC_24_6
{
    internal class Program
    {
        public enum Direction
        {
            // Index is used for 90 degree turns
            Forward = 0,
            Right = 1,
            Back = 2,
            Left = 3,
        }
        public class Coordinates
        {
            public int x = -1;
            public int y = -1;
        }

        static void Main(string[] args)
        {
            string[] input = File.ReadAllLines("input-aoc-24-6.txt");
            List<List<char>> map = new();
            bool enableAnimation = false;
            bool delay = false;

            foreach (var line in input)
            {
                map.Add(line.ToList());
            }

            Direction direction = GameController.GetInitialDirection();

            Coordinates position = GameController.GetCoordinates(direction, map);

            if (enableAnimation)
            {
                GameController.PrintMap(map);
                Console.WriteLine(position.x + "|" + position.y);
                //Thread.Sleep(500);
            }

            int steps = 0;
            while (position.x > -1 && position.y > -1)
            {
                steps++;

                if (GameController.CheckIfCellIsClear(direction, position, map))
                {
                    try
                    {
                        GameController.Move(direction, position, map);

                        if (enableAnimation)
                        {
                            GameController.PrintMap(map);
                            Console.WriteLine(position.x + "|" + position.y);
                            if (delay){ Thread.Sleep(100); }
                        }
                        else
                        {
                            Console.WriteLine("Steps: " + steps);
                        }
                    }
                    catch
                    {
                        break;
                    }
                }
                else
                {
                    GameController.TurnRight(ref direction, position, map);

                    if (enableAnimation)
                    {
                        GameController.PrintMap(map);
                        Console.WriteLine(position.x + "|" + position.y);
                        if (delay) { Thread.Sleep(100); }
                    }
                    else
                    {
                        Console.WriteLine("Steps: " + steps + " (turn)");
                    }
                }
            }

            Console.WriteLine("\nCompleted run. Calculating score...");
            int score = 0;

            foreach (var row in map)
            {
                foreach (var col in row)
                {
                    if (col == 'X')
                    {
                        score++;
                    }
                }
            }

            if (!enableAnimation)
            {
                GameController.PrintMap(map);
            }

            Console.WriteLine("\nFINAL SCORE IS: " + score);
        }

        public static class GameController
        {
            public static void PrintMap(List<List<char>> map)
            {
                //Console.Clear();

                foreach (var row in map)
                {
                    foreach (var col in row)
                    {
                        Console.Write(col.ToString());
                    }
                    Console.Write("\n");
                }
            }

            public static Direction GetDirectionFromChar(char ch)
            {
                if (ch == '^')
                {
                    return Direction.Forward;
                }
                else if (ch == 'v')
                {
                    return Direction.Back;
                }
                else if (ch == '<')
                {
                    return Direction.Left;
                }
                else if (ch == '>')
                {
                    return Direction.Right;
                }
                else
                {
                    Console.WriteLine("ERROR! Direction could not be parsed from char: " + ch);
                    return Direction.Forward;
                }
            }

            public static char GetCharFromDirection(Direction dir)
            {
                if (dir == Direction.Forward)
                {
                    return '^';
                }
                else if (dir == Direction.Back)
                {
                    return 'v';
                }
                else if (dir == Direction.Left)
                {
                    return '<';
                }
                else if (dir == Direction.Right)
                {
                    return '>';
                }
                else
                {
                    Console.WriteLine("ERROR! Direction could not be parsed into char: " + dir);
                    throw new Exception("Error: Direction could not be parsed");
                }
            }

            public static Direction GetInitialDirection()
            {
                char foundGuard = '^';

                // Add logic to work with multiple starting positions

                return GetDirectionFromChar(foundGuard);
            }

            public static Coordinates GetCoordinates(Direction currentDirection, List<List<char>> map)
            {
                Coordinates coords = new Coordinates();
                char direction = GetCharFromDirection(currentDirection);

                for (int y = 0; y < map.Count; y++)
                {
                    for (int x = 0; x < map[y].Count; x++)
                    {
                        if (map[y][x] == direction)
                        {
                            coords.x = x;
                            coords.y = y;
                            break;
                        }
                    }

                    if (coords.x >= 0)
                    {
                        break;
                    }

                }

                return coords;
            }

            // Check next cell
            public static bool CheckIfCellIsClear(Direction direction, Coordinates coords, List<List<char>> map)
            {
                int x = coords.x;
                int y = coords.y;

                if (direction == Direction.Left)
                {
                    x--;
                }
                else if (direction == Direction.Right)
                {
                    x++;
                }
                else if (direction == Direction.Forward)
                {
                    y--;
                }
                else if (direction == Direction.Back)
                {
                    y++;
                }

                try
                {
                    if (map[y][x] != '#')
                    {
                        return true;
                    }
                }
                catch
                {
                    Console.WriteLine("Coordinates were out of bounds");
                    return true;
                }

                return false;
            }

            // Turn
            public static void TurnRight(ref Direction direction, Coordinates coords, List<List<char>> map)
            {
                int directionIndex = (int)direction;

                directionIndex++; // Make a right turn

                if (directionIndex >= Enum.GetNames(typeof(Direction)).Length)
                {
                    directionIndex = 0;
                }

                direction = (Direction)directionIndex;
                map[coords.y][coords.x] = GetCharFromDirection(direction);
            }

            // Move safely
            public static void Move(Direction direction, Coordinates coords, List<List<char>> map)
            {
                map[coords.y][coords.x] = 'X';

                if (direction == Direction.Left)
                {
                    coords.x--;
                }
                else if (direction == Direction.Right)
                {
                    coords.x++;
                }
                else if (direction == Direction.Forward)
                {
                    coords.y--;
                }
                else if (direction == Direction.Back)
                {
                    coords.y++;
                }

                try
                {
                    map[coords.y][coords.x] = GetCharFromDirection(direction);
                }
                catch
                {
                    Console.WriteLine("Attempted movement out of bounds");
                    throw new Exception("Error: Movement out of range");
                }
            }
        }
    }
}
