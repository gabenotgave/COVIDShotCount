using COVIDVaccinationCount.Data;
using COVIDVaccinationCount.Data.Models;
using System;
using System.Linq;
using System.Threading.Tasks;


namespace COVIDVaccinationCount
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Instantiating MongoDB class
            MongoCRUD db = new MongoCRUD("COVIDShotCount");
            // Querying most recently stored vaccination data
            var latestDbVaccinationRecord = db.GetLatestVaccinationRecord();

            // Instantiating CDC class (instantiation scrapes CDC website)
            CDC cdc = new CDC();
            // Getting necessary CDC data-points
            int cdcFirstDoses = cdc.Get1stDosesAdministered("US");
            int cdcSecondDoses = cdc.Get2ndDosesAdministered("US");
            int cdcDosesDistributed = cdc.GetDosesDistributed("US");
            int cdcUSPopulation = cdc.Get2019Census("US");

            // Checking if CDC has updated vaccination data by comparing their first doses to first doses stored in database
            if (cdcFirstDoses != latestDbVaccinationRecord.FirstDosesAdministered)
            {
                // Generating COVID vaccination tweet to post
                var generatedTweet = Twitter.GenerateCovidTweet(cdcFirstDoses, cdcSecondDoses);

                // Instantiating Twitter class
                Twitter twitter = new Twitter(
                    Credentials.GetValue("Twitter_Consumer_Key"),
                    Credentials.GetValue("Twitter_Consumer_Secret"),
                    Credentials.GetValue("Twitter_Access_Key"),
                    Credentials.GetValue("Twitter_Access_Secret")
                );
                // Posting tweet and replying to said tweet
                long tweetId = await twitter.Tweet(generatedTweet);
                await twitter.Reply(Twitter.GenerateImmunityTweet(Calculations.CalculateImmunity(cdcUSPopulation, cdcFirstDoses, cdcSecondDoses)), tweetId);

                // Inserting new data into database
                db.InsertRecord("Vaccinations", new VaccinationRecord
                {
                    FirstDosesAdministered = cdcFirstDoses,
                    SecondDosesAdministered = cdcSecondDoses,
                    DosesDistributed = cdcDosesDistributed,
                    DateTimeAdded = DateTime.Now
                });
            }

            // Checking if it is 1 P.M. on Saturday
            if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday && DateTime.Now.ToString("t") == "1:00 PM")
            {
                // Querying database for all vaccination records
                var _vaccinationRecords = db.LoadAllRecords<VaccinationRecord>("Vaccinations").OrderBy(x => x.DateTimeAdded);

                // Generating graph and assigning its bytes to variable
                var chartImageBytes = Chart.GenerateTwoBarGraph(
                    dateTimes: _vaccinationRecords.Select(x => x.DateTimeAdded).ToList(),
                    dataOne: _vaccinationRecords.Select(x => x.FirstDosesAdministered).ToList(),
                    barTitleOne: "First Doses Administered",
                    dataTwo: _vaccinationRecords.Select(x => x.SecondDosesAdministered).ToList(),
                    barTitleTwo: "Second Doses Administered"
                );

                // Instantiating Twitter class
                Twitter twitter = new Twitter(
                    Credentials.GetValue("Twitter_Consumer_Key"),
                    Credentials.GetValue("Twitter_Consumer_Secret"),
                    Credentials.GetValue("Twitter_Access_Key"),
                    Credentials.GetValue("Twitter_Access_Secret")
                );
                // Tweeting chart with generated text
                var generatedTweet = Twitter.GenerateVaccinationChartTweet(latestDbVaccinationRecord.DateTimeAdded);
                await twitter.TweetWithImage(generatedTweet, chartImageBytes);
            }
        }
    }
}
