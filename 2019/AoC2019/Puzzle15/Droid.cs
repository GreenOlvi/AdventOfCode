using AoC2019.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace AoC2019.Puzzle15
{
    public class Droid
    {
        public Droid(IntcodeMachine machine)
        {
            _position = new Position(0, 0);

            _machine = machine;
            _machine.Run();

            _map = new Map();
            _map.AddTile(_position, Tile.Start);
        }

        private readonly Map _map;
        private readonly IntcodeMachine _machine;
        private Position _position;

        public Map Map => _map;

        public void Run(bool draw = false)
        {
            var run = true;

            var stack = new Stack<(Direction, bool)>();
            stack.Push((Direction.Up, false));
            stack.Push((Direction.Right, false));
            stack.Push((Direction.Down, false));
            stack.Push((Direction.Left, false));

            while (run)
            {
                if (!stack.Any())
                {
                    break;
                }

                var (dir, backtrack) = stack.Pop();
                if (TryMove(dir) && !backtrack)
                {
                    stack.Push((dir.Opposite(), true));

                    var left = dir.TurnLeft();
                    if (_map.GetTile(_position.Move(left)) == Tile.Unknown)
                    {
                        stack.Push((dir.TurnLeft(), false));
                    }

                    if (_map.GetTile(_position.Move(dir)) == Tile.Unknown)
                    {
                        stack.Push((dir, false));
                    }

                    var right = dir.TurnRight();
                    if (_map.GetTile(_position.Move(right)) == Tile.Unknown)
                    {
                        stack.Push((dir.TurnRight(), false));
                    }
                }

                if (draw)
                {
                    _map.ConsoleDraw(_position);
                    Thread.Sleep(100);
                }
            }
        }

        private bool TryMove(Direction direction)
        {
            var newPos = _position.Move(direction);
            _machine.AddInputAndRun((long)direction);

            var result = _machine.GetOutput();
            switch (result)
            {
                case 0:
                    _map.AddTile(newPos, Tile.Wall);
                    return false;
                case 1:
                    _map.AddTile(newPos, Tile.Empty);
                    _position = newPos;
                    return true;
                case 2:
                    _map.AddTile(newPos, Tile.System);
                    _position = newPos;
                    return true;
                default:
                    throw new ArgumentOutOfRangeException(nameof(result));
            }
        }
    }
}
