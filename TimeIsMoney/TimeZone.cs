using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeIsMoney
{
    /// <summary>
    /// This class will be a base class to
    /// all different timezones we want to create
    /// </summary>
    abstract class TimeZone : ITimeZone
    {
        // name of the timezone
        public string Name { get; }

        // current time of the timezone
        public string CurrentTime { get; set; }

        // the hour difference from the UTC time
        public int UtcDifference { get; }
        
        public TimeZone(string name, int utcDiff)
        {
            Name = name;
            UtcDifference = utcDiff;
        }
    }
}
