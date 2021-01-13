using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi;

namespace COVIDVaccinationCount
{
    public class Twitter
    {
        private TwitterClient twitterClient;
        public Twitter(string CONSUMER_KEY, string CONSUMER_SECRET, string ACCESS_KEY, string ACCESS_SECRET)
        {
            this.twitterClient = new TwitterClient(CONSUMER_KEY, CONSUMER_SECRET, ACCESS_KEY, ACCESS_SECRET);
        }

        public async Task Tweet(string text)
        {
            await twitterClient.Tweets.PublishTweetAsync(text);
        }

        public static string GenerateCovidTweet(int count)
        {
            return @$"{count.IntToStrWithComma()} COVID-19 vaccinations have now been administered in the U.S.

{Math.Round((count / 328200000.0) * 100, 2).ToString()}% of the U.S. population now has immunity.";
        }
    }
}
