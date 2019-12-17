using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AoC2019.Common;

namespace AoC2019.Puzzle17
{
    public class Solution : IPuzzle
    {
        public Solution(string input)
        {
            _input = IntcodeMachine.ParseInput(input).ToArray();
        }

        private readonly long[] _input;

        public static long Solve1(IEnumerable<long> input)
        {
            var m = new IntcodeMachine(input);
            m.Run();

            var grid = new Grid(m.GetAllOutput().Select(o => (char)o));
            return grid.GetIntersections().Select(p => p.X * p.Y).Sum();
        }

        public static long Solve2(IEnumerable<long> input)
        {
            var mod = new[] { 2L }.Concat(input.Skip(1));
            var r = new Robot(new IntcodeMachine(mod));

            var path = string.Join(",", r.FindPath());
            Console.WriteLine(path);

            return 0;
        }

        public Task<string> Solve1Async() =>
            Task.Run(() => Solve1(_input).ToString());

        public Task<string> Solve2Async() =>
            Task.Run(() => Solve2(_input).ToString());
    }

    public class Robot
    {
        public Robot(IntcodeMachine machine)
        {
            _machine = machine;
            _machine.Run();

            var o = _machine.GetAllOutput().Select(i => (char)i);
            _grid = new Grid(o);

            _position = _grid.RobotPosition;
            _direction = _grid.RobotDirection;
        }

        private readonly IntcodeMachine _machine;
        private Grid _grid;

        private Position _position;
        private Direction _direction;

        public IEnumerable<string> FindPath()
        {
            var i = 0;
            var finished = false;
            while (!finished)
            {
                var front = _position.Move(_direction);
                if (_grid.IsScaffold(front))
                {
                    i++;
                    _position = front;
                }
                else
                {
                    if (i > 0)
                    {
                        yield return i.ToString();
                        i = 0;
                    }

                    var left = _position.Move(_direction.TurnLeft());
                    if (_grid.IsScaffold(left))
                    {
                        _direction = _direction.TurnLeft();
                        yield return "L";
                    }
                    else
                    {
                        var right = _position.Move(_direction.TurnRight());
                        if (_grid.IsScaffold(right))
                        {
                            _direction = _direction.TurnRight();
                            yield return "R";
                        }
                        else
                        {
                            finished = true;
                        }
                    }
                }
            }
        }
    }
}
