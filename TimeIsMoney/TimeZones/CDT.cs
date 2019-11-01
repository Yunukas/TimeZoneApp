using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TimeIsMoney.TimeZones
{
    class CDT : TimeZone
    {
        private const string name = "Central Daylight Time";
    
        private const int utcDifference = -5;

        public CDT() : base(name, utcDifference)
        {

        }


    }
}
