using System.Text;

namespace AOC2022.Common;

public static class PointExtensions
{
    public static string PrintPoints(this IEnumerable<Point2> points)
    {
        var sb = new StringBuilder();
        DrawPoints(points, sb);
        return sb.ToString();
    }

    public static void DrawPoints(this IEnumerable<Point2> points, StringBuilder sb)
    {
        var set = new HashSet<Point2>(points);
        var minX = set.Min(p => p.X) - 1;
        var minY = set.Min(p => p.Y) - 1;
        var maxX = set.Max(p => p.X) + 1;
        var maxY = set.Max(p => p.Y) + 1;

        for (var y = minY; y <= maxY; y++)
        {
            for (var x = minX; x <= maxX; x++)
            {
                if (set.Contains(new Point2(x, y)))
                {
                    sb.Append('#');
                }
                else
                {
                    sb.Append('.');
                }
            }
            sb.AppendLine();
        }
    }
}
