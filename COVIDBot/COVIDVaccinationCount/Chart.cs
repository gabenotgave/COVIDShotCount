using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace COVIDVaccinationCount
{
    static class Chart
    {
        public static byte[] GenerateOneDatapointGraph<T>(
            string chartType,
            List<DateTime> dateTimes,
            List<T> data,
            string barTitle
            ) where T : IConvertible
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
                type: '" + chartType + @"',
                data: {
                    labels: [" + _dateTimes + @"],
                    datasets: [{
                    label: '" + barTitle + @"',
                    data: [" + _data + @"]
                    }]
                },
                options: {
                    layout: {
                      padding: {
                        left: 20,
                        right: 20,
                        top: 20,
                        bottom: 20
                        }
                    }
                }
            }";

            // Retrieving URL from QuickChart method
            var url = qc.GetUrl();

            // Getting photo bytes from web
            using var imgClient = new WebClient();
            var chartImageBytes = imgClient.DownloadData(url);

            return chartImageBytes;
        }

        public static byte[] GenerateTwoDatapointGraph<T>(
            string chartType,
            List<DateTime> dateTimes,
            List<T> dataOne,
            string barTitleOne,
            List<T> dataTwo,
            string barTitleTwo
            ) where T : IConvertible
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
                type: '" + chartType + @"',
                data: {
                    labels: [" + _dateTimes + @"],
                    datasets: [{
                    label: '" + barTitleOne + @"',
                    data: [" + _dataOne + @"]
                    }, {
                    label: '" + barTitleTwo + @"',
                    data: [" + _dataTwo + @"]
                    }]
                },
                options: {
                    layout: {
                      padding: {
                        left: 20,
                        right: 20,
                        top: 20,
                        bottom: 20
                        }
                    }
                }
            }";

            // Retrieving URL from QuickChart method
            var url = qc.GetUrl();

            // Getting photo bytes from web
            using var imgClient = new WebClient();
            var chartImageBytes = imgClient.DownloadData(url);

            return chartImageBytes;
        }
    }
}
