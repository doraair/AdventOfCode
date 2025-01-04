namespace AdventOfCode.Day15
{
    using System.Drawing;
    using System.Linq;

    internal class PartTwo
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
            PrintMap();
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
                    if (map[y, x] == '[')
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
                Point halfLeft;
                Point halfRight;
                var item = map[newPosition.Y, newPosition.X];
                if (item == '[')
                {
                    halfLeft = newPosition;
                    halfRight = new Point(newPosition.X + 1, newPosition.Y);
                }
                else
                {
                    halfRight = newPosition;
                    halfLeft = new Point(newPosition.X - 1, newPosition.Y);
                }
                switch (movement)
                {
                    case Movement.Up:
                    case Movement.Down:
                        return CanMove(halfRight, movement) && CanMove(halfLeft, movement);
                    case Movement.Left:
                        return CanMove(halfLeft, movement);
                    case Movement.Right:
                        return CanMove(halfRight, movement);
                }

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
                Point theOtherBox;
                var item = map[newPosition.Y, newPosition.X];
                theOtherBox = new Point(newPosition.X + (item == '[' ? 1 : -1), newPosition.Y);
                Move(theOtherBox, movement);
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
            var item = map[position.Y, position.X];
            return item == '[' || item == ']';
        }

        private Movement GetMovement(char move)
        {
            return move switch
            {
                '^' => Movement.Up,
                'v' => Movement.Down,
                '<' => Movement.Left,
                '>' => Movement.Right,
                _ => throw new InvalidOperationException(),
            };
        }

        private void GetInput()
        {
            var lines = File.ReadLines(input);

            var mapLine = lines
                 .Where(line => line.StartsWith('#'))
                 .ToList();

            map = new char[mapLine.Count, mapLine[0].Length * 2];

            for (var y = 0; y < mapLine.Count; y++)
            {
                var xPostion = 0;
                for (var x = 0; x < mapLine[y].Length; x++)
                {
                    var item = mapLine[y][x];
                    if (item == '#' || item == '.')
                    {
                        map[y, xPostion] = item;
                        xPostion++;
                        map[y, xPostion] = item;

                    }
                    else if (item == 'O')
                    {
                        map[y, xPostion] = '[';
                        xPostion++;
                        map[y, xPostion] = ']';
                    }
                    else if (item == '@')
                    {
                        robotStartingPoint = new Point(xPostion, y);
                        map[y, xPostion] = '@';
                        xPostion++;
                        map[y, xPostion] = '.';
                    }

                    xPostion++;
                }
            }

            PrintMap();

            var movesLIne = lines.Where(line => !string.IsNullOrEmpty(line) && !line.StartsWith('#')).ToList();
            moves = string.Join("", movesLIne);
        }
    }
}
