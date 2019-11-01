using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TimeIsMoney
{
    public static class Utility
    {
        // Nist server connection information
        public static readonly List<string> nistServerList = new List<string>()
        {
            "time-a-g.nist.gov",
            "time-b-g.nist.gov",
            "time-c-g.nist.gov",
            "time-d-g.nist.gov",
            "time-e-g.nist.gov"
        };

        // this is the index pointing to the current server
        // in the nistServerList
        private static int serverIndex = 0;

        // port we will use for connection to nist server
        private const int port = 13;

        // the date time format received from Nist server
        internal const string dateTimeFormatOfNist = "yy-MM-dd HH:mm:ss";
        internal const string customDateTimeFormat = "MM/dd/yyyy   hh:mm:ss tt";

        // this method returns a TCP client connection to a given server at a given port
        private static async Task<TcpClient> GetTcpClient(string server, int port)
        {
            var client = new TcpClient();

            client.BeginConnect(server, port, null, null);

            await Task.Delay(500);

            if (!client.Connected)
            {
                throw new ConnectionFailedException($"Connection Failed! <Port: {port}>");
            }
            return client;
        }

        // this method returns the datetime in string format from a time server
        public static async Task<string> GetUTCTimeStringAsync()
        {
            // get the TCP client asyncronously
            var client = await GetTcpClient(nistServerList[serverIndex], port);
        
            using (var stream = client.GetStream())
            using (var ms = new MemoryStream())
            {
                // Set buffer size to 4K
                int BufferSize = 4096;
                var buffer = new byte[BufferSize];
                int read = 0;

                while ((read = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }

                string response = Encoding.UTF8.GetString(ms.ToArray());
                Console.WriteLine(response);

                if (response.Length > 0)
                {
                    if (response.StartsWith("Access denied"))
                        throw new AccessDeniedException("Too many requests!");

                    // filter out the time information from the received text
                    // received text is in below format:
                    // 58788 19-11-01 00:37:14 03 0 0 487.0 UTC(NIST) *
                    var utcDateTimeString = response.Substring(7, 17);
                    
                    return utcDateTimeString;
                }
                else
                {
                    throw new NoResponseException("Empty response from the server!");
                }
            }
            
        }
        // this method is called after and error
        // it updates the server index, so the next server is polled
        public static void UpdateServerIndex() {
            serverIndex = (serverIndex + 1) % nistServerList.Count;
        }

        // this method returns the current server
        public static string GetCurrentServer()
        {
            return nistServerList[serverIndex];
        }
    }
}
