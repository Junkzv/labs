using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CalendarApp
{
    public class Event
    {
        public DateTime Date { get; set; }
        public string Description { get; set; }

        public Event(DateTime date, string description)
        {
            Date = date;
            Description = description;
        }

        public override string ToString()
        {
            return $"{Date:yyyy-MM-dd} - {Description}";
        }
    }

    public class CalendarManager
    {
        private const string FilePath = "events.txt";
        public List<Event> Events { get; private set; }

        public CalendarManager()
        {
            Events = new List<Event>();
            LoadEvents();
        }

        public void AddEvent(Event e)
        {
            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            Events.Add(e);
            SaveEvents();
        }

        public void RemoveEvent(Event e)
        {
            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            Events.Remove(e);
            SaveEvents();
        }

        public void SaveEvents()
        {
            File.WriteAllLines(FilePath, Events.Select(e =>
                $"{e.Date:yyyy-MM-dd}|{e.Description}"));
        }

        public void LoadEvents()
        {
            if (File.Exists(FilePath))
            {
                var lines = File.ReadAllLines(FilePath);
                foreach (var line in lines)
                {
                    var parts = line.Split('|');
                    if (parts.Length == 2)
                    {
                        if (DateTime.TryParse(parts[0], out DateTime date))
                        {
                            Events.Add(new Event(date, parts[1]));
                        }
                    }
                }
            }
        }
    }
}
