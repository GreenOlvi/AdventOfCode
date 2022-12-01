using AoCHelper;

namespace AOC2022;

public abstract class CustomBaseDay : BaseDay
{
    protected IEnumerable<string> ReadLinesFromFile() => File.ReadLines(InputFilePath);
}
