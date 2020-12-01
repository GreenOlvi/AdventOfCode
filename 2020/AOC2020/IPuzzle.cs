using System.Threading.Tasks;

namespace AOC2020
{
    public interface IPuzzle
    {
        Task<string> Solve1();
        Task<string> Solve2();
        string? GetProgress1();
        string? GetProgress2();
    }
}
