using System.Text;

namespace AOC2022.Puzzles;

public class Day15 : CustomBaseDay
{
    private readonly string[] _lines;

    public Day15()
    {
        _lines = ReadLinesFromFile().ToArray();
    }

    public Day15(IEnumerable<string> lines)
    {
        _lines = lines.ToArray();
    }

    private static readonly Regex SensorPattern = new(@"Sensor at x=(?<x>-?\d+), y=(?<y>-?\d+): closest beacon is at x=(?<beacon_x>-?\d+), y=(?<beacon_y>-?\d+)", RegexOptions.Compiled);
    private static IEnumerable<(Point2 Sensor, Point2 Beacon)> ParseInput(IEnumerable<string> lines)
    {
        foreach (var line in lines)
        {
            if (!SensorPattern.TryParseMany(line, ("x", int.Parse), ("y", int.Parse), ("beacon_x", int.Parse), ("beacon_y", int.Parse), out var coords))
            {
                throw new InvalidDataException();
            }

            yield return (new Point2(coords.Item1, coords.Item2), new Point2(coords.Item3, coords.Item4));
        }
    }

    private readonly record struct Range(int L, int R);

    private static int Dist(Point2 a, Point2 b) => (int)(Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y));
    private static bool IsInRange(Point2 point, Point2 beacon, long range) => Dist(point, beacon) <= range;
    private static bool AffectsLine(int line, Point2 sensor, long distance) => Math.Abs(sensor.Y - line) <= distance;
    private static Range AffectedRange(int line, Point2 sensor, long minRange)
    {
        var d = minRange - Math.Abs(sensor.Y - line);
        var l = (int)(sensor.X - d);
        var r = (int)(sensor.X + d);
        return new Range(l, r);
    }

    private static bool TryMergeRanges(Range a, Range b, out Range combined)
    {
        if (a.R < b.L - 1)
        {
            combined = a;
            return false;
        }

        combined = new Range(a.L, Math.Max(a.R, b.R));
        return true;
    }

    private bool TryFindBeacon((Point2 Sensor, int MinRange)[] sensors, int y, out Point2 beacon)
    {
        var ranges = sensors.Where(s => AffectsLine(y, s.Sensor, s.MinRange))
            .Select(s => AffectedRange(y, s.Sensor, s.MinRange))
            .OrderBy(r => r.L)
            .ToArray();

        var a = ranges[0];
        for (var i = 1; i < ranges.Length; i++)
        {
            var b = ranges[i];
            if (!TryMergeRanges(a, b, out var combined))
            {
                beacon = new Point2(a.R + 1, y);
                return true;
            }

            a = combined;
        }

        if (a.L > 0 || a.R < MaxCoord)
        {
            if (a.L > 0)
            {
                beacon = new Point2(0, y);
            }
            else
            {
                beacon = new Point2(MaxCoord, y);
            }

            return true;
        }

        beacon = Point2.Zero;
        return false;
    }

    public int CheckLevel { get; init; } = 2_000_000;
    public int MaxCoord { get; init; } = 4_000_000;

    public override ValueTask<string> Solve_1()
    {
        var input = ParseInput(_lines).ToArray();
        var sensors = input.Select(p => (p.Sensor, MinRange: Dist(p.Sensor, p.Beacon))).ToArray();

        var range = sensors.Where(s => AffectsLine(CheckLevel, s.Sensor, s.MinRange))
            .Select(s => AffectedRange(CheckLevel, s.Sensor, s.MinRange))
            .OrderBy(r => r.L)
            .Aggregate((a, b) =>
            {
                if (TryMergeRanges(a, b, out var c))
                {
                    return c;
                }
                throw new InvalidOperationException();
            });

        var beacons = input.Where(i => i.Beacon.Y == CheckLevel)
            .Select(i => i.Beacon)
            .Distinct()
            .Count();

        var count = range.R - range.L + 1 - beacons;
        return count.ToResult();
    }

    public override ValueTask<string> Solve_2()
    {
        var input = ParseInput(_lines).ToArray();
        var sensors = input.Select(p => (p.Sensor, MinRange: Dist(p.Sensor, p.Beacon))).ToArray();

        var beacon = new Point2(-1, -1);
        for (var y = 0; y <= MaxCoord; y++)
        {
            if (TryFindBeacon(sensors, y, out beacon))
            {
                break;
            }
        }

        var frequency = 4_000_000 * beacon.X + beacon.Y;
        return frequency.ToResult();
    }
}
