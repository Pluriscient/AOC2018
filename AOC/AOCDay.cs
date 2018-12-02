using System.IO;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace AOC
{
    /// <summary>
    /// A day in the Advent Of Code adventure
    /// </summary
    [UsedImplicitly]
    public abstract class AOCDay
    {
        protected AOCDay()
        {
        }

        /// <summary>
        /// Path to the input file of the first part
        /// </summary>
        public virtual string InputOneFileName { get; } = "input.txt";

        /// <summary>
        /// Path to the input file of the second part
        /// </summary>
        public virtual string InputTwoFileName { get; } = "input.txt";


        /// <summary>
        /// Runs part one of this day, loading the given input file
        /// marked as virtual as it should be replaced in those cases
        /// where we don't want to just load the lines 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public virtual async Task<string> RunPartOne(string path)
        {
            string[] lines = await File.ReadAllLinesAsync(path).ConfigureAwait(false);
            return await RunPartOne(lines);
        }

        /// <summary>
        /// Runs part one of the day
        /// </summary>
        /// <param name="lines"></param>
        /// <remarks>Mark as deprecated if the virtual task is changed</remarks>
        /// <returns>String of output to display and store</returns>
        public abstract Task<string> RunPartOne(string[] lines);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public virtual async Task<string> RunPartTwo(string path)
        {
            string[] lines = await File.ReadAllLinesAsync(path).ConfigureAwait(false);
            return await RunPartTwo(lines);
        }

        /// <summary>
        /// Runs part two of this day, loading the given input file
        /// marked as virtual as it should be replaced in those cases
        /// where we don't want to just load the lines 
        /// </summary>
        /// <param name="lines"></param>
        /// <remarks>Mark as deprecated if the virtual task is changed</remarks>
        /// <returns>String of output to display and store</returns>
        public abstract Task<string> RunPartTwo(string[] lines);
    }
}