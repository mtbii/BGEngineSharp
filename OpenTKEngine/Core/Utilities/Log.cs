using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTKEngine.Core.Utilities
{
    public static class Log
    {
        public static void Error(string message)
        {
#if DEBUG
            Console.WriteLine("ERROR: {0}", message);
#endif
        }

        public static void Message(string message)
        {
#if DEBUG
            Console.WriteLine("MESSAGE: {0}", message);
#endif
        }

        public static void Warning(string message)
        {
#if DEBUG
            Console.WriteLine("WARNING: {0}", message);
#endif
        }

        public static void Fatal(string message)
        {
#if DEBUG
            Console.WriteLine("FATAL: {0}", message);
            System.Environment.Exit(1);
#endif
        }
    }
}
