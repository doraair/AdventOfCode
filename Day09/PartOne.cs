namespace AdventOfCode.Day09
{
    internal class PartOne
    {
        private const string inputPath = "./Day09/input.txt";

        internal void Run()
        {
            var input = GetInput(inputPath) ?? string.Empty;

            var disk = ConvertFileAndFreeSpace(input);

            MoveFileBlocks(disk);
            //PrintDisk(disk);
            CalculateCheckSum(disk);
        }

        internal List<int> ConvertFileAndFreeSpace(string files)
        {
            var disk = new List<int>();
            var totalLength = files.Length;

            var fileId = 0;
            for (int i = 0; i < totalLength; i++)
            {
                var length = (int)char.GetNumericValue(files[i]);
                var isFile = i == 0 || i % 2 == 0;
                if (isFile)
                {
                    // file
                    for (int j = 0; j < length; j++)
                    {
                        disk.Add(fileId);
                    }
                    fileId++;
                }
                else
                {
                    // free space
                    for (int j = 0; j < length; j++)
                    {
                        disk.Add(-1);
                    }
                }
            }

            return disk;
        }

        internal void MoveFileBlocks(List<int> disk)
        {
            var fileIndex = disk.FindLastIndex(o => o != -1);
            var spaceIndex = disk.FindIndex(o => o == -1);

            for (int i = 0; i < disk.Count; i++)
            {
                if (disk[i] == -1)
                {
                    fileIndex = disk.FindLastIndex(o => o != -1);
                    if (fileIndex <= i)
                    {
                        break;
                    }
                    disk[i] = disk[fileIndex];
                    disk[fileIndex] = -1;
                }
            }
        }

        internal int FindSpace(List<int> disk, int startIndex, int size)
        {
            if (startIndex == -1)
            {
                return -1;
            }
            var spaceIndex = disk.FindIndex(startIndex, o => o == -1);
            if (spaceIndex == -1)
            {
                return -1;
            }

            var endOfSpaceIndex = disk.FindIndex(spaceIndex, o => o != -1);
            var spaceLength = endOfSpaceIndex - spaceIndex;
            if (spaceLength >= size)
            {
                return spaceIndex;
            }

            startIndex = endOfSpaceIndex;
            return FindSpace(disk, startIndex, size);
        }

        internal void PrintDisk(List<int> disk)
        {
            foreach (var item in disk)
            {
                Console.Write(item == -1 ? "." : item);
            }
            Console.WriteLine();
        }

        internal void CalculateCheckSum(List<int> disk)
        {
            ulong checkSum = 0;
            for (int i = 0; i < disk.Count; i++)
            {
                var fileId = disk[i];
                if (fileId == -1)
                {
                    continue;
                }
                checkSum += (ulong)(fileId * i);

                Console.WriteLine($"{checkSum}");
            }


            Console.WriteLine($"Check Sum: {checkSum}");
        }
        private static string GetInput(string path)
        {
            var content = string.Empty;
            var lines = File.ReadLines(path).Select(o => o).ToArray();
            content = string.Join("", lines);
            return content;
        }
    }

}
