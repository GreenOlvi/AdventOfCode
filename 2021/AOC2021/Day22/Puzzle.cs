using System.Text.RegularExpressions;
using AOC2021.Common;

namespace AOC2021.Day22
{
    public class Puzzle : PuzzleBase<long, long>
    {
        public Puzzle(IEnumerable<string> lines)
        {
            _input = lines.Select(ParseLine).ToArray();
        }

        private static readonly Regex stepPattern = new(@"^(?<action>on|off) x=(?<x1>-?\d+)\.\.(?<x2>-?\d+),y=(?<y1>-?\d+)\.\.(?<y2>-?\d+),z=(?<z1>-?\d+)\.\.(?<z2>-?\d+)$", RegexOptions.Compiled);

        private readonly Step[] _input;

        private Step ParseLine(string line)
        {
            if (!stepPattern.TryMatchAll(line, out var match))
            {
                throw new InvalidDataException(line);
            }

            var action = match["action"] == "on";

            var start = new Point3
            {
                X = int.Parse(match["x1"]),
                Y = int.Parse(match["y1"]),
                Z = int.Parse(match["z1"]),
            };

            var end = new Point3
            {
                X = int.Parse(match["x2"]),
                Y = int.Parse(match["y2"]),
                Z = int.Parse(match["z2"]),
            };

            if (start.X > end.X || start.Y > end.Y || start.Z > end.Z)
            {
                throw new InvalidDataException("Lower first");
            }

            return new Step { On = action, Area = new Cuboid { Start = start, End = end } };
        }

        private static IEnumerable<Point3> GetCubesLimited(Cuboid area)
        {
            var start = new Point3
            {
                X = Math.Clamp(area.Start.X, -50, 51),
                Y = Math.Clamp(area.Start.Y, -50, 51),
                Z = Math.Clamp(area.Start.Z, -50, 51),
            };

            var end = new Point3
            {
                X = Math.Clamp(area.End.X, -51, 50),
                Y = Math.Clamp(area.End.Y, -51, 50),
                Z = Math.Clamp(area.End.Z, -51, 50),
            };

            for (var z = start.Z; z <= end.Z; z++)
            {
                for (var y = start.Y; y <= end.Y; y++)
                {
                    for (var x = start.X; x <= end.X; x++)
                    {
                        yield return new Point3(x, y, z);
                    }
                }
            }
        }

        public override long Solution1()
        {
            var lit = new HashSet<Point3>();
            foreach (var step in _input)
            {
                var cubes = GetCubesLimited(step.Area);
                if (step.On)
                {
                    foreach (var cube in cubes)
                    {
                        lit.Add(cube);
                    }
                }
                else
                {
                    foreach (var cube in cubes)
                    {
                        lit.Remove(cube);
                    }
                }
            }

            return lit.Count();
        }

        public override long Solution2()
        {
            throw new NotImplementedException();
        }
    }

    public readonly record struct Cuboid
    {
        public Point3 Start { get; init; }
        public Point3 End { get; init; }
    } 

    public readonly struct Step
    {
        public bool On { get; init; }
        public Cuboid Area { get; init; }

        public override string ToString() =>
            $"{(On ? "on" : "off")} x={Area.Start.X}..{Area.End.X},y={Area.Start.Y}..{Area.End.Y},z={Area.Start.Z}..{Area.End.Z}";
    }
}
