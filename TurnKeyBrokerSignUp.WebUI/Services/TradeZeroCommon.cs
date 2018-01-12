
using System.Linq;
using TurnKeyBrokerSignUp.WebUI.Models;
using System.Collections.Generic;

namespace TurnKeyBrokerSignUp.WebUI.Services
{
    public class TradeZeroCommon
    {
        public string GetStepInformation(int accountId,
                                        List<tb_communication_choice> communicationChoices,
                                        List<tb_managing_cash> managingCashes,
                                        List<tb_margin_account> marginAccounts,
                                        List<tb_financial_investment> financialInvestments,
                                        List<tb_risk_tolerance> riskTolerances,
                                        List<tb_goal> goals,
                                        List<tb_funding> fundings,
                                        List<tb_financial> financials,
                                        List<tb_affiliation> affiliations,
                                        List<tb_patriot> patriots,
                                        List<tb_employementstatus> employmentStatus,
                                        List<tb_address> addresses,
                                        List<tb_account> accounts,
                                        List<tb_bank_ProfessionalLink> professionalLink

            )
        {

            string returnValue = "Initial";

            



            var sign_documents = (from s in accounts
                                  where s.account_id == accountId
                                  select s).FirstOrDefault();


            if(sign_documents!=null)
            {
                if (sign_documents.Completed == true)
                    return "Finished";
            }
            

            var sign_documents_info = (from s in professionalLink
                                       where s.AccountId.Equals(accountId)
                                       select s).FirstOrDefault();
            if (sign_documents_info != null)
                return "SIGN DOCS";

            var margin_account_status = (from s in marginAccounts
                                         where s.account_id == accountId
                                         select s).FirstOrDefault();

            if (margin_account_status != null)
                return "IDENTIFICATION PROCESS";



            var communication_choice_status = (from s in communicationChoices
                                               where s.account_id == accountId
                                               select s).FirstOrDefault();

            if (communication_choice_status != null)
                return "Finished";

            var managing_your_cash_status = (from s in managingCashes
                                             where s.account_id == accountId
                                             select s).FirstOrDefault();

            if (managing_your_cash_status != null)
                return "Margin Accounts";

            //var margin_account_status = (from s in dataContext.tb_margin_accounts
            //                             where s.account_id == accountId
            //                             select s).FirstOrDefault();

            //if (margin_account_status != null)
            //    return "Margin Accounts";

            var finacial_investment_experience_status = (from s in financialInvestments
                                                         where s.account_id == accountId
                                                         select s).FirstOrDefault();

            if (finacial_investment_experience_status != null)
                return "Financial Investment Experience";


            var risk_tolerance_status = (from s in riskTolerances
                                         where s.account_id == accountId
                                         select s).FirstOrDefault();

            if (risk_tolerance_status != null)
                return "Risk Tolerance";

            var goals_status = (from s in goals
                                where s.account_id == accountId
                                select s).FirstOrDefault();

            if (goals_status != null)
                return "Goals";

            var fundings_status = (from s in fundings
                                   where s.account_id == accountId
                                   select s).FirstOrDefault();

            if (fundings_status != null)
                return "Fundings";

            var financials_status = (from s in financials
                                     where s.account_id == accountId
                                     select s).FirstOrDefault();

            if (financials_status != null)
                return "Financials";


            var affiliations_status = (from s in affiliations
                                       where s.account_id == accountId
                                       select s).FirstOrDefault();

            if (affiliations_status != null)
                return "Affiliations";

            var patriot_act_status = (from s in patriots
                                      where s.account_id == accountId
                                      select s).FirstOrDefault();

            if (patriot_act_status != null)
                return "Patriot Act";


            var employment_status_status = (from s in employmentStatus
                                            where s.account_id == accountId
                                            select s).FirstOrDefault();

            if (employment_status_status != null)
                return "Employment Status";

            var applicant_info = (from s in addresses
                                  where s.account_id == accountId && s.address_type_id == 1
                                  select s).FirstOrDefault();

            if (applicant_info != null)
                return "Applicant Address";

            if (sign_documents != null)
                return "INITIAL";


            return returnValue;
        }





        public string GetStepInformation(TurnKeyBrokerSignUpDataContext dataContext, int accountId)
        {

            string returnValue = "Initial";

            if (dataContext == null)
                dataContext = new TurnKeyBrokerSignUpDataContext();



            var sign_documents = (from s in dataContext.tb_accounts
                                  where s.account_id == accountId
                                  select s).FirstOrDefault();



            if (sign_documents.Completed == true)
                return "Finished";

            var sign_documents_info = (from s in dataContext.tb_bank_ProfessionalLinks
                                       where s.AccountId.Equals(accountId)
                                       select s).FirstOrDefault();
            if (sign_documents_info != null)
                return "SIGN DOCS";

            var margin_account_status = (from s in dataContext.tb_margin_accounts
                                         where s.account_id == accountId
                                         select s).FirstOrDefault();

            if (margin_account_status != null)
                return "IDENTIFICATION PROCESS";



            var communication_choice_status = (from s in dataContext.tb_communication_choices
                                               where s.account_id == accountId
                                               select s).FirstOrDefault();

            if (communication_choice_status != null)
                return "Finished";

            var managing_your_cash_status = (from s in dataContext.tb_managing_cashes
                                             where s.account_id == accountId
                                             select s).FirstOrDefault();

            if (managing_your_cash_status != null)
                return "Margin Accounts";

            //var margin_account_status = (from s in dataContext.tb_margin_accounts
            //                             where s.account_id == accountId
            //                             select s).FirstOrDefault();

            //if (margin_account_status != null)
            //    return "Margin Accounts";

            var finacial_investment_experience_status = (from s in dataContext.tb_financial_investments
                                                         where s.account_id == accountId
                                                         select s).FirstOrDefault();

            if (finacial_investment_experience_status != null)
                return "Financial Investment Experience";


            var risk_tolerance_status = (from s in dataContext.tb_risk_tolerances
                                         where s.account_id == accountId
                                         select s).FirstOrDefault();

            if (risk_tolerance_status != null)
                return "Risk Tolerance";

            var goals_status = (from s in dataContext.tb_goals
                                where s.account_id == accountId
                                select s).FirstOrDefault();

            if (goals_status != null)
                return "Goals";

            var fundings_status = (from s in dataContext.tb_fundings
                                   where s.account_id == accountId
                                   select s).FirstOrDefault();

            if (fundings_status != null)
                return "Fundings";

            var financials_status = (from s in dataContext.tb_financials
                                     where s.account_id == accountId
                                     select s).FirstOrDefault();

            if (financials_status != null)
                return "Financials";


            var affiliations_status = (from s in dataContext.tb_affiliations
                                       where s.account_id == accountId
                                       select s).FirstOrDefault();

            if (affiliations_status != null)
                return "Affiliations";

            var patriot_act_status = (from s in dataContext.tb_patriots
                                      where s.account_id == accountId
                                      select s).FirstOrDefault();

            if (patriot_act_status != null)
                return "Patriot Act";


            var employment_status_status = (from s in dataContext.tb_employementstatus
                                            where s.account_id == accountId
                                            select s).FirstOrDefault();

            if (employment_status_status != null)
                return "Employment Status";

            var applicant_info = (from s in dataContext.tb_addresses
                                  where s.account_id == accountId && s.address_type_id == 1
                                  select s).FirstOrDefault();

            if (applicant_info != null)
                return "Applicant Address";

            if (sign_documents != null)
                return "INITIAL";

            return returnValue;
        }


        public string GetStepInformationInstitutional(TurnKeyBrokerSignUpDataContext dataContext, int accountId)
        {

            string returnValue = "Initial";

            if (dataContext == null)
                dataContext = new TurnKeyBrokerSignUpDataContext();



            var sign_documents = (from s in dataContext.tb_accounts
                                  where s.account_id == accountId
                                  select s).FirstOrDefault();



            if (sign_documents.Completed == true)
                return "Finished";

            var sign_documents_info = (from s in dataContext.tb_bank_ProfessionalLinks
                                       where s.AccountId.Equals(accountId)
                                       select s).FirstOrDefault();
            if (sign_documents_info != null)
                return "SIGN DOCS";

            var margin_account_status = (from s in dataContext.tb_margin_accounts
                                         where s.account_id == accountId
                                         select s).FirstOrDefault();

            if (margin_account_status != null)
                return "IDENTIFICATION PROCESS";



            var communication_choice_status = (from s in dataContext.tb_communication_choices
                                               where s.account_id == accountId
                                               select s).FirstOrDefault();

            if (communication_choice_status != null)
                return "Finished";

            var managing_your_cash_status = (from s in dataContext.tb_managing_cashes
                                             where s.account_id == accountId
                                             select s).FirstOrDefault();

            if (managing_your_cash_status != null)
                return "Margin Accounts";

            //var margin_account_status = (from s in dataContext.tb_margin_accounts
            //                             where s.account_id == accountId
            //                             select s).FirstOrDefault();

            //if (margin_account_status != null)
            //    return "Margin Accounts";

            var finacial_investment_experience_status = (from s in dataContext.tb_financial_investments
                                                         where s.account_id == accountId
                                                         select s).FirstOrDefault();

            if (finacial_investment_experience_status != null)
                return "Financial Investment Experience";


            var risk_tolerance_status = (from s in dataContext.tb_risk_tolerances
                                         where s.account_id == accountId
                                         select s).FirstOrDefault();

            if (risk_tolerance_status != null)
                return "Risk Tolerance";

            var goals_status = (from s in dataContext.tb_goals
                                where s.account_id == accountId
                                select s).FirstOrDefault();

            if (goals_status != null)
                return "Goals";

            var fundings_status = (from s in dataContext.tb_fundings
                                   where s.account_id == accountId
                                   select s).FirstOrDefault();

            if (fundings_status != null)
                return "Fundings";

            var financials_status = (from s in dataContext.tb_financials
                                     where s.account_id == accountId
                                     select s).FirstOrDefault();

            if (financials_status != null)
                return "Financials";


            var affiliations_status = (from s in dataContext.tb_affiliations
                                       where s.account_id == accountId
                                       select s).FirstOrDefault();

            if (affiliations_status != null)
                return "Affiliations";

            var patriot_act_status = (from s in dataContext.tb_patriots
                                      where s.account_id == accountId
                                      select s).FirstOrDefault();

            if (patriot_act_status != null)
                return "Patriot Act";


            var employment_status_status = (from s in dataContext.tb_employementstatus
                                            where s.account_id == accountId
                                            select s).FirstOrDefault();

            if (employment_status_status != null)
                return "Employment Status";

            var applicant_info = (from s in dataContext.tb_addresses
                                  where s.account_id == accountId && s.address_type_id == 1
                                  select s).FirstOrDefault();

            if (applicant_info != null)
                return "Applicant Address";

            if (sign_documents != null)
                return "INITIAL";

            return returnValue;
        }

        //public string GetStepInformation(TurnKeyBrokerSignUpDataContext dataContext, int accountId)
        //{

        //    string returnValue = "Initial";

        //    if (dataContext == null)
        //        dataContext = new TurnKeyBrokerSignUpDataContext();

        //    var sign_documents = (from s in dataContext.tb_accounts
        //                                 where s.account_id == accountId
        //                                 select s).FirstOrDefault();

        //    if (sign_documents.is_agreeement_signed != true)
        //        return "SIGN DOCS";

        //    var margin_account_status = (from s in dataContext.tb_margin_accounts
        //                                   where s.account_id == accountId
        //                                   select s).FirstOrDefault();

        //    if (margin_account_status != null)
        //        return "IDENTIFICATION PROCESS";



        //    var communication_choice_status = (from s in dataContext.tb_communication_choices
        //                                       where s.account_id == accountId
        //                                       select s).FirstOrDefault();

        //    if (communication_choice_status != null)
        //        return "Finished";

        //    var managing_your_cash_status = (from s in dataContext.tb_managing_cashes
        //                                     where s.account_id == accountId
        //                                     select s).FirstOrDefault();

        //    if (managing_your_cash_status != null)
        //        return "Margin Accounts";

        //    //var margin_account_status = (from s in dataContext.tb_margin_accounts
        //    //                             where s.account_id == accountId
        //    //                             select s).FirstOrDefault();

        //    //if (margin_account_status != null)
        //    //    return "Margin Accounts";

        //    var finacial_investment_experience_status = (from s in dataContext.tb_financial_investments
        //                                                 where s.account_id == accountId
        //                                                 select s).FirstOrDefault();

        //    if (finacial_investment_experience_status != null)
        //        return "Financial Investment Experience";


        //    var risk_tolerance_status = (from s in dataContext.tb_risk_tolerances
        //                                 where s.account_id == accountId
        //                                 select s).FirstOrDefault();

        //    if (risk_tolerance_status != null)
        //        return "Risk Tolerance";

        //    var goals_status = (from s in dataContext.tb_goals
        //                        where s.account_id == accountId
        //                        select s).FirstOrDefault();

        //    if (goals_status != null)
        //        return "Goals";

        //    var fundings_status = (from s in dataContext.tb_fundings
        //                           where s.account_id == accountId
        //                           select s).FirstOrDefault();

        //    if (fundings_status != null)
        //        return "Fundings";

        //    var financials_status = (from s in dataContext.tb_financials
        //                             where s.account_id == accountId
        //                             select s).FirstOrDefault();

        //    if (financials_status != null)
        //        return "Financials";


        //    var affiliations_status = (from s in dataContext.tb_affiliations
        //                               where s.account_id == accountId
        //                               select s).FirstOrDefault();

        //    if (affiliations_status != null)
        //        return "Affiliations";

        //    var patriot_act_status = (from s in dataContext.tb_patriots
        //                              where s.account_id == accountId
        //                              select s).FirstOrDefault();

        //    if (patriot_act_status != null)
        //        return "Patriot Act";


        //    var employment_status_status = (from s in dataContext.tb_employementstatus
        //                                    where s.account_id == accountId
        //                                    select s).FirstOrDefault();

        //    if (employment_status_status != null)
        //        return "Employment Status";

        //    var applicant_info = (from s in dataContext.tb_addresses
        //                          where s.account_id == accountId && s.address_type_id == 1
        //                          select s).FirstOrDefault();

        //    if (applicant_info != null)
        //        return "Applicant Address";

        //    return returnValue;
        //}
    }
}