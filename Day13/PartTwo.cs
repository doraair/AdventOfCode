namespace AdventOfCode.Day13
{
    using System.Collections.Generic;
    using System.Drawing;

    internal class PartTwo
    {
        private const string inputPath = "./Day13/input.txt";

        internal void Run()
        {
            var clawMachines = GetInput(inputPath);
            double totalToken = 0;
            foreach (var clawMachine in clawMachines)
            {
                var token = FindingTokenToWinPrize(clawMachine);
                if (token != -1)
                {
                    totalToken += token;
                }
            }
            Console.WriteLine($"Total Token: {totalToken}");
        }

        private List<ClawMachine> GetInput(string path)
        {
            var clawMachines = new List<ClawMachine>();
            using (StreamReader sr = new StreamReader(path))
            {
                string? line;
                // Read and display lines from the file until the end of
                // the file is reached.
                while ((line = sr.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        continue;
                    }

                    var buttonALine = line;
                    var buttonBLine = sr.ReadLine() ?? string.Empty;
                    var prizeLine = sr.ReadLine() ?? string.Empty;

                    var movement = buttonALine.Split(':')[1].Split(',');
                    var buttonAX = int.Parse(movement[0].Trim().Substring(2));
                    var buttonAY = int.Parse(movement[1].Trim().Substring(2));

                    movement = buttonBLine.Split(':')[1].Split(',');
                    var buttonBX = int.Parse(movement[0].Trim().Substring(2));
                    var buttonBY = int.Parse(movement[1].Trim().Substring(2));

                    movement = prizeLine.Split(':')[1].Split(',');
                    var prizeX = int.Parse(movement[0].Trim().Substring(2));
                    var prizeY = int.Parse(movement[1].Trim().Substring(2));


                    var clawMachine = new ClawMachine()
                    {
                        ButtonA = new Point(buttonAX, buttonAY),
                        ButtonB = new Point(buttonBX, buttonBY),
                        Prize = new Point(prizeX, prizeY)
                    };
                    clawMachines.Add(clawMachine);
                }
            }

            return clawMachines;
        }

        private double FindingTokenToWinPrize(ClawMachine clawMachine)
        {
            double ax = clawMachine.ButtonA.X;
            double ay = clawMachine.ButtonA.Y;
            double bx = clawMachine.ButtonB.X;
            double by = clawMachine.ButtonB.Y;
            double prizeX = 10000000000000 + clawMachine.Prize.X;
            double prizeY = 10000000000000 + clawMachine.Prize.Y;

            double b = Math.Round((prizeX * ay - prizeY * ax) / (bx * ay - by * ax));
            double a = Math.Round((prizeX - (b * bx)) / ax);

            double destinationX = (a * ax) + (b * bx);
            double destinationY = (a * ay) + (b * by);
            if (destinationX == prizeX && destinationY == prizeY)
            {
                return (a * 3) + b;
            }
            return -1;
        }
    }
}
