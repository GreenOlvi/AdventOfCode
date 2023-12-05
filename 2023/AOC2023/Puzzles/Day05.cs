namespace AOC2023.Puzzles;
public partial class Day05 : CustomBaseDay
{
    private readonly Almanac _almanac;

    public Day05()
    {
        _almanac = ParseAlmanac(ReadLinesFromFile());
    }

    public Day05(IEnumerable<string> lines)
    {
        _almanac = ParseAlmanac(lines);
    }

    private static readonly Regex MapNamePattern = BuildMapNamePattern();

    private static Almanac ParseAlmanac(IEnumerable<string> lines)
    {
        var groups = lines.SplitGroups().ToArray();

        var seeds = groups[0][0].Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .Skip(1)
            .ParseLines<long>()
            .ToArray();

        var seedRanges = seeds.Chunk(2).Select(p => new Range(p[0], p[1])).ToList();

        var maps = new List<Map>(groups.Length - 1);

        foreach (var group in groups[1..])
        {
            var match = MapNamePattern.Match(group[0]);
            var from = match.Groups["from"].Value;
            var to = match.Groups["to"].Value;

            var ranges = new List<MapRange>();
            foreach (var line in group[1..])
            {
                var m = line.Split(" ", StringSplitOptions.TrimEntries)
                    .ParseLines<long>()
                    .ToArray();
                ranges.Add(new MapRange(m[0], m[1], m[2]));
            }

            maps.Add(new Map(from, to, ranges));
        }

        return new Almanac(seeds, seedRanges, maps);
    }

    private long ConvertToEnd(long seed)
    {
        var what = "seed";
        var value = seed;
        while (_almanac.HasMap(what))
        {
            (what, value) = _almanac.Convert(what, value);
        }
        return value;
    }

    private long GetLowestConvertToEnd(Range seedRange)
    {
        Range[] currentRanges = [seedRange];
        var mapName = "seed";

        while (_almanac.HasMap(mapName))
        {
            var map = _almanac.GetMap(mapName);
            var newRanges = currentRanges.SelectMany(r => map.FindMatchingRanges(r)).ToArray();
            currentRanges = newRanges;
            mapName = _almanac.GetNextMapName(mapName);
        }

        return currentRanges.Min(r => r.Start);
    }

    public override ValueTask<string> Solve_1() =>
        _almanac.Seeds.Min(ConvertToEnd).ToResult();

    public override ValueTask<string> Solve_2() =>
        _almanac.SeedRanges.Min(GetLowestConvertToEnd).ToResult();

    private class Almanac(long[] Seeds, IEnumerable<Range> SeedsRanges, IEnumerable<Map> Maps)
    {
        private readonly Dictionary<string, Map> _maps = Maps.ToDictionary(m => m.From);

        public long[] Seeds { get; init; } = Seeds;
        public List<Range> SeedRanges { get; init; } = SeedsRanges.ToList();

        public (string What, long Value) Convert(string what, long value)
        {
            var map = _maps[what];
            return (map.To, map.Convert(value));
        }

        public bool HasMap(string name) => _maps.ContainsKey(name);
        public Map GetMap(string name) => _maps[name];
        public string GetNextMapName(string name) => _maps[name].To;
    }

    private class Map(string from, string to, IEnumerable<MapRange> Ranges)
    {
        private readonly List<MapRange> _ranges = Ranges.OrderBy(r => r.SourceStart).ToList();

        public string From { get; init; } = from;
        public string To { get; init; } = to;

        public long Convert(long value)
        {
            var range = _ranges.FirstOrDefault(r => r.ContainsSource(value));
            return range == default ? value : range.Convert(value);
        }

        public IEnumerable<Range> FindMatchingRanges(Range range)
        {
            var matched = _ranges.Where(r => r.Source.End >= range.Start && r.Source.Start <= range.End)
                .OrderBy(r => r.SourceStart)
                .ToList();

            if (matched.Count == 0)
            {
                yield return range;
                yield break;
            }

            if (matched[0].Source.Start < range.Start)
            {
                // trim start
                var first = matched[0];
                var newDestinationStart = first.Convert(range.Start);
                var newLength = first.Length - (range.Start - first.Source.Start);
                matched[0] = new MapRange(newDestinationStart, range.Start, newLength);
            }

            var lastIndex = matched.Count - 1;
            var last = matched[lastIndex];
            if (matched[lastIndex].Source.End > range.End)
            {
                // trim end
                var newLength = last.Length - (last.Source.End - range.End);
                matched[lastIndex] = new MapRange(last.Destination.Start, last.SourceStart, newLength);
            }

            var start = range.Start;

            foreach (var mapRange in matched)
            {
                if (mapRange.Source.Start > start)
                {
                    // add fillers between ranges
                    yield return new Range(start, mapRange.Source.Start - start);
                }

                yield return new Range(mapRange.DestinationStart, mapRange.Length);
                start = mapRange.Source.End + 1;
            }

            if (start < range.End)
            {
                // add filler at end
                yield return new Range(start, range.End - start + 1);
            }
        }

        public override string ToString() => $"{From}-to-{To} map";
    }

    private readonly record struct MapRange(long DestinationStart, long SourceStart, long Length)
    {
        public Range Destination { get; init; } = new Range(DestinationStart, Length);
        public Range Source { get; init; } = new Range(SourceStart, Length);

        public bool ContainsSource(long value) =>
            value >= SourceStart && value < SourceStart + Length;

        public long Convert(long value) => DestinationStart + (value - SourceStart);
    }

    private readonly record struct Range(long Start, long Length)
    {
        public long End => Start + Length - 1;
    }

    [GeneratedRegex(@"^(?<from>\w+)-to-(?<to>\w+) map:")]
    private static partial Regex BuildMapNamePattern();
}
