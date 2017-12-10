namespace Tweets.Web.Models
{
    public class Tweet
    {
        public string FullText { get; set; }
        public string UserName { get; set; }
        public string ScreenName { get; set; }
        public string ProfileImageUrl { get; set; }
        public string MediaUrl { get; set; }
        public int RetweetCount { get; set; }
        public string TweetDate { get; set; }
    }
}
