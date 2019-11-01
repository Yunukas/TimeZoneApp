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
        // List of TimeZone to be used in this program
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
        static private DateTime utcTime;
        static private DateTime currentTimeZoneTime;

        public MainWindow()
        {
            InitializeComponent();

            // bind the items soure for the data grid
            time_zone_data_grid.ItemsSource = timeZoneList;

            // show the current server
            RefreshCurrentServerText();
        }

        // this method updates the current server to the next one in the list
        private void UpdateCurrentServer()
        {
            Utility.UpdateServerIndex();
            RefreshCurrentServerText();
        }
       
        // this method refreshes the current serer text block
        private void RefreshCurrentServerText()
        {
            current_server_text_block.Text = Utility.GetCurrentServer();
        }

        // this method updates the connection issue text block
        // with a proper message
        private void UpdateConnectionIssueTextBlock(string text)
        {
            connection_issue_text_block.Text = text;
        }

        // this method updates the time difference text block
        // with a proper message
        private void UpdateTimeDifferenceTextBlock(string text)
        {
            time_difference_text_block.Text = text;
        }

        // this method asynchronously gets the time information
        // from a chosen server, and based on the result, 
        // calls other functions to update GUI
        private async Task GetUtcTimeAsync()
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

            //calculate and print the time difference
            CalculateTimeDifference();

            // fill in the data grid with timezoneList
            CalculateAllTimeZones();
           
        }

        // this method will calculate and print the time difference 
        // between PC and the time server in seconds
        private void CalculateTimeDifference()
        {
            string message;
            var pcTime = DateTime.Now;
            double diff = (pcTime - currentTimeZoneTime).TotalSeconds;
            if (diff < 0)
                message = String.Format("PC time is {0} seconds behind the server time.", Math.Abs(diff));
            else if (diff > 0 )
                message = String.Format("PC time is {0} seconds ahead of the server time.", diff);
            else
                message = String.Format("PC time and server time are in sync!");

            // print it on the GUI
            UpdateTimeDifferenceTextBlock(message);
        }
        // This method uses UTC difference information in each time zone and
        // calculates the current time for them, 
        // using the UTC time we received from server
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

        // This method runs an async method to get the time information
        // if there is an error with the connection, 
        // we will print the exception on a textblock
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
