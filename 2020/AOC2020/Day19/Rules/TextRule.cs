namespace AOC2020.Day19
{
    public class TextRule : Rule
    {
        public TextRule(string text) : base($"\"{text}\"")
        {
             Text = text;
        }

        public string Text { get; }

        public override MatchResult Match(string text) =>
            text.StartsWith(Text)
                ? Matched(text[Text.Length..])
                : (MatchResult)NotMatched;

        public override string ToRegex() => Text;
    }
}
