namespace AdventOfCode.Day06
{
    using System;
    using System.Drawing;
    using System.Linq;

    internal class PartTwo
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

            var startingPoint = new Point(guardX, guardY);

            var guardPatrolRoute = FindingGuardRoute(new Point(guardX, guardY), guardFacing);
            var obstaclePosition = new List<Point>();
            var walkThroughs = new List<(Point Position, Facing Facing)>();
            var i = 0;
            foreach (var item in guardPatrolRoute)
            {
                Console.WriteLine($"Step {i} - {item.Position}, {item.Facing}");
                walkThroughs.Add(item);
                if (IsNextTurnRightIsGuardRoute(item.Position, item.Facing, walkThroughs))
                {
                    var nextPosition = Move(item.Position, item.Facing);
                    if (startingPoint != nextPosition)
                    {
                        obstaclePosition.Add(nextPosition);
                    }
                }
                i++;
            }

            Console.WriteLine($"Total steps - {obstaclePosition.Count()}");
            Console.WriteLine($"Total steps Distince - {obstaclePosition.GroupBy(o => o).Count()}");
            foreach (var item in obstaclePosition)
            {
                Console.WriteLine($"{item.X}, {item.Y}");
            }
        }

        private Point Move(Point position, Facing facing)
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
            return position;
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
            var nextPosition = Move(position, guardFacing);

            if (OutOfTheMap(nextPosition))
            {
                return true;
            }

            return IsObstacle(nextPosition);
        }

        private bool IsNextMoveOutOfTheMap(Point position, Facing guardFacing)
        {
            var nextPosition = Move(position, guardFacing);

            return OutOfTheMap(nextPosition);
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
                    position = Move(position, guardFacing);
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

        private bool IsNextTurnRightIsGuardRoute(Point position, Facing guardFacing, List<(Point Position, Facing Facing)> guardPatrolRoute)
        {
            TurnRight(ref guardFacing);
            var foundRoute = false;
            while (!foundRoute)
            {
                position = Move(position, guardFacing);

                if (OutOfTheMap(position))
                {
                    return false;
                }

                foundRoute = guardPatrolRoute.Any(o => o.Position == position && o.Facing == guardFacing);
            }
            return foundRoute;
        }
    }
}
