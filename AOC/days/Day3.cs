using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AOC.days
{
    public class Day3 : AOCDay
    {
        public override Task<string> RunPartOne(string[] lines)
        {
            // The non-optimized way
            var commonPoints = new HashSet<Point>();
            Box[] res = lines.Select(Box.ParseBox).ToArray();
            Console.WriteLine();
            for (var i = 0; i < res.Length; i++)
            {
                Console.Write($"\rCurrently at box {i} of {res.Length}");
                var current = res[i];
                foreach (var other in res.Skip(i + 1))
                {
                    foreach (var overlappingPoint in current.OverlappingPoints(other))
                    {
                        commonPoints.Add(overlappingPoint);
                    }
                }
            }

            return Task.FromResult(commonPoints.Count.ToString());
        }

        private class Box
        {
            private int LeftOffset { get; set; }
            private int TopOffset { get; set; }
            private int Height { get; set; }
            private int Width { get; set; }

            public IEnumerable<Point> OverlappingPoints(Box other)
            {
                return ToPoints().Intersect(other.ToPoints());
            }


            private IEnumerable<Point> ToPoints()
            {
                for (var x = LeftOffset; x <= LeftOffset + Width; x++)
                {
                    for (var y = TopOffset; y <= TopOffset + Height; y++)
                    {
                        yield return new Point(x, y);
                    }
                }
            }

            private static readonly Regex Parse = new Regex(@"#(\d+) @ (\d+),(\d+): (\d+)x(\d+)");

            public static Box ParseBox(string line)
            {
                int[] m = Parse.Match(line).Groups.Skip(1).Select(gr => int.Parse(gr.Value)).ToArray();
                return new Box
                {
                    LeftOffset = m[0],
                    TopOffset = m[1],
                    Width = m[2],
                    Height = m[3]
                };
            }
        }

        public override Task<string> RunPartTwo(string[] lines)
        {
            throw new System.NotImplementedException();
        }
    }
}