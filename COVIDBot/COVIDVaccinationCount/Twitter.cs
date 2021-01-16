using System;
using System.Collections.Generic;
using System.Text;
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

        public async Task Reply(string text, long tweetId)
        {
            // Retrieving tweet through provided tweet id
            var tweetToReplyTo = await twitterClient.Tweets.GetTweetAsync(tweetId);

            // Publishing reply
            await twitterClient.Tweets.PublishTweetAsync(new PublishTweetParameters(text)
            {
                InReplyToTweet = tweetToReplyTo
            });
        }

        public static string GenerateImmunityTweet(double percentage)
        {
            return @$"@COVIDShotCount {percentage.ToString()}% of the U.S. population now has immunity.

We project herd immunity to be attained on {Calculations.ProjectHerdImmunity(percentage).ToString("M/d/yyyy")}.";
        }

        public static string GenerateCovidTweet(int firstDoses, int secondDoses)
        {
            return @$"U.S. COVID-19 VACCINATIONS:

{firstDoses.IntToStrWithComma()} 1st doses administered
{secondDoses.IntToStrWithComma()} 2nd doses administered

(CDC, {DateTime.Now.ToString("M/d/yyyy")})";
        }
    }
}
