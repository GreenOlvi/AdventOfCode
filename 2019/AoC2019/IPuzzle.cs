using System.Threading.Tasks;

namespace AoC2019
{
    interface IPuzzle
    {
        Task<string> Solve1Async();
        Task<string> Solve2Async();
    }
}
