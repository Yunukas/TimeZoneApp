using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TimeIsMoney.TimeZones
{
    class EDT : TimeZone
    {
        private const string name = "Eastern Daylight Time";
    
        private const int utcDifference = -4;

        public EDT() : base(name, utcDifference)
        {

        }


    }
}
