using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

namespace AOC.days
{
    public class Day5 : AOCDay
    {
        public override Task<string> RunPartOne(string[] lines)
        {
            var polymer = lines[0].ToCharArray();
            int originalLength = polymer.Length;
            var result = new List<char> {polymer[0]};
            for (int i = 1; i < polymer.Length; i++)
            {
                char current = polymer[i];
                char prev = result[result.Count - 1];

                if (current == prev || char.ToLower(current) != char.ToLower(prev))
                {
                    result.Add(current);
                    continue; // AA or aa}
                }

                result.RemoveAt(result.Count - 1);
                if (result.Count != 0) continue;
                result.Add(polymer[i + 1]);
                i++;
            }

            int final = result.Count;
            Console.WriteLine($"new Polymer:\n{string.Join("", result)}");

            return Task.FromResult(final.ToString());
        }

        public override Task<string> RunPartTwo(string[] lines)
        {
            var origpolymer = lines[0].ToCharArray();
            int minLength = int.MaxValue;
            for (var currentChar = 'a'; currentChar <= 'z'; currentChar++)
            {
                var polymer = origpolymer.Where(c => char.ToLower(c) != currentChar).ToArray();
              
                var result = new List<char> { polymer[0] };
                for (var i = 1; i < polymer.Length; i++)
                {
                    char current = polymer[i];
                    char prev = result[result.Count - 1];

                    if (current == prev || char.ToLower(current) != char.ToLower(prev))
                    {
                        result.Add(current);
                        continue; // AA or aa}
                    }

                    result.RemoveAt(result.Count - 1);
                    if (result.Count != 0) continue;
                    result.Add(polymer[i + 1]);
                    i++;
                }

                minLength = Math.Min(minLength, result.Count);

            }


            int final = minLength;

            return Task.FromResult(final.ToString());
        }
    }
}