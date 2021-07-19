using System;
using System.Collections.Generic;
using System.Text;

namespace WatchGuard.RoverApi.Exercise.Models
{
    public class EarthDateResults
    {
        public List<string> FormattedEarthDates { get; set; }

        public List<string> UnformattableDateEntries { get; set; }

        public EarthDateResults(List<string> formattedEarthDates, List<string> unformattableDateEntries)
        {
            FormattedEarthDates = formattedEarthDates;
            UnformattableDateEntries = unformattableDateEntries;
        }
    }
}
