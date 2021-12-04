using System;
using System.IO;

namespace LanacHotelaServiceLayer
{
    internal class Constants
    {
        public static readonly int INITIAL_CONNECTIONS = 50;
        public static readonly int CONNECTIONS_INCREMENT = 25;
        public static string CONNECTOR_FILE = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + System.IO.Path.DirectorySeparatorChar + "HotelChainConnector.txt";
        public static string CONNECTION_STRING = "";

        static Constants()
        {
            CONNECTION_STRING = File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + Path.DirectorySeparatorChar + "HotelChainConnector.txt");
        }
    }
}
