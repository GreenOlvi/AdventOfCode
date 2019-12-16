using AoC2019.Common;
using System;
using System.Collections.Generic;

namespace AoC2019.Puzzle11
{
    public class Robot
    {
        public Robot(IntcodeMachine processor, Grid grid) 
        {
            Direction = Direction.Up;
            Position = new Position(0, 0);
            _processor = processor;
            _grid = grid;
            _visited = new HashSet<Position>();
            processor.Run();
        }

        public Direction Direction { get; private set; }
        public Position Position { get; private set; }
        public int Visited => _visited.Count;

        private readonly IntcodeMachine _processor;
        private readonly Grid _grid;
        private readonly HashSet<Position> _visited;

        public void Run()
        {
            while (!_processor.IsHalted)
            {
                Step();
            }
        }

        private void Step()
        {
            var input = _grid.GetColor(Position);
            _processor.AddInputAndRun((long)input);

            var newColor = (int)_processor.GetOutput();
            if (newColor != 0 && newColor != 1)
            {
                throw new InvalidOperationException("newColor");
            }
            _grid.SetColor(Position, (GridColor)newColor);

            _visited.Add(Position);

            var turnDir = _processor.GetOutput();
            if (turnDir != 0 && turnDir != 1)
            {
                throw new InvalidOperationException("turnDir");
            }
            Direction = turnDir == 1 ? Direction.TurnRight() : Direction.TurnLeft();

            Position = Position.Move(Direction);
        }
    }
}
