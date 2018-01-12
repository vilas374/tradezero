using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Configuration;

namespace TurnKeyBrokerSignUp.WebUI.Models
{
    public class NumberVerification
    {
        public string apiKey =ConfigurationManager.AppSettings["NexmoAPIKey"].ToString();
        public string apiSecret = ConfigurationManager.AppSettings["NexmoAPISecret"].ToString();
        public string apiBrand = ConfigurationManager.AppSettings["NexmoAPIBrand"].ToString();

        //For general trade
        public string generalTradeApiKey = ConfigurationManager.AppSettings["GeneralTradeNexmoAPIKey"].ToString();
        public string generalTradeApiSecret = ConfigurationManager.AppSettings["GeneralTradeNexmoAPISecret"].ToString();
        public string generalTradeApiBrand = ConfigurationManager.AppSettings["GeneralTradeNexmoAPIBrand"].ToString();

        //For livestream
        public string liveStreamApiKey = ConfigurationManager.AppSettings["LiveStreamNexmoAPIKey"].ToString();
        public string liveStreamApiSecret = ConfigurationManager.AppSettings["LiveStreamNexmoAPISecret"].ToString();
        public string liveStreamApiBrand = ConfigurationManager.AppSettings["LiveStreamNexmoAPIBrand"].ToString();

        //For ascend trading 
        public string ascendTradingApiKey = ConfigurationManager.AppSettings["AscendTradingNexmoAPIKey"].ToString();
        public string ascendTradingApiSecret = ConfigurationManager.AppSettings["AscendTradingNexmoAPISecret"].ToString();
        public string ascendTradingApiBrand = ConfigurationManager.AppSettings["AscendTradingNexmoAPIBrand"].ToString();

        //For TKL trading 
        public string tklTradingApiKey = ConfigurationManager.AppSettings["TKLTradingNexmoAPIKey"].ToString();
        public string tklTradingApiSecret = ConfigurationManager.AppSettings["TKLTradingNexmoAPISecret"].ToString();
        public string tklTradingApiBrand = ConfigurationManager.AppSettings["TKLTradingNexmoAPIBrand"].ToString();

        public string apiURL = "";
        public string Data = "";
        public string Method = "GET";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="phNo"></param>
        /// <returns></returns>
        public string RequestPIN()
        {
            string response = "";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiURL);
            request.Host = "api.nexmo.com";
            request.Method = Method;
            try
            {
                WebResponse webResponse = request.GetResponse();
                using (Stream webStream = webResponse.GetResponseStream())
                {
                    if (webStream != null)
                    {
                        using (StreamReader responseReader = new StreamReader(webStream))
                        {
                             response = responseReader.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception e)
            {
               response = e.Message;
            }
            return response;
        }
    }
}