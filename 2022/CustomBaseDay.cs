using AoCHelper;

namespace AOC2022;

public abstract class CustomBaseDay : BaseDay
{
    protected Lazy<string[]> Input;

    public CustomBaseDay()
    {
        Input = new Lazy<string[]>(() => File.ReadLines(InputFilePath).ToArray());
        Init();
    }

    public CustomBaseDay(IEnumerable<string> lines)
    {
        Input = new Lazy<string[]>(() => lines.ToArray());
        Init();
    }

    protected abstract void Init();
}
