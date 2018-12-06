using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AOC
{
    /// <summary>
    /// The Main File that runs the Day you want to run
    /// Reads in the available Days dynamically
    /// </summary>
    internal class Program
    {
        private static void Main(string[] args)
        {
            // Load all the available days, in order
            List<AOCDay> list = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(AOCDay)))
                .OrderBy(t => t.FullName)
                .Select(Activator.CreateInstance)
                .Select(o => (AOCDay) o)
                .ToList();
            // Default choice is the last available one
            var choice = GetChoice(list, list.Count);

            // Get our choice
            var chosenRunnable = list[choice - 1];
            var checkDay = Regex.Match(chosenRunnable.GetType().Name, "\\d+$").Value;
            Console.WriteLine($"Executing Day {checkDay}");

            // determine part to run
            var isPartOne = GetPart(true);

            // Get the base path
            var aocPath = Environment.GetEnvironmentVariable("AOC_PATH");
            if (aocPath == null)
            {
                Console.WriteLine("Could not find the AOC path, please check if it is set");
                Console.Read();
                throw new FileNotFoundException("AOC PATH NOT SET");
            }

            var basePath = Path.Combine(aocPath, $"day{choice}");


            var path = Path.Combine(basePath,
                isPartOne ? chosenRunnable.InputOneFileName : chosenRunnable.InputTwoFileName);
            if (!File.Exists(path))
            {
                Console.WriteLine("Invalid path: " + path);
                Console.Read();
                throw new FileNotFoundException("INVALID PATH", path);
            }

            string result;
            try
            {
                var task = isPartOne
                    ? chosenRunnable.RunPartOne(path)
                    : chosenRunnable.RunPartTwo(path);
                result = task.GetAwaiter().GetResult();
                Console.WriteLine(result);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Code ran into error: {e.Message}");
                throw;
            }

            SaveResult(basePath, isPartOne, result);
        }

        // Save the result to file
        private static void SaveResult(string basePath, bool isPartOne, string result)
        {
            Console.WriteLine("done, press y to save to file");
            var c = (char) Console.Read();
            if (char.ToLower(c) != 'y') return;
            var outPath = Path.Combine(basePath, $"out{(isPartOne ? 1 : 2)}.txt");
            File.WriteAllText(outPath, result);
            Console.WriteLine($"Saved output to {outPath}");
        }

        /// <summary>
        /// Get the part of the day to run
        /// </summary>
        /// <param name="isPartOne"></param>
        /// <returns></returns>
        private static bool GetPart(bool isPartOne)
        {
            Console.WriteLine("Please choose a part to run");
            var done = false;
            while (!done)
            {
                var line = Console.ReadLine();
                switch (line?.Trim())
                {
                    case "2":
                        isPartOne = false;
                        done = true;
                        break;
                    case "1":
                    case "0":
                    default:
                        done = true;
                        break;
                }
            }

            return isPartOne;
        }

        private static int GetChoice(ICollection list, int choice)
        {
            Console.WriteLine(@"Advent Of Code solutions");
            Console.WriteLine($"Please choose the day to execute from 1 to {choice} (or enter for the last one)");
            while (true)
            {
                var line = Console.ReadLine();
                var oldChoice = choice;
                if (int.TryParse(line, out choice) && choice > 0 && choice <= list.Count) break;
                choice = oldChoice;
                if (string.IsNullOrWhiteSpace(line))
                {
                    break;
                }

                Console.WriteLine($"Please enter a valid choice from 1 to {choice}");
            }

            return choice;
        }
    }
}