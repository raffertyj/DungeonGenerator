using System;

namespace DungeonGenerator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var dg = new DungeonGenerator(200, 200);
            dg.GenerateEmptyMap();
            dg.PrintMap();
        }
    }
}
