using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AOC.days
{
    public class Day6 : AOCDay
    {
        public override Task<string> RunPartOne(string[] lines)
        {
            var points = lines.Select(line =>
            {
                var l = line.Split(",");
                return (int.Parse(l[0].Trim()), int.Parse(l[1].Trim()));
            }).ToArray();

            int minX = points.Min(tuple => tuple.Item1);
            int minY = points.Min(tuple => tuple.Item2);
            int maxX = points.Max(tuple => tuple.Item1);
            int maxY = points.Max(tuple => tuple.Item2);

            var map = new int[maxX - minX + 1, maxY - minY + 1];
            for (var x = 0; x < map.GetLength(0); x++)
            for (var y = 0; y < map.GetLength(1); y++)
                map[x, y] = ClosestPoint(x + minX, y + minY, points);

            // now remove all the numbers present at the edges
            var toRemove = new HashSet<int>();
            for (var x = 0; x < map.GetLength(0); x++)
            {
                toRemove.Add(map[x, 0]);
                toRemove.Add(map[x, map.GetLength(1) - 1]);
            }

            for (var y = 0; y < map.GetLength(1); y++)
            {
                toRemove.Add(map[0, y]);
                toRemove.Add(map[map.GetLength(0) - 1, y]);
            }

            var counts = new int[points.Length];
            for (var x = 0; x < map.GetLength(0); x++)
            for (var y = 0; y < map.GetLength(1); y++)
                if (toRemove.Contains(map[x, y]))
                    map[x, y] = -1;
                else
                    counts[map[x, y]]++;


            return Task.FromResult(counts.Max(x => x).ToString());
        }

        private static int ClosestPoint(int x, int y, IReadOnlyList<(int, int)> points)
        {
            int curMin = int.MaxValue;
            int curPoint = -1;
            for (var i = 0; i < points.Count; i++)
            {
                (int px, int py) = points[i];
                int distance = Math.Abs(px - x) + Math.Abs(py - y);
                if (distance > curMin) continue;
                if (distance == curMin)
                {
                    curPoint = -1;
                }
                else
                {
                    curMin = distance;
                    curPoint = i;
                }
            }

            return curPoint;
        }

        public override Task<string> RunPartTwo(string[] lines)
        {
            var points = lines.Select(line =>
            {
                var l = line.Split(",");
                return (int.Parse(l[0].Trim()), int.Parse(l[1].Trim()));
            }).ToArray();

            int minX = points.Min(tuple => tuple.Item1);
            int minY = points.Min(tuple => tuple.Item2);
            int maxX = points.Max(tuple => tuple.Item1);
            int maxY = points.Max(tuple => tuple.Item2);

            var map = new int[maxX - minX + 1, maxY - minY + 1];
            for (var x = 0; x < map.GetLength(0); x++)
            for (var y = 0; y < map.GetLength(1); y++)
                map[x, y] = SumOfDistances(x + minX, y + minY, points);
            const int regionLimit = 10000;
            int regionSize = map.Cast<int>().Count(i => i < regionLimit);


            return Task.FromResult(regionSize.ToString());
        }

        private static int SumOfDistances(int x, int y, IEnumerable<(int, int)> points)
        {
            var sum = 0;

            foreach (var t in points)
            {
                (int px, int py) = t;
                int distance = Math.Abs(px - x) + Math.Abs(py - y);
                sum += distance;
            }

            return sum;
        }
    }
}