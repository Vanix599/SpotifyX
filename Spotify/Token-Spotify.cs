﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;

namespace SpotifyX_Console.Spotify
{
    class Auth_Spotify
    {
        class SpotifyAuthentication
        {
            public string clientID = "";
            public string clientSecret = "";
            public string redirectURL = "http://localhost:8080/";
        }
        public static void Access()
        {
            SpotifyAuthentication sAuth = new SpotifyAuthentication();
            using (HttpClient client = new HttpClient())
            {
              
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(sAuth.clientID + ":" + sAuth.clientSecret)));

                FormUrlEncodedContent formContent = new FormUrlEncodedContent(new[]
                {
                        new KeyValuePair<string, string>("code", Program.Token_user),
                        new KeyValuePair<string, string>("redirect_uri", "http://localhost:8080/"),
                        new KeyValuePair<string, string>("grant_type", "authorization_code"),
                    });

                var response = client.PostAsync("https://accounts.spotify.com/api/token", formContent).Result;

                var responseContent = response.Content;
                string responseString = responseContent.ReadAsStringAsync().Result;
                Console.WriteLine(responseString);
                Program.auth = Regex.Match(responseString, "{\"access_token\":\"(.*?)\",").Groups[1].Value.ToString();
                Program.refreshToken = Regex.Match(responseString, "\"refresh_token\":\"(.*?)\",").Groups[1].Value.ToString();
            }
        }
    }
}
