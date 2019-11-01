using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeIsMoney
{
    interface ITimeZone
    {
        string Name { get; }
        string CurrentTime { get; set; }
        int UtcDifference { get; }

    }
}
