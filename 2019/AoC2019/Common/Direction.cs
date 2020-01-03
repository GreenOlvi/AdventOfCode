using System;

namespace AoC2019.Common
{
    public enum Direction
    {
        None = 0,
        Up,
        Down,
        Left,
        Right,
    }

    public static class DirectionExtensions
    {
        public static Direction[] Directions = new[] { Direction.Up, Direction.Right, Direction.Down, Direction.Left };

        public static Direction TurnLeft(this Direction direction) =>
            direction switch
            {
                Direction.Up => Direction.Left,
                Direction.Down => Direction.Right,
                Direction.Left => Direction.Down,
                Direction.Right => Direction.Up,
                _ => throw new ArgumentException("Invalid direction", nameof(direction)),
            };

        public static Direction TurnRight(this Direction direction) =>
            direction switch
            {
                Direction.Up => Direction.Right,
                Direction.Down => Direction.Left,
                Direction.Left => Direction.Up,
                Direction.Right => Direction.Down,
                _ => throw new ArgumentException("Invalid direction", nameof(direction)),
            };

        public static Direction Opposite(this Direction direction) =>
            direction switch
            {
                Direction.Up => Direction.Down,
                Direction.Down => Direction.Up,
                Direction.Left => Direction.Right,
                Direction.Right => Direction.Left,
                _ => throw new ArgumentException("Invalid direction", nameof(direction)),
            };
    }
}
