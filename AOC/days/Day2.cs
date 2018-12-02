using System.Linq;
using System.Threading.Tasks;

namespace AOC.days
{
    public class Day2 : AOCDay
    {
        /// <summary>
        /// Goal: Count the occurrences of double and triple characters
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        public override Task<string> RunPartOne(string[] lines)
        {
            // LINQ FTW
            // I'm a functional programmer it seems lol
            var result2 = lines.SelectMany(l =>
                l.ToCharArray()
                    .GroupBy(c => c)
                    .Select(group => group.Count())
                    .Where(size => size == 2)
                    .Distinct()).Count();
            var result3 = lines.SelectMany(l =>
                l.ToCharArray()
                    .GroupBy(c => c)
                    .Select(group => group.Count())
                    .Where(size => size == 3)
                    .Distinct()).Count();
            var result = result2 * result3;
            return Task.FromResult(result.ToString());
        }

        /// <summary>
        /// Goal: Find the Box IDs which only differ by one character
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        public override Task<string> RunPartTwo(string[] lines)
        {
            for (var i = 0; i < lines.Length; i++)
            {
                var curId = lines[i];
                foreach (var line in lines.Skip(i + 1))
                {
                    var diffCount = curId.Zip(line, (c1, c2) => c1 != c2 ? 1 : 0).Sum();
                    if (diffCount != 1) continue;
                    // we got our desired result
                    var result = string.Join("", curId.Zip(line, (c1, c2) => c1 == c2 ? c1 : ' ')
                        .Where(c => c != ' '));
                    return Task.FromResult(result);
                }
            }

            return Task.FromResult("Failed to find it");
        }
    }
}