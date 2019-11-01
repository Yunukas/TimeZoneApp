using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TimeIsMoney.TimeZones
{
    class PDT : TimeZone
    {
        private const string name = "Pacific Daylight Time";
    
        private const int utcDifference = -7;

        public PDT() : base(name, utcDifference)
        {

        }
    }
}
