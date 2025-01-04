namespace AdventOfCode
{
    using System.Drawing;

    internal class PointExtension
    {
        internal static readonly IEnumerable<Point> Directions = new List<Point>
        {
            new Point(0, -1), // North
            new Point(0, 1), // South
            new Point(-1, 0), // West
            new Point(1, 0) // East
        };

        internal static Point Move(Point currentPosition, Movement movement)
        {
            var x = 0; var y = 0;
            switch (movement)
            {
                case Movement.Up:
                    y--;
                    break;
                case Movement.Down:
                    y++;
                    break;
                case Movement.Left:
                    x--;
                    break;
                case Movement.Right:
                    x++;
                    break;
            }

            return new Point(currentPosition.X + x, currentPosition.Y + y);
        }

        internal static Point Move(Movement movement)
        {
            var x = 0; var y = 0;
            switch (movement)
            {
                case Movement.Up:
                    y--;
                    break;
                case Movement.Down:
                    y++;
                    break;
                case Movement.Left:
                    x--;
                    break;
                case Movement.Right:
                    x++;
                    break;
            }

            return new Point(x, y);
        }

    }
}
