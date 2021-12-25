namespace AOC2021.Day25
{
    public class Puzzle : PuzzleBase<long, long>
    {
        public Puzzle(IEnumerable<string> lines)
        {
            _map = Map.FromInput(lines);
        }

        private readonly Map _map;

        public override long Solution1()
        {
            var map = _map;
            bool changed;
            var steps = 0L;

            do
            {
                (changed, map) = map.Step();
                steps++;
            }
            while (changed);

            return steps;
        }

        public override long Solution2() => 0;
    }
}
