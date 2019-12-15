using System;
using AoC2019.Common;

namespace AoC2019.Puzzle15
{
    public class RCDroid
    {
        public RCDroid(IntcodeMachine machine)
        {
            _position = new Position(0, 0);

            _machine = machine;
            _machine.Run();

            _map = new Map();
            _map.AddTile(_position, Tile.Empty);
        }

        private readonly Map _map;
        private readonly IntcodeMachine _machine;
        private Position _position;

        public void Run()
        {
            var run = true;
            while (run)
            {
                _map.ConsoleDraw(_position);
                Console.WriteLine("Waiting for input...");

                var key = Console.ReadKey();
                var dir = key.Key switch
                {
                    ConsoleKey.UpArrow => Direction.Up,
                    ConsoleKey.DownArrow => Direction.Down,
                    ConsoleKey.LeftArrow => Direction.Left,
                    ConsoleKey.RightArrow => Direction.Right,
                    _ => Direction.None,
                };

                if (dir != Direction.None)
                {
                    TryMove(dir);
                }
                else
                {
                    if (key.Key == ConsoleKey.Escape)
                    {
                        run = false;
                    }
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
