using System;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Parameters;

namespace COVIDVaccinationCount
{
    public class Twitter
    {
        private TwitterClient twitterClient;
        public Twitter(string CONSUMER_KEY, string CONSUMER_SECRET, string ACCESS_KEY, string ACCESS_SECRET)
        {
            this.twitterClient = new TwitterClient(CONSUMER_KEY, CONSUMER_SECRET, ACCESS_KEY, ACCESS_SECRET);
        }

        public async Task<long> Tweet(string text)
        {
            var tweet = await twitterClient.Tweets.PublishTweetAsync(text);
            return tweet.Id;
        }

        public async Task<long> TweetWithImage(string text, byte[] imageBytes)
        {
            //var tweetinviLogoBinary = System.IO.File.ReadAllBytes(imageBytes);
            var uploadedImage = await twitterClient.Upload.UploadTweetImageAsync(imageBytes);

            // Publishing tweet with image
            var tweet = await twitterClient.Tweets.PublishTweetAsync(new PublishTweetParameters(text)
            {
                Medias = { uploadedImage }
            });

            return tweet.Id;
        }

        public async Task<long> Reply(string text, long tweetId)
        {
            // Retrieving tweet through provided tweet id
            var tweetToReplyTo = await twitterClient.Tweets.GetTweetAsync(tweetId);

            // Publishing reply
            var tweet = await twitterClient.Tweets.PublishTweetAsync(new PublishTweetParameters(text)
            {
                InReplyToTweet = tweetToReplyTo
            });

            return tweet.Id;
        }

        public static string GenerateVaccinationChartTweet(DateTime asOfDate)
        {
            return $"Progress of COVID-19 vaccinations in the U.S. (as of {asOfDate.ToString("d")}):";
        }

        public static string GenerateImmunityChartTweet(DateTime asOfDate, double percentage)
        {
            return $"As of {asOfDate.ToString("d")}, {percentage.ToString()}% of the U.S. population has immunity. " +
                $"If the United States maintains this pace, herd immunity (70% of the population) will be attained on " +
                $"{Calculations.ProjectHerdImmunity(percentage).ToString("M/d/yyyy")}.";
        }

        public static string GenerateFractionTweet(int fraction)
        {
            return $"1 in {fraction} people have now received a COVID-19 vaccination in the United States.";
        }

        public static string GenerateImmunityTweet(double percentage)
        {
            return @$"@COVIDShotCount {percentage.ToString()}% of the U.S. population now has immunity.

We project herd immunity (70%) to be attained on {Calculations.ProjectHerdImmunity(percentage).ToString("M/d/yyyy")}.";
        }

        public static string GenerateAdministrationByManufacturerTweet(int pfizer, int moderna, int janssen)
        {
            return $@"BY MANUFACTURER:

Pfizer - {pfizer.IntToStrWithComma()} doses
Moderna - {moderna.IntToStrWithComma()} doses
J&J - {janssen.IntToStrWithComma()} doses";
        }

        public static string GenerateWorldTweet(int dosesAdministered, int peopleFullyVaccinated, int increase)
        {
            return @$"GLOBAL COVID-19 VACCINATIONS 🌎

{dosesAdministered.IntToStrWithComma()} people vaccinated
{peopleFullyVaccinated.IntToStrWithComma()} people fully vaccinated
+{increase.IntToStrWithComma()} since previous update

(OWID, {DateTime.Now.ToString("M/d/yyyy")})";
        }

        public static string GenerateCovidTweet(int distributed, int firstDoses, int secondDoses, int increase)
        {
            return @$"U.S. COVID-19 VACCINATIONS:

{distributed.IntToStrWithComma()} doses distributed
{firstDoses.IntToStrWithComma()} first doses administered
{secondDoses.IntToStrWithComma()} fully vaccinated people
+{increase.IntToStrWithComma()} since previous update

(CDC, {DateTime.Now.ToString("M/d/yyyy")})";
        }

        public static string GenerateMinorsTweet(int minorsFullyVaccinated)
        {
            return @$"{minorsFullyVaccinated.IntToStrWithComma()} children (age < 18) have been vaccinated against COVID-19 in the United States.

This constitutes {Math.Round(minorsFullyVaccinated/56600000.0*100, 2)}% of K-12 pupils.";
        }
    }
}
