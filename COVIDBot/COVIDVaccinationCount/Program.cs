using COVIDVaccinationCount.Data;
using COVIDVaccinationCount.Data.Models;
using System;
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
            var dbFirstDoses = db.GetLatestVaccinationCount();

            // Instantiating CDC class (instantiation scrapes CDC website)
            CDC cdc = new CDC();
            // Getting necessary CDC data-points
            int cdcFirstDoses = cdc.Get1stDosesAdministered("US");
            int cdcSecondDoses = cdc.Get2ndDosesAdministered("US");
            int cdcDosesDistributed = cdc.GetDosesDistributed("US");

            // Checking if CDC has updated vaccination data by comparing their first doses to first doses stored in database
            if (cdcFirstDoses != dbFirstDoses)
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
                await twitter.Reply(Twitter.GenerateImmunityTweet(Calculations.CalculateImmunity(328200000, cdcFirstDoses, cdcSecondDoses)), tweetId);

                // Inserting new data into database
                db.InsertRecord("Vaccinations", new VaccinationRecord
                {
                    FirstDosesAdministered = cdcFirstDoses,
                    SecondDosesAdministered = cdcSecondDoses,
                    DosesDistributed = cdcDosesDistributed,
                    DateTimeAdded = DateTime.Now
                });
            }
        }
    }
}
