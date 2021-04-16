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
            int cdcFirstDoses = cdc.Get1stDosesAdministered("US"); // First doses does not include Janssen
            int cdcFullyVaccinated = cdc.GetFullyVaccinated("US");
            int cdcDosesDistributed = cdc.GetDosesDistributed("US");
            int cdcUSPopulation = cdc.Get2019Census("US");
            int cdcFullyVaccinatedMinors = cdc.GetFullyVaccinatedMinors("US"); // Fully vaccinated total - fully vaccinated adults

            // Checking if CDC has updated vaccination data by comparing their first doses to first doses stored in database
            if (cdcFirstDoses > latestDbVaccinationRecord.FirstDosesAdministered)
            {
                // Calculating increase of total vaccinations since previous data record
                int increase = (cdcFirstDoses + cdcFullyVaccinated) - (latestDbVaccinationRecord.FirstDosesAdministered + latestDbVaccinationRecord.SecondDosesAdministered);

                // Instantiating Twitter class
                Twitter twitter = new Twitter(
                    Credentials.GetValue("Twitter_Consumer_Key"),
                    Credentials.GetValue("Twitter_Consumer_Secret"),
                    Credentials.GetValue("Twitter_Access_Key"),
                    Credentials.GetValue("Twitter_Access_Secret")
                );

                // Posting tweet
                long tweetId = await twitter.Tweet(Twitter.GenerateCovidTweet(cdcDosesDistributed, cdcFirstDoses, cdcFullyVaccinated, increase));

                // Instantiating Owid class (instantiation scrapes Owid GitHub)
                Owid owid = new Owid();
                int owidPeopleVaccinated = owid.GetVaccinatedPeople("World");
                int owidPeopleFullyVaccinated = owid.GetFullyVaccinatedPeople("World");
                int owidTotalVaccinations = owid.GetTotalVaccinations("World");
                int globalIncrease = owidTotalVaccinations - latestDbVaccinationRecord.GlobalDosesAdministered; // Calculating increase of global vaccinations since previous data record

                // Replying global vaccine administrations tweet
                await twitter.Reply(Twitter.GenerateWorldTweet(owidPeopleVaccinated, owidPeopleFullyVaccinated, globalIncrease), tweetId);

                // Getting vaccine data by manufacturer to use in reply
                //int cdcPfizerDoses = cdc.GetPfizerDosesAdministered("US");
                //int cdcModernaDoses = cdc.GetModernaDosesAdministered("US");
                //int cdcJanssenDoses = cdc.GetJanssenDosesAdministered("US");

                // Replying vaccine administration tweet by manufacturer
                //await twitter.Reply(Twitter.GenerateAdministrationByManufacturerTweet(cdcPfizerDoses, cdcModernaDoses, cdcJanssenDoses), tweetId);

                // Inserting new data into database
                db.InsertRecord("Vaccinations", new VaccinationRecord
                {
                    FirstDosesAdministered = cdcFirstDoses,
                    SecondDosesAdministered = cdcFullyVaccinated,
                    FullyVaccinatedMinors = cdcFullyVaccinatedMinors,
                    DosesDistributed = cdcDosesDistributed,
                    GlobalDosesAdministered = owidTotalVaccinations,
                    DateTimeAdded = DateTime.Now
                });

                // Making POST request to website (COVIDShotCount.org) to clear cache
                await Website.ClearCache("vaccinations-cg");


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

            // Checking if current execution started at 7 P.M. on Friday
            if (executionTime.DayOfWeek == DayOfWeek.Friday && executionTime.ToString("t") == "7:00 PM")
            {
                // Instantiating Twitter class
                Twitter twitter = new Twitter(
                    Credentials.GetValue("Twitter_Consumer_Key"),
                    Credentials.GetValue("Twitter_Consumer_Secret"),
                    Credentials.GetValue("Twitter_Access_Key"),
                    Credentials.GetValue("Twitter_Access_Secret")
                );
                // Tweeting chart with generated text
                var generatedTweet = Twitter.GenerateMinorsTweet(cdcFullyVaccinatedMinors);
                // Posting tweet
                await twitter.Tweet(generatedTweet);
            }

            // Checking if current execution started at 1 P.M. on Saturday
            if (executionTime.DayOfWeek == DayOfWeek.Saturday && executionTime.ToString("t") == "1:00 PM")
            {
                // Querying database for all vaccination records
                var vaccinationRecords = db.LoadAllRecords<VaccinationRecord>("Vaccinations").OrderBy(x => x.DateTimeAdded);

                // Generating graph and assigning its image bytes to variable
                var chartImageBytes = Chart.GenerateTwoDatapointGraph(
                    chartType: "bar",
                    dateTimes: vaccinationRecords.Select(x => x.DateTimeAdded).ToList(),
                    dataOne: vaccinationRecords.Select(x => x.FirstDosesAdministered).ToList(),
                    barTitleOne: "1st Doses Administered",
                    dataTwo: vaccinationRecords.Select(x => x.SecondDosesAdministered).ToList(),
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
                    data: vaccinationRecords.Select(x => Calculations.CalculateImmunity(cdcUSPopulation, x.FirstDosesAdministered, x.SecondDosesAdministered)).ToList(),
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
                var generatedTweet = Twitter.GenerateImmunityChartTweet(latestDbVaccinationRecord.DateTimeAdded, Calculations.CalculateImmunity(cdcUSPopulation, cdcFirstDoses, cdcFullyVaccinated));
                await twitter.TweetWithImage(generatedTweet, chartImageBytes);
            }
        }
    }
}
