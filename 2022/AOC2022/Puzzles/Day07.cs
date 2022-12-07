namespace AOC2022.Puzzles;

public class Day07 : CustomBaseDay
{
    public Day07()
    {
        _root = ParseInput(ReadLinesFromFile());
    }

    public Day07(IEnumerable<string> lines)
    {
        _root = ParseInput(lines);
    }

    private static readonly Regex CdPattern = new(@"^\$ cd (?<dir>.+)$", RegexOptions.Compiled);
    private static readonly Regex LsPattern = new(@"^\$ ls", RegexOptions.Compiled);
    private static readonly Regex LsDirPattern = new(@"^dir (?<name>.+)$", RegexOptions.Compiled);
    private static readonly Regex LsFilePattern = new(@"^(?<size>\d+) (?<name>.+)$", RegexOptions.Compiled);

    public static Dir ParseInput(IEnumerable<string> lines)
    {
        var root = new Dir("/", null);
        var input = new Queue<string>(lines);
        Dir cwd = root;
        while (input.TryDequeue(out var line))
        {
            if (CdPattern.TryMatch(line, "dir", out var dirName))
            {
                cwd = dirName switch
                {
                    "/" => root,
                    ".." => cwd.Parent!,
                    _ => cwd.Dirs[dirName],
                };
            }
            else if (LsPattern.IsMatch(line))
            {
                while (input.TryPeek(out var lsOut) && !lsOut.StartsWith("$"))
                {
                    var lsOutLine = input.Dequeue();
                    if (LsDirPattern.TryMatch(lsOutLine, "name", out var lsDirName))
                    {
                        var dir = new Dir(lsDirName, cwd);
                        cwd.Dirs.Add(lsDirName, dir);
                    }
                    else if (LsFilePattern.TryParseMany(lsOutLine, ("size", int.Parse), ("name", s => s), out var result))
                    {
                        var file = new File(result.Item2, result.Item1);
                        cwd.Files.Add(file);
                    }
                }
            }
            else
            {
                throw new InvalidOperationException(line);
            }
        }

        var _ = root.TotalSize;
        return root;
    }

    private readonly Dir _root;

    public override ValueTask<string> Solve_1()
    {
        var found = _root.Find(d => d.TotalSize <= 100000).ToArray();
        return found.Sum(d => d.TotalSize).ToResult();
    }

    public override ValueTask<string> Solve_2()
    {
        var spaceToDelete = 30000000 - (70000000 - _root.TotalSize);
        var foundDir = _root.Find(d => d.TotalSize >= spaceToDelete).MinBy(d => d.TotalSize)!;
        return foundDir.TotalSize.ToResult();
    }

    public readonly record struct File(string Name, int Size);
    public class Dir
    {
        public string Name { get; init; }
        public Dictionary<string, Dir> Dirs { get; } = new();
        public List<File> Files { get; } = new();
        public Dir? Parent { get; init; }

        public int TotalSize => _totalSize.Value;
        private Lazy<int> _totalSize => new(GetTotalSize);

        public Dir(string name, Dir? parent)
        {
            Name = name;
            Parent = parent;
        }

        private int GetTotalSize() =>
            Files.Sum(f => f.Size)
            + Dirs.Values.Sum(d => d.TotalSize);

        public override string ToString() => $"Dir({Name})";

        public IEnumerable<Dir> Find(Func<Dir, bool> predicate)
        {
            var collection = predicate(this) ? new[] { this } : Array.Empty<Dir>();
            return collection.Concat(Dirs.Values.SelectMany(d => d.Find(predicate)));
        }
    }
}
