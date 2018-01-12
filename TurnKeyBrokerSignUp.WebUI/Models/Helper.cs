using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using TurnKeyBrokerSignUp.WebUI.Models.HttpUtils;


namespace TurnKeyBrokerSignUp.WebUI.Models
{
    public class Helper
    {
        #region
        RegistrationInfo regModel = new RegistrationInfo();
        string postString = string.Empty;
        string responseData = string.Empty;
        #endregion
        public string GetResponse(string URLAuth, string user, string code)
        {
            try
            {
                if (code != "")
                {
                    postString = string.Format("login={0}&code={1}", user, code);
                }
                else

                {
                    postString = string.Format("login={0}", user);
                }

                const string contentType = "application/x-www-form-urlencoded";
                ServicePointManager.Expect100Continue = false;

                CookieContainer cookies = new CookieContainer();
                HttpWebRequest webRequest = WebRequest.Create(URLAuth) as HttpWebRequest;
                webRequest.Method = "POST";
                webRequest.ContentType = contentType;
                webRequest.CookieContainer = cookies;

                StreamWriter requestWriter = new StreamWriter(webRequest.GetRequestStream());
                requestWriter.Write(postString);
                requestWriter.Close();

                StreamReader responseReader = new StreamReader(webRequest.GetResponse().GetResponseStream());
                string responseData = responseReader.ReadToEnd();

                responseReader.Close();
                webRequest.GetResponse().Close();
            }
            catch (Exception ex)
            {
                Elmah.ErrorLog.GetDefault(System.Web.HttpContext.Current).Log(new Elmah.Error(ex, System.Web.HttpContext.Current));
            }
            return responseData;
        }

        public string FirstCharToUpper(string input)
        {
            if (String.IsNullOrEmpty(input))
                throw new ArgumentException("ARGH!");
            return input.First().ToString().ToUpper() + input.Substring(1);
        }

        public string Decode(string decodeMe)
        {
            byte[] encoded = Convert.FromBase64String(decodeMe);
            return System.Text.Encoding.UTF8.GetString(encoded);
        }

        public string EasyOFACApiExecution(string Uri, string AccountNumber, string FirstName, string LastName, int AccountType)
        {

            string response = string.Empty;
            string postString = string.Empty;
            string _easyOFACKey = ConfigurationManager.AppSettings["EasyOFACApiKey"].ToString();
            var client = new RestClient(Uri);
            client.Method = HttpVerb.GET;
            if (AccountType == 1 || AccountType == 2)
            {

                postString = string.Format("api_key={0}&id={1}&first_name={2}&last_name={3}&kyc={4}", _easyOFACKey, AccountNumber, FirstName, LastName, true);
            }
            else
            {

                postString = string.Format("api_key={0}&id={1}&name={2}&kyc={3}&taxid_hash={4}", _easyOFACKey, AccountNumber, FirstName, true, "");
            }
            response = client.MakeRequest("?" + postString);
            return response;
        }


        
    }
}