using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AOC.days
{
    public class Day3 : AOCDay
    {
        public override Task<string> RunPartOne(string[] lines)
        {
            Box[] res = lines.Select(Box.ParseBox).ToArray();
            var maxWidth = res.Max(b => b.LeftOffset + b.Width);
            var maxHeight = res.Max(b => b.TopOffset + b.Height);
            var carpet = new int[maxWidth, maxHeight];
            
            foreach (var box in res)
            foreach (var (x, y) in box.ToPoints())
                carpet[x, y]++;

            return Task.FromResult(carpet.Cast<int>().Count(el => el > 1).ToString());
        }

        private static void SaveMatrixToFile(int[,] carpet)
        {
            var sb = new StringBuilder("Our final matrix:");
            sb.AppendLine();
            for (var y = 0; y < carpet.GetLength(0); y++)
            {
                for (var x = 0; x < carpet.GetLength(1); x++)
                {
                    sb.Append(carpet[x, y]);
                }

                sb.AppendLine();
            }

            File.WriteAllText("./foo.txt", sb.ToString());
            Console.WriteLine("Wrote to file....");
        }

        private class Box
        {
            public int Id { get; private set; }

            public int LeftOffset { get; private set; }
            public int TopOffset { get; private set; }
            public int Height { get; private set; }
            public int Width { get; private set; }

            public IEnumerable<(int, int)> ToPoints()
            {
                for (var x = LeftOffset; x < LeftOffset + Width; x++)
                for (var y = TopOffset; y < TopOffset + Height; y++)
                    yield return (x, y);
            }

            private static readonly Regex Parse = new Regex(@"#(\d+) @ (\d+),(\d+): (\d+)x(\d+)");

            public static Box ParseBox(string line)
            {
                int[] m = Parse.Match(line).Groups.Skip(1).Select(gr => int.Parse(gr.Value)).ToArray();
                return new Box
                {
                    Id = m[0],
                    LeftOffset = m[1],
                    TopOffset = m[2],
                    Width = m[3],
                    Height = m[4]
                };
            }
        }

        public override Task<string> RunPartTwo(string[] lines)
        {
            Box[] res = lines.Select(Box.ParseBox).ToArray();
            var maxWidth = res.Max(b => b.LeftOffset + b.Width) + 1;
            var maxHeight = res.Max(b => b.TopOffset + b.Height) + 1;
            Console.WriteLine($"We have a matrix: ({maxWidth}x{maxHeight})");
            var carpet = new int[maxWidth, maxHeight];
            foreach (var box in res)
            foreach (var (x, y) in box.ToPoints())
            {
                carpet[x, y]++;
            }


            return Task.FromResult(res
                .First(box => box.ToPoints().All((tuple => carpet[tuple.Item1, tuple.Item2] == 1))).Id.ToString());
        }
    }
}