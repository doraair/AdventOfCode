namespace AdventOfCode.Day17
{
    using System.Linq;

    internal class PartTwo
    {
        private const string inputPath = "./Day17/input.txt";
        private short[] program;
        private EightInstructions eightInstructions;

        internal PartTwo()
        {
            eightInstructions = new EightInstructions(0, 0, 0);
        }

        internal void Run()
        {
            eightInstructions.Reverse();
        }

        private void InitProgram(string path)
        {
            long registerA = 0;
            long registerB = 0;
            long registerC = 0;
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

                    var registerALine = line;
                    var registerBLine = sr.ReadLine() ?? string.Empty;
                    var registerCLine = sr.ReadLine() ?? string.Empty;

                    sr.ReadLine();
                    var programeLine = sr.ReadLine() ?? string.Empty;

                    var data = long.Parse(registerALine.Split(':')[1].Trim());
                    registerA = data;

                    data = long.Parse(registerBLine.Split(':')[1].Trim());
                    registerB = data;

                    data = long.Parse(registerCLine.Split(':')[1].Trim());
                    registerC = data;

                    program = programeLine.Split(':')[1].Split(',').Select(short.Parse).ToArray();
                }
            }

            eightInstructions = new EightInstructions(registerA, registerB, registerC);
        }

    }
}
