namespace AOC2020.Day19
{
    public class OrRule : Rule
    {
        public OrRule(Rule left, Rule right) : base($"( {left} | {right} )")
        {
            Left = left;
            Right = right;
        }

        public Rule Left { get; }
        public Rule Right { get; }

        public override string ToRegex() => $"({Left.ToRegex()}|{Right.ToRegex()})";
    }
}
