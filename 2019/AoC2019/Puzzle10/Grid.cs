using AoC2019.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AoC2019.Puzzle10
{
    public class Grid
    {
        public Grid(IEnumerable<Position> input)
        {
            _set = new HashSet<Position>(input);
            _width = _set.Max(p => p.X) + 1;
            _height = _set.Max(p => p.Y) + 1;
        }

        private readonly HashSet<Position> _set;
        private readonly int _width;
        private readonly int _height;

        public IEnumerable<Position> Asteroids => _set;
        public bool Contains(Position a) => _set.Contains(a);

        public bool IsVisible(Position a, Position b)
        {
            if (a == b)
            {
                return false;
            }

            if (a.X == b.X)
            {
                foreach (var y in Between(a.Y, b.Y))
                {
                    if (Contains(new Position(a.X, y)))
                    {
                        return false;
                    }
                }
                return true;
            }

            {
                var A = (b.Y - a.Y) / (double)(b.X - a.X);
                var B = (a.Y * b.X - b.Y * a.X) / (double)(b.X - a.X);
                double f(int x) => A * x + B;
                foreach (var x in Between(a.X, b.X))
                {
                    var y = f(x);
                    if (IsVeryCloseToInt(y) && Contains(new Position(x, (int)Math.Round(y))))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private bool IsVeryCloseToInt(double n) =>
            Math.Abs(n - Math.Round(n)) < 0.000001;

        private static bool IsInt(double n) => n % 1 == 0;

        public static IEnumerable<int> Between(int a, int b)
        {
            var d = a > b ? -1 : 1;
            if (a > b)
            {
                for (var i = a - 1; i > b; i--)
                {
                    yield return i;
                }
            }
            else
            {
                for (var i = a + 1; i < b; i++)
                {
                    yield return i;
                }
            }
        }

        public string DrawVisible(Position from)
        {
            var visible = Asteroids.Where(a => IsVisible(from, a)).ToHashSet();
            var sb = new StringBuilder();

            foreach (var y in Enumerable.Range(0, _height))
            {
                foreach (var x in Enumerable.Range(0, _width))
                {
                    var p = new Position(x, y);
                    if (Asteroids.Contains(p))
                    {
                        if (p == from)
                        {
                            sb.Append("X ");
                        }
                        else
                        {
                            if (visible.Contains(p))
                            {
                                sb.Append("# ");
                            }
                            else
                            {
                                sb.Append("* ");
                            }
                        }
                    }
                    else
                    {
                        sb.Append(". ");
                    }
                }
                sb.Append(Environment.NewLine);
            }

            sb.Append(visible.Contains(from));
            return sb.ToString();
        }
    }
}
