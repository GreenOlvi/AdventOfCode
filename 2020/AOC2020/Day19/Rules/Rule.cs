namespace AOC2020.Day19
{
    public abstract class Rule
    {
        protected Rule(string text)
        {
            _text = text;
        }

        private readonly string _text;

        public abstract string ToRegex();

        public override string ToString() => _text;
    }
}
