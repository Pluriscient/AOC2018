using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace AOC.days
{
    public class Day1 : AOCDay
    {
        public override string InputTwoFileName { get; } = "inputGees";

        public override Task<string> RunPartOne(string[] lines)
        {

            long sum = lines.Select(long.Parse).Sum();
            return Task.FromResult(sum.ToString());
        }

        public override Task<string> RunPartTwo(string[] lines)
        {
            var before = DateTime.Now;
            var list = lines.Select(int.Parse).ToList();
            var history = new HashSet<int>();
            var current = 0;
            history.Add(0);
            while (true)
            {
                foreach (var i in list)
                {
                    current += i;
                    if (history.Contains(current)) return Task.FromResult(current.ToString());
                    history.Add(current);
                }
            }

            

        }
    }
}