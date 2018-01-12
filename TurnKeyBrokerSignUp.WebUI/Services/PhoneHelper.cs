using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Net.Http;

namespace TurnKeyBrokerSignUp.WebUI.Services
{
    public static class PhoneHelper
    {
        private class APIResult
        {
            public String status { get; set; }
            public String linetype { get; set; }
            public String location { get; set; }
            public String countrycode { get; set; }
            public String formatnational { get; set; }
            public String formatinternational { get; set; }
        }
        public static string ValidatePhoneNumber(string CountryCode, string PhoneNumber)
        {
            string output = "ERROR";
            string phoneAPIKey = ConfigurationManager.AppSettings["PhoneValidatorAPIKey"].ToString();
            string defaultLocale = ConfigurationManager.AppSettings["PhoneValidatorDefaultLocale"].ToString();
            string phoneAPIURL = ConfigurationManager.AppSettings["PhoneValidatorAPIURL"].ToString();

            var postData = new List<KeyValuePair<string, string>>();
            postData.Add(new KeyValuePair<string, string>("PhoneNumber", PhoneNumber));
            postData.Add(new KeyValuePair<string, string>("CountryCode", CountryCode));
            postData.Add(new KeyValuePair<string, string>("Locale", defaultLocale));
            postData.Add(new KeyValuePair<string, string>("APIKey", phoneAPIKey));

            HttpClient client = new HttpClient();
            HttpContent content = new FormUrlEncodedContent(postData);

            HttpResponseMessage result = client.PostAsync(phoneAPIURL, content).Result;
            string resultContent = result.Content.ReadAsStringAsync().Result;

            APIResult res = new System.Web.Script.Serialization.JavaScriptSerializer().
                                Deserialize<APIResult>(resultContent);
            switch (res.status)
            {
                case "VALID_CONFIRMED":
                case "VALID_UNCONFIRMED":
                    output = res.formatinternational;
                    break;
                case "INVALID":
                default:
                    output = "ERROR";
                    break;
            }

            return output;
        }
    }
}