(function ($) {
    var Tweet = Backbone.Model.extend({
        urlRoot: '/TwitterFeed/GetAll',
        defaults: function () {
            return {
                fullText: '',
                mediaUrl: '',
                profileImageUrl: '',
                retweetCount: '',
                screenName: '',
                tweetDate: '',
                userName:''
            }
        }
    });
    var TweetsList = Backbone.Collection.extend({
        model: Tweet,
        url: '/TwitterFeed/GetAll'
    });

    var tweets = new TweetsList();

    var TweetView = Backbone.View.extend({
        model: new Tweet(),
        tagName: 'li',
        initialize: function () {
            this.template = _.template($('#nu-tweet-template').html());
        },
        render: function () {
            this.$el.html(this.template(this.model.toJSON()));
            return this;
        }
    });

    var TweetsView = Backbone.View.extend({
        model: tweets,
        el: $('#nu-tweets-container'),
        events: {
            'click #tweetfilterbutton': 'filterTweets'
        },
        initialize: function () {
            var self = this;
            this.model.on('add', this.render, this);
            tweets.fetch({
                success: function () {
                    self.render();
                },
                error: function () {
                    consol.log("Error occured");
                }
            });
            setInterval(function () {
                tweets.fetch({
                success: function () {
                    self.render();
                },
                error: function () {
                    consol.log("Error occured");
                }
            });
            }, 60000);

        },
        filterTweets: function (ev) {
            console.log(tweets);
        },
        render: function () {
            var self = this;
            self.$el.html('');
            _.each(this.model.toArray(), function (tweet, i) {
                self.$el.append((new TweetView({ model: tweet })).render().$el);
            });
            return this;
        }
    });

    $(document).ready(function () {

        $('#FilterTweet').submit(function (ev) {
            var searchString = $('#tweetfilter').val();
            var tempTweets = _.filter(tweets.toJSON(), function (item) {
                return item.fullText.indexOf(searchString) != -1;
            });
            tweets.reset();
            var tempTweetsCol = new TweetsList();
            _.each(tempTweets, function (tweet, i) {
                var tempTweet = new Tweet({
                    fullText: tweet.fullText,
                    mediaUrl: tweet.mediaUrl,
                    profileImageUrl: tweet.profileImageUrl,
                    retweetCount: tweet.retweetCount,
                    screenName: tweet.screenName,
                    tweetDate: tweet.tweetDate,
                    userName: tweet.userName
                });
                tweets.add(tempTweet);
            });
            console.log(tweets.toJSON());
            console.log(tempTweets);
            return false;
        });

        var appView = new TweetsView();
    });
    
})(jQuery);