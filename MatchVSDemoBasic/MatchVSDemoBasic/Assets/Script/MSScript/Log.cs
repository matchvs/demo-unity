using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MatchVS
{
    class Log
    {
        internal static void i(Object v)
        {
            Console.WriteLine("[I] " + v);
        }
        internal static void i(Object tag,Object v)
        {
            Console.WriteLine("[I]["+ tag+"] " + v);
        }

        internal static void w(Object e)
        {
            Console.WriteLine("[W] "+ e);
        }
    }
}
