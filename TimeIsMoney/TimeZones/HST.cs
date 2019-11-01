using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TimeIsMoney.TimeZones
{
    class HST : TimeZone
    {
        private const string name = "Hawaii Standard Time";
    
        private const int utcDifference = -10;

        public HST() : base(name, utcDifference)
        {

        }


    }
}
