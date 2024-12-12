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

        public class Turn
        {
            public Direction direction;
            public Coordinates coords = new Coordinates();
        }

        static void Main(string[] args)
        {
            string[] input = File.ReadAllLines("input-aoc-24-6.txt");

            bool enableAnimation = true;
            bool delay = true;

            foreach (var line in input)
            {
                GameController.map.Add(line.ToList());
                GameController.mapCopy.Add(line.ToList());
            }

            GameController.direction = GameController.GetInitialDirection();
            GameController.coords = GameController.GetCoordinates(GameController.direction, GameController.map);

            if (enableAnimation)
            {
                GameController.PrintMap(GameController.map);
                Console.WriteLine(GameController.coords.x + "|" + GameController.coords.y);
                if (delay && enableAnimation)
                {
                    Thread.Sleep(500);
                }
            }

            // PART 1
            GameController.PlayPatrol(enableAnimation, delay);
            GameController.CalculateStepScore(!enableAnimation);


            //PART 2
            GameController.ResetBoard();

            int infiniteLoopsFound = 0;
            for (int y = 0; y < GameController.map.Count; y++)
            {
                for (int x = 0; x < GameController.map[y].Count; x++)
                {
                    if (GameController.PlaceObstacle(x, y))
                    {
                        var gameCompleted = GameController.PlayPatrol(enableAnimation, delay);

                        if (!gameCompleted)
                        {
                            infiniteLoopsFound++;
                            Console.WriteLine($"Found infinite loop #{infiniteLoopsFound} at " + x + "," + y);
                        }
                    }
                    GameController.ResetBoard();
                }
            }

            Console.WriteLine("\nFOUND INFINITE LOOPS: " + infiniteLoopsFound);
        }

        public static class GameController
        {
            public static List<List<char>> mapCopy = new();
            public static List<List<char>> map = new();
            public static List<Turn> turnCoords = new();
            public static Direction direction;
            public static Coordinates coords = new();

            public static void ResetBoard()
            {
                map.Clear();

                foreach (var line in mapCopy)
                {
                    map.Add(line.ToList());
                }

                direction = GetInitialDirection();
                coords = GetCoordinates(direction, map);
                turnCoords.Clear();
            }

            public static bool PlaceObstacle(int x, int y)
            {
                if (map[y][x] != GetCharFromDirection(direction) && map[y][x] != '#')
                {
                    map[y][x] = 'O';
                    return true;
                }

                return false;
            }

            public static bool PlayPatrol(bool enableAnimation, bool delay)
            {
                //int steps = 0;
                while (coords.x > -1 && coords.y > -1)
                {
                    //steps++;

                    if (CheckIfCellIsClear(direction, coords, map))
                    {
                        try
                        {
                            Move(direction, coords, map);

                            if (enableAnimation)
                            {
                                PrintMap(map);
                                Console.WriteLine(coords.x + "|" + coords.y);
                                if (delay && enableAnimation) { Thread.Sleep(100); }
                            }
                            //else
                            //{
                            //    Console.WriteLine("Steps: " + steps);
                            //}
                        }
                        catch
                        {
                            break;
                        }
                    }
                    else
                    {
                        if (turnCoords.FirstOrDefault(t => t.direction == direction && t.coords.x == coords.x && t.coords.y == coords.y) == null)
                        {
                            turnCoords.Add(new() { direction = direction, coords = new() { x = coords.x, y = coords.y } });

                            TurnRight(ref direction, coords, map);

                            if (enableAnimation)
                            {
                                PrintMap(map);
                                Console.WriteLine(coords.x + "|" + coords.y);
                                if (delay && enableAnimation) { Thread.Sleep(100); }
                            }
                            //else
                            //{
                            //    Console.WriteLine("Steps: " + steps + " (turn)");
                            //}
                        }
                        else
                        {
                            //Console.WriteLine("FOUND REPEATING TURN");
                            return false; // Play ended in infinite loop
                        }
                    }
                }

                return true; // Play did not end in infinite loop
            }


            public static void CalculateStepScore(bool printMap)
            {
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

                if (printMap)
                {
                    PrintMap(map);
                }

                Console.WriteLine("\nFINAL SCORE IS: " + score);
            }
            public static void PrintMap(List<List<char>> map)
            {
                Console.Clear();

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

                foreach (var row in map)
                {
                    foreach (var col in row)
                    {
                        if (col != '#' && col != '.' && col != 'O')
                        {
                            foundGuard = col;
                            break;
                        }
                    }
                }

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
                    if (map[y][x] != '#' && map[y][x] != 'O')
                    {
                        return true;
                    }
                }
                catch
                {
                    //Console.WriteLine("Coordinates were out of bounds");
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
                    //Console.WriteLine("Attempted movement out of bounds");
                    throw new Exception("Error: Movement out of range");
                }
            }
        }
    }
}
