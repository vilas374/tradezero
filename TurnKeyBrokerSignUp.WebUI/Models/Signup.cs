using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
namespace TurnKeyBrokerSignUp.WebUI.Models
{
   
    public class Signup
    {

        #region
        RegistrationInfo regModel = new RegistrationInfo();
        bool output = false;
        TurnKeyBrokerSignUpDataContext dataContext = new TurnKeyBrokerSignUpDataContext();
        bool isDebug = false;
        string ob = string.Empty;
        string _cookieType = string.Empty;
        Helper objHelper = new Helper();
        SignupHelper objSignupHelper = new SignupHelper();
        string StoreURI = " ";
        tb_account account = new tb_account();
        tb_address addrs = new tb_address();
        //Plaid.Net.HttpPlaidClient objplaid = new Plaid.Net.HttpPlaidClient(serviceUri,clientId,clientSecret);
        #endregion
        public int Account_id { get; set; }
        public string request_id { get; set; }
        public int status { get; set; }

        public string CustomerId { get; set; }
        public string Password { get; set; }

        public string Message { get; set; }
        public int Amount { get; set; }
        public string Access_token { get; set; }

        public string Stripe_bank_account_token { get; set; }
        public string GenerateAccountNumber(string firstName, string lastName)
        {
            string returnValue = string.Empty;

            if (string.IsNullOrEmpty(firstName))
            {
                var dataContext = new TurnKeyBrokerSignUpDataContext();
                var a = (from s in dataContext.tb_settings
                         where s.key == "DefaultFirstName"
                         && s.storeId == regModel.activeStore.storeId
                         && s.active == true
                         select s.value).FirstOrDefault();
                if (a == null)
                    firstName = "T";
                else
                    firstName = a;
            }
            if (string.IsNullOrEmpty(lastName))
            {
                var dataContext = new TurnKeyBrokerSignUpDataContext();
                var a = (from s in dataContext.tb_settings
                         where s.key == "DefaultLastName"
                         && s.active == true
                         && s.storeId == regModel.activeStore.storeId
                         select s.value).FirstOrDefault();
                if (a == null)
                    lastName = "TB";
                else
                    lastName = a;
            }

            if (!string.IsNullOrEmpty(lastName) && lastName.Length < 2)
            {
                lastName += "TB";
            }

            int seed = (int)DateTime.Now.Ticks;
            Random r = new Random(seed);
            int randomNumber = r.Next(10000, 99999);

            returnValue = firstName.Substring(0, 1).ToUpper() + lastName.Substring(0, 2).ToUpper() + randomNumber.ToString();
            return returnValue;
        }

        public bool SaveCustomerId(TurnKeyBrokerSignUpDataContext dataContext, RegistrationInfo info, string StoreURI)
        {
            bool bCustomerIdGenerated = false;
            try
            {
                if (dataContext == null)
                    dataContext = new TurnKeyBrokerSignUpDataContext();

                info.CustomerId = GenerateAccountNumber(info.firstName, info.lastName);

                int customer_id_generated_count = 0;

                var account = (from s in dataContext.tb_accounts
                                   //where s.email_address == info.EmailAddress
                               where s.account_id == info.AccountId
                               && s.active == true && s.deleted == false
                               && s.storeURI == StoreURI
                               select s).FirstOrDefault();

                while (true)
                {
                    var a = (from s in dataContext.tb_accounts
                             where s.customer_id == info.CustomerId
                             select s).FirstOrDefault();

                    if (a == null)
                    {
                        bCustomerIdGenerated = true;
                        break;
                    }
                    else
                        regModel.CustomerId = GenerateAccountNumber(info.firstName, info.lastName);
                    customer_id_generated_count++;
                    if (customer_id_generated_count > 1000)
                        break;
                }



                if (account != null)
                {
                    account.customer_id = info.CustomerId;
                    if (!isDebug)
                        dataContext.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
            }
            return bCustomerIdGenerated;
        }

        public void SendOFACFailedEmail(TurnKeyBrokerSignUpDataContext dataContext, tb_account acc)
        {
            if (dataContext == null)
                dataContext = new TurnKeyBrokerSignUpDataContext();
            string email_id = "";
            emailTemplate em;
            string fullName = string.Empty;
            tb_account account = dataContext.tb_accounts.Where(a => a.storeURI == regModel.activeStore.uri && a.account_id == acc.account_id).FirstOrDefault();
            em = dataContext.emailTemplates.Where(e => e.storeId == regModel.activeStore.storeId && e.emailTemplateId == 48).FirstOrDefault();
            var addresses = dataContext.tb_addresses.Where(add => add.account_id == account.account_id);
            tb_address primaryAddress;
            if (account.account_type_id == 3)
            {
                primaryAddress = addresses.Where(add => add.address_type_id == 1 && add.is_coapplicant == true).FirstOrDefault();
                email_id = account.email_address;
            }
            else
            {
                primaryAddress = addresses.Where(add => add.address_type_id == 1).FirstOrDefault();
                email_id = primaryAddress.email_address;
            }
            if (em != null && account != null && addresses != null && primaryAddress != null)
            {
                int templateID = em.emailTemplateId;

                string emailSubject = em.emailSubject;
                string emailBody = em.emailBody;

                string firmAddress = string.Empty;

                firmAddress = string.IsNullOrEmpty(regModel.activeStore.address1) ? string.Empty : regModel.activeStore.address1 + "<br/>";
                firmAddress = firmAddress + (string.IsNullOrEmpty(regModel.activeStore.city) ? string.Empty : regModel.activeStore.city + " ");
                firmAddress = firmAddress + (string.IsNullOrEmpty(regModel.activeStore.state) ? string.Empty : regModel.activeStore.state + " ");
                firmAddress = firmAddress + (string.IsNullOrEmpty(regModel.activeStore.zipcode) ? string.Empty : regModel.activeStore.zipcode + " ");

                emailBody = emailBody.Replace("[FNAME]", primaryAddress.first_name);
                emailBody = emailBody.Replace("[LNAME]", primaryAddress.last_name);
                emailBody = emailBody.Replace("[FIRM]", regModel.activeStore.storeDisplayName);
                emailBody = emailBody.Replace("[FIRM ADDRESS]", firmAddress);
                emailBody = emailBody.Replace("[FIRM CUSTOMER SERVICE NUMBER]", (string.IsNullOrEmpty(regModel.activeStore.contactPhone) ? "" : regModel.activeStore.contactPhone));

                int EmailLogID = EmailHelper.AddToEmailQueue(regModel.activeStore.uri, em.fromName, em.fromEmail, primaryAddress.first_name + " " + primaryAddress.last_name, email_id, em.ccEmail, em.bccEmail, emailSubject, emailBody);
                string message = EmailHelper.SendEmail(em.fromName, em.fromEmail, primaryAddress.first_name + " " + primaryAddress.last_name, email_id, em.ccEmail, em.bccEmail, emailSubject, emailBody, em.isHtml);


                if (EmailLogID > 0)
                    EmailHelper.UpdateEmailQueueStatus(EmailLogID, message.Trim().ToUpper().Equals("OK"), message);
            }
        }

        /// This method will save or update easy ofac data in db
        /// <param name="account_id"></param>
        /// <param name="account_type_id"></param>
        /// <param name="CustomerId"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// </summary>
        /// <returns>bool</returns>
        /// 
        public bool SaveEasyOFACDetails(int account_type_id, int account_id, string CustomerId, string firstName, string lastName)
        {
            bool status = true;
        
            string url = string.Empty;
            var dataContext = new TurnKeyBrokerSignUpDataContext();
            try
            {
                if (account_type_id == 1 || account_type_id == 2)
                {
                    url = ConfigurationManager.AppSettings["EasyOFACEndPoint"].ToString() + "addCustomer";
                }
                else
                {
                    var _instEntity = (from i in dataContext.tb_institutional_signups.Where(i => i.account_id == account_id) select i).FirstOrDefault();
                    if (_instEntity != null)
                    {
                        regModel.firstName = _instEntity.account_name;

                    }
                    url = ConfigurationManager.AppSettings["EasyOFACEndPoint"].ToString() + "addCompany";
                }
                string response = objHelper.EasyOFACApiExecution(url, CustomerId, firstName, lastName, account_type_id);
                JObject o = JObject.Parse(response);
                if (!response.Contains("failed"))
                {
                    if (account_type_id == 1 || account_type_id == 2)
                    {
                        tb_EasyOFACHistory _obj = new tb_EasyOFACHistory();
                        _obj.AccountNumber = (string)o["id"];
                        _obj.CustomerDateTime = DateTime.Now;
                        _obj.CustomerKyc = (bool?)o["customer_kyc"];
                        _obj.Status = (string)o["customer_status"];
                        dataContext.tb_EasyOFACHistories.InsertOnSubmit(_obj);
                        dataContext.SubmitChanges();
                        status = true;
                    }
                    else
                    {
                        tb_EasyOFACHistory _obj = new tb_EasyOFACHistory();
                        _obj.AccountNumber = (string)o["id"];
                        _obj.CustomerDateTime = DateTime.Now;
                        _obj.CustomerKyc = (bool?)o["company_kyc"];
                        _obj.Status = (string)o["company_statusCurrent"];
                        dataContext.tb_EasyOFACHistories.InsertOnSubmit(_obj);
                        dataContext.SubmitChanges();
                        status = true;
                    }

                }
                else
                {
                    //save failed response
                    if (account_type_id == 1 || account_type_id == 2)
                    {
                        tb_EasyOFACHistory _obj = new tb_EasyOFACHistory();
                        _obj.AccountNumber = (string)o["id"];
                        _obj.CustomerDateTime = DateTime.Now;
                        _obj.Status = (string)o["customer_status"];
                        dataContext.tb_EasyOFACHistories.InsertOnSubmit(_obj);
                        dataContext.SubmitChanges();
                        status = false;
                    }
                    else
                    {
                        tb_EasyOFACHistory _obj = new tb_EasyOFACHistory();
                        _obj.AccountNumber = (string)o["id"];
                        _obj.CustomerDateTime = DateTime.Now;
                        _obj.Status = (string)o["company_statusCurrent"];
                        dataContext.tb_EasyOFACHistories.InsertOnSubmit(_obj);
                        dataContext.SubmitChanges();
                        status = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Elmah.ErrorLog.GetDefault(System.Web.HttpContext.Current).Log(new Elmah.Error(ex, System.Web.HttpContext.Current));
            }

            return status;
        }

        /// This method will create user in trade reporting api
        /// <param name="affiliateType"></param>
        /// <param name="ipAddress"></param>
        /// </summary>
        /// <returns>bool</returns>
        public void SaveToExternalService(TurnKeyBrokerSignUpDataContext dataContext, RegistrationInfo info)
        {
            var activeStore = dataContext.store_bases.FirstOrDefault(t => t.storeId == 1);
            if (dataContext == null)
                dataContext = new TurnKeyBrokerSignUpDataContext();
            var account = (from s in dataContext.tb_accounts
                           where s.account_id == info.AccountId
                           && s.active == true
                           select s).FirstOrDefault();

            if (account != null)
            {
                var configTradingServiceURL = (from s in dataContext.tb_settings
                                               where s.key == "ServiceTradingServiceURL"
                                                    && s.active == true
                                                    && s.storeId == 1 
                                               select s).FirstOrDefault();

                var configTradingServiceUsername = (from s in dataContext.tb_settings
                                                    where s.key == "ServiceTradingUsername"
                                                    && s.active == true
                                                    && s.storeId == 1 
                                                    select s).FirstOrDefault();
                var configTradingServicePassword = (from s in dataContext.tb_settings
                                                    where s.key == "ServiceTradingPassword"
                                                    && s.storeId == 1 
                                                    && s.active == true
                                                    select s).FirstOrDefault();
                var configTradingServiceAccount = (from s in dataContext.tb_settings
                                                   where s.key == "ServiceTradingAccount"
                                                   && s.storeId == 1 
                                                   && s.active == true
                                                   select s).FirstOrDefault();

                if (configTradingServiceUsername != null && configTradingServicePassword != null && configTradingServiceAccount != null && configTradingServiceURL != null)
                {
                    try
                    {
                        System.ServiceModel.EndpointAddress ea = new System.ServiceModel.EndpointAddress(configTradingServiceURL.value);
                        TradingService.TRServiceSoapClient service = new TradingService.TRServiceSoapClient("TRServiceSoap", ea);

                        string _userCountry = string.Empty;
                        string _firstname = string.Empty;
                        string _lastname = string.Empty;

                        string token = service.TRLogin(configTradingServiceUsername.value, configTradingServicePassword.value);
                        int accountExists = service.TRAccountExist(token, account.customer_id);
                        if (accountExists == 0)
                        {
                           // tb_address address = objSignupHelper.GetAddress(dataContext, info.AccountId, "Permanent");
                            var updateAcc = dataContext.tb_addresses.FirstOrDefault(t => t.account_id == info.AccountId);
                            //if (info.AccountType == 3)
                            //{
                            //    var _add = (from a in dataContext.tb_addresses where a.account_id == info.AccountId select a).ToList();
                            //    _firstname = _add[1].first_name;
                            //    _lastname = _add[1].last_name;
                            //    var country = (from s in dataContext.tb_countries
                            //                   where s.country_id == _add[1].country_id
                            //                   && s.active == true
                            //                   select s).FirstOrDefault();
                            //    _userCountry = country.country;
                            //}
                            //else
                            //{
                            //    var country = (from s in dataContext.tb_countries
                            //                   where s.country_id == address.country_id
                            //                   && s.active == true
                            //                   select s).FirstOrDefault();
                            //    _firstname = address.first_name;
                            //    _lastname = address.last_name;
                            //    _userCountry = country.country;
                            //}


                            //string state = string.Empty;
                            //if (address.country_id == 1)
                            //{
                            //    var states = (from s in dataContext.tb_states
                            //                  where s.state_id == int.Parse(address.state)
                            //                  && s.active == true
                            //                  select s).FirstOrDefault();
                            //    if (states != null)
                            //        state = states.state_initial;
                            //}
                            //else
                            //{
                            //    state = address.state;
                            //}



                            //if (_userCountry != null)
                            //{

                            string accountOpen = "";

                                //If registration type is tradezero then no cookie creation
                                if (account.Affiliate_Type == "TradeZero")
                                {


                                    accountOpen = service.TROpenAccount(token, account.customer_id, updateAcc.first_name + " " + updateAcc.last_name, string.Empty, string.Empty, string.Empty,
                                                                                          string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, account.email_address, account.password);
                                                                                        //  updateAcc.city, updateAcc.state, updateAcc.zipcode, updateAcc.country_id.ToString(), updateAcc.work_phone, string.Empty, account.email_address, account.password);
                                
                            }
                            ////If registration type is other then check cookie existance
                            //else
                            //{
                            //    if (account.Affiliate_Type == "livestream")
                            //    {

                            //        accountOpen = service.TROpenAccountAffiliate(token, account.customer_id, _firstname + " " + _lastname, address.address_1, address.address_2, string.Empty,
                            //                       address.city, state, address.zipcode, _userCountry, address.work_phone, string.Empty, account.email_address, account.password, "LiveStream");

                            //        //Remove respective cookie immediatly
                            //        ReadandDeleteCookie(account.Affiliate_Type, objSignupHelper.getvisitorip());

                            //    }
                            //    else if (account.Affiliate_Type == "tradingschools")
                            //    {

                            //        accountOpen = service.TROpenAccountAffiliate(token, account.customer_id, _firstname + " " + _lastname, address.address_1, address.address_2, string.Empty,
                            //                                                                  address.city, state, address.zipcode, _userCountry, address.work_phone, string.Empty, account.email_address, account.password, "TradingSchool");


                            //        //Remove respective cookie immediatly
                            //        ReadandDeleteCookie(account.Affiliate_Type, objSignupHelper.getvisitorip());
                            //    }
                            //    else if (account.Affiliate_Type == "stocktrad3rblog")
                            //    {

                            //        accountOpen = service.TROpenAccountAffiliate(token, account.customer_id, _firstname + " " + _lastname, address.address_1, address.address_2, string.Empty,
                            //                                                            address.city, state, address.zipcode, _userCountry, address.work_phone, string.Empty, account.email_address, account.password, "StockTrad3r");

                            //        //Remove respective cookie immediatly
                            //        ReadandDeleteCookie(account.Affiliate_Type, objSignupHelper.getvisitorip());
                            //    }
                            //    else if (account.Affiliate_Type == "munozinvestments")
                            //    {

                            //        accountOpen = service.TROpenAccountAffiliate(token, account.customer_id, _firstname + " " + _lastname, address.address_1, address.address_2, string.Empty,
                            //                                                            address.city, state, address.zipcode, _userCountry, address.work_phone, string.Empty, account.email_address, account.password, "MunozInvestments");
                            //        //Remove respective cookie immediatly
                            //        ReadandDeleteCookie(account.Affiliate_Type, objSignupHelper.getvisitorip());
                            //    }
                            //    else if (account.Affiliate_Type == "stockpro")
                            //    {

                            //        accountOpen = service.TROpenAccountAffiliate(token, account.customer_id, _firstname + " " + _lastname, address.address_1, address.address_2, string.Empty,
                            //                                                            address.city, state, address.zipcode, _userCountry, address.work_phone, string.Empty, account.email_address, account.password, "StockPro");

                            //        //Remove respective cookie immediatly
                            //        ReadandDeleteCookie(account.Affiliate_Type, objSignupHelper.getvisitorip());
                            //    }
                            //    else if (account.Affiliate_Type == "generaltrade")
                            //    {

                            //        accountOpen = service.TROpenAccountAffiliate(token, account.customer_id, _firstname + " " + _lastname, address.address_1, address.address_2, string.Empty,
                            //                                                            address.city, state, address.zipcode, _userCountry, address.work_phone, string.Empty, account.email_address, account.password, "GeneralTrade");

                            //        //Remove respective cookie immediatly
                            //        ReadandDeleteCookie(account.Affiliate_Type, objSignupHelper.getvisitorip());
                            //    }
                            //    else if (account.Affiliate_Type == "ascendtrading")
                            //    {

                            //        accountOpen = service.TROpenAccountAffiliate(token, account.customer_id, _firstname + " " + _lastname, address.address_1, address.address_2, string.Empty,
                            //                                                            address.city, state, address.zipcode, _userCountry, address.work_phone, string.Empty, account.email_address, account.password, "Ascend");

                            //        //Remove respective cookie immediatly
                            //        ReadandDeleteCookie(account.Affiliate_Type, objSignupHelper.getvisitorip());
                            //    }
                            //    else if (account.Affiliate_Type == "tkltrading")
                            //    {

                            //        accountOpen = service.TROpenAccountAffiliate(token, account.customer_id, _firstname + " " + _lastname, address.address_1, address.address_2, string.Empty,
                            //                                                            address.city, state, address.zipcode, _userCountry, address.work_phone, string.Empty, account.email_address, account.password, "tkltrading");

                            //        //Remove respective cookie immediatly
                            //        ReadandDeleteCookie(account.Affiliate_Type, objSignupHelper.getvisitorip());
                            //    }
                            //    else if (account.Affiliate_Type == "stockspy")
                            //    {

                            //        accountOpen = service.TROpenAccountAffiliate(token, account.customer_id, _firstname + " " + _lastname, address.address_1, address.address_2, string.Empty,
                            //                                                            address.city, state, address.zipcode, _userCountry, address.work_phone, string.Empty, account.email_address, account.password, "stockspy");

                            //        //Remove respective cookie immediatly
                            //        ReadandDeleteCookie(account.Affiliate_Type, objSignupHelper.getvisitorip());
                            //    }

                            //    else if (account.Affiliate_Type == "smartfinance")
                            //    {
                            //        accountOpen = service.TROpenAccountAffiliate(token, account.customer_id, _firstname + " " + _lastname, address.address_1, address.address_2, string.Empty,
                            //                                                             address.city, state, address.zipcode, _userCountry, address.work_phone, string.Empty, account.email_address, account.password, "smartfinance");

                            //        //Remove respective cookie immediatly
                            //        ReadandDeleteCookie(account.Affiliate_Type, objSignupHelper.getvisitorip());
                            //    }

                            //    //curso trader
                            //    else if (account.Affiliate_Type == "cursotrader")
                            //    {
                            //        accountOpen = service.TROpenAccountAffiliate(token, account.customer_id, _firstname + " " + _lastname, address.address_1, address.address_2, string.Empty,
                            //                                                             address.city, state, address.zipcode, _userCountry, address.work_phone, string.Empty, account.email_address, account.password, "cursotrader");

                            //        //Remove respective cookie immediatly
                            //        ReadandDeleteCookie(account.Affiliate_Type, objSignupHelper.getvisitorip());
                            //    }
                            //    else if (account.Affiliate_Type == "blueinvesting")
                            //    {
                            //        accountOpen = service.TROpenAccountAffiliate(token, account.customer_id, _firstname + " " + _lastname, address.address_1, address.address_2, string.Empty,
                            //                                                             address.city, state, address.zipcode, _userCountry, address.work_phone, string.Empty, account.email_address, account.password, "blueinvest");

                            //        //Remove respective cookie immediatly
                            //        ReadandDeleteCookie(account.Affiliate_Type, objSignupHelper.getvisitorip());
                            //    }
                            //    else if (account.Affiliate_Type == "traderssky")
                            //    {
                            //        accountOpen = service.TROpenAccountAffiliate(token, account.customer_id, _firstname + " " + _lastname, address.address_1, address.address_2, string.Empty,
                            //                                                             address.city, state, address.zipcode, _userCountry, address.work_phone, string.Empty, account.email_address, account.password, "traderssky");

                            //        //Remove respective cookie immediatly
                            //        ReadandDeleteCookie(account.Affiliate_Type, objSignupHelper.getvisitorip());
                            //    }
                            //    else if (account.Affiliate_Type == "bubbashow")
                            //    {

                            //        accountOpen = service.TROpenAccountAffiliate(token, account.customer_id, _firstname + " " + _lastname, address.address_1, address.address_2, string.Empty,
                            //                                                                  address.city, state, address.zipcode, _userCountry, address.work_phone, string.Empty, account.email_address, account.password, "bubbashow");


                            //        //Remove respective cookie immediatly
                            //        ReadandDeleteCookie(account.Affiliate_Type, objSignupHelper.getvisitorip());
                            //    }
                            //    else if (account.Affiliate_Type == "warrior")
                            //    {

                            //        accountOpen = service.TROpenAccountAffiliate(token, account.customer_id, _firstname + " " + _lastname, address.address_1, address.address_2, string.Empty,
                            //                                                            address.city, state, address.zipcode, _userCountry, address.work_phone, string.Empty, account.email_address, account.password, "warrior");

                            //        //Remove respective cookie immediatly
                            //        ReadandDeleteCookie(account.Affiliate_Type, objSignupHelper.getvisitorip());
                            //    }
                            //    else if (account.Affiliate_Type == "mexicobolsas")
                            //    {

                            //        accountOpen = service.TROpenAccountAffiliate(token, account.customer_id, _firstname + " " + _lastname, address.address_1, address.address_2, string.Empty,
                            //                                                            address.city, state, address.zipcode, _userCountry, address.work_phone, string.Empty, account.email_address, account.password, "mexicobolsas");

                            //        //Remove respective cookie immediatly
                            //        ReadandDeleteCookie(account.Affiliate_Type, objSignupHelper.getvisitorip());
                            // }
                            //}
                            if (!string.IsNullOrEmpty(accountOpen))
                                {
                                    account.account_id_service = accountOpen;
                                    if (!isDebug)
                                        dataContext.SubmitChanges();
                                }
                                else
                                {
                                    account.account_id_service = "ERROR";
                                    if (!isDebug)
                                        dataContext.SubmitChanges();
                                }

                            }
                      //  }
                    }
                    catch (Exception ex)
                    {
                        using (StreamWriter outfile = new StreamWriter(System.Configuration.ConfigurationManager.AppSettings["LogFilePath"].ToString(), true))
                        {
                            outfile.Write(ex.Message + Environment.NewLine);
                            outfile.Write(ex.InnerException + Environment.NewLine);
                            outfile.Write(ex.Source + Environment.NewLine);
                        }

                        account.account_id_service = "ERROR CONNECTING";
                        if (!isDebug)
                            dataContext.SubmitChanges();
                    }
                }
            }
        }

        public void SendThankYouEmail(TurnKeyBrokerSignUpDataContext dataContext, RegistrationInfo info)
        {
            string uri = "TradeZero";
            if (dataContext == null)
                dataContext = new TurnKeyBrokerSignUpDataContext();
            string email_id = "";
            emailTemplate em;
            string fullName = string.Empty;
            tb_account account = dataContext.tb_accounts.Where(a => a.storeURI == uri && a.account_id == info.AccountId).FirstOrDefault();//regModel.activeStore.uri
            if (account.trading_type == "Bitcoin")
            {
                em = dataContext.emailTemplates.Where(e => e.storeId == 1 && e.templateName == "New Bitcoin Account").FirstOrDefault();//regModel.activeStore.storeId

            }
            else
            {
                em = dataContext.emailTemplates.Where(e => e.storeId == 1 && e.templateName == "NEWACCOUNT").FirstOrDefault();//regModel.activeStore.storeId
            }
            var addresses = dataContext.tb_addresses.Where(add => add.account_id == account.account_id);
            tb_address primaryAddress;
            if (account.account_type_id == 3)
            {
                primaryAddress = addresses.Where(add => add.address_type_id == 1 && add.is_coapplicant == true).FirstOrDefault();
                email_id = account.email_address;
            }
            else
            {
                primaryAddress = addresses.Where(add => add.address_type_id == 1).FirstOrDefault();
                email_id = primaryAddress.email_address;
            }
            if (em != null && account != null && addresses != null && primaryAddress != null)
            {
                int templateID = em.emailTemplateId;

                string emailSubject = em.emailSubject;
                string emailBody = em.emailBody;

                string firmAddress = string.Empty;

                firmAddress = string.IsNullOrEmpty(regModel.activeStore.address1) ? string.Empty : regModel.activeStore.address1 + "<br/>";
                firmAddress = firmAddress + (string.IsNullOrEmpty(regModel.activeStore.city) ? string.Empty : regModel.activeStore.city + " ");
                firmAddress = firmAddress + (string.IsNullOrEmpty(regModel.activeStore.state) ? string.Empty : regModel.activeStore.state + " ");
                firmAddress = firmAddress + (string.IsNullOrEmpty(regModel.activeStore.zipcode) ? string.Empty : regModel.activeStore.zipcode + " ");

                emailBody = emailBody.Replace("[FNAME]", primaryAddress.first_name);
                emailBody = emailBody.Replace("[LNAME]", primaryAddress.last_name);
                emailBody = emailBody.Replace("[FIRM]", regModel.activeStore.storeDisplayName);
                emailBody = emailBody.Replace("[ACCOUNT]", account.customer_id);
                emailBody = emailBody.Replace("[FIRM ADDRESS]", firmAddress);
                emailBody = emailBody.Replace("[FIRM CUSTOMER SERVICE NUMBER]", (string.IsNullOrEmpty(regModel.activeStore.contactPhone) ? "" : regModel.activeStore.contactPhone));

                int EmailLogID = TurnKeyBrokerSignUp.WebUI.Services.EmailHelper.AddToEmailQueue(regModel.activeStore.uri, em.fromName, em.fromEmail, primaryAddress.first_name + " " + primaryAddress.last_name, email_id, em.ccEmail, em.bccEmail, emailSubject, emailBody);
                string message = TurnKeyBrokerSignUp.WebUI.Services.EmailHelper.SendEmail(em.fromName, em.fromEmail, primaryAddress.first_name + " " + primaryAddress.last_name, email_id, em.ccEmail, em.bccEmail, emailSubject, emailBody, em.isHtml);

                if (EmailLogID > 0)
                    TurnKeyBrokerSignUp.WebUI.Services.EmailHelper.UpdateEmailQueueStatus(EmailLogID, message.Trim().ToUpper().Equals("OK"), message);
            }
        }
        public void SendThankYouEmail1(TurnKeyBrokerSignUpDataContext dataContext,RegistrationInfo info)
        {
            var activeStore = dataContext.store_bases.FirstOrDefault(t => t.storeId == 1);
           // tb_account account = dataContext.store_bases.Where(a => a.storeURI == uri && a.account_id == info.AccountId).FirstOrDefault();
            string uri = "TradeZero";
            if (dataContext == null)
                dataContext = new TurnKeyBrokerSignUpDataContext();
            string email_id = "";
            emailTemplate em;
            string fullName = string.Empty;
            tb_account account = dataContext.tb_accounts.Where(a => a.storeURI == activeStore.uri && a.account_id == info.AccountId).FirstOrDefault();//regModel.activeStore.uri
            if (account.trading_type == "Bitcoin")
            {
                em = dataContext.emailTemplates.Where(e => e.storeId == activeStore.storeId && e.templateName == "New Bitcoin Account").FirstOrDefault();//regModel.activeStore.storeId

            }
            else
            {
                em = dataContext.emailTemplates.Where(e => e.storeId == activeStore.storeId && e.templateName == "NEWACCOUNT").FirstOrDefault();//regModel.activeStore.storeId
            }
            var addresses = dataContext.tb_addresses.Where(add => add.account_id == account.account_id);
            tb_address primaryAddress;
            if (account.account_type_id == 3)
            {
                primaryAddress = addresses.Where(add => add.address_type_id == 1 && add.is_coapplicant == true).FirstOrDefault();
                email_id = account.email_address;
            }
            else
            {
                primaryAddress = addresses.Where(add => add.address_type_id == 1).FirstOrDefault();
                email_id = account.email_address;
            }
            if (em != null && account != null && addresses != null && primaryAddress != null)
            {
                int templateID = em.emailTemplateId;

                string emailSubject = em.emailSubject;
                string emailBody = em.emailBody;

                string firmAddress = string.Empty;

                firmAddress = string.IsNullOrEmpty(activeStore.address1) ? string.Empty : activeStore.address1 + "<br/>";
                firmAddress = firmAddress + (string.IsNullOrEmpty(activeStore.city) ? string.Empty : activeStore.city + " ");
                firmAddress = firmAddress + (string.IsNullOrEmpty(activeStore.state) ? string.Empty : activeStore.state + " ");
                firmAddress = firmAddress + (string.IsNullOrEmpty(activeStore.zipcode) ? string.Empty : activeStore.zipcode + " ");

                emailBody = emailBody.Replace("[FNAME]", primaryAddress.first_name);
                emailBody = emailBody.Replace("[LNAME]", primaryAddress.last_name);
                emailBody = emailBody.Replace("[FIRM]", activeStore.storeDisplayName);
                emailBody = emailBody.Replace("[ACCOUNT]", account.customer_id);
                emailBody = emailBody.Replace("[FIRM ADDRESS]", firmAddress);
                emailBody = emailBody.Replace("[FIRM CUSTOMER SERVICE NUMBER]", (string.IsNullOrEmpty(activeStore.contactPhone) ? "" : activeStore.contactPhone));

                int EmailLogID = TurnKeyBrokerSignUp.WebUI.Services.EmailHelper.AddToEmailQueue(activeStore.uri, em.fromName, em.fromEmail, primaryAddress.first_name + " " + primaryAddress.last_name, email_id, em.ccEmail, em.bccEmail, emailSubject, emailBody);
                string message = TurnKeyBrokerSignUp.WebUI.Services.EmailHelper.SendEmail(em.fromName, em.fromEmail, primaryAddress.first_name + " " + primaryAddress.last_name, email_id, em.ccEmail, em.bccEmail, emailSubject, emailBody, em.isHtml);

                if (EmailLogID > 0)
                    TurnKeyBrokerSignUp.WebUI.Services.EmailHelper.UpdateEmailQueueStatus(EmailLogID, message.Trim().ToUpper().Equals("OK"), message);
            }
        }
        public void SendOFACFailedEmail1(TurnKeyBrokerSignUpDataContext dataContext, RegistrationInfo info)
        {
            var activeStore = dataContext.store_bases.FirstOrDefault(t => t.storeId == 1);
            if (dataContext == null)
                dataContext = new TurnKeyBrokerSignUpDataContext();
            string email_id = "";
            emailTemplate em;
            string fullName = string.Empty; 
             // tb_account account = dataContext.tb_accounts.Where(a => a.storeURI == regModel.activeStore.uri && a.account_id == acc.account_id).FirstOrDefault();
             tb_account account = dataContext.tb_accounts.Where(a => a.storeURI == regModel.activeStore.uri && a.account_id == info.AccountId).FirstOrDefault();
            em = dataContext.emailTemplates.Where(e => e.storeId == activeStore.storeId && e.emailTemplateId == 48).FirstOrDefault();
            var addresses = dataContext.tb_addresses.Where(add => add.account_id == account.account_id);
            tb_address primaryAddress;
            if (account.account_type_id == 3)
            {
                primaryAddress = addresses.Where(add => add.address_type_id == 1 && add.is_coapplicant == true).FirstOrDefault();
                email_id = account.email_address;
            }
            else
            {
                primaryAddress = addresses.Where(add => add.address_type_id == 1).FirstOrDefault();
                email_id = account.email_address;  //primaryAddress
            }
            if (em != null && account != null && addresses != null && primaryAddress != null)
            {
                int templateID = em.emailTemplateId;

                string emailSubject = em.emailSubject;
                string emailBody = em.emailBody;

                string firmAddress = string.Empty;

                firmAddress = string.IsNullOrEmpty(activeStore.address1) ? string.Empty : activeStore.address1 + "<br/>";
                firmAddress = firmAddress + (string.IsNullOrEmpty(activeStore.city) ? string.Empty : activeStore.city + " ");
                firmAddress = firmAddress + (string.IsNullOrEmpty(activeStore.state) ? string.Empty : activeStore.state + " ");
                firmAddress = firmAddress + (string.IsNullOrEmpty(activeStore.zipcode) ? string.Empty : activeStore.zipcode + " ");

                emailBody = emailBody.Replace("[FNAME]", primaryAddress.first_name);
                emailBody = emailBody.Replace("[LNAME]", primaryAddress.last_name);
                emailBody = emailBody.Replace("[FIRM]", activeStore.storeDisplayName);
                emailBody = emailBody.Replace("[FIRM ADDRESS]", firmAddress);
                emailBody = emailBody.Replace("[FIRM CUSTOMER SERVICE NUMBER]", (string.IsNullOrEmpty(activeStore.contactPhone) ? "" : activeStore.contactPhone));

                int EmailLogID = EmailHelper.AddToEmailQueue(activeStore.uri, em.fromName, em.fromEmail, primaryAddress.first_name + " " + primaryAddress.last_name, email_id, em.ccEmail, em.bccEmail, emailSubject, emailBody);
                string message = EmailHelper.SendEmail(em.fromName, em.fromEmail, primaryAddress.first_name + " " + primaryAddress.last_name, email_id, em.ccEmail, em.bccEmail, emailSubject, emailBody, em.isHtml);


                if (EmailLogID > 0)
                    EmailHelper.UpdateEmailQueueStatus(EmailLogID, message.Trim().ToUpper().Equals("OK"), message);
            }
        }

        /// <summary>
        /// This method will send reset password email
        /// </summary>
        /// <param name="dataContext"></param>
        /// <param name="acc"></param>
        /// <returns></returns>
        public int SendResetPasswordEmail(TurnKeyBrokerSignUpDataContext dataContext, RegistrationInfo info)  //tb_account acc
        {
            var activeStore = dataContext.store_bases.FirstOrDefault(t => t.storeId == 1);
            int msgId = 1;
            if (dataContext == null)
                dataContext = new TurnKeyBrokerSignUpDataContext();
            try
            {
                emailTemplate em = dataContext.emailTemplates.Where(e => e.storeId == activeStore.storeId && e.templateName == "NEWACCOUNT").FirstOrDefault();
                 //var accountLst = dataContext.tb_accounts.Where(a => a.storeURI == regModel.activeStore.uri && a.email_address == acc.email_address && (a.Completed.HasValue && a.Completed.Value)).ToList();
                var accountLst = dataContext.tb_accounts.Where(a => a.storeURI == activeStore.uri && a.account_id == info.AccountId && (a.Completed.HasValue && a.Completed.Value)).ToList();
                foreach (var account in accountLst)
                {
                    var addresses = dataContext.tb_addresses.Where(add => add.account_id == account.account_id);
                    var primaryAddress = addresses.Where(add => add.address_type_id == 1).FirstOrDefault();

                    if (em != null && account != null && addresses != null && primaryAddress != null)
                    {

                        string emailSubject = "Tradezero password reset (Customer Id=" + account.customer_id + ")";
                        string emailBody = em.emailBody;

                        string firmAddress = string.Empty;
                        var url = ConfigurationManager.AppSettings["WebURL"].ToString();
                        firmAddress = string.IsNullOrEmpty(activeStore.address1) ? string.Empty : activeStore.address1 + "<br/>";
                        firmAddress = firmAddress + (string.IsNullOrEmpty(activeStore.address2) ? string.Empty : activeStore.address2 + "<br/>");
                        firmAddress = firmAddress + (string.IsNullOrEmpty(activeStore.city) ? string.Empty : activeStore.city + " ");
                        firmAddress = firmAddress + (string.IsNullOrEmpty(activeStore.state) ? string.Empty : activeStore.state + " ");
                        firmAddress = firmAddress + (string.IsNullOrEmpty(activeStore.zipcode) ? string.Empty : activeStore.zipcode + " ");

                        emailBody = string.Format("Hi {0},", primaryAddress.first_name);
                        emailBody = emailBody + "<br/>Your username: " + account.customer_id + "<br/>";
                        emailBody = emailBody + "<br/><br/>Someone recently requested a password change for your tradezero account. If this was you, you can set a new password ";
                        emailBody = emailBody + string.Format("<a href='{0}{1}' > here. </a>", url, "Home/ResetPassword?AccId=" + HttpUtility.HtmlEncode(primaryAddress.account_id.ToString()) + "");
                        emailBody = emailBody + "<br/><br/>If you don't want to change your password or didn't request this, just ignore and delete this message";

                        emailBody = emailBody + "<br/><br/>To keep your account secure, please don't forward this email to anyone.";
                        emailBody = emailBody + "<br/><br/>Thanks!";
                        int EmailLogID = TurnKeyBrokerSignUp.WebUI.Services.EmailHelper.AddToEmailQueue(activeStore.uri, em.fromName, em.fromEmail, primaryAddress.first_name + " " + primaryAddress.last_name, account.email_address, em.ccEmail, em.bccEmail, emailSubject, emailBody);
                        string message = TurnKeyBrokerSignUp.WebUI.Services.EmailHelper.SendEmail(em.fromName, em.fromEmail, primaryAddress.first_name + " " + primaryAddress.last_name, account.email_address, em.ccEmail, em.bccEmail, emailSubject, emailBody, em.isHtml);
                        if (!message.ToLower().Contains("ok"))
                        {
                            msgId = 0;
                        }
                        if (EmailLogID > 0)
                            TurnKeyBrokerSignUp.WebUI.Services.EmailHelper.UpdateEmailQueueStatus(EmailLogID, message.Trim().ToUpper().Equals("OK"), message);

                    }
                }

            }
            catch (Exception ex)
            {
                Elmah.ErrorLog.GetDefault(System.Web.HttpContext.Current).Log(new Elmah.Error(ex, System.Web.HttpContext.Current));
            }
            return msgId;
        }
    }
}

