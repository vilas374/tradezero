using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TurnKeyBrokerSignUp.WebUI.Models;
using System.IO;
using System.Configuration;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;
using TurnKeyBrokerSignUp.WebUI.Services;
using System.Security.Cryptography;
using System.Web;
using System.Security.Claims;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Owin;
using Owin;
using System.Net.Http.Headers;
using System.Net.Http.Formatting;
using Stripe;

namespace TurnKeyBrokerSignUp.WebUI.Controllers
{
    [RoutePrefix("api/SignProcess")]
    public class SignProcessController : ApiController
    {

        #region
        RegistrationInfo regModel = new RegistrationInfo();
        HttpPostedFileBase file;
        bool output = false;
        TurnKeyBrokerSignUpDataContext dataContext = new TurnKeyBrokerSignUpDataContext();
        bool isDebug = false;
        string ob = null;
        string _cookieType = string.Empty;
        Helper objHelper = new Helper();
        SignupHelper objSignupHelper = new SignupHelper();
        string StoreURI = "TradeZero";
        private const int SaltByteLength = 24;
        private const int DerivedKeyLength = 24;
        string UserName = string.Empty;
        string Password = string.Empty;
        tb_account account =new tb_account();
        tb_address addrs = new tb_address();
        tb_patriot patriot = new tb_patriot();
        Signup objsign = new Signup();






        // public object Response { get; private set; }

        //Plaid.Net.HttpPlaidClient objplaid = new Plaid.Net.HttpPlaidClient(serviceUri,clientId,clientSecret);
        #endregion


        //public SignProcessController()
        //{

        //}
      
        [HttpPost]
        [Route("createSignupStep")]
        public HttpResponseMessage CreateSignupStep(RegistrationInfo info) //, string stepsNo
        {
            string Message = "";
            try
                {
                  

                var acc = new tb_account();
                    acc.customer_id = info.account.email_address;
                    acc.trading_type = info.account.trading_type;
                    acc.tax_id = info.account.tax_id;
                    acc.citizenship_country_id = info.account.citizenship_country_id;
                    acc.account_guid = Guid.NewGuid();
                    acc.password = info.account.password;
                    acc.email_address = info.account.email_address;
                    acc.account_type_id = info.account.account_type_id;
                    acc.date_created = DateTime.Now;
                    acc.active = true;
                    acc.deleted = false;
                    acc.Completed = false;
                    acc.last_logged_in = DateTime.Now;
                    acc.storeURI = info.account.storeURI;
                    acc.E_notification = true;
                    acc.Affiliate_Type = info.account.Affiliate_Type;

                    dataContext.tb_accounts.InsertOnSubmit(acc);
                if (!isDebug)
                    dataContext.SubmitChanges();
                info.AccountId = acc.account_id;

                //if (acc.account_id != 0)
                //{
                //    dataContext.SubmitChanges();
                //    // info.StepNo = 2;
                //}
                var add = new tb_address();
                add.account_id = info.AccountId;
               
                add.address_type_id = info.address.address_type_id;
                add.first_name = info.address.first_name;
                add.last_name = info.address.last_name;
                //add.address_1 = info.address.address_1;
                //add.address_2 = info.address.address_2;
                add.work_phone = info.address.work_phone;
                add.date_created = DateTime.Now;
                add.city = info.address.city;
                add.state = info.address.state;
                add.country_id = info.address.country_id;
             
                //add.first_name = info.firstName;
                //add.last_name = info.lastName;
                //add.work_phone = info.WorkPhone;
                //add.date_created = DateTime.Now;
                //add.city = info.address.city;
                //add.state = info.address.state;
                //add.country_id = info.address.country_id;
                add.zipcode = info.address.zipcode;
                regModel.AccountId = info.AccountId;
                regModel.EmailAddress = info.account.email_address;
                regModel.Password = info.account.password;
                regModel.AccountType = info.account.account_type_id;
                regModel.firstName = info.address.first_name;
                regModel.lastName = info.address.last_name;
                regModel.TradingType = info.account.trading_type;
                dataContext.tb_addresses.InsertOnSubmit(add);
                if (!isDebug)
                    dataContext.SubmitChanges();

                var tb_patr = new tb_patriot();
                tb_patr.account_id = info.AccountId;
                tb_patr.date_of_birth = info.pat.date_of_birth;
                tb_patr.identification_type_id = info.pat.identification_type_id;
                tb_patr.identitication_number = info.pat.identitication_number;
                tb_patr.identification_expiration = info.pat.identification_expiration;
                tb_patr.residence_country_id = info.pat.residence_country_id;
                dataContext.tb_patriots.InsertOnSubmit(tb_patr);
                if (!isDebug)
                    dataContext.SubmitChanges();
                //info.StepNo = 2;
                //string type = null;
                //var w = PhoneNumberVerification(add.work_phone, type);
                //Signup obj = new Signup();
                //obj.Account_id = add.account_id;
                //obj.request_id = w.request_id;
                //obj.status = w.status;


                bool bChanged = true;
                //update account table
                var updateAcc = dataContext.tb_accounts.FirstOrDefault(t => t.account_id == regModel.AccountId);
                //   var updateAdd = dataContext.tb_addresses.FirstOrDefault(t => t.account_id == regModel.AccountId);

                if (updateAcc != null && bChanged == true)
                {
                    // updateAcc.is_agreeement_signed = is_agreement_signed;
                    updateAcc.Completed = true;
                    updateAcc.E_notification = false;
                    dataContext.SubmitChanges();
                }
                //  tb_account account;
                //Create user in service trade login
                bool _status = false;
                //if (!AgreementModuleEnabled())
                //{
                var addr = dataContext.tb_addresses.Where(a => a.account_id == regModel.AccountId).FirstOrDefault();
                var inst_entity = dataContext.tb_institutional_signups.Where(a => a.account_id == regModel.AccountId).FirstOrDefault();
                // regModel.firstName = inst_entity.account_name;
                regModel.firstName = addr.first_name;
                regModel.lastName = addr.last_name;
                bool bCustomerIdGenerated = objsign.SaveCustomerId(dataContext, regModel, StoreURI);
                _status = objsign.SaveEasyOFACDetails(acc.account_type_id, acc.account_id, regModel.CustomerId, regModel.firstName, regModel.lastName);
               
                var configTradingService = (from s in dataContext.tb_settings
                                            where s.key == "ServiceTradingLogin"
                                            && s.active == true
                                            && s.storeId == 1
                                            select s).FirstOrDefault();

                if (configTradingService != null && bCustomerIdGenerated)
                {
                    if (!isDebug)
                        //regModel.registrationType = TempData["RegistrationType"].ToString();
                        objsign.SaveToExternalService(dataContext, regModel);
                }
                if (!isDebug)
                    objSignupHelper.SaveUserToPortal(dataContext, regModel, StoreURI);
               
                //  }
                //end user creation
                //Send thank you email
                var thankyou_content = (from s in dataContext.tb_ContentManagements
                                        where s.content_key == "thank_you"
                                        && s.storeId == 1//regModel.activeStore.storeId
                                        select s).FirstOrDefault();
                if (_status == true)
                {
                    objsign.SendThankYouEmail1(dataContext,info);
                    Message = "SignUp Process Susscefully Completed.Please Check Email";
                }
                else
                {
                    objsign.SendOFACFailedEmail1(dataContext, info);
                    Message = "Not Completed Signup Process!.";
                }

                Signup obj = new Signup();
                obj.Account_id = add.account_id;
                obj.CustomerId = acc.customer_id;
                obj.Password = acc.password;
                obj.Message = Message;
                //CreateSignupStep2(info, info.AccountId, info.StepNo);
                return Request.CreateResponse(HttpStatusCode.OK, obj);
               // return Req_details;
                }
                catch (Exception e)
                {

                    throw e;
                }



        }


        [HttpPost]
        [Route("phoneNumberVerification")]
        public HttpResponseMessage PhoneNumberVerification(int Accountid,string phoneNumber, string type)
        {
            string Message = "";
            regModel.AccountId = Accountid;
            var updateAcc = dataContext.tb_addresses.FirstOrDefault(t => t.account_id == regModel.AccountId);
            // var add = new tb_address();
            //add.account_id = Accountid;

            updateAcc.work_phone = phoneNumber;
            if (!isDebug)
                dataContext.SubmitChanges();

            NumberVerification verifynum = new NumberVerification();
            if (string.IsNullOrEmpty(type))
            {
                verifynum.apiURL = "https://api.nexmo.com/verify/json?api_key=" + verifynum.apiKey + "&api_secret=" + verifynum.apiSecret + "&number=" + phoneNumber + "&brand=" + verifynum.apiBrand + "";
               // Message = "Please Check Your Phone Once.";
            }
            else if (type == "generaltrade")
            {
                verifynum.apiURL = "https://api.nexmo.com/verify/json?api_key=" + verifynum.generalTradeApiKey + "&api_secret=" + verifynum.generalTradeApiSecret + "&number=" + phoneNumber + "&brand=" + verifynum.generalTradeApiBrand + "";
            }
            else if (type == "ascendtrading")
            {
                verifynum.apiURL = "https://api.nexmo.com/verify/json?api_key=" + verifynum.ascendTradingApiKey + "&api_secret=" + verifynum.ascendTradingApiSecret + "&number=" + phoneNumber + "&brand=" + verifynum.ascendTradingApiBrand + "";
            }
            else if (type == "livestream")
            {
                verifynum.apiURL = "https://api.nexmo.com/verify/json?api_key=" + verifynum.liveStreamApiKey + "&api_secret=" + verifynum.liveStreamApiSecret + "&number=" + phoneNumber + "&brand=" + verifynum.liveStreamApiBrand + "";
            }
            else if (type == "tkltrading")
            {
                verifynum.apiURL = "https://api.nexmo.com/verify/json?api_key=" + verifynum.tklTradingApiKey + "&api_secret=" + verifynum.tklTradingApiSecret + "&number=" + phoneNumber + "&brand=" + verifynum.tklTradingApiBrand + "";
            }
            else if (type == "StockSpy")
            {
                verifynum.apiURL = "https://api.nexmo.com/verify/json?api_key=" + verifynum.tklTradingApiKey + "&api_secret=" + verifynum.tklTradingApiSecret + "&number=" + phoneNumber + "&brand=" + verifynum.tklTradingApiBrand + "";
            }
            else if (type == "SmartFinance")
            {
                verifynum.apiURL = "https://api.nexmo.com/verify/json?api_key=" + verifynum.tklTradingApiKey + "&api_secret=" + verifynum.tklTradingApiSecret + "&number=" + phoneNumber + "&brand=" + verifynum.tklTradingApiBrand + "";
            }
            else if (type == "CursoTrader")
            {
                verifynum.apiURL = "https://api.nexmo.com/verify/json?api_key=" + verifynum.tklTradingApiKey + "&api_secret=" + verifynum.tklTradingApiSecret + "&number=" + phoneNumber + "&brand=" + verifynum.tklTradingApiBrand + "";
            }
            else if (type == "blueinvesting")
            {
                verifynum.apiURL = "https://api.nexmo.com/verify/json?api_key=" + verifynum.tklTradingApiKey + "&api_secret=" + verifynum.tklTradingApiSecret + "&number=" + phoneNumber + "&brand=" + verifynum.tklTradingApiBrand + "";
            }
            else if (type == "mexicobolsas")
            {
                verifynum.apiURL = "https://api.nexmo.com/verify/json?api_key=" + verifynum.tklTradingApiKey + "&api_secret=" + verifynum.tklTradingApiSecret + "&number=" + phoneNumber + "&brand=" + verifynum.tklTradingApiBrand + "";
            }

            var response = verifynum.RequestPIN();
            var req_id = JObject.Parse(response)["request_id"].ToString();
            int status = Convert.ToInt32(JObject.Parse(response)["status"].ToString());
            if (status == 0)
            {
                Message = "Please Check Your Phone Once.";
            }
            else
            {
                Message = "Please wait, You Will Receive A Verification Call Shortly.";
            }

            //List<Signup> obj1=new Signup();
            //obj1.request_id = req_id;
            //obj1.status = status;
            Signup obj1 = new Signup();
            obj1.request_id = req_id;
            obj1.status = status;
            obj1.Message = Message;
           // return obj1;
            return Request.CreateResponse(HttpStatusCode.OK, obj1);
            // return Json(new { isVerified = response }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Route("oTPVerification")]
        public HttpResponseMessage OTPVerification(string requestID, int code, string type)
        {
            string Message = "";
            NumberVerification verifyOTP = new NumberVerification();
            if (string.IsNullOrEmpty(type))
            {
                verifyOTP.apiURL = "https://api.nexmo.com/verify/check/json?api_key=" + verifyOTP.apiKey + "&api_secret=" + verifyOTP.apiSecret + "&request_id=" + requestID + "&code=" + code + "";
              
            }
            else if (type == "generaltrade")
            {
                verifyOTP.apiURL = "https://api.nexmo.com/verify/check/json?api_key=" + verifyOTP.generalTradeApiKey + "&api_secret=" + verifyOTP.generalTradeApiSecret + "&request_id=" + requestID + "&code=" + code + "";
            }
            else if (type == "ascendtrading")
            {
                verifyOTP.apiURL = "https://api.nexmo.com/verify/check/json?api_key=" + verifyOTP.ascendTradingApiKey + "&api_secret=" + verifyOTP.ascendTradingApiSecret + "&request_id=" + requestID + "&code=" + code + "";
            }
            else if (type == "livestream")
            {
                verifyOTP.apiURL = "https://api.nexmo.com/verify/check/json?api_key=" + verifyOTP.liveStreamApiKey + "&api_secret=" + verifyOTP.liveStreamApiSecret + "&request_id=" + requestID + "&code=" + code + "";
            }
            else if (type == "tkltrading")
            {
                verifyOTP.apiURL = "https://api.nexmo.com/verify/check/json?api_key=" + verifyOTP.tklTradingApiKey + "&api_secret=" + verifyOTP.tklTradingApiSecret + "&request_id=" + requestID + "&code=" + code + "";
            }
            else if (type == "StockSpy")
            {
                verifyOTP.apiURL = "https://api.nexmo.com/verify/check/json?api_key=" + verifyOTP.tklTradingApiKey + "&api_secret=" + verifyOTP.tklTradingApiSecret + "&request_id=" + requestID + "&code=" + code + "";
            }

            else if (type == "SmartFinance")
            {
                verifyOTP.apiURL = "https://api.nexmo.com/verify/check/json?api_key=" + verifyOTP.tklTradingApiKey + "&api_secret=" + verifyOTP.tklTradingApiSecret + "&request_id=" + requestID + "&code=" + code + "";
            }
            else if (type == "CursoTrader")
            {
                verifyOTP.apiURL = "https://api.nexmo.com/verify/check/json?api_key=" + verifyOTP.tklTradingApiKey + "&api_secret=" + verifyOTP.tklTradingApiSecret + "&request_id=" + requestID + "&code=" + code + "";
            }
            else if (type == "blueinvesting")
            {
                verifyOTP.apiURL = "https://api.nexmo.com/verify/check/json?api_key=" + verifyOTP.tklTradingApiKey + "&api_secret=" + verifyOTP.tklTradingApiSecret + "&request_id=" + requestID + "&code=" + code + "";
            }
            else if (type == "mexicobolsas")
            {
                verifyOTP.apiURL = "https://api.nexmo.com/verify/check/json?api_key=" + verifyOTP.tklTradingApiKey + "&api_secret=" + verifyOTP.tklTradingApiSecret + "&request_id=" + requestID + "&code=" + code + "";
            }
          

            var response = verifyOTP.RequestPIN();
            var req_id = JObject.Parse(response)["request_id"].ToString();
            int status = Convert.ToInt32(JObject.Parse(response)["status"].ToString());//
            if(status==0)
            {
                Message = "Verify Your Phone Number Susscefully Completed.";
            }
            else
            {
                Message = "Please Check Your Phone Once Again Verify Code Message!";
            }

            //Signup obj = new Signup();
            //obj.request_id = req_id;
            //obj.status = status;
            //return obj;

            //return Json(new { isVerified = response }, JsonRequestBehavior.AllowGet);


            //regModel.AccountId = AccountId;
         
           

            Signup obj = new Signup();
            obj.request_id = req_id;
            obj.status = status;
            obj.Message = Message;
            //obj.Account_id = account.account_id;
            //obj.CustomerId = account.customer_id;
            //obj.Password = account.password;
            return Request.CreateResponse(HttpStatusCode.OK, obj);


        }

        /// <summary>
        /// This function is used to check emailaddress has complete signup process or not
        /// </summary>
        /// <createddate>18-12-2017</createddate>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        
        [HttpPost]
        [Route("checkEmailAddress")]
        public HttpResponseMessage CheckEmailAddress(string emailAddress)
        {
            string Message = "";
           // int ob=0;
            dataContext = new TurnKeyBrokerSignUpDataContext();
           
            try
            {
               // var _account = dataContext.tb_accounts.Where(a => (a.email_address == emailAddress)).OrderByDescending(t => t.account_id).FirstOrDefault();// && a.account_type_id == Convert.ToInt32(acc_type)

                var maildata = dataContext.tb_accounts.Where(x => x.email_address == emailAddress).FirstOrDefault();

                if ( maildata != null)
                {
                    ob = "1";
                    output = true;
                    Message = "Email Address Already Exit.";
                
                }
                else
                {
                    ob = "0";
                    output = false;
                    Message = "Email Address Not Exit in Database!";


                }
                dataContext.Dispose();
            }
            catch (Exception ex)
            {
                Elmah.ErrorLog.GetDefault(System.Web.HttpContext.Current).Log(new Elmah.Error(ex, System.Web.HttpContext.Current));
            }
            Signup objs = new Signup();
           
            objs.status = Convert.ToInt32(ob.ToString());
            objs.Message = Message;
            //return Json(ob);
            return Request.CreateResponse(HttpStatusCode.OK, objs);
        }
        [HttpPost]
        [Route("residentialAddress")]
        public HttpResponseMessage ResidentialAddress(RegistrationInfo info, int accountId)
        {
            string Message = "";
            if (dataContext == null)
                dataContext = new TurnKeyBrokerSignUpDataContext();
            try
            {
                //int update = 0;
                //if (StepNo == 2)
                //{
                    var tb_addre = dataContext.tb_addresses.Where(t => t.account_id == accountId).FirstOrDefault(); //&& t.address_type_id == accTypeId

                    tb_addre.account_id = accountId;

                    tb_addre.address_1 = info.address.address_1;
                    tb_addre.address_2 = info.address.address_2;
                    tb_addre.city = info.address.city;
                    // address.business_telephone = business_tel;
                    // address.fax = fax;
                    //if (countryId == 1)
                    //    address.state = stateId.ToString();
                    //else
                    //    address.state = otherState;
                    regModel.allStates = objSignupHelper.GetAllStates(dataContext, 1);
                    regModel.allCountries = objSignupHelper.GetAllCountries(dataContext, null);
                    tb_addre.state = info.address.state;
                    tb_addre.zipcode = info.address.zipcode;
                    tb_addre.country_id = info.address.country_id;
                    tb_addre.date_modified = DateTime.Now;
                    if (tb_addre != null)
                    {
                        if (!isDebug)
                            dataContext.SubmitChanges();
                        Message = "ResidentialAddress Updated Susscefully Completed.";
                    }
                    else
                    {
                        Message = "ResidentialAddress Not Updated Please Try Again!";
                    }
                //}
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
            Signup obje = new Signup();
            obje.Account_id = accountId;
            obje.Message = Message;
            return Request.CreateResponse(HttpStatusCode.OK, obje);
        }
        [HttpPost]
        [Route("emplyeestatus")]
        public HttpResponseMessage Emplyeestatus(RegistrationInfo info, int accountId)
        {
            string Message = "";
            try
            {
                //int update = 0;
                if (dataContext == null)
                    dataContext = new TurnKeyBrokerSignUpDataContext();
                //  var accTypeId = GetAddressType(dataContext, type);
                // var tb_emp = dataContext.tb_employementstatus.Where(t => t.account_id == accountId).FirstOrDefault(); //&& t.address_type_id == accTypeId
                var tb_emp = new tb_employementstatus();
                tb_emp.account_id = accountId;
                tb_emp.employment_status_id = info.empl.employment_status_id;
                dataContext.tb_employementstatus.InsertOnSubmit(tb_emp);


                var tb_aff = new tb_affiliation();
                tb_aff.account_id = accountId;
                tb_aff.is_broker_dealer_security_dealer = info.affi.is_broker_dealer_security_dealer;
                tb_aff.is_shareholder_public_company = info.affi.is_shareholder_public_company;
                tb_aff.date_created = DateTime.Now;
                dataContext.tb_affiliations.InsertOnSubmit(tb_aff);


                var investment = new tb_financial_investment();
                investment.account_id = accountId;
                investment.year_mutual_funds = info.tfi.year_mutual_funds;
                investment.date_created = DateTime.Now;
                //investment.number_social_securities = info.tfi.number_social_securities;
                dataContext.tb_financial_investments.InsertOnSubmit(investment);

                var agree = new tb_margin_account();
                agree.account_id = accountId;
                agree.is_margin_loan_agreement = info.agrd.is_margin_loan_agreement;
                agree.date_created = DateTime.Now;
                dataContext.tb_margin_accounts.InsertOnSubmit(agree);

                if (tb_emp != null && tb_aff != null && investment != null && agree != null)
                {
                    if (!isDebug)
                        dataContext.SubmitChanges();
                    Message = "Updated Susscefully Completed.";
                }
                else
                {
                    Message = "Not Updated Please Try Again!";

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            Signup obje = new Signup();
            obje.Account_id = accountId;
            obje.Message = Message;
            return Request.CreateResponse(HttpStatusCode.OK, obje);
        }

        [HttpPost]
        [Route("indetityDocs")]
        public HttpResponseMessage IndetityDocs(RegistrationInfo info, HttpPostedFileBase idpoof, HttpPostedFileBase addressproof, int accountId)
        {
            string Message = "";
            try
            {

                //if (Request.Files.Count > 0)
                //{

                // file =idpoof.ToString();
                string dbFileName = string.Empty;
                if (idpoof.ContentLength > 0)
                {
                    string fileExt = Path.GetExtension(idpoof.FileName);
                    string directory = ConfigurationManager.AppSettings["IdentificationFilePath"].ToString();
                    string directoryPath = HttpContext.Current.Server.MapPath("~") + directory;

                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }
                    dbFileName = account.customer_id + "_id" + fileExt;
                    string path = Path.Combine(directoryPath, dbFileName);
                    idpoof.SaveAs(path);

                    account.identification_file = dbFileName;
                    dataContext.SubmitChanges();
                }
                //file =addressproof;
                if (addressproof.ContentLength > 0)
                {
                    string fileExt = Path.GetExtension(addressproof.FileName);
                    string directory = ConfigurationManager.AppSettings["IdentificationFilePath"].ToString();
                    string directoryPath = System.Web.HttpContext.Current.Server.MapPath("~") + directory;

                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }
                    dbFileName = account.customer_id + "_additional" + fileExt;
                    string path = Path.Combine(directoryPath, dbFileName);
                    addressproof.SaveAs(path);

                    account.additional_file = dbFileName;
                    dataContext.SubmitChanges();
                }

                //  }
                var updateAcc = dataContext.tb_patriots.FirstOrDefault(t => t.account_id == accountId);

                // var tb_patr = new tb_patriot();
                updateAcc.account_id = accountId;
                //tb_patr.date_of_birth = info.pat.date_of_birth;
                updateAcc.identification_type_id = info.pat.identification_type_id;
                updateAcc.identitication_number = info.pat.identitication_number;
                updateAcc.residence_country_id = info.pat.residence_country_id;
                updateAcc.identification_expiration = info.pat.identification_expiration;
                updateAcc.identitifcation_place_of_issue = info.pat.identitifcation_place_of_issue;
                //  dataContext.tb_patriots.InsertOnSubmit(tb_patr);
                if (!isDebug)
                    dataContext.SubmitChanges();


                if (updateAcc != null)
                {
                    if (!isDebug)
                        dataContext.SubmitChanges();
                    Message = "Updated Susscefully Completed.";
                }
                else
                {
                    Message = "Not Updated Please Try Again!";

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            Signup obje = new Signup();
            obje.Account_id = accountId;
            obje.Message = Message;
            return Request.CreateResponse(HttpStatusCode.OK, obje);
        }

        [HttpGet]
        [Route("getAllCountries")]
        public HttpResponseMessage GetAllCountries()
        {
           
            if (dataContext == null)
                dataContext = new TurnKeyBrokerSignUpDataContext();
           
            var dataCon = (from i in dataContext.tb_countries
                           where i.active && !i.deleted && i.country_id != 1
                           select i).Distinct().OrderBy(c => c.country).ToList();

            List<NewCountries> newcount = new List<NewCountries>();

            dataCon.ForEach(
                x => {
                NewCountries obj = new NewCountries(){
                    country = x.country,
                    country_code = x.country_code
                   
                };
                    newcount.Add(obj);
            });


        


            return Request.CreateResponse(HttpStatusCode.OK, newcount);
        }

        [HttpPost]
        [Route("getAllStates")]
        public HttpResponseMessage GetAllStates(int country_id)
        {
            if (dataContext == null)
                dataContext = new TurnKeyBrokerSignUpDataContext();

           var statelist=(from i in dataContext.tb_states
                    where i.country_id == country_id
                    select i).Distinct().ToList() ;
            List<NewSates> newstate = new List<NewSates>();

            statelist.ForEach(
                x =>{
                    NewSates obj = new NewSates()
                    {
                        country_id = x.country_id,
                        state=x.state
                    };
                    newstate.Add(obj);
                });


            return Request.CreateResponse(HttpStatusCode.OK, newstate);
        }

        #region integrationPlaid
        [HttpPost]
        [Route("getAccessToken")]
        public async Task<HttpResponseMessage> GetAccessToken(string public_token) 
        {
            string Message = "";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 |
                                      SecurityProtocolType.Tls11 |
                                      SecurityProtocolType.Tls |
                                      SecurityProtocolType.Ssl3;

            string PLAID_CLIENT_ID = ConfigurationManager.AppSettings["PLAID_CLIENT_ID"].ToString();
            string PLAID_SECRET = ConfigurationManager.AppSettings["PLAID_SECRET"].ToString();
            string BasePlaidUrl = ConfigurationManager.AppSettings["BasePlaidUrl"].ToString();
            //string baseAddress = "https://sandbox.plaid.com/item/public_token/exchange";
            string Access_token = null;
            //  string PLAID_CLIENT_ID = "5a01af1dbdc6a46372916619";
            // string PLAID_SECRET = "ab908082766e85bce10408799af2e0";
            try
            {
                var parameters = new Dictionary<string, string>()

            {

                { "client_id",PLAID_CLIENT_ID},
                {"secret", PLAID_SECRET},
                {"public_token", public_token}


            };
                var client = new HttpClient();

                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri(BasePlaidUrl + "item/public_token/exchange"),//, "exchange_token"
                    Method = HttpMethod.Post,
                    // Content = new FormUrlEncodedContent(parameters)
                    Content = new StringContent(JsonConvert.SerializeObject(parameters), Encoding.UTF8, "application/json")
                };


                var requestResult = await client.SendAsync(request);
                var stringResponse = requestResult.Content.ReadAsStringAsync();

                Access_token = JObject.Parse(stringResponse.Result)["access_token"].ToString();
                var Item_id = JObject.Parse(stringResponse.Result)["item_id"].ToString();
                var Request_id = JObject.Parse(stringResponse.Result)["request_id"].ToString();
                Message = "Get Access_Token Successfully.";
                //  bankAccountToken = Access_token;
            }
            catch (Exception ex)
            {
                // Trace.TraceError("PlaidHelper::ExchangeToken() hit error " + ex.Message);
            }
            Signup obje = new Signup();
            //obje.Account_id = accountId;
            obje.Access_token = Access_token;
            obje.Message = Message;
            return Request.CreateResponse(HttpStatusCode.OK, obje);
        }

        [HttpPost]
        [Route("getBankToken")]
        public async Task<HttpResponseMessage> GetBankToken(int Accountid,string Access_token,string account_id)
        {
            string Message = "";
            string PLAID_CLIENT_ID = ConfigurationManager.AppSettings["PLAID_CLIENT_ID"].ToString();
            string PLAID_SECRET = ConfigurationManager.AppSettings["PLAID_SECRET"].ToString();
            string BasePlaidUrl = ConfigurationManager.AppSettings["BasePlaidUrl"].ToString();
          //  string baseAddress = "https://sandbox.plaid.com/processor/stripe/bank_account_token/create";
            string Stripe_bank_account_token = null;
            //  string PLAID_CLIENT_ID = "5a01af1dbdc6a46372916619";
            // string PLAID_SECRET = "ab908082766e85bce10408799af2e0";
            try
            {

                var client = new HttpClient();
                var parameters = new Dictionary<string, string>()

            {

                { "client_id",PLAID_CLIENT_ID},
                {"secret", PLAID_SECRET},
                {"access_token", Access_token},
                 {"account_id", account_id}

            };
                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri(BasePlaidUrl + "processor/stripe/bank_account_token/create"),//, "exchange_token"
                    Method = HttpMethod.Post,
                    // Content = new FormUrlEncodedContent(parameters)
                    Content = new StringContent(JsonConvert.SerializeObject(parameters), Encoding.UTF8, "application/json")
                };


                var requestResult = await client.SendAsync(request);
                var stringResponse = requestResult.Content.ReadAsStringAsync();
                Stripe_bank_account_token = JObject.Parse(stringResponse.Result)["stripe_bank_account_token"].ToString();
                var request_id = JObject.Parse(stringResponse.Result)["request_id"].ToString();
                Message = "Get Stripe_bank_account_token Successfully.";


                var token = new tb_savetoken();
                token.account_id =Accountid;//account.account_id;
                token.access_token = Access_token;
                token.bank_token = Stripe_bank_account_token;
                token.Account_Id_Plaid = account_id;
               // token.Customer_Id = customer.Id;
                dataContext.tb_savetokens.InsertOnSubmit(token);
                if (!isDebug)
                    dataContext.SubmitChanges();
                // Stripe_bank_account_token = bankAccountToken;
                //StripeConfiguration.SetApiKey("sk_test_BQokikJOvBiI2HlWgH4olfQ2");

                //  string URI = "https://api.stripe.com/v1/customers";
                //  string token = "sk_test_gVgWXEfaFAmI4ykRqSPFmMQz";
            
            }
            catch (Exception ex)
            {
                //Trace.TraceError("PlaidHelper::ExchangeToken() hit error " + ex.Message);
            }
            Signup obje1 = new Signup();
            //obje.Account_id = accountId;
            obje1.Stripe_bank_account_token = Stripe_bank_account_token;
            obje1.Message = Message;
            return Request.CreateResponse(HttpStatusCode.OK, obje1);
        }

        [HttpPost]
        [Route("stripeCreate")]
        public HttpResponseMessage StripeCreate(int Accountid,string Stripe_bank_account_token,int Amount)
        {
            var Secret_key = string.Empty;
           string Stripe_Key = ConfigurationManager.AppSettings["Stripe_Key"].ToString();
           // string token1 = "sk_test_gVgWXEfaFAmI4ykRqSPFmMQz";
            StripeConfiguration.SetApiKey(Stripe_Key);
            var customerOptions = new StripeCustomerCreateOptions()
            {
                Description = "Customer for Payment",
                SourceToken = Stripe_bank_account_token
                //Email= Email
            };
            var customerService = new StripeCustomerService();
            StripeCustomer customer = customerService.Create(customerOptions);
            //   Session["customer_id"] = customer.Id;
            //  Session["bank_id"] = customer.DefaultSourceId;

            // this code for save the access token and bank token
            var tb_sav = dataContext.tb_savetokens.Where(t => t.account_id == Accountid).FirstOrDefault();
            tb_sav.Customer_Id = customer.Id;
            if (!isDebug)
                dataContext.SubmitChanges();
           Signup ss= TransAmount(Accountid, tb_sav.Customer_Id, Amount);
            return Request.CreateResponse(HttpStatusCode.OK,ss);
        }
        [HttpPost]
        public Signup TransAmount(int Accountid,string Customer_Id,int Amount)
        {
            string Message = "";
            string Secret_key = string.Empty;
            Signup obje1 = new Signup();
            //string ac = Session["account_id"].ToString();
            //var fund = (from s in dataContext.tb_savetokens
            //            where s.account_id == Convert.ToInt32(Session["account_id"].ToString())
            //            select s).FirstOrDefault();

            //var tok = new tb_savetoken();
            // tok.account_id = account.account_id;

            int amu = Amount * 100;
            string Stripe_Key = ConfigurationManager.AppSettings["Stripe_Key"].ToString();
            // string token1 = "sk_test_gVgWXEfaFAmI4ykRqSPFmMQz";
            StripeConfiguration.SetApiKey(Stripe_Key);
            var chargeOptions = new StripeChargeCreateOptions()
            {
                Amount = amu,
                Currency = "usd",
                CustomerId = Customer_Id,
              //  SourceTokenOrExistingSourceId = Session["bank_id"].ToString(),
                Metadata = new Dictionary<String, String>()
                                        {
                                                { "OrderId", "6735"}
                                        }
            };
            if (chargeOptions.Amount != 0)
            {
                var chargeService = new StripeChargeService();
                StripeCharge charge = chargeService.Create(chargeOptions);

                var amountpay = new tb_payamount();
                amountpay.account_id = Accountid;
                //    int usdamount = (charge.Amount) / 100;
                amountpay.amount = charge.Amount;
                amountpay.customer_id = charge.CustomerId;
                amountpay.payment_id = charge.Id;
                dataContext.tb_payamounts.InsertOnSubmit(amountpay);
                if (!isDebug)
                    dataContext.SubmitChanges();
                Message = "Amount Added Successfully In TradeZero Account.";
                // TempData["notice"] = "Amount Added Successfully In TradeZero Account.";
                // ViewBag.msg = "Amount Added Successfully In TradeZero Account.";
                obje1.CustomerId = Customer_Id;
                obje1.Amount = charge.Amount;
                obje1.Message = Message;
            }
            else
            {
                //  ViewBag.msg = "Not Transfer Amount In TradeZero Account.";
            }

            //obje.Account_id = accountId;
           
            return obje1;
        }
        #endregion

        [HttpPost]
        [Route("validateuser")]
        public async Task<HttpResponseMessage> Validateuser(string Username, string Password)
        {
            string message = "";
            string Access_token = string.Empty;
            string Errror = string.Empty;
               
            string baseAddress = "http://localhost:50031";

            try
            {
                var client = new HttpClient();
                var parameters = new Dictionary<string, string>()
             {
                   {"grant_type", "password"},
                   {"username", Username},
                   {"password", Password}
               };
                //var request = new HttpRequestMessage()
                //{
                //    RequestUri = new Uri(baseAddress + "/token"),//, "exchange_token"
                //    Method = HttpMethod.Post,
                //    // Content = new FormUrlEncodedContent(parameters)
                //    Content = new StringContent(JsonConvert.SerializeObject(parameters), Encoding.UTF8, "application/json")
                //};
                var tokenResponse = client.PostAsync(baseAddress + "/token", new FormUrlEncodedContent(parameters)).Result;

                var token = tokenResponse.Content.ReadAsStringAsync();
                //   var token = tokenResponse.Content.ReadAsAsync<Token>(new[] { new JsonMediaTypeFormatter() }).Result;

               // message = JObject.Parse(token.Result)["error_description"].ToString();

                //var oken = JObject.Parse(token.Result)[""].ToString();
                Access_token = JObject.Parse(token.Result)["access_token"].ToString();
                if (Access_token != null)
                {
                  
                    message = "Signin Susscefully Completed.";
                }
               
            }
            catch(Exception ex)
            {
               // return null;
                message = "Provided username and password is incorrect.";
            }
                //var mess= JObject.Parse(token.Result)["message"].ToString();
                Signup obje = new Signup();
            //obje.Account_id = accountId;
            obje.Access_token = Access_token;
            obje.Message = message;
         
            return Request.CreateResponse(HttpStatusCode.OK, obje);
        }

        /// <summary>
        /// This function is Dateof birth insert.
        /// </summary>
        /// <createddate>18-12-2017</createddate>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
       
        [HttpPost]
        public HttpResponseMessage SavePatriot(int accountId, string dateofBirth)
        {  
            string Message = "";
            if (dataContext == null)
                dataContext = new TurnKeyBrokerSignUpDataContext();
            int update = 0;
          
            var tb_patriot = dataContext.tb_patriots.Where(t => t.account_id == accountId).FirstOrDefault();
            patriot.account_id = accountId;
            patriot.date_of_birth = dateofBirth;
            //patriot.identification_type_id = identification_type_id;
            //patriot.identitication_number = identification_number;
            //patriot.identification_expiration = identification_expiration;
            //patriot.identitifcation_place_of_issue = identification_place_of_issue;
            //patriot.identification_issue_date = identification_issue_date;
            //patriot.residence_country_id = residence_country_id;
            if (update == 0)
            {
                dataContext.tb_patriots.InsertOnSubmit(patriot);
                Message = "DateOfBirth Updated Susscefully Completed.";
            }
            if (!isDebug)
                dataContext.SubmitChanges();
            Signup obje = new Signup();
            obje.Account_id = patriot.account_id;
            obje.Message = Message;
            return Request.CreateResponse(HttpStatusCode.OK,obje);
        }
        [HttpPost]
        [Route("forgotPassword")]
        public HttpResponseMessage ForgotPassword(string userId)
        {
            Signup obje = new Signup();
            var activeStore = dataContext.store_bases.FirstOrDefault(t => t.storeId == 1);
            string Message = "";
            //if (EnforceSiteLock())
            //{
            //    return null;
            //}
            //else
            //{

            //if (Request.IsAuthenticated)
            //    return RedirectToAction("Index");
            //else
            //{
            regModel = new RegistrationInfo();
            dataContext = new TurnKeyBrokerSignUpDataContext();

            //  if (string.IsNullOrEmpty(StoreURI))
            StoreURI = ConfigurationManager.AppSettings["DefaultURI"].ToString();
            store_base sb = dataContext.store_bases.Where(_sb => _sb.uri == StoreURI && _sb.active == true && _sb.deleted == false).FirstOrDefault();
            regModel.activeStore = sb;
            //if (Request.HttpMethod.ToUpper().Equals("POST"))
            //{
            //   string userId = Request.Form["userId"] != null ? Request.Form["userId"].ToString() : string.Empty;
            if (!string.IsNullOrEmpty(userId))
            {

                var _account = dataContext.tb_accounts.Where(a => a.email_address == userId && a.active).OrderByDescending(a => a.account_id).FirstOrDefault();
                if (_account != null)
                {
                    var _aspnetuser = dataContext.aspnet_Users.Where(a => a.UserName == _account.customer_id).FirstOrDefault();
                    var _aspnetMember = dataContext.aspnet_Memberships.Where(m => m.UserId == _aspnetuser.UserId).FirstOrDefault();
                    if (_account == null)

                        Message = "No Account Found";
                    else
                    {
                        var _address = dataContext.tb_addresses.Where(a => a.account_id == _account.account_id).OrderBy(a => a.address_type_id).FirstOrDefault();
                        string firstName = string.Empty;
                        string lastName = string.Empty;

                        if (_address != null)
                        {
                            firstName = _address.first_name;
                            lastName = _address.last_name;

                            emailTemplate em = dataContext.emailTemplates.Where(e => e.templateName.Trim() == "FORGOT PASSWORD").FirstOrDefault();
                            string emailBody = em.emailBody;
                            string firmAddress = string.Empty;
                            firmAddress = string.IsNullOrEmpty(activeStore.address1) ? string.Empty : activeStore.address1 + "<br/>";
                            firmAddress = firmAddress + (string.IsNullOrEmpty(activeStore.city) ? string.Empty : activeStore.city + " ");
                            firmAddress = firmAddress + (string.IsNullOrEmpty(activeStore.state) ? string.Empty : activeStore.state + " ");
                            firmAddress = firmAddress + (string.IsNullOrEmpty(activeStore.zipcode) ? string.Empty : activeStore.zipcode + " ");
                            emailBody = emailBody.Replace("[FirstName]", firstName);
                            emailBody = emailBody.Replace("[LastName]", lastName);
                            emailBody = emailBody.Replace("[FIRM]", activeStore.storeDisplayName);
                            emailBody = emailBody.Replace("[UserName]", _account.customer_id);
                            emailBody = emailBody.Replace("[Password]", _aspnetMember.Password);
                            emailBody = emailBody.Replace("[FIRM CUSTOMER SERVICE NUMBER]", (string.IsNullOrEmpty(activeStore.contactPhone) ? "" : activeStore.contactPhone));

                            int EmailLogID = TurnKeyBrokerSignUp.WebUI.Services.EmailHelper.AddToEmailQueue(ConfigurationManager.AppSettings["DefaultURI"].ToString(), em.fromName, em.fromEmail, firstName + " " + lastName, _account.email_address, em.ccEmail, em.bccEmail, em.emailSubject, emailBody);
                            string message = TurnKeyBrokerSignUp.WebUI.Services.EmailHelper.SendEmail(em.fromName, em.fromEmail, firstName + " " + lastName, _account.email_address, em.ccEmail, em.bccEmail, em.emailSubject, emailBody, em.isHtml);

                            if (EmailLogID > 0)
                                TurnKeyBrokerSignUp.WebUI.Services.EmailHelper.UpdateEmailQueueStatus(EmailLogID, message.Trim().ToUpper().Equals("OK"), message);
                            // ViewData["PageMessage"] = "1";
                            Message = "Send To your registered email address";
                           
                           
                        }
                        else
                        {
                            //   ViewData["PageMessage"] = "0";
                            Message = "Please Enter your registered email address.";
                        }

                    }
                    obje.Account_id = _account.account_id;
                    obje.CustomerId = _aspnetuser.UserName;
                    obje.Password = _aspnetMember.Password;
                    obje.Message = Message;
                }
                else
                {
                    //   ViewData["PageMessage"] = "0";
                    Message = "Please Enter your registered email address.";
                }

            }
            // }
            // }
            //  return View();
            //Signup obje = new Signup();
            //obje.Account_id = patriot.account_id;
            //obje.Message = Message;
            return Request.CreateResponse(HttpStatusCode.OK,obje);
            // }
        }

        [HttpPost]
        public HttpResponseMessage CreateSignupAddressStep2(TurnKeyBrokerSignUpDataContext dataContext, RegistrationInfo info, string type, string title, string first_name, string last_name, string middle_name, string suffix, string address_1, string address_2,
                                string city, int stateId, string otherState, string zipCode, int countryId, bool is_address_last_one_year, int martial_status_id, string work_phone, string num_dependents, bool isCoApplicant, string business_tel, string fax, bool isDebug)
        {
           
            try
            {

                if (info.AccountType != 3)
                {
                    objSignupHelper.SaveAddress(dataContext,info,type,title,first_name,last_name,middle_name,suffix,address_1,address_2,
                                city, stateId,otherState,zipCode,countryId,is_address_last_one_year,martial_status_id, work_phone,  num_dependents,isCoApplicant, business_tel, fax,isDebug);
                    }
             return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception e)
            {

                throw e;
            }


        }


    }

   
}
