using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AoC2019.Puzzle19
{
    public class Grid
    {
        public Grid(int width, int height)
        {
            _width = width;
            _height = height;
            _fields = new Field[_width * _height];
        }

        private readonly int _width;
        private readonly int _height;
        private readonly Field[] _fields;

        public void Set(int x, int y, Field value)
        {
            _fields[x + y * _width] = value;
        }

        public long GetAffected()
        {
            return _fields.Count(f => f == Field.Affected);
        }

        public string Draw()
        {
            var sb = new StringBuilder();
            foreach (var (f, i) in _fields.Select((v, i) => (value: v, index: i)))
            {
                if (i % _width == 0)
                {
                    sb.Append(Environment.NewLine);
                }
                sb.Append(_chars[f]);
            }
            return sb.ToString();
        }

        private readonly Dictionary<Field, char> _chars = new Dictionary<Field, char>()
        {
            { Field.Unchecked, ' ' },
            { Field.Stationary, '.' },
            { Field.Affected, '#' },
        };
    }
}
