using System;
using System.Collections.Generic;
using System.Text;

namespace MasterServer.Sources
{
    public static class ConsoleClr
    {
        public static void WriteLine(object obj, ConsoleColor color)
        {
            ConsoleColor oldColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(obj);
            Console.ForegroundColor = oldColor;
        }

        public static void WriteLine(string str, ConsoleColor color, params object[] args)
        {
            ConsoleColor oldColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(str, args);
            Console.ForegroundColor = oldColor;
        }

        public static void Write(object obj, ConsoleColor color)
        {
            ConsoleColor oldColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write(obj);
            Console.ForegroundColor = oldColor;
        }

        public static void Write(string str, ConsoleColor color, params object[] args)
        {
            ConsoleColor oldColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write(str, args);
            Console.ForegroundColor = oldColor;
        }
    }
}
