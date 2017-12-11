# TWEETS
_Tweets_ is a web application created to complete below challenge.

[![Cross platform Build Status](https://travis-ci.org/shibut12/Tweets.svg?branch=master)](https://travis-ci.org/shibut12/Tweets) [![Visual Studio build status](https://ci.appveyor.com/api/projects/status/6bx7tqof6godmelg?svg=true)](https://ci.appveyor.com/project/shibut12/tweets) 

## DEMO
Visit [tweets.codewithdot.net](http://tweets.codewithdot.net) to see a running version Tweets!. 

## CHALLENGE
* Build a simple Twitter feed reader that shows the last 10 tweets from [@salesforce](https://twitter.com/salesforce) user timeline.
* Each minute this list of tweets should automatically update to show only the 10 most recent tweets.
* There should also be an input field where the user can type to filter the current list of tweets by whether the input string is present anywhere in the content of a tweet.
* The following details should be displayed in a pleasing format for each tweet.
  * User name
  * User screen name (@screen_name)
  * User profile image
  * Tweet content (including images)
  * How many times the message was retweeted
  * Tweet date
* Build a simple server to handle authentication and interactions with Twitter’s API.
* The solution must be published to a public repository on Github.
* App should be hosted.
### FRONT-END RESTRICTIONS
* Following are the only allowed 3rd party JavaScript libraries.
  * Backbone.js
  * Underscore.js
  * jQuery
  * Jasmine
* Use the Lightning Design System (LDS) to style the application.
### BACK-END RESTRICTIONS
* The server should be runnable as a standalone process for evaluation.
* Server code should have effective unit tests.
---
### CONFIGURATION
* Application configuration
* 
---
### HOW TO RUN
This application needs .Net core 2.0 + runtime for server components. The development / build agents or machine should have Node JS, npm, and bower.
#### Prerequisites
* Install latest .Net Core
	https://www.microsoft.com/net/learn/get-started/windows
* Install Node js & npm
	https://nodejs.org/en/
* Install bower
 ```posh
 npm install -g bower
 ```
 #### Verify installed of Prerequisites
 Run following commands to verify you have all the dependencies got right.
 ```posh
dotnet --version
node --version
npm --version
bower --version
 ```
 #### Build, Test and Run Tweets
 ##### Restore and build web application
 Run following commands in a shell
 ```posh
cd Tweets.Web
dotnet restore
bower install
dotnet build
```
#### Restore, build, and test Test application
```posh
cd Tweets.Web.Tests
dotnet restore
dotnet build
dotnet test
```
#### Run Tweets web application
```posh
cd Tweets.Web
dotnet run
```
