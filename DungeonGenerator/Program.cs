using System;

namespace DungeonGenerator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var dg = new DungeonGenerator(50, 50);
            dg.GenerateEmptyMap();
            dg.PrintMap();
        }
    }
}
