using System;
using System.Diagnostics;
using FluentAssertions;
using NUnit.Framework;
using Puzzle11;

namespace Puzzle11Tests
{
    [TestFixture]
    public class SolverTests
    {
        [Test]
        public void Solver1Test()
        {
            var solver = new Solver(
                "The first floor contains a hydrogen-compatible microchip and a lithium-compatible microchip.",
                "The second floor contains a hydrogen generator.",
                "The third floor contains a lithium generator.",
                "The fourth floor contains nothing relevant.");

            solver.Solve1().Should().Be(11);
        }

        [Test]
        public void ConfigurationTest()
        {
            var config = new Configuration(new[]
            {
                new Floor(new Item(ItemType.Microchip, "hydrogen"), new Item(ItemType.Microchip, "lithium")),
                new Floor(new Item(ItemType.Generator, "hydrogen")),
                new Floor(new Item(ItemType.Generator, "lithium")),
                new Floor(), 
            });

            Debug.WriteLine(config.ToString());
            Debug.WriteLine(String.Join("; ", config.AvaliableMoves()));
            config.IsStable().Should().BeTrue();

            var config2 = config.Move(new Move(MoveDirection.Up, new Item(ItemType.Microchip, "hydrogen")));
            Debug.WriteLine(config2.ToString());
            Debug.WriteLine(String.Join("; ", config2.AvaliableMoves()));
            config2.IsStable().Should().BeTrue();

            var config3 = config2.Move(new Move(MoveDirection.Down, new Item(ItemType.Generator, "hydrogen")));
            Debug.WriteLine(config3.ToString());
            Debug.WriteLine(String.Join("; ", config3.AvaliableMoves()));
            config3.IsStable().Should().BeFalse();
            config3.MoveCount.Should().Be(2);
        }

        [Test]
        public void ConfigurationWinTest()
        {
            var config = new Configuration(3, new[]
            {
                new Floor(),
                new Floor(),
                new Floor(),
                new Floor(new Item(ItemType.Generator, "hydrogen"), new Item(ItemType.Microchip, "hydrogen"), new Item(ItemType.Generator, "lithium"), new Item(ItemType.Microchip, "lithium")),
            });

            config.IsGoal().Should().BeTrue();
        }

        [Test]
        public void MoveTest()
        {
            var move = new Move(MoveDirection.Up, new Item(ItemType.Generator, "lithium"));
            move.ReverseMove().Should().Be(new Move(MoveDirection.Down, new Item(ItemType.Generator, "lithium")));
        }
    }
}
