using System;
using Telepathy;
using MessagePack;

namespace MasterServer.Sources
{
    class Program
    {
        const string intro = 
            "---------------------------\n" +
            "----- MASTER SERVER -------\n" +
            "----- VERSION 0.1.0 -------\n" +
            "---------------------------\n";

        static void Main(string[] args)
        {
            Console.WriteLine(intro);

            var lobbyServer = new MasterServer();
        }
    }
}
