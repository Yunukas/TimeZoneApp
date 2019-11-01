using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TimeIsMoney
{
    public static class Utility
    {
        // Nist server connection information
        static readonly List<string> nistServerList = new List<string>()
        {
            "time-a-g.nist.gov",
            "time-b-g.nist.gov",
            "time-c-g.nist.gov",
            "time-d-g.nist.gov",
            "time-e-g.nist.gov"
        };

        // this is the index pointing to the current server
        // in the nistServerList
        static int serverIndex = 0;

        // port we will use for connection to nist server
        const int port = 13;

        // the date time format received from Nist server
        internal const string dateTimeFormatOfNist = "yy-MM-dd HH:mm:ss";
        internal const string customDateTimeFormat = "MM/dd/yyyy   hh:mm:ss tt";
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

        public static async Task<string> GetUTCTimeStringAsync()
        {
            var client = await GetTcpClient(nistServerList[serverIndex], port);
        
            using (var stream = client.GetStream())
            using (var ms = new MemoryStream())
            {
                int BufferSize = client.ReceiveBufferSize;
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
                    // convert the time string to DateTime
                    //DateTime utcTime = DateTime.ParseExact(
                    //                            utcDateTimeString,
                    //                            dateTimeFormatOfNist,
                    //                            CultureInfo.CreateSpecificCulture("en-US"));

                    //// calculate the time in current time zone
                    //currentTimeZoneTime = DateTime.ParseExact(
                    //                                        utcDateTimeString,
                    //                                        dateTimeFormatOfNist,
                    //                                        CultureInfo.InvariantCulture,
                    //                                        DateTimeStyles.AssumeUniversal);

                    return utcDateTimeString;
                }
                else
                {
                    throw new NoResponseException("No response was received from the server!");
                }
            }
            
        }

        public static void UpdateServerIndex() {
            serverIndex = (serverIndex + 1) % nistServerList.Count;
        }

        public static string GetCurrentServer()
        {
            return nistServerList[serverIndex];
        }
    }
}
