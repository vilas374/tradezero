using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace TurnKeyBrokerSignUp.WebUI.Models
{
    public class SignupHelper
    {

        RegistrationInfo regModel = new RegistrationInfo();

        // <summary>
        /// This method will save agreement data
        /// <param name="dataContext"></param>
        /// <param name="customerId"></param>
        /// <param name="detailId"></param>
        /// <param name="agreementId"></param>
        /// <param name="value"></param>
        /// </summary>
        /// <returns></returns>
        public void SaveAgreementFieldValue(TurnKeyBrokerSignUpDataContext dataContext, string customerId, int detailId, int agreementId, string value)
        {
            if (dataContext == null)
                dataContext = new TurnKeyBrokerSignUpDataContext();

            try
            {
                var ua = new UserAgrement();
                ua.UserID = customerId;
                ua.AgreementID = agreementId;
                ua.AgreementDetailsID = detailId;
                ua.SignatureValue = value;
                ua.CreatedBy = "User";
                ua.DateCreated = DateTime.Now;
                dataContext.UserAgrements.InsertOnSubmit(ua);
                dataContext.SubmitChanges();
            }
            catch (Exception ex)
            {
                Elmah.ErrorLog.GetDefault(System.Web.HttpContext.Current).Log(new Elmah.Error(ex, System.Web.HttpContext.Current));
            }
        }

        
        public List<AgreementDetail> GetActiveAgreementDetails(TurnKeyBrokerSignUpDataContext dataContext)
        {
            if (dataContext == null)
                dataContext = new TurnKeyBrokerSignUpDataContext();

            return dataContext.AgreementDetails.OrderBy(ad => ad.SortOrder).ToList();
        }

        public IQueryable<tb_initial_lu> GetAllInitials(TurnKeyBrokerSignUpDataContext dataContext)
        {
            if (dataContext == null)
                dataContext = new TurnKeyBrokerSignUpDataContext();

            return (from i in dataContext.tb_initial_lus
                    select i).Distinct();
        }
        public IQueryable<tb_suffix_lu> GetAllSuffixs(TurnKeyBrokerSignUpDataContext dataContext)
        {
            if (dataContext == null)
                dataContext = new TurnKeyBrokerSignUpDataContext();

            return (from i in dataContext.tb_suffix_lus
                    select i).Distinct();
        }
        public IQueryable<tb_state> GetAllStates(TurnKeyBrokerSignUpDataContext dataContext, int country_id)
        {
            if (dataContext == null)
                dataContext = new TurnKeyBrokerSignUpDataContext();

            return (from i in dataContext.tb_states
                    where i.country_id == country_id
                    select i).Distinct();
        }
        public IQueryable<tb_country> GetAllCountries(TurnKeyBrokerSignUpDataContext dataContext, string trading_type)
        {
            if (dataContext == null)
                dataContext = new TurnKeyBrokerSignUpDataContext();
            if (trading_type != "Bitcoin")
            {
                return (from i in dataContext.tb_countries
                        where i.active && !i.deleted && i.country_id != 1
                        select i).Distinct().OrderBy(c => c.country);
            }
            else
            {
                return (from i in dataContext.tb_countries
                        where i.active && !i.deleted
                        select i).Distinct().OrderBy(c => c.country);
            }


        }
        public IQueryable<tb_martial_status_lu> GetAllMartialStatus(TurnKeyBrokerSignUpDataContext dataContext)
        {
            if (dataContext == null)
                dataContext = new TurnKeyBrokerSignUpDataContext();

            return (from i in dataContext.tb_martial_status_lus
                    where i.martial_status_id > 0
                    select i).Distinct();
        }
        public IQueryable<tb_employment_status_lu> GetAllEmploymentStatus(TurnKeyBrokerSignUpDataContext dataContext)
        {
            if (dataContext == null)
                dataContext = new TurnKeyBrokerSignUpDataContext();

            return (from i in dataContext.tb_employment_status_lus
                    select i).Distinct();
        }
        public IQueryable<tb_identification_type_lu> GetAllIdentificationTypes(TurnKeyBrokerSignUpDataContext dataContext)
        {
            if (dataContext == null)
                dataContext = new TurnKeyBrokerSignUpDataContext();

            return (from i in dataContext.tb_identification_type_lus
                    select i).Distinct();
        }
        public IQueryable<tb_account_type_lu> GetAllAcccountTypes(TurnKeyBrokerSignUpDataContext dataContext)
        {
            if (dataContext == null)
                dataContext = new TurnKeyBrokerSignUpDataContext();

            return dataContext.tb_account_type_lus.Distinct().OrderBy(a => a.account_type);
        }

       
        public int GetAddressType(TurnKeyBrokerSignUpDataContext dataContext, string type)
        {
            if (dataContext == null)
                dataContext = new TurnKeyBrokerSignUpDataContext();

            var a = (from s in dataContext.tb_address_type_lus
                     where s.address_type == type
                     select s).FirstOrDefault();

            if (a != null)
                return a.address_type_id;
            else
                if (type == "Permanent")
                return 1;
            else
                return 0;
        }
        public void SaveCommunicationChoices(TurnKeyBrokerSignUpDataContext dataContext, int accountId, string communication_option, string communication_email,bool isDebug)
        {
            if (dataContext == null)
                dataContext = new TurnKeyBrokerSignUpDataContext();

            tb_communication_choice communication = new tb_communication_choice();
            communication.account_id = accountId;
            communication.communication_option = communication_option;
            communication.communication_email = communication_email;
            communication.date_created = DateTime.Now;
            dataContext.tb_communication_choices.InsertOnSubmit(communication);
            if (!isDebug)
                dataContext.SubmitChanges();
        }
        public void SaveMangaingYourCash(TurnKeyBrokerSignUpDataContext dataContext, int accountId, string sales_proceed, string sales_other, string dividend_proceed, string dividend_other1, string other_brokerage_account, string other_brokerage_account_firms, string securities_proceed, bool isDebug)
        {
            if (dataContext == null)
                dataContext = new TurnKeyBrokerSignUpDataContext();

            tb_managing_cash cash = new tb_managing_cash();
            cash.account_id = accountId;
            cash.sales_proceed = sales_other;
            cash.sales_other = sales_other;
            cash.dividend_proceed = dividend_proceed;
            cash.dividend_other1 = dividend_other1;
            cash.maintain_other_brokerage = other_brokerage_account;
            cash.maintain_other_brokeerage_firms = other_brokerage_account_firms;
            cash.securities_proceed = securities_proceed;

            cash.date_created = DateTime.Now;

            dataContext.tb_managing_cashes.InsertOnSubmit(cash);
            if (!isDebug)
                dataContext.SubmitChanges();

        }

        public void SaveMarginAccounts(TurnKeyBrokerSignUpDataContext dataContext, int accountId, bool is_margin_loan_agreement, bool is_borrow_funds_account, bool isDebug)
        {
            if (dataContext == null)
                dataContext = new TurnKeyBrokerSignUpDataContext();
            int update = 0;
            tb_margin_account margin;
            var tb_margin_account = dataContext.tb_margin_accounts.Where(t => t.account_id == accountId).FirstOrDefault();
            if (tb_margin_account != null)
            {
                update = 1;
                margin = tb_margin_account;

            }
            else {
                margin = new tb_margin_account();
            }
            margin.account_id = accountId;
            margin.is_margin_loan_agreement = is_margin_loan_agreement;
            margin.is_borrow_funds_account = is_borrow_funds_account;
            margin.date_created = DateTime.Now;
            if (update == 0)
            {
                dataContext.tb_margin_accounts.InsertOnSubmit(margin);
            }
            if (!isDebug)
                dataContext.SubmitChanges();

        }

        public void SaveFinancialInvestmentExperience(TurnKeyBrokerSignUpDataContext dataContext, int accountId,
                string year_mutual_funds,
                string number_mutual_funds,
                string average_size_per_trade,
                string year_investment_stocks,
                string number_investment_stocks,
                string year_bonds,
                string number_bonds,
                string year_options,
                string number_options,
                string year_social_securities,
                string number_social_securities,
                string year_annuties,
                string number_annuties,
                string year_alternatives,
                string number_alternatives,
                string year_margins,
                string number_margins,
                string decision_making_experience,
                string other_investment_type_1,
                string other_investment_firm_1,
                string other_investment_amount_1,
                string other_investment_type_2,
                string other_investment_firm_2,
                string other_investment_amount_2,
                string other_investment_type_3,
                string other_investment_firm_3,
                string other_investment_amount_3, bool isDebug
            )
        {
            if (dataContext == null)
                dataContext = new TurnKeyBrokerSignUpDataContext();
            tb_financial_investment investment;
            int update = 0;
            var tb_financial_investment = dataContext.tb_financial_investments.Where(t => t.account_id == accountId).FirstOrDefault();
            if (tb_financial_investment != null)
            {
                investment = tb_financial_investment;
                update = 1;
            }
            else
            {
                investment = new tb_financial_investment();
            }

            investment.account_id = accountId;
            investment.year_mutual_funds = year_mutual_funds;
            investment.number_mutual_funds = number_mutual_funds;
            investment.average_size_per_trade = average_size_per_trade;
            investment.year_investment_stocks = year_investment_stocks;
            investment.number_investment_stocks = number_investment_stocks;
            investment.year_bonds = year_bonds;
            investment.number_bonds = number_bonds;
            investment.year_options = year_options;
            investment.number_options = number_options;
            investment.year_social_securities = year_social_securities;
            investment.number_social_securities = number_social_securities;
            investment.year_annuties = year_annuties;
            investment.number_annuties = number_annuties;
            investment.year_alternatives = year_alternatives;
            investment.number_alternatives = number_alternatives;
            investment.year_margins = year_margins;
            investment.number_margins = number_margins;
            investment.decision_making_experience = decision_making_experience;
            investment.other_investment_type_1 = other_investment_type_1;
            investment.other_investment_firm_1 = other_investment_firm_1;
            investment.other_investment_amount_1 = other_investment_amount_1;
            investment.other_investment_type_2 = other_investment_type_2;
            investment.other_investment_firm_2 = other_investment_firm_2;
            investment.other_investment_amount_2 = other_investment_amount_2;
            investment.other_investment_type_3 = other_investment_type_3;
            investment.other_investment_firm_3 = other_investment_firm_3;
            investment.other_investment_amount_3 = other_investment_amount_3;
            investment.date_created = DateTime.Now;
            if (update == 0)
            {
                dataContext.tb_financial_investments.InsertOnSubmit(investment);
            }
            if (!isDebug)
                dataContext.SubmitChanges();
        }
        public void SaveRiskTolerance(TurnKeyBrokerSignUpDataContext dataContext, int accountId, string riskTolerance, bool isDebug)
        {
            if (dataContext == null)
                dataContext = new TurnKeyBrokerSignUpDataContext();
            int update = 0;
            tb_risk_tolerance risk;
            var tb_risk_tolerance = dataContext.tb_risk_tolerances.Where(t => t.account_id == accountId).FirstOrDefault();
            if (tb_risk_tolerance != null)
            {
                update = 1;
                risk = tb_risk_tolerance;
            }
            else
            {
                risk = new tb_risk_tolerance();
            }

            risk.account_id = accountId;
            risk.account_risk_tolerance = riskTolerance;
            risk.date_created = DateTime.Now;
            if (update == 0)
            {
                dataContext.tb_risk_tolerances.InsertOnSubmit(risk);
            }
            if (!isDebug)
                dataContext.SubmitChanges();

        }
        public void SaveGoals(TurnKeyBrokerSignUpDataContext dataContext, int accountId, string investment_account_option, string account_future, string account_future_other, string earliest_funding_time_frame, bool isDebug)
        {
            if (dataContext == null)
                dataContext = new TurnKeyBrokerSignUpDataContext();
            int update = 0;
            tb_goal goal;
            var tb_goal = dataContext.tb_goals.Where(t => t.account_id == accountId).FirstOrDefault();

            if (tb_goal != null)
            {
                goal = tb_goal;
                update = 1;
            }
            else
            {
                goal = new tb_goal();
            }

            goal.account_id = accountId;
            goal.investment_account_option = investment_account_option;
            goal.account_future = account_future;
            goal.account_future_other = account_future_other;
            goal.earliest_funding_time_frame = earliest_funding_time_frame;
            goal.date_created = DateTime.Now;
            if (update == 0)
            {
                dataContext.tb_goals.InsertOnSubmit(goal);
            }
            if (!isDebug)
                dataContext.SubmitChanges();


        }

        public void SaveFunding(TurnKeyBrokerSignUpDataContext dataContext, int accountId,
                    string funding_option, string funding_option_other, string funding_sourcefunds, string bankName, string more_info, bool isDebug)
        {
            if (dataContext == null)
                dataContext = new TurnKeyBrokerSignUpDataContext();
            tb_funding funding;
            int update = 0;
            var tb_funding = dataContext.tb_fundings.Where(t => t.account_id == accountId).FirstOrDefault();
            if (tb_funding != null)
            {
                funding = tb_funding;
                update = 1;
            }
            else
            {
                funding = new tb_funding();
            }
            funding.funding_sourcefunds_option = funding_sourcefunds;
            funding.account_id = accountId;
            funding.funding_option = funding_option;
            funding.funding_option_other = funding_option_other;
            funding.date_created = DateTime.Now;
            funding.more_info = more_info;

            if (update == 0)
            {
                dataContext.tb_fundings.InsertOnSubmit(funding);
            }
            int refUpdate = 0;
            if (!string.IsNullOrWhiteSpace(bankName))
            {
                tb_account_bankreference bankref;
                var tb_account_bankreference = dataContext.tb_account_bankreferences.Where(t => t.Account_id == accountId).FirstOrDefault();
                if (tb_account_bankreference != null)
                {
                    bankref = tb_account_bankreference;
                    refUpdate = 1;
                }
                else
                {
                    bankref = new tb_account_bankreference();
                }

                bankref.Account_id = accountId;
                bankref.BankName = bankName;
                bankref.DateAdded = DateTime.Now;
                if (refUpdate == 0)
                {
                    dataContext.tb_account_bankreferences.InsertOnSubmit(bankref);
                }
                
            }
            if (!isDebug)
                dataContext.SubmitChanges();

        }

        public void SaveFinancials(TurnKeyBrokerSignUpDataContext dataContext, int accountId,
                               string annual_income,
                               string tax_rate,
                               string net_worth,
                               string liquid_net_worth,
                               string annual_expense,
                               string special_expense,
                               string special_expense_time,bool isDebug)
        {
            if (dataContext == null)
                dataContext = new TurnKeyBrokerSignUpDataContext();
            tb_financial financial;
            int update = 0;
            var tb_financial = dataContext.tb_financials.Where(t => t.account_id == accountId).FirstOrDefault();

            if (tb_financial != null)
            {
                financial = tb_financial;
                update = 1;
            }
            else
            {

                financial = new tb_financial();
            }

            financial.account_id = accountId;
            financial.annual_income = annual_income;
            financial.tax_rate = tax_rate;
            financial.net_worth = net_worth;
            financial.liquid_net_worth = liquid_net_worth;
            financial.annual_expense = annual_expense;
            financial.special_expense = special_expense;
            financial.special_expense_time = special_expense_time;
            financial.date_created = DateTime.Now;
            if (update == 0)
            {
                dataContext.tb_financials.InsertOnSubmit(financial);
            }
            if (!isDebug)
                dataContext.SubmitChanges();
        }

        public void SaveAffiliations(TurnKeyBrokerSignUpDataContext dataContext, int accountId,
                            bool is_employed_security_industry,
                            bool is_broker_dealer_security_dealer,
                            bool is_investment_advisor,
                            bool is_state_federal_regulator,
                            bool is_FINRA,
                            string entity_name,
                            bool is_shareholder_public_company,
                            string company_name,
                            bool is_senior_govt_nonus,
                            int senior_govt_nonus_country_id, bool isDebug
            )
        {
            if (dataContext == null)
                dataContext = new TurnKeyBrokerSignUpDataContext();
            int update = 0;
            tb_affiliation aff;
            var tb_affilication = dataContext.tb_affiliations.Where(t => t.account_id == accountId).FirstOrDefault();
            if (tb_affilication != null)
            {
                update = 1;
                aff = tb_affilication;

            }
            else {
                aff = new tb_affiliation();
            }

            aff.account_id = accountId;
            aff.is_employed_security_industry = is_employed_security_industry;
            aff.is_broker_dealer_security_dealer = is_broker_dealer_security_dealer;
            aff.is_investment_advisor = is_investment_advisor;
            aff.is_state_federal_regulator = is_state_federal_regulator;
            aff.is_FINRA = is_FINRA;
            aff.entity_name = entity_name;
            aff.is_shareholder_public_company = is_shareholder_public_company;
            aff.company_name = company_name;
            aff.is_senior_govt_nonus = is_senior_govt_nonus;
            aff.senior_govt_nonus_country_id = senior_govt_nonus_country_id;
            aff.date_created = DateTime.Now;
            if (update == 0)
            {
                dataContext.tb_affiliations.InsertOnSubmit(aff);
            }
            if (!isDebug)
                dataContext.SubmitChanges();
        }

        public void SavePatriot(TurnKeyBrokerSignUpDataContext dataContext, int accountId, string dateofBirth, int identification_type_id, string identification_number, string identification_expiration, string identification_place_of_issue, string identification_issue_date, int residence_country_id, bool isCoApplicant, bool isDebug)
        {
            if (dataContext == null)
                dataContext = new TurnKeyBrokerSignUpDataContext();
            int update = 0;
            tb_patriot patriot;
            var tb_patriot = dataContext.tb_patriots.Where(t => t.account_id == accountId).FirstOrDefault();
            if (tb_patriot != null)
            {

                if (isCoApplicant == true)
                {
                    patriot = new tb_patriot();
                    patriot.is_coapplicant = false;
                }
                else
                {
                    patriot = tb_patriot;
                    update = 1;
                    patriot.is_coapplicant = isCoApplicant;
                }
            }
            else
            {
                patriot = new tb_patriot();
                patriot.is_coapplicant = isCoApplicant;
            }

            patriot.account_id = accountId;
            patriot.date_of_birth = dateofBirth;
            patriot.identification_type_id = identification_type_id;
            patriot.identitication_number = identification_number;
            patriot.identification_expiration = identification_expiration;
            patriot.identitifcation_place_of_issue = identification_place_of_issue;
            patriot.identification_issue_date = identification_issue_date;
            patriot.residence_country_id = residence_country_id;

            if (update == 0)
            {
                dataContext.tb_patriots.InsertOnSubmit(patriot);
            }
            if (!isDebug)
                dataContext.SubmitChanges();
        }

        public void SaveEmploymentDetails(TurnKeyBrokerSignUpDataContext dataContext, RegistrationInfo info, int address_id, int employmentStatus, string employmentStatusOther, string jobTitle, string jobOccupation, string employerName, string yearsWithEmployer, bool isCoApplicant, bool isDebug)
        {
            if (dataContext == null)
                dataContext = new TurnKeyBrokerSignUpDataContext();
            int update = 0;
            tb_employementstatus employment_status;
            var tb_employeestatus = dataContext.tb_employementstatus.Where(t => t.account_id == info.AccountId).FirstOrDefault();
            if (tb_employeestatus != null)
            {

                if (isCoApplicant == true)
                {
                    employment_status = new tb_employementstatus();
                    employment_status.is_coapplicant = false;
                }
                else
                {
                    update = 1;
                    employment_status = tb_employeestatus;
                    employment_status.is_coapplicant = isCoApplicant;
                }


            }
            else
            {
                employment_status = new tb_employementstatus();
            }

            employment_status.account_id = info.AccountId;
            employment_status.business_address_id = address_id;
            employment_status.employment_status_id = employmentStatus;
            employment_status.employement_status_other = employmentStatusOther;
            employment_status.job_title = jobTitle;
            employment_status.occupation = jobOccupation;
            employment_status.employer = employerName;
            employment_status.years_with_employment = yearsWithEmployer;

            if (update == 0)
            {
                dataContext.tb_employementstatus.InsertOnSubmit(employment_status);
            }

            if (!isDebug)
                dataContext.SubmitChanges();


        }
        

        public bool SaveAddressWithEmail(TurnKeyBrokerSignUpDataContext dataContext, RegistrationInfo info, string type, string title, string first_name, string last_name, string middle_name, string suffix, string address_1, string address_2,
                               string city, int stateId, string otherState, string zipCode, int countryId, bool is_address_last_one_year, int martial_status_id, string work_phone, string num_dependents, string email, string photo_id_path, string business_phone, string fax, string contact_name, int address_id)
        {
            try
            {
                if (dataContext == null)
                    dataContext = new TurnKeyBrokerSignUpDataContext();

                int update = 0;
                var objAdd = dataContext.tb_addresses.Where(t => t.account_id == info.AccountId && t.address_id == address_id).FirstOrDefault();
                tb_address address;
                if (objAdd != null)
                {
                    address = objAdd;
                    update = 1;
                }
                else {
                    address = new tb_address();
                }



                address.account_id = info.AccountId;
                address.address_type_id = GetAddressType(dataContext, type);
                address.title = title;
                address.first_name = first_name;
                address.last_name = last_name;
                address.middle_name = middle_name;
                address.suffix = suffix;
                address.address_1 = address_1;
                address.address_2 = address_2;
                address.city = city;
                address.business_telephone = business_phone;
                address.fax = fax;
                if (countryId == 1)
                    address.state = stateId.ToString();
                else
                    address.state = otherState;

                address.zipcode = zipCode;
                address.country_id = countryId;
                address.is_address_last_one_year = is_address_last_one_year;
                address.martial_status_id = martial_status_id;
                address.work_phone = work_phone;
                address.num_dependents = num_dependents;
                address.email_address = email;
                address.photo_id_path = photo_id_path;
                address.date_created = DateTime.Now;
                address.contact_name = contact_name;
                address.is_coapplicant = true;
                if (update == 0)
                {
                    dataContext.tb_addresses.InsertOnSubmit(address);
                }
                else
                {
                    dataContext.SubmitChanges();
                }





                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }


        public bool SaveAddress(TurnKeyBrokerSignUpDataContext dataContext, RegistrationInfo info, string type, string title, string first_name, string last_name, string middle_name, string suffix, string address_1, string address_2,
                                string city, int stateId, string otherState, string zipCode, int countryId, bool is_address_last_one_year, int martial_status_id, string work_phone, string num_dependents, bool isCoApplicant, string business_tel, string fax, bool isDebug)
        {
            try
            {
                int update = 0;
                if (dataContext == null)
                    dataContext = new TurnKeyBrokerSignUpDataContext();
                var accTypeId = GetAddressType(dataContext, type);
                var tb_address = dataContext.tb_addresses.Where(t => t.account_id == info.AccountId && t.address_type_id == accTypeId).FirstOrDefault();

                tb_address address;
                if (tb_address != null)
                {
                    if (info.AccountType == 2)
                    {
                        if (isCoApplicant == true)
                        {
                            address = new tb_address();
                        }
                        else
                        {
                            address = tb_address;
                            update = 1;
                        }
                    }
                    else
                    {
                        address = tb_address;
                        update = 1;
                    }

                }
                else {
                    address = new tb_address();
                }
                address.account_id = info.AccountId;
                address.address_type_id = accTypeId;
                address.title = title;
                address.first_name = first_name;
                address.last_name = last_name;
                address.middle_name = middle_name;
                address.suffix = suffix;
                address.address_1 = address_1;
                address.address_2 = address_2;
                address.city = city;
                address.business_telephone = business_tel;
                address.fax = fax;
                if (countryId == 1)
                    address.state = stateId.ToString();
                else
                    address.state = otherState;

                address.zipcode = zipCode;
                address.country_id = countryId;
                address.is_address_last_one_year = is_address_last_one_year;
                address.martial_status_id = martial_status_id;
                address.work_phone = work_phone;
                address.num_dependents = num_dependents;
                address.email_address = info.EmailAddress;
                address.date_created = DateTime.Now;
                address.is_coapplicant = isCoApplicant;
                if (update == 0)
                {
                    dataContext.tb_addresses.InsertOnSubmit(address);
                    dataContext.SubmitChanges();
                }
                else
                {
                    if (!isDebug)
                        dataContext.SubmitChanges();
                    //dataContext.SubmitChanges();
                }


                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public tb_address GetAddress(TurnKeyBrokerSignUpDataContext dataContext, int accountId, string type)
        {
            if (dataContext == null)
                dataContext = new TurnKeyBrokerSignUpDataContext();

            return (from s in dataContext.tb_addresses
                    join a in dataContext.tb_address_type_lus on s.address_type_id equals a.address_type_id
                    where s.account_id == accountId
                    && a.address_type == type
                    select s).FirstOrDefault();

        }

        //public void SaveUserToTurboTick(TurnKeyBrokerSignUpDataContext dataContext, RegistrationInfo info)
        //{
        //    if (dataContext == null)
        //        dataContext = new TurnKeyBrokerSignUpDataContext();

        //    var _account = (from s in dataContext.tb_accounts
        //                        //  where s.email_address == info.EmailAddress
        //                    where s.account_id == info.AccountId
        //                      && s.active == true
        //                    select s).FirstOrDefault();

        //    if (_account != null)
        //    {
        //        tb_address address = GetAddress(dataContext, info.AccountId, "Permanent");

        //        string state = string.Empty;
        //        if (address.country_id == 1)
        //        {
        //            var states = (from s in dataContext.tb_states
        //                          where s.state_id == int.Parse(address.state)
        //                          && s.active == true
        //                          select s).FirstOrDefault();
        //            if (states != null)
        //                state = states.state_initial;
        //        }
        //        else
        //        {
        //            state = address.state;
        //        }


        //        string valid = "";
        //        string managerUsername = System.Configuration.ConfigurationManager.AppSettings["TurboTickDemoManagerUsername"].ToString();
        //        string managerPassword = System.Configuration.ConfigurationManager.AppSettings["TurboTickDemoManagerPassword"].ToString();

        //        valid = SingleSignOnLib.SingleSignOnOverHttp.RequestManagerLogin(managerUsername, managerPassword, SingleSignOnClien.Web.Common.TurboTickSingleSignOn);

        //        if (valid.ToLower().Equals("yes"))
        //        {
        //            SingleSignOnLib.SingleSignOnUser newUser = new SingleSignOnLib.SingleSignOnUser();
        //            SingleSignOnLib.SingleSignOnAccount account = new SingleSignOnLib.SingleSignOnAccount();
        //            newUser.DisableUser = "false";
        //            newUser.Email = _account.email_address;
        //            newUser.Firstname = address.first_name;
        //            newUser.Lastname = address.last_name;
        //            newUser.Password = _account.password;//SingleSignOnLib.SHA.EncryptSHA256(_account.password);
        //            newUser.Phone = address.work_phone;
        //            newUser.DisableUser = "false";
        //            newUser.WebEdition = "true";
        //            newUser.DesktopEdition = "true";
        //            newUser.IsUpdate = false;
        //            newUser.Username = _account.customer_id;
        //            account.Account = _account.customer_id;
        //            account.City = address.city;
        //            account.FirstName = address.first_name;
        //            account.LastName = address.last_name;
        //            account.Address = address.address_1;
        //            account.BPMultiplier = int.Parse(System.Configuration.ConfigurationManager.AppSettings["txtBPMultiplier"].ToString());
        //            account.MarginBuyingPower = decimal.Parse(System.Configuration.ConfigurationManager.AppSettings["txtBuyingPower"].ToString());
        //            account.MasterAccount = "XXXXXXXX";
        //            account.MaximumOrderSize = int.Parse(System.Configuration.ConfigurationManager.AppSettings["txtAccMaxOrderSize"].ToString());
        //            account.OptionsTrading = "true";
        //            account.RoutingCommisionType = System.Configuration.ConfigurationManager.AppSettings["txtAccRoutingComissionTypes"].ToString();
        //            account.State = state;
        //            account.TradingSuspended = "false";

        //            account.Zip = address.zipcode;
        //            newUser.Account = account;
        //            newUser.MobileEdition = "true";
        //            newUser.WebEditionPro = "true";
        //            account.Equity = 0;
        //            account.NightBuyingPower = 0;
        //            if (SingleSignOnLib.SingleSignOnOverHttp.SendNewSingleSignOnUser(SingleSignOnClien.Web.Common.TurboTickSingleSignOn + "?usercreation=1&managerName=" + System.Configuration.ConfigurationManager.AppSettings["TurboTickDemoManagerUsername"].ToString(), System.Configuration.ConfigurationManager.AppSettings["TurboTickDemoManagerUsername"].ToString(), newUser).Equals("success"))
        //            {
        //                _account.turbo_tick_log = string.Format("User '{0}' with account '{1}' was created. Operation succeeded!", newUser.Username, account.Account);
        //                dataContext.SubmitChanges();
        //            }
        //            else
        //            {
        //                _account.turbo_tick_log = "Operation failed! Please retry the operation";
        //                dataContext.SubmitChanges();
        //            }


        //        }
        //    }

        //}

        public void SaveUserToPortal(TurnKeyBrokerSignUpDataContext dataContext, RegistrationInfo info, string StoreURI)

        {
            if (dataContext == null)
                dataContext = new TurnKeyBrokerSignUpDataContext();

            var account = (from s in dataContext.tb_accounts
                               // where s.email_address == info.EmailAddress
                           where s.account_id == info.AccountId

                            && s.active == true
                           select s).FirstOrDefault();

            if (account != null)
            {
                tb_address address = GetAddress(dataContext, info.AccountId, "Permanent");
                string tradeZeroApplicationId = dataContext.aspnet_Applications.Where(a => a.ApplicationName == "tradezero").FirstOrDefault().ApplicationId.ToString();

                if (!string.IsNullOrEmpty(tradeZeroApplicationId))
                {
                    using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["TradeZeroSQLProvider"].ConnectionString))
                    {
                        string strUserCreateUser = "exec dbo.aspnet_Users_CreateUser '" + tradeZeroApplicationId + "','" + account.customer_id + "',0,'" + DateTime.Now.Date.ToString("yyyy-MM-dd HH:mm:ss") + "','" + Guid.NewGuid() + "'";
                        conn.Open();
                        System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand();
                        command.CommandType = System.Data.CommandType.Text;
                        command.Connection = conn;
                        command.CommandText = strUserCreateUser;
                        command.ExecuteNonQuery();

                        Guid guidNewUserId = dataContext.aspnet_Users.Where(u => u.UserName == account.customer_id).FirstOrDefault().UserId;
                        string newUserID = guidNewUserId.ToString();

                        store_base sb = dataContext.store_bases.Where(_sb => _sb.name == StoreURI).FirstOrDefault();
                        int storeId = 1;
                        if (sb != null)
                            storeId = sb.storeId;

                        var _aspNetUser = dataContext.aspnet_Users.Where(a => a.UserId == guidNewUserId).FirstOrDefault();
                        if (_aspNetUser != null)
                        {
                            _aspNetUser.storeId = storeId;
                            dataContext.SubmitChanges();
                        }

                        var emailAddress = account.email_address.Replace("@", "_" + account.customer_id + "@");
                        if (!string.IsNullOrEmpty(newUserID))
                        {
                            string strMembershipCreateUser = "exec dbo.aspnet_Membership_CreateUser 'tradezero','" + account.customer_id + "','" + account.password + "','1234','" + emailAddress + "',NULL,NULL,1,'" + DateTime.Now.Date.ToString("yyyy-MM-dd HH:mm:ss") + "','" + DateTime.Now.Date.ToString("yyyy-MM-dd HH:mm:ss") + "',1,0,'" + newUserID + "'";
                            command.CommandText = strMembershipCreateUser;
                            command.Connection = conn;
                            command.CommandType = System.Data.CommandType.Text;
                            command.ExecuteNonQuery();
                        }
                        command.Dispose();
                        conn.Close();
                    }
                }
            }

        }


        public string getvisitorip()
        {
            string IP4Address = String.Empty;

            foreach (IPAddress IPA in Dns.GetHostAddresses(System.Web.HttpContext.Current.Request.UserHostAddress))
            {
                if (IPA.AddressFamily.ToString() == "InterNetwork")
                {
                    IP4Address = IPA.ToString();
                    break;
                }
            }

            if (IP4Address != String.Empty)
            {
                return IP4Address;
            }

            foreach (IPAddress IPA in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                if (IPA.AddressFamily.ToString() == "InterNetwork")
                {
                    IP4Address = IPA.ToString();
                    break;
                }
            }

            return IP4Address;
        }


    }
}