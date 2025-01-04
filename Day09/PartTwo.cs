namespace AdventOfCode.Day09
{
    internal class PartTwo
    {
        private const string inputPath = "./Day09/input.txt";

        internal void Run()
        {
            var input = GetInput(inputPath) ?? string.Empty;

            var disk = ConvertFileAndFreeSpace(input);

            MoveFileBlocksV2(disk);
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

        internal void MoveFileBlocksV2(List<int> disk)
        {
            var highestFileIdNumber = disk.Max();

            for (int id = highestFileIdNumber; id > 0; id--)
            {
                var fileFirstIndex = disk.FindIndex(o => o == id);
                var fileLastIndex = disk.FindLastIndex(o => o == id);

                var fileLength = fileLastIndex - fileFirstIndex + 1;

                // finding space
                var spaceIndex = FindSpace(disk, 0, fileLength);

                if (spaceIndex == -1 || spaceIndex > fileFirstIndex)
                {
                    continue;
                }

                // move
                for (int i = 0; i < fileLength; i++)
                {
                    disk[spaceIndex + i] = disk[fileFirstIndex + i];
                    disk[fileFirstIndex + i] = -1;
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
