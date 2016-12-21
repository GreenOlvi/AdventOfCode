using System.Diagnostics;
using FluentAssertions;
using NUnit.Framework;
using Puzzle13;

namespace Puzzle13Tests
{
    [TestFixture]
    public class SolverTests
    {
        [Test]
        public void Solve1Test()
        {
            var pathfinder = new PathFinder(new Maze(1364));
            Debug.WriteLine(pathfinder.Maze.ToString(0, 0, 33, 41));
            var path = pathfinder.FindPath(new Point(1, 1), new Point(31, 39));

            Debug.WriteLine(pathfinder.Maze.ToString(path));

            path.Count.Should().Be(86);
        }

        [Test]
        public void MazeTest()
        {
            var pathfinder = new PathFinder(new Maze(10));

            var path = pathfinder.FindPath(new Point(1, 1), new Point(7, 4));
            path.Count.Should().Be(11);

            Debug.WriteLine(pathfinder.Maze.ToString(path));

            Debug.WriteLine(pathfinder.Maze.ToString(-52, -52, 105, 105));
        }

        [Test]
        public void WalkerTest()
        {
            var walker = new Walker(new Maze(10));

            walker.WalkedInRadius(new Point(1, 1), 1).Should().Be(3);
            walker.WalkedInRadius(new Point(1, 1), 2).Should().Be(5);
            walker.WalkedInRadius(new Point(1, 1), 3).Should().Be(7);
            walker.WalkedInRadius(new Point(1, 1), 4).Should().Be(13);
            walker.WalkedInRadius(new Point(1, 1), 5).Should().Be(17);
            walker.WalkedInRadius(new Point(1, 1), 6).Should().Be(19);
            walker.WalkedInRadius(new Point(1, 1), 7).Should().Be(21);
            walker.WalkedInRadius(new Point(1, 1), 8).Should().Be(23);
            walker.WalkedInRadius(new Point(1, 1), 9).Should().Be(25);
            walker.WalkedInRadius(new Point(1, 1), 10).Should().Be(31);
            walker.WalkedInRadius(new Point(1, 1), 11).Should().Be(35);
            walker.WalkedInRadius(new Point(1, 1), 12).Should().Be(37);
            walker.WalkedInRadius(new Point(1, 1), 13).Should().Be(40);
            walker.WalkedInRadius(new Point(1, 1), 14).Should().Be(42);
            walker.WalkedInRadius(new Point(1, 1), 15).Should().Be(44);
            walker.WalkedInRadius(new Point(1, 1), 16).Should().Be(48);
            Debug.WriteLine(walker.ToString());
        }

        [Test]
        public void Solve2Test()
        {
            var walker = new Walker(new Maze(1364));
            walker.WalkedInRadius(new Point(1, 1), 50);
            Debug.WriteLine(walker.ToString());

            Debug.WriteLine(walker.Maze.ToString(-2, -2, 5, 5));
            Debug.WriteLine(walker.Maze.ToString(-25, -25, 51, 51));
        }
    }
}
