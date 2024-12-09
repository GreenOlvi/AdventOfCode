namespace AOC2024.Puzzles;

public class Day09 : CustomBaseProblem<long>
{
    private readonly int[] _disk;

    public Day09()
    {
        _disk = ParseInput(ReadLinesFromFile()).ToArray();
    }

    public Day09(IEnumerable<string> lines)
    {
        _disk = ParseInput(lines).ToArray();
    }

    private static IEnumerable<int> ParseInput(IEnumerable<string> lines) => lines.First().Select(static c => c - '0');

    private (List<File> Files, List<Space> freeSpace) ExtractFiles()
    {
        var files = new List<File>();
        var freeSpace = new List<Space>();
        var block = 0;
        var i = 0;
        foreach (var f in _disk)
        {
            if (i % 2 == 0)
            {
                files.Add(new File(i / 2, f, block));
            }
            else
            {
                freeSpace.Add(new Space(f, block));
            }

            block += f;
            i++;
        }

        return (files, freeSpace);
    }

    private static long FileSum(File f) => Enumerable.Range(f.Start, f.Size).Sum(b => b * f.Id);

    private static long Checksum(IEnumerable<File> files) => files.Sum(FileSum);

    public override long Solve1()
    {
        var (files, freeSpace) = ExtractFiles();

        var fileIndex = 0;
        var revFileIndex = files.Count - 1;

        var spaceQueue = new Queue<Space>(freeSpace);
        var newFiles = new List<File>();

        Space space;
        File endFile;

        while (true)
        {
            if (fileIndex > revFileIndex)
            {
                break;
            }

            newFiles.Add(files[fileIndex]);

            if (fileIndex == revFileIndex)
            {
                break;
            }

            space = spaceQueue.Dequeue();

            while (space.Size > 0)
            {
                endFile = files[revFileIndex];
                if (space.Size >= endFile.Size)
                {
                    newFiles.Add(new File(endFile.Id, endFile.Size, space.Start));
                    space = new Space(space.Size - endFile.Size, space.Start + endFile.Size);
                    revFileIndex--;
                }
                else
                {
                    newFiles.Add(new File(endFile.Id, space.Size, space.Start));
                    files[revFileIndex] = new File(endFile.Id, endFile.Size - space.Size, endFile.Start);
                    space = new Space(0, 0);
                }
            }
            fileIndex++;
        }

        return Checksum(newFiles);
    }

    public override long Solve2()
    {
        var (files, freeSpace) = ExtractFiles();

        var reverseFiles = files.OrderByDescending(static f => f.Id)
            .Take(freeSpace.Count - 1)
            .ToArray();
        foreach (var file in reverseFiles)
        {
            var space = freeSpace.OrderBy(s => s.Start).FirstOrDefault(s => s.Size >= file.Size && s.Start < file.Start);
            if (space == Space.Default)
            {
                continue;
            }

            _ = files.Remove(file);
            files.Add(new File(file.Id, file.Size, space.Start));

            var left = freeSpace.FirstOrDefault(s => s.End == file.Start);
            if (left == Space.Default)
            {
                left = new Space(0, file.Start);
            }

            var right = freeSpace.FirstOrDefault(s => s.Start == file.End);
            if (right == Space.Default)
            {
                right = new Space(0, file.End);
            }

            _ = freeSpace.Remove(left);
            _ = freeSpace.Remove(right);
            freeSpace.Add(new Space(right.End - left.Start, left.Start));

            _ = freeSpace.Remove(space);
            if (space.Size - file.Size > 0)
            {
                freeSpace.Add(new Space(space.Size - file.Size, space.Start + file.Size));
            }
        }

        return Checksum(files);
    }

    private readonly record struct File(long Id, int Size, int Start)
    {
        public readonly int End = Start + Size;
    }
    private readonly record struct Space(int Size, int Start)
    {
        public static readonly Space Default = new();
        public readonly int End = Start + Size;
    }
}
