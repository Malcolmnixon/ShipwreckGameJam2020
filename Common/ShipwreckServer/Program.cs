using System;

namespace ShipwreckServer
{
    public class Program
    {
        public static void Main()
        {
            // Create and start server
            var world = new Shipwreck.WorldWeb.WebServerWorld();
            world.Start();

            Console.WriteLine("Server started. Press ENTER to terminate.");
            Console.ReadLine();
        }
    }
}
