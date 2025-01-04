namespace AdventOfCode.Day15
{
    using System.Drawing;
    using System.Linq;

    internal class PartOne
    {
        private const string input = @"D:\source-code-poc\AdventOfCode\Day15\input.txt";
        private char[,] map;
        private string moves = string.Empty;
        private Point robotStartingPoint;


        public void Run()
        {
            GetInput();
            foreach (var move in moves)
            {
                if (CanMove(robotStartingPoint, GetMovement(move)))
                {
                    robotStartingPoint = Move(robotStartingPoint, GetMovement(move));
                    continue;
                }
            }

            GetTotalBoxGps();
        }

        private void PrintMap()
        {

            for (var y = 0; y < map.GetLength(0); y++)
            {
                for (var x = 0; x < map.GetLength(1); x++)
                {
                    Console.Write(map[y, x]);
                }
                Console.WriteLine();
            }
        }

        private void GetTotalBoxGps()
        {
            var totalBoxGps = 0;
            for (var y = 0; y < map.GetLength(0); y++)
            {
                for (var x = 0; x < map.GetLength(1); x++)
                {
                    if (map[y, x] == 'O')
                    {
                        totalBoxGps += 100 * y + x;
                    }
                }

            }
            Console.WriteLine($"Total Box Gps - {totalBoxGps}");
        }

        private bool CanMove(Point position, Movement movement)
        {
            var newPosition = PointExtension.Move(position, movement);
            if (IsWall(newPosition))
            {
                return false;
            }
            if (IsBox(newPosition))
            {
                return CanMove(newPosition, movement);
            }

            return true;
        }

        private Point Move(Point position, Movement movement)
        {
            var newPosition = PointExtension.Move(position, movement);
            if (IsWall(newPosition))
            {
                return position;
            }
            if (IsBox(newPosition))
            {
                Move(newPosition, movement);
            }

            map[newPosition.Y, newPosition.X] = map[position.Y, position.X];
            map[position.Y, position.X] = '.';
            return newPosition;
        }

        private bool IsWall(Point position)
        {
            return map[position.Y, position.X] == '#';
        }

        private bool IsBox(Point position)
        {
            return map[position.Y, position.X] == 'O';
        }

        private Movement GetMovement(char move)
        {
            switch (move)
            {
                case '^':
                    return Movement.Up;
                case 'v':
                    return Movement.Down;
                case '<':
                    return Movement.Left;
                case '>':
                    return Movement.Right;
                default:
                    throw new InvalidOperationException();
            };
        }

        private void GetInput()
        {
            var lines = File.ReadLines(input);

            var mapLine = lines
                 .Where(line => line.StartsWith('#'))
                 .ToList();

            map = new char[mapLine.Count, mapLine[0].Length];

            for (var y = 0; y < mapLine.Count; y++)
            {
                for (var x = 0; x < mapLine[y].Length; x++)
                {
                    map[y, x] = mapLine[y][x];
                    if (mapLine[y][x] == '@')
                    {
                        robotStartingPoint = new Point(x, y);
                    }
                }
            }

            var movesLIne = lines.Where(line => !string.IsNullOrEmpty(line) && !line.StartsWith('#')).ToList();
            moves = string.Join("", movesLIne);
        }
    }
}
