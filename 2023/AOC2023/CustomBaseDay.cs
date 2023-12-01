namespace AOC2023;

public abstract class CustomBaseDay : BaseDay
{
    protected IEnumerable<string> ReadLinesFromFile() => File.ReadLines(InputFilePath);
}
