namespace AOC2020.Day19
{
    public class TextRule : Rule
    {
        public TextRule(string text) : base($"\"{text}\"")
        {
             Text = text;
        }

        public string Text { get; }

        public override string ToRegex() => Text;
    }
}
