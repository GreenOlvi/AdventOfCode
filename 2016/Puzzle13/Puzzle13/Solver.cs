using System;
using System.IO;

namespace Puzzle13
{
    public class Solver
    {
        public Solver(int input)
        {
            Maze = new Maze(input);
        }

        private Maze Maze { get; }

        public int Solve1()
        {
            var pathfinder = new PathFinder(Maze);
            var path = pathfinder.FindPath(new Point(1, 1), new Point(31, 39));
            return path.Count;
        }

        public int Solve2()
        {
            var walker = new Walker(Maze);
            var walked = walker.WalkedInRadius(new Point(1, 1), 49);
            return walked;
        }
    }
}
