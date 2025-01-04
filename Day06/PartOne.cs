namespace AdventOfCode.Day06
{
    using System;
    using System.Drawing;
    using System.Linq;

    internal class PartOne
    {
        private const string inputPath = "./Day06/input.txt";
        private string[] map;

        internal void Run()
        {
            map = GetInput(inputPath);
            var guardX = -1;
            var guardY = -1;
            var guardFacing = Facing.North;

            /// Find the guard position
            for (int y = 0; y < map.Length; y++)
            {
                guardX = map[y].IndexOf('^');
                if (guardX != -1)
                {
                    guardY = y;
                    break;
                }
            }

            var guardPatrolRoute = FindingGuardRoute(new Point(guardX, guardY), guardFacing);

            for (int y = 0; y < map.Length; y++)
            {
                for (int x = 0; x < map[y].Length; x++)
                {
                    if (guardPatrolRoute.Any(o => o.Position.X == x && o.Position.Y == y))
                    {
                        Console.Write("X");
                    }
                    else
                    {
                        Console.Write(map[y][x]);
                    }
                }
                Console.WriteLine();
            }

            Console.WriteLine($"Total steps - {guardPatrolRoute.GroupBy(o => o.Position).Count()}");
        }

        private void Move(ref Point position, Facing facing)
        {
            var x = 0;
            var y = 0;
            switch (facing)
            {
                case Facing.North:
                    y--;
                    break;
                case Facing.South:
                    y++;
                    break;
                case Facing.West:
                    x--;
                    break;
                case Facing.East:
                    x++;
                    break;
            }
            position.X += x;
            position.Y += y;
        }

        private void TurnRight(ref Facing facing)
        {
            switch (facing)
            {
                case Facing.North:
                    facing = Facing.East;
                    break;
                case Facing.South:
                    facing = Facing.West;
                    break;
                case Facing.West:
                    facing = Facing.North;
                    break;
                case Facing.East:
                    facing = Facing.South;
                    break;
            }
        }

        private bool IsNextMoveIsObstacle(Point position, Facing guardFacing)
        {
            Move(ref position, guardFacing);

            if (OutOfTheMap(position))
            {
                return true;
            }

            return IsObstacle(position);
        }

        private bool IsNextMoveOutOfTheMap(Point position, Facing guardFacing)
        {
            Move(ref position, guardFacing);

            return OutOfTheMap(position);
        }

        private bool OutOfTheMap(Point position)
        {
            return position.X < 0 || position.Y < 0 || position.X >= map[0].Length || position.Y >= map.Length;
        }

        private bool IsObstacle(Point position)
        {
            return map[position.Y][position.X] == '#';
        }

        private string[] GetInput(string path)
        {
            return File.ReadLines(path).ToArray();
        }

        private List<(Point Position, Facing Facing)> FindingGuardRoute(Point position, Facing guardFacing)
        {
            var guardPatrolRoute = new List<(Point Position, Facing Facing)>
            {
                (position, guardFacing)
            };

            while (!OutOfTheMap(position))
            {
                while (!IsNextMoveIsObstacle(position, guardFacing))
                {
                    Move(ref position, guardFacing);
                    guardPatrolRoute.Add((position, guardFacing));
                }

                if (IsNextMoveOutOfTheMap(position, guardFacing))
                {
                    break;
                }
                TurnRight(ref guardFacing);
            }

            return guardPatrolRoute;
        }
    }
}
