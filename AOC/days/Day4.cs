using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AOC.days
{
    public class Day4 : AOCDay
    {
        public override Task<string> RunPartOne(string[] lines)
        {
            var updates = lines.Select(Update.ParseUpdate);

            var guards = new Dictionary<int, List<(int, int)>>();
            int current = -1;
            int lastEvent = -1;
            foreach (var group in updates.GroupBy(x=>x.DateTime.Date))
            {
                var el = group.OrderBy(x => x.DateTime);
                int i = 0;
                foreach (var update in el)
                {
                    if ()
                }
            }

//            foreach (var update in updates.OrderBy(u => u.DateTime))
//            {
//                switch (update.UpdateKind)
//                {
//                    case Update.UpdateType.StartShift:
//                        current = update.Guard ?? -1;
//                        if (!guards.ContainsKey(current))
//                            guards.Add(current, new List<(int, int)>());
//                        break;
//                    case Update.UpdateType.Awaken:
//                        var sleep = (lastEvent, update.DateTime.Minute);
//                        guards[current].Add(sleep);
//                        break;
//                    case Update.UpdateType.Asleep:
//                        lastEvent = update.DateTime.Minute;
//                        break;
//                    default:
//                        throw new InvalidDataException();
//                }
//            }

            if (guards.ContainsKey(-1) || guards.Any(kvp => kvp.Value.Any(t => t.Item1 < 0)))
            {
                throw new Exception();
            }

            int max = int.MinValue;
            int sleepGuard = -1;
            foreach (KeyValuePair<int, List<(int, int)>> kvp in guards)
            {
                int sleeptime = kvp.Value.Select(tuple => Math.Abs(tuple.Item2 - tuple.Item1)).Sum();
                if (sleeptime <= max) continue;
                sleepGuard = kvp.Key;
                max = sleeptime;
            }
            
            int[] sleepMinutes = Enumerable.Range(0, 60).ToArray();
            
            foreach ((int start, int end) in guards[sleepGuard])
            {
                for (int i = start; i < end; i++)
                {
                    sleepMinutes[i]++;
                }
            }

            int maxSleep = sleepMinutes.Max(x => x);
            int bestIndex = -1;
            for (var i = 0; i < sleepMinutes.Length; i++)
            {
                if (sleepMinutes[i] != maxSleep) continue;
                bestIndex = i;
                break;
            }

            Console.WriteLine($"min: {bestIndex}, guard: {sleepGuard}");
            int result = bestIndex * sleepGuard;
            return Task.FromResult(result.ToString());
        }


        private class Update
        {
            public DateTime DateTime { get; private set; }

            public enum UpdateType
            {
                Asleep,
                Awaken,
                StartShift
            }

            public UpdateType UpdateKind { get; private set; }

            public int? Guard { get; private set; }

            private static readonly Regex ParseRegex = new Regex(@"\[(.*)\] (.*)$");
            private static readonly DateTimeFormat DateFormat = new DateTimeFormat("YYYY-MM-DD HH:MM");

            public static Update ParseUpdate(string line)
            {
                line = line.ToLowerInvariant();
                var m = ParseRegex.Match(line);
                string dateString = m.Groups[1].Value;
                string typeString = m.Groups[2].Value;
                var date = DateTime.Parse(dateString, DateFormat.FormatProvider);
                int? guard = null;
                UpdateType type;
                switch (typeString)
                {
                    case "wakes up":
                        type = UpdateType.Awaken;
                        break;
                    case "falls asleep":
                        type = UpdateType.Asleep;
                        break;
                    default:
                        type = UpdateType.StartShift;
                        guard = int.Parse(Regex.Match(typeString, @"(\d+)").Groups[1].Value);
                        break;
                }

                return new Update
                {
                    DateTime = date,
                    UpdateKind = type,
                    Guard = guard
                };
            }
        }

        public override Task<string> RunPartTwo(string[] lines)
        {
            throw new NotImplementedException();
        }
    }
}