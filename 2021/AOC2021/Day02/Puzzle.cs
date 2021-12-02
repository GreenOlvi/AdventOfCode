using System.Text.RegularExpressions;
using AOC2021.Common;

namespace AOC2021.Day02
{
    public class Puzzle : PuzzleBase<long, long>
    {
        private enum Direction
        {
            forward,
            down,
            up,
        };

        private static readonly Regex dirRegex = new(@"^(?<dir>\w+)\s(?<value>\d+)$");

        public Puzzle(IEnumerable<string> lines)
        {
            _directions = lines.Select(ParseDirection).ToArray();
        }

        private readonly (Direction, int)[] _directions;

        private (Direction, int) ParseDirection(string line) =>
            dirRegex.Parse(line, ("dir", Enum.Parse<Direction>), ("value", int.Parse));

        private static Point Move1(Point current, (Direction Direction, int Value) dir) =>
            dir.Direction switch
            {
                Direction.forward => current + new Point(dir.Value, 0),
                Direction.down => current + new Point(0, dir.Value),
                Direction.up => current + new Point(0, -dir.Value),
                _ => throw new ArgumentOutOfRangeException(nameof(dir.Direction)),
            };

        public override long Solution1() => _directions.Aggregate(Point.Zero, Move1, d => d.X * d.Y);

        private static (Point Pos, int Aim) Move2((Point Pos, int Aim) current, (Direction Direction, int Value) dir) =>
            dir.Direction switch
            {
                Direction.forward => (current.Pos + new Point(dir.Value, current.Aim * dir.Value), current.Aim),
                Direction.down => (current.Pos, current.Aim + dir.Value),
                Direction.up => (current.Pos, current.Aim - dir.Value),
                _ => throw new ArgumentOutOfRangeException(nameof(dir.Direction)),
            };

        public override long Solution2() => _directions.Aggregate((Pos: Point.Zero, Aim: 0), Move2, d => d.Pos.X * d.Pos.Y);
    }
}
