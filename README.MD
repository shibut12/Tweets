# TWEETS
_Tweets_ is a web application created to complete below challenge.

[![Cross platform Build Status](https://travis-ci.org/shibut12/Tweets.svg?branch=master)](https://travis-ci.org/shibut12/Tweets) [![Visual Studio build status](https://ci.appveyor.com/api/projects/status/6bx7tqof6godmelg?svg=true)](https://ci.appveyor.com/project/shibut12/tweets) 

## DEMO
Visit [tweets.codewithdot.net](http://tweets.codewithdot.net) to see a running version of Tweets!. 

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


## DESIGN

---
### SERVER SIDE COMPONENTS
Server side component is implemented in ASP.NET Core. The server module is responsible for, running own web server, authenticating user sessions, serving static files, and communicating with Twitter API's to retrieve user's time line feed.
##### SECURITY (AUTHENTICATION & AUTHORIZATION)
_Tweets_ application uses TWITTER as an identity provider for authenticating user session. Application uses _Authentication_ middleware for generating application cookie, create a challenge to redirect user to Twitter sign in page, respond to call back from Twitter after user session is authenticated. 
This application uses _CookieAuthentication_ as Authorization mechanism to preserve user identity till user session is signed out.
This application uses _TwitterAuthentication_ to validate user identity. This application neither create user profiles nor retain any information about users.
##### MIDDLEWARE ARCHITECTURE
_Middleware_ is an independent application component, it is integrated into ASP.NET Core application pipeline. Each middleware will get its own order and time to process each request and response.

_Authentication modules_, _ASP.NET Core MVC_, _StaticFiles_ are the middleware used by _Tweets_ application.

![alt text](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/_static/request-delegate-pipeline.png "Middleware architecture")
##### DEPENDENCY INJECTION OF SERVICES
_Tweets_ application defines reusable, and independent components as _Services_. These services are _Injected_ into ASP.NET Core _DI Container_. The instantiation adn destruction of these components are controlled by ASP.NET Core. Every _Controller_ or _Class_ that needed a specific service will receive instance of the required service in its constructor.

_Tweets_ application defines, _TwitterService_ and _HttpHelper_ classes are services. As these are injected into the controllers, these services are decoupled from main application and can be easily _mocked_ while unit testing.

__Twitter APIs__

The application uses [https://api.twitter.com/1.1/statuses/user_timeline.json](https://api.twitter.com/1.1/statuses/user_timeline.json) to Get recent tweets for a given user name. The `count=10` parameter fetch only last 10 Tweets from users timeline.
The query string parameter `include_entities=true`  force twitter not to _truncate_ the response and include all the associated fileds such as `media_url_https` and `profile_image_url_https` in the response payload.

##### UNIT TESTING
XUnit.Net is used along with MOQ framework for unit testing server side components. Each controller and service is unit tested to ensure they return expected types.
Dependencies are Mocked using _MOQ_.


### CLIENT SIDE COMPONENTS
---

The user facing components are build using _Razor Views (CSHTML)_ along with _Backbone_, _Underscore_, and jQuery.
##### MODELS AND VIEWS USING BACKBONE & UNDERSCORE
__Backbone Model__

Tweets application has defined a __Backbone Model__ for following fields,
  * User name
  * User screen name (@screen_name)
  * User profile image
  * Tweet content (including images)
  * How many times the message was retweeted
  * Tweet date

This Model will enforce the consistency of structure single twitter feed across the life cycle of program. 

__Backbone Model Collection__

The generated __Backbone Model__ is then used for creating a __Model Collection__. This Model will enforce the consistency of structure user timeline feed across the life cycle of program.

__Backbone View__

A __Backbone View__ is created to hold information about a single tweet. A collection of Views are extended to hold a _Twitter feed_ from user's time line  (10 Tweets at any time). Using a __Backbone template__ this View is Rendered on __web page__.

In order to refresh latest tweets every 1 Minute (60 seconds), the _initialize_ of View is extended amd a _setInterval_ is set with value  _60000_.

As the complexity of data retrieval is fairly simple, application uses _fetch_ function provided in _backbone_ to retrieve Twitterfeed from Server side component.

To implement the filter to display the tweets that have a __search term__ somewhere in the Tweet content, we use the `_.filter` of Underscore. Once the data is filtered, _Model Collection (tweets)_ is reset (cleaned) and set filtered values to it. Since the values in tweets collection is changed it will trigger `add` event of `view` and it will force the view to `Render`.

__Backbone view template__

This application makes use of the template engine from _Backbone_, A `text/template` script tag is  is defined `index.html` of `HomeController`. This template holds the structure of `slds-feed-item` of `Feeds` defined by _Salesforce lightning design  system_. backbone use this template to `Render` twitter timeline of @salesforce.

##### VANILLA JAVASCRIPT
_Tweets_ application has very minimal _vanilla javascript_ code in it. __IIFE (Immediately Invoked Function Expression)__ design pattern is used for implementing all vanilla javascript code to avoid polluting global scope.

##### SALESFORCE LIGHTNING DESIGN SYSTEM
Markup's are styled using __Salesforce Lightning design system__. To display latest 10 tweets from @salesforce user timeline, the __Feeds__ component is used. [Read more about it feed here](https://www.lightningdesignsystem.com/components/feeds). 


### CONTINUOUS INTEGRATION
---

#### AUTOMATED BUILD & TEST: APPVEYOR
__APPVEYOR__ provides a free Visual studio environment for building .NET application that are hosted in public repositories. it provides quick integration with __GitHub__ and easy to setup. I use _APPVEYOR_ for automated building and Unit testing.

[Link to AppVeyor](https://www.appveyor.com/)
#### AUTOMATED BUILD & TEST: TRAVIS CI
__Travis CI__ provides a _Linux(Ubuntu)_ environment for automated building and Unit testing Open source projects. Similar to_AppVeyor_ it is easy to integrate with __GitHub__ and easy to setup a build.
I use __Travis CI__ for ensuring that the application is compatible __CROSS PLATFORM__.

[Link to Travis CI](https://travis-ci.org/)
#### CODE COVERAGE: OPENCOVER & CODECOVE.IO 
In the beginning, I decided to use __OpenCover__ for generating Code coverage report for server side components and __CodeCove.io__ to to Display reports. It was included in the _CI pipeline_ but due to a Bug in _OpenCover_ with _MOQ_, it is impossible to generate code coverage reports. 
[Click here to read more about OpenCover open issue with MOQ](https://github.com/OpenCover/opencover/issues/771).


### CONFIGURATION
---
#### Application configuration
Before you start running teh application, you need to install the application in [apps.twitter.com](https://apps.twitter.com). Once you complete application registration update the appSettings.json file with Settings from _Tweets and Acess Tokens_ tab.

AppSettings.json file is never checked in with real key in it. 
```json
{
  "Authentication": {
    "Twitter": {
      "ConsumerKey": "YOUR-CONSUMER-KEY",
      "ConsumerSecret": "YOUR-CONSUMER-SECRET",
      "oauth_token": "YOUR-ACCESS-TOKEN",
      "oauth_token_secret": "YOUR-ACCESS-TOKEN-SECRET"
    }
  }
}
```


### HOW TO RUN
---

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
 #### Verify installation of Prerequisites
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
 git clone https://github.com/shibut12/Tweets.git
cd Tweets.Web
dotnet restore
bower install
dotnet build
```
#### Restore, build, and test Test application
```posh
git clone https://github.com/shibut12/Tweets.git
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
Open [http://localhost:5007](http://localhost:5007) on your favorite browser to open _Tweets_ application.


### DEMO OF TWEETS APPLICATION
---

* Link to demo application is [http://tweets.codewithdot.net](http://tweets.codewithdot.net).
#### HOW IS IT HOSTED?
* The demo web application is hosted in a shared Ubuntu 16.04LTS web server.
* On the host application server, application listens on port 5007 (http://*5007).
* __Supervisor__ is responsible for keeping application alive. 
* __NGINX__ listens on Port 80 of the server and redirect requests to port 5007, when incoming url matches tweets.codewithdot.net or www.tweets.codewithdot.net
![alt text](https://raw.githubusercontent.com/shibut12/Tweets/master/hosting-setup.png "Hosting environment")
