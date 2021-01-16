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
            var deleteme = Calculations.CalculateImmunity(328200000, 10595866, 1610524);

            CDC cdc = new CDC();
            int cdcFirstDoses = cdc.Get1stDosesAdministered("US");
            int cdcSecondDoses = cdc.Get2ndDosesAdministered("US");
            //Bloomberg bloomberg = new Bloomberg();
            //int bloombergDosesCount = bloomberg.GetDosesAdministered();

            File file = new File("/home/pi/Desktop/CovidBot/firstDoses.txt");
            int storedFirstDoses = file.ReadLines().First().StrToInt();

            if (cdcFirstDoses != storedFirstDoses)
            {
                var generatedTweet = Twitter.GenerateCovidTweet(cdcFirstDoses, cdcSecondDoses);

                Twitter twitter = new Twitter(
                    Credentials.GetValue("Twitter_Consumer_Key"),
                    Credentials.GetValue("Twitter_Consumer_Secret"),
                    Credentials.GetValue("Twitter_Access_Key"),
                    Credentials.GetValue("Twitter_Access_Secret")
                );

                long tweetId = await twitter.Tweet(generatedTweet);
                await twitter.Reply(Twitter.GenerateImmunityTweet(Calculations.CalculateImmunity(328200000, cdcFirstDoses, cdcSecondDoses)), tweetId);

                file.Update(cdcFirstDoses.ToString());
            }
        }
    }
}
