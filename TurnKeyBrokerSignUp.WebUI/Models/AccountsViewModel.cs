using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TurnKeyBrokerSignUp.WebUI.Models
{
    public class AccountsViewModel
    {
        public int account_id { get; set; }
        public string customer_id { get; set; }
        public string storeURI { get; set; }
        public string account_id_service { get; set; }
        public string email_address { get; set; }
        public string password { get; set; }
        public int account_type_id { get; set; }
        public string trading_type { get; set; }
        public string tax_id { get; set; }
        public int citizenship_country_id { get; set; }
        
       public DateTime date_created { get; set; }
        
       public bool active { get; set; }
        
       public bool deleted { get; set; }
        
      public bool Completed { get; set; }
        
      public DateTime last_logged_in { get; set; }
      public bool E_notification { get; set; }
        
     public string Affiliate_Type { get; set; }
      public Guid account_guid { get; set; }
    }
    public class AddressViewModel
    {
        public int address_id { get; set; }
        public int account_id { get; set; }

        public int account_type_id { get; set; }
        public string title { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string middle_name { get; set; }
        public string suffix { get; set; }
        public string address_1 { get; set; }
        public string address_2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zipcode { get; set; }
        public int country_id { get; set; }
        public bool is_address_last_one_year { get; set; }
        public int martial_status_id { get; set; }
        public string work_phone { get; set; }
        public string home_phone { get; set; }
        public string mobile_phone { get; set; }
        public string num_dependents { get; set; }
        public string email_address { get; set; }

        public string photo_id_path { get; set; }
        public bool is_coapplicant { get; set; }
        public string business_telephone { get; set; }
        public string fax { get; set; }
     

    }

    public class PatriotViewModel
    {
        public int patriot_id { get; set; }
        public int account_id { get; set; }


        public string date_of_birth { get; set; }
        public int identification_type_id { get; set; }

        public string identitication_number { get; set; }
        public string identification_expiration { get; set; }
        public string identitifcation_place_of_issue { get; set; }
        public string identification_issue_date { get; set; }

        public int residence_country_id { get; set; }
        public bool is_coapplicant { get; set; }


    }

    public class AffiliationViewModel
    {
        public int account_affiliation_id { get; set; }
        public int account_id { get; set; }

        public bool is_employed_security_industry { get; set; }
        public bool is_broker_dealer_security_dealer { get; set; }
        public bool is_investment_advisor { get; set; }
        public bool is_state_federal_regulator { get; set; }
        public bool is_FINRA { get; set; }

        public string entity_name { get; set; }
        public bool is_shareholder_public_company { get; set; }

        public string company_name { get; set; }
        public bool is_senior_govt_nonus { get; set; }
        public int senior_govt_nonus_country_id { get; set; }





    }

    public class FinancialViewModel
    {
        public int account_financial_id { get; set; }
        public int account_id { get; set; }


        public string annual_income { get; set; }


        public string tax_rate { get; set; }
        public string net_worth { get; set; }
        public string liquid_net_worth { get; set; }
        public string annual_expense { get; set; }

        public string special_expense { get; set; }

        public string special_expense_time { get; set; }


    }

    public class GoalViewModel
    {
        public int account_goal_id { get; set; }
        public int account_id { get; set; }


        public string investment_account_option { get; set; }


        public string account_future { get; set; }
        public string account_future_other { get; set; }
        public string earliest_funding_time_frame { get; set; }

    }

    public class Risk_toleranceViewModel
    {
        public int account_risk_tolerance_id { get; set; }
        public int account_id { get; set; }


        public string account_risk_tolerance { get; set; }



    }
    public class Financial_investmentViewModel
    {
        public int account_financial_investment_experience_id { get; set; }
        public int account_id { get; set; }


        public string year_mutual_funds { get; set; }


        public string number_mutual_funds { get; set; }
        public string year_investment_stocks { get; set; }
        public string number_investment_stocks { get; set; }

        public string year_bonds { get; set; }


        public string number_bonds { get; set; }
        public string year_options { get; set; }
        public string number_options { get; set; }

        public string year_social_securities { get; set; }


        public string number_social_securities { get; set; }
        public string year_annuties { get; set; }
        public string number_annuties { get; set; }

        public string year_alternatives { get; set; }
        public string number_alternatives { get; set; }
        public string year_margins { get; set; }

        public string number_margins { get; set; }


        public string decision_making_experience { get; set; }
        public string other_investment_type_1 { get; set; }
        public string other_investment_firm_1 { get; set; }

        public string other_investment_amount_1 { get; set; }
        public string other_investment_type_2 { get; set; }
        public string other_investment_type_3 { get; set; }

        public string other_investment_firm_2 { get; set; }


        public string other_investment_firm_3 { get; set; }
        public string other_investment_amount_3 { get; set; }
        public string other_investment_amount_2 { get; set; }
        
       public string average_size_per_trade { get; set; }
    }
    

    public class Margin_accountViewModel
    {
        public int account_margin_account_id { get; set; }
        public int account_id { get; set; }


        public bool is_margin_loan_agreement { get; set; }

        public bool is_borrow_funds_account { get; set; }

    }

    public class Managing_cashViewModel
    {
        public int account_managing_cash { get; set; }
        public int account_id { get; set; }

        public bool is_sales_mail_check_to_address { get; set; }
        public bool is_sales_send_check_to_bank_account { get; set; }

        public string sales_account_number { get; set; }
        public string sales_routing_number { get; set; }
        public bool is_sales_sweep_into_money_market { get; set; }
        public bool is_sales_sweep_into_market_deposit { get; set; }
        public bool is_sales_other { get; set; }

        public string sales_other { get; set; }
        public bool is_dividend_mail_check_to_address1 { get; set; }
        public bool is_dividend_send_check_to_bank_account1 { get; set; }
      
        public string dividend_account_number1 { get; set; }
        public string dividend_routing_number1 { get; set; }
        
        public bool is_dividend_sweep_into_money_market1 { get; set; }
        public bool is_dividend_sweep_into_market_deposit1 { get; set; }
        public bool is_dividend_other1 { get; set; }

        public string dividend_other1 { get; set; }
        public string sales_proceed { get; set; }
        public string dividend_proceed { get; set; }
        
        public string securities_proceed { get; set; }

        public string maintain_other_brokerage { get; set; }
        public string maintain_other_brokeerage_firms { get; set; }
    }

    public class Communication_choiceViewModel
    {
        public int account_communication_choice_id { get; set; }
        public int account_id { get; set; }

        public bool is_all_communication { get; set; }
        public bool is_all_communication_except { get; set; }
        
        public bool is_all_communication_except_tax { get; set; }

        public string communication_option { get; set; }
        public string communication_email { get; set; }
      
    }
}