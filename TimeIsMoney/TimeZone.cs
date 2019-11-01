using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeIsMoney
{
    /// <summary>
    /// This class will be Super class to
    /// All different timezones we want to create
    /// </summary>
    abstract class TimeZone
    {
        // name of the timezone
        public string Name { get; }

        // the hour difference from the UTC time
        private int utcDifference;

        // current time of the timezone
        public string CurrentTime { get; set; }

        public TimeZone(string name, int utcDiff)
        {
            Name = name;
            utcDifference = utcDiff;
        }

        public int GetUtcDifference()
        {
            return utcDifference;
        }
    }
}
