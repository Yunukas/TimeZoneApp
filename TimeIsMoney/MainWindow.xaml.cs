using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TimeIsMoney
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        

        // TimeZone instances we will use in this program
        static List<TimeZone> timeZoneList = new List<TimeZone>()
        {
            new TimeZones.AKDT(),
            new TimeZones.CDT(),
            new TimeZones.EDT(),
            new TimeZones.HST(),
            new TimeZones.MDT(),
            new TimeZones.MST(),
            new TimeZones.PDT()
        };
        
 
        // INSTANCE VARIABLES
        private DateTime utcTime;
        private DateTime currentTimeZoneTime;

        public MainWindow()
        {
            InitializeComponent();

            time_zone_data_grid.ItemsSource = timeZoneList;
            RefreshCurrentServerText();
        }

        private void UpdateCurrentServer()
        {
            Utility.UpdateServerIndex();
            RefreshCurrentServerText();
        }
       
        private void RefreshCurrentServerText()
        {
            current_server_text_block.Text = Utility.GetCurrentServer();
        }

        private void UpdateConnectionIssueTextBlock(string text)
        {
            connection_issue_text_block.Text = text;
        }

        private void UpdateTimeDifferenceTextBlock(string text)
        {
            time_difference_text_block.Text = text;
        }

        // this method will asynchronously get the time information
        // from a chosen server, and based on the result, call other functions to update GUI
        public async Task GetUtcTimeAsync()
        {
            UpdateTimeDifferenceTextBlock("");
            UpdateConnectionIssueTextBlock("Connecting...");
            string utcDateTimeString = await Utility.GetUTCTimeStringAsync();

            utcTime = DateTime.ParseExact(
                                                    utcDateTimeString,
                                                    Utility.dateTimeFormatOfNist,
                                                    CultureInfo.CreateSpecificCulture("en-US"));

            // calculate the time in current time zone
            currentTimeZoneTime = DateTime.ParseExact(
                                                    utcDateTimeString,
                                                    Utility.dateTimeFormatOfNist,
                                                    CultureInfo.InvariantCulture,
                                                    DateTimeStyles.AssumeUniversal);

            Console.WriteLine(utcTime);

            //print time difference
            PrintTimeDifference();

            // fill in the data grid with timezoneList
            CalculateAllTimeZones();
            //using (var client = new TcpClient())
            //{
            //    UpdateConnectionIssueTextBlock("Connecting...");


            //    var result = client.BeginConnect(nistServerList[serverIndex], port, null, null);

            //    await Task.Delay(500);

            //    if (!client.Connected)
            //    {
            //        throw new ConnectionFailedException($"Connection Failed! <Port: {port}>");
            //    }



            //    // await client.ConnectAsync(nistServerList[serverIndex], port);

            //    Console.WriteLine("here");
            //    using (var stream = client.GetStream())
            //    using (var ms = new MemoryStream())
            //    {

            //        int BufferSize = client.ReceiveBufferSize;
            //        var buffer = new byte[BufferSize];
            //        int read = 0;

            //        while ((read = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
            //        {
            //            ms.Write(buffer, 0, read);
            //        }

            //        string response = Encoding.UTF8.GetString(ms.ToArray());
            //        Console.WriteLine(response);

            //        if (response.Length > 0)
            //        {
            //            if (response.StartsWith("Access denied"))
            //                throw new AccessDeniedException("Too many requests!");

            //            // filter out the time information from the received text
            //            // received text is in below format:
            //            // 58788 19-11-01 00:37:14 03 0 0 487.0 UTC(NIST) *
            //            var utcDateTimeString = response.Substring(7, 17);
            //            // convert the time string to DateTime
            //            utcTime = DateTime.ParseExact(
            //                                        utcDateTimeString,
            //                                        dateTimeFormatOfNist,
            //                                        CultureInfo.CreateSpecificCulture("en-US"));

            //            // calculate the time in current time zone
            //            currentTimeZoneTime = DateTime.ParseExact(
            //                                                    utcDateTimeString,
            //                                                    dateTimeFormatOfNist,
            //                                                    CultureInfo.InvariantCulture,
            //                                                    DateTimeStyles.AssumeUniversal);

            //            Console.WriteLine(utcTime);

            //            //print time difference
            //            PrintTimeDifference();

            //            // fill in the data grid with timezoneList
            //            CalculateAllTimeZones();
            //        }
            //        else
            //        {
            //            throw new NoResponseException("No response was received from the server!");
            //        }
            //    }
            //}
        }

        // this method will print the time difference between PC and the time server in seconds
        private void PrintTimeDifference()
        {
            string message = "";
            var pcTime = DateTime.Now;
            double diff = (pcTime - currentTimeZoneTime).TotalSeconds;
            if (diff < 0)
                message = String.Format("PC time is {0} seconds behind the server time.", Math.Abs(diff));
            else if (diff > 0 )
                message = String.Format("PC time is {0} seconds ahead of the server time.", diff);
            else
                message = String.Format("PC time and server time are in sync!");

            UpdateTimeDifferenceTextBlock(message);
        }
        // This method will use UTC difference information in each time zone and
        // calculate the current time for them, using the UTC time we received from server
        private void CalculateAllTimeZones()
        {
            foreach (TimeZone tz in timeZoneList)
            {
                // add UTC time difference for each Time Zone, and format to our needs
                tz.CurrentTime = utcTime.AddHours(tz.GetUtcDifference()).ToString(Utility.customDateTimeFormat);
            }
   
            // refresh the Data Grid with new time information
            time_zone_data_grid.Items.Refresh();
        }

        // This will run an async method to Get the time information
        // if there is an error with the connection, 
        private async void collect_button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // get time asynchronously
                await GetUtcTimeAsync();
                // request was successful, clear connection issue message
                UpdateConnectionIssueTextBlock("");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                // if there was an issue retrieving the time, move to the next server in the list
                UpdateCurrentServer();
                // print the issue on gui
                UpdateConnectionIssueTextBlock(ex.Message + " Try again!");
            }
           
        }
    }


}
