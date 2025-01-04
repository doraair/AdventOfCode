namespace AdventOfCode.Day16
{
    using System.Drawing;
    using System.Linq;

    internal class PartTwo
    {
        private const string input = @"D:\source-code-poc\AdventOfCode\Day16\input.txt";
        private char[,] maze;
        private int mazeWidth;
        private int mazeHeight;
        private string moves = string.Empty;
        private Point startingPoint;
        private Point finishedPoint;

        // Direction
        // 0 - North = Up = new Point(0, -1)
        // 1 - East = Right = new Point(1, 0)
        // 2 - South = Down = new Point(0, 1)
        // 3 - West = Left = new Point(-1, 0)

        public record State(Point Position, Point Direction);
        public void Run()
        {
            GetInput();
            FindRoute();
        }

        private void FindRoute()
        {
            var visited = new List<State>();
            var states = new PriorityQueue<State, int>();
            var startingState = new State(startingPoint, new Point(1, 0));
            states.Enqueue(startingState, 0);

            var path = new Dictionary<State, List<State>>();

            var minimumCosts = new Dictionary<State, int>();
            minimumCosts.Add(startingState, 0);
            while (states.Count > 0)
            {
                var currentState = states.Dequeue();

                // mark as visited
                visited.Add(currentState);

                // 1.Move forward
                var nextPosition = new Point(currentState.Position.X + currentState.Direction.X,
                                             currentState.Position.Y + currentState.Direction.Y);
                var nextState = new State(nextPosition, currentState.Direction);
                QueueIfCanMove(nextState, minimumCosts[currentState] + 1);

                // 2.turn clockwise
                nextState = new State(
                    currentState.Position,
                    new Point(-currentState.Direction.Y, currentState.Direction.X));
                QueueIfCanMove(nextState, minimumCosts[currentState] + 1000);

                // 3.turn anti-clockwise
                nextState = new State(
                    currentState.Position,
                    new Point(currentState.Direction.Y, -currentState.Direction.X));
                QueueIfCanMove(nextState, minimumCosts[currentState] + 1000);

                void QueueIfCanMove(State nextState, int cost)
                {
                    //if (nextState.Position == finishedPoint)
                    //{
                    //    return;
                    //}

                    if (OutOfTheMap(nextState.Position) || IsWall(nextState.Position))
                    {
                        return;
                    }

                    if (visited.Any(o => o == nextState))
                    {
                        return;
                    }

                    if (minimumCosts.TryGetValue(nextState, out var existingCost) && existingCost <= cost)
                    {
                        return;
                    }


                    minimumCosts[nextState] = cost;
                    states.Enqueue(nextState, cost);
                }
            }


            // finding lowest cost
            var lowestCost = int.MaxValue;
            foreach (var direction in PointExtension.Directions)
            {
                var state = new State(finishedPoint, direction);
                if (minimumCosts.TryGetValue(state, out var cost))
                {
                    lowestCost = Math.Min(lowestCost, cost);
                }
            }
            Console.WriteLine($"Lowest cost: {lowestCost}");
        }

        private void PrintMap()
        {
            for (var y = 0; y < maze.GetLength(0); y++)
            {
                for (var x = 0; x < maze.GetLength(1); x++)
                {
                    Console.Write(maze[y, x]);
                }
                Console.WriteLine();
            }
        }

        private bool IsWall(Point position)
        {
            return maze[position.Y, position.X] == '#';
        }

        private bool OutOfTheMap(Point point)
        {
            return point.X < 0 || point.Y < 0 || point.X >= mazeWidth || point.Y >= mazeHeight;
        }

        private void GetInput()
        {
            var lines = File.ReadLines(input);

            var mapLine = lines
                 .Where(line => line.StartsWith('#'))
                 .ToList();
            mazeHeight = mapLine.Count;
            mazeWidth = mapLine[0].Length;
            maze = new char[mazeHeight, mazeHeight];

            for (var y = 0; y < mapLine.Count; y++)
            {
                for (var x = 0; x < mapLine[y].Length; x++)
                {
                    maze[y, x] = mapLine[y][x];
                    if (mapLine[y][x] == 'S')
                    {
                        startingPoint = new Point(x, y);
                    }
                    if (mapLine[y][x] == 'E')
                    {
                        finishedPoint = new Point(x, y);
                    }
                }
            }
        }
    }
}
