using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TimeIsMoney.TimeZones
{
    class MDT : TimeZone
    {
        private const string name = "Mountain Daylight Time";
    
        private const int utcDifference = -6;

        public MDT() : base(name, utcDifference)
        {

        }

    }
}
