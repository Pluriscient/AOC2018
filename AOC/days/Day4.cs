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

            var guards = new Dictionary<int, List<(DateTime, DateTime)>>();

            int currentGuard = -1;
            Update lastUpdate = null;
            foreach (var update in updates.OrderBy(update => update.DateTime))
            {
                switch (update.UpdateKind)
                {
                    case Update.UpdateType.StartShift:
                        currentGuard = update.Guard ?? throw new Exception("GUard not FoUNd");
                        if (!guards.ContainsKey(currentGuard))
                            guards.Add(currentGuard, new List<(DateTime, DateTime)>());
                        break;
                    case Update.UpdateType.Awaken
                        when lastUpdate == null || lastUpdate.UpdateKind != Update.UpdateType.Asleep:
                        throw new Exception("Can't awaken w/o sleep");
                    case Update.UpdateType.Awaken:
                        guards[currentGuard].Add((lastUpdate.DateTime, update.DateTime));
                        break;
                    case Update.UpdateType.Asleep:
                    default:
                        break;
                }

                lastUpdate = update;
            }

            foreach (var guard in guards)
            {
//                Console.WriteLine($"WE have guard {guard.Key} that falls asleep on these dates:");
                foreach (var (start, end) in guard.Value)
                {
//                    Console.WriteLine(
//                        $"({start.ToShortTimeString()}-{end.ToShortTimeString()}) on {start.ToShortDateString()}");
                    if (start.Date != end.Date) throw new Exception("BLEH");
                }
            }

            int sleepiestGuard = -1, mostSleep = int.MinValue;
            foreach (var guard in guards)
            {
                int sleep = guard.Value.Select(tuple => (tuple.Item2 - tuple.Item1).Minutes).Sum();
                if (sleep <= mostSleep) continue;
                sleepiestGuard = guard.Key;
                mostSleep = sleep;
            }

            Console.WriteLine(
                $"Our sleepiest guard is #{sleepiestGuard} with a grand total of {mostSleep} minutes of sleep");

            int[] minutes = new int[60];
            foreach (var (start, end) in guards[sleepiestGuard])
            {
                for (int i = start.Minute; i < end.Minute; i++)
                {
                    minutes[i]++;
                }
            }

            int sleepiestMinute = -1;
            int daysSlept = -1;
            for (int i = 0; i < minutes.Length; i++)
            {
                if (minutes[i] <= daysSlept) continue;
                sleepiestMinute = i;
                daysSlept = minutes[i];
            }

            var result = sleepiestMinute * sleepiestGuard;
            Console.WriteLine($"We have {sleepiestMinute}");
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
            var updates = lines.Select(Update.ParseUpdate);

            var guards = new Dictionary<int, List<(DateTime, DateTime)>>();

            int currentGuard = -1;
            Update lastUpdate = null;
            foreach (var update in updates.OrderBy(update => update.DateTime))
            {
                switch (update.UpdateKind)
                {
                    case Update.UpdateType.StartShift:
                        currentGuard = update.Guard ?? throw new Exception("GUard not FoUNd");
                        if (!guards.ContainsKey(currentGuard))
                            guards.Add(currentGuard, new List<(DateTime, DateTime)>());
                        break;
                    case Update.UpdateType.Awaken
                        when lastUpdate == null || lastUpdate.UpdateKind != Update.UpdateType.Asleep:
                        throw new Exception("Can't awaken w/o sleep");
                    case Update.UpdateType.Awaken:
                        guards[currentGuard].Add((lastUpdate.DateTime, update.DateTime));
                        break;
                    case Update.UpdateType.Asleep:
                    default:
                        break;
                }

                lastUpdate = update;
            }

            Console.WriteLine("Starting...");

            int mostSleptInMinute = -1;
            int sleepiestMinute = -1, sleepiestGuard = -1;
            foreach (var guard in guards)
            {
                var minutes = new int[60];
                foreach (var (start, end) in guard.Value)
                {
                    for (int i = start.Minute; i < end.Minute; i++)
                    {
                        minutes[i]++;
                    }
                }
                if (minutes.Max(x => x) <= mostSleptInMinute) continue;
                sleepiestGuard = guard.Key;
                for (var i = 0; i < minutes.Length; i++)
                {
                    if (minutes[i] <= mostSleptInMinute) continue;
                    sleepiestMinute = i;
                    mostSleptInMinute = minutes[i];
                }
            }

            Console.WriteLine($"Found guard #{sleepiestGuard} at minute {sleepiestMinute} ({mostSleptInMinute})");
            int result = sleepiestGuard * sleepiestMinute;
            return Task.FromResult(result.ToString());
        }
    }
}