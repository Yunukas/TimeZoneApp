using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TimeIsMoney.TimeZones
{
    class AKDT : TimeZone
    {
        private const string name = "Alaska Daylight Time";
    
        private const int utcDifference = -8;

        public AKDT(): base(name, utcDifference)
        {

        }


    }
}
