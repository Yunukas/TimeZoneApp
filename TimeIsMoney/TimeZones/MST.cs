using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TimeIsMoney.TimeZones
{
    class MST : TimeZone
    {
        private const string name = "Mountain Standard Time";
    
        private const int utcDifference = -7;

        public MST() : base(name, utcDifference)
        {

        }
    }
}
