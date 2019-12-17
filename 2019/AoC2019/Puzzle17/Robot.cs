using System.Collections.Generic;
using System.Linq;
using AoC2019.Common;

namespace AoC2019.Puzzle17
{
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

        public string Draw() => _grid.Draw();

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
