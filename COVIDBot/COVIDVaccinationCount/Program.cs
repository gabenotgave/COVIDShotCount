using System;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Models;
using HtmlAgilityPack;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;


namespace COVIDVaccinationCount
{
    class Program
    {
        static async Task Main(string[] args)
        {
            CDC cdc = new CDC();
            int cdcDosesCount = cdc.GetDosesAdministered("US");
            //Bloomberg bloomberg = new Bloomberg();
            //int bloombergDosesCount = bloomberg.GetDosesAdministered();

            File file = new File("/home/pi/Desktop/CovidBot/count.txt");
            int storedDosesCount = file.ReadLines().First().StrToInt();

            if (cdcDosesCount != storedDosesCount)
            {
                var generatedTweet = Twitter.GenerateCovidTweet(cdcDosesCount);

                Twitter twitter = new Twitter(
                    Credentials.GetValue("Twitter_Consumer_Key"),
                    Credentials.GetValue("Twitter_Consumer_Secret"),
                    Credentials.GetValue("Twitter_Access_Key"),
                    Credentials.GetValue("Twitter_Access_Secret")
                );

                await twitter.Tweet(generatedTweet);

                file.Update(cdcDosesCount.ToString());
            }
        }
    }
}
