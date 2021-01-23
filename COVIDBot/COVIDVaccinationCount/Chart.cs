using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace COVIDVaccinationCount
{
    static class Chart
    {
        public static byte[] GenerateOneBarGraph(
            List<DateTime> dateTimes,
            List<int> data,
            string barTitle
            )
        {

            // Converting lists to string
            var _dateTimes = string.Join(", ", dateTimes.Select(x => "'" + x.ToString("d") + "'"));
            var _data = string.Join(", ", data);

            // Instantiating QuickChart (NuGet) class
            var qc = new QuickChart.Chart();
            // Setting size of graph
            qc.Width = 1000;
            qc.Height = 600;
            qc.BackgroundColor = "white";
            // Creating GET request URL
            qc.Config = @"{
                type: 'bar',
                data: {
                    labels: [" + _dateTimes + @"],
                    datasets: [{
                    label: '" + barTitle + @"',
                    data: [" + _data + @"]
                    }]
                }
            }";

            // Retrieving URL from QuickChart method
            var url = qc.GetUrl();

            // Getting photo from web (bytes)
            using var imgClient = new WebClient();
            var chartImageBytes = imgClient.DownloadData(url);

            return chartImageBytes;
        }

        public static byte[] GenerateTwoBarGraph(
            List<DateTime> dateTimes,
            List<int> dataOne,
            string barTitleOne,
            List<int> dataTwo,
            string barTitleTwo
            )
        {

            // Converting lists to string
            var _dateTimes = string.Join(", ", dateTimes.Select(x => "'" + x.ToString("d") + "'"));
            var _dataOne = string.Join(", ", dataOne);
            var _dataTwo = string.Join(", ", dataTwo);

            // Instantiating QuickChart (NuGet) class
            var qc = new QuickChart.Chart();
            // Setting size of graph
            qc.Width = 1000;
            qc.Height = 600;
            qc.BackgroundColor = "white";
            // Creating GET request URL
            qc.Config = @"{
                type: 'bar',
                data: {
                    labels: [" + _dateTimes + @"],
                    datasets: [{
                    label: '" + barTitleOne + @"',
                    data: [" + _dataOne + @"]
                    }, {
                    label: '" + barTitleTwo + @"',
                    data: [" + _dataTwo + @"]
                    }]
                }
            }";

            // Retrieving URL from QuickChart method
            var url = qc.GetUrl();

            // Getting photo from web (bytes)
            using var imgClient = new WebClient();
            var chartImageBytes = imgClient.DownloadData(url);

            return chartImageBytes;
        }
    }
}
