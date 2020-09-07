//using DevDefined.OAuth.Consumer;
//using DevDefined.OAuth.Framework;

using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using TinyOAuth1;

namespace HTStats
{
    public class ChppAccess
    {
        string requestUrl = "https://chpp.hattrick.org/oauth/request_token.ashx";
        string userAuthorizeUrl = "https://chpp.hattrick.org/oauth/authorize.aspx";
        string accessUrl = "https://chpp.hattrick.org/oauth/access_token.ashx";
        string consumerKey = "nonsense string here";  //get this from HT when your product is approved
        string consumerSecret = "nonsense string here"; //get this from HT when your product is approved
        string accessToken = "nonsense string here";  //request this from HT, see below
        string accessTokenSecret = "nonsense string here"; //request this from HT, see below
        public HttpClient session;

        public ChppAccess() 
        {
            // Setup basic config
            var config = new TinyOAuthConfig
            {
                AccessTokenUrl = accessUrl,
                AuthorizeTokenUrl = userAuthorizeUrl,
                RequestTokenUrl = requestUrl,
                ConsumerKey = consumerKey,
                ConsumerSecret = consumerSecret
            };

            // Use the library
            var tinyO = new TinyOAuth(config);
            //create the session
            GetOAuthSession(config, tinyO);

        }


        //Very clumsy OAuth implementation. Only provides for 1 user and
        //requires you run the application several times to complete the process.
        public async Task GetOAuthSession(TinyOAuthConfig config, TinyOAuth tinyO)
        {
            // STEP 1
            //Get the request token and request token secret.
            //Uncomment the 3 lines below 
            //var requestTokenInfo = await tinyO.GetRequestTokenAsync();
            //Debug.WriteLine("REQ TOKEN: " + requestTokenInfo.RequestToken);
            //Debug.WriteLine("REQ TOKEN SECRET: " + requestTokenInfo.RequestTokenSecret);

            //Construct the authorization url
            //Uncomment the 2 lines below.
            //var authorizationUrl = tinyO.GetAuthorizationUrl(requestTokenInfo.RequestToken);
            //Debug.WriteLine("AUTH URL: " + authorizationUrl);

            //Stop the program and then comment out the 5 lines again. <-- Important, start over if you forget

            //From the debug output, copy/paste the request token and request token secret into
            //the matching parameters below in step 3.

            //STEP 2
            //From the debug output, copy/paste the URL into your browser and approve your product.
            //The code provided by Hattrick's website is your verifier code to use in step 3. 

            //STEP 3
            //Get the access token & secret
            //Uncomment the 3 lines below. 
            //var accessTokenInfo = await tinyO.GetAccessTokenAsync("your req token", "your req token secret", "your chpp verifier code");
            //Debug.WriteLine("ACCESS TOKEN: " + accessTokenInfo.AccessToken);  
            //Debug.WriteLine("ACCESS TOKEN SECRET: " + accessTokenInfo.AccessTokenSecret);

            //Stop the program again and comment out the 3 lines again. <-- Important, start over if you forget

            //Finally, copy/paste the access token and the access secret into the variables at the start of this file. 
            //You can also delete the tinyO parameter/variable if you wish. 

            //you can comment out this try/catch if you don't care for the errors you'll get on the
            //3 steps above, but it's the only code which should remain uncommented when you're done.
            try
            {
                var httpClient = new HttpClient(new TinyOAuthMessageHandler(config, accessToken, accessTokenSecret));
                session = httpClient;
            }
            catch (Exception e)
            {
                //TODO: Write to log file
                Debug.WriteLine(e.Message);
            }

            //Now we just use the HttpClient like normally
            //To test that your authentication is approved, uncomment the 3 lines below.
            //If you receive an XML structure with match data you're good to go. 
            //var resp = await session.GetAsync("https://chpp.hattrick.org/chppxml.ashx?file=matchdetails&version=3.0&matchEvents=false&matchID=52282172");
            //var respJson = await resp.Content.ReadAsStringAsync();
            //Debug.WriteLine("MATCH DETAILS: " + respJson);
        }
    }

   
}
