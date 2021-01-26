using COVIDVaccinationCount.Data;
using COVIDVaccinationCount.Data.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace COVIDVaccinationCount
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Assigning execution date/time to variable in case processes before routine checks exceed total of a minute
            var executionTime = DateTime.Now;

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
                // Posting tweet
                long tweetId = await twitter.Tweet(generatedTweet);
                //await twitter.Reply(Twitter.GenerateImmunityTweet(Calculations.CalculateImmunity(cdcUSPopulation, cdcFirstDoses, cdcSecondDoses)), tweetId);

                // Inserting new data into database
                db.InsertRecord("Vaccinations", new VaccinationRecord
                {
                    FirstDosesAdministered = cdcFirstDoses,
                    SecondDosesAdministered = cdcSecondDoses,
                    DosesDistributed = cdcDosesDistributed,
                    DateTimeAdded = DateTime.Now
                });


                // OTHER CHECKS

                // Calculating 1 in how many people received a vaccination
                // If the result of the calculation is the same as that of the last CDC data update (stored in db), the method will return zero
                int oneInHowManyPeople = Calculations.OneInHowManyPeopleReceivedVaccination(
                    latestDbVaccinationRecord.FirstDosesAdministered,
                    cdcFirstDoses,
                    cdcUSPopulation);

                // Checking if there is a new result (if greater than zero, there is)
                if (oneInHowManyPeople > 0)
                {
                    // Sleeping for fifteen minutes to give time between tweets
                    Thread.Sleep(1000 * 60 * 15);

                    // Generating and posting tweet
                    var generatedFractionTweet = Twitter.GenerateFractionTweet(oneInHowManyPeople);
                    await twitter.Tweet(generatedFractionTweet);
                }
            }


            // ROUTINES

            // Checking if current execution started at 1 P.M. on Saturday
            if (executionTime.DayOfWeek == DayOfWeek.Saturday && executionTime.ToString("t") == "1:00 PM")
            {
                // Querying database for all vaccination records
                var vaccinationRecords = db.LoadAllRecords<VaccinationRecord>("Vaccinations").OrderBy(x => x.DateTimeAdded);

                // Generating graph and assigning its image bytes to variable
                var chartImageBytes = Chart.GenerateTwoDatapointGraph(
                    chartType: "bar",
                    dateTimes: vaccinationRecords.Select(x => x.DateTimeAdded).ToList(),
                    dataOne: vaccinationRecords.Select(x => x.FirstDosesAdministered).Cast<object>().ToList(),
                    barTitleOne: "1st Doses Administered",
                    dataTwo: vaccinationRecords.Select(x => x.SecondDosesAdministered).Cast<object>().ToList(),
                    barTitleTwo: "2nd Doses Administered"
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

            // Checking if current execution started at 1 P.M. on Sunday
            if (executionTime.DayOfWeek == DayOfWeek.Sunday && executionTime.ToString("t") == "1:00 PM")
            {
                // Querying database for all vaccination records
                var vaccinationRecords = db.LoadAllRecords<VaccinationRecord>("Vaccinations").OrderBy(x => x.DateTimeAdded);

                // Generating graph and assigning its image bytes to variable
                var chartImageBytes = Chart.GenerateOneDatapointGraph(
                    chartType: "line",
                    dateTimes: vaccinationRecords.Select(x => x.DateTimeAdded).ToList(),
                    data: vaccinationRecords.Select(x => Calculations.CalculateImmunity(cdcUSPopulation, x.FirstDosesAdministered, x.SecondDosesAdministered)).Cast<object>().ToList(),
                    barTitle: "Immunity (% of U.S. population)"
                );

                // Instantiating Twitter class
                Twitter twitter = new Twitter(
                    Credentials.GetValue("Twitter_Consumer_Key"),
                    Credentials.GetValue("Twitter_Consumer_Secret"),
                    Credentials.GetValue("Twitter_Access_Key"),
                    Credentials.GetValue("Twitter_Access_Secret")
                );
                // Tweeting chart with generated text
                var generatedTweet = Twitter.GenerateImmunityChartTweet(latestDbVaccinationRecord.DateTimeAdded, Calculations.CalculateImmunity(cdcUSPopulation, cdcFirstDoses, cdcSecondDoses));
                await twitter.TweetWithImage(generatedTweet, chartImageBytes);
            }
        }
    }
}
