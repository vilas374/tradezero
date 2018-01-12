using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TurnKeyBrokerSignUp.WebUI.Models
{
    public class SignProces
    {

        public string Regtype { get; set; }
        public int StepNo { get; set; }
        public List<int> EmploymentTypes { get; set; }
        public List<int> officers { get; set; }
        public int officerId { get; set; }
        public int AccountType { get; set; }
        public string TradingType { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string NewPassword { get; set; }
        public string WorkPhone { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string IdentificationFile { get; set; }
        public string AddressProof { get; set; }
        public int? CountryId { get; set; }
        public string AccKey { get; set; }
        public int CitizenShipCountryId { get; set; }
        public int AccountId { get; set; }
        public string CustomerId { get; set; }
        public bool CoApplicantInformation { get; set; }
        public string pageContent { get; set; }
        public IQueryable<tb_account_type_lu> allAccountTypes { get; set; }
        public IQueryable<tb_initial_lu> allInitials { get; set; }
        public IQueryable<tb_suffix_lu> allSuffixs { get; set; }
        public IQueryable<tb_state> allStates { get; set; }
        public IQueryable<tb_PostType> allPostType { get; set; }
        public IQueryable<tb_country> allCountries { get; set; }
        public IQueryable<tb_martial_status_lu> allMartialStatus { get; set; }
        public IQueryable<tb_employment_status_lu> allEmploymentStatus { get; set; }
        public IQueryable<tb_identification_type_lu> allIdentificationTypes { get; set; }
        public IQueryable<tb_setting> allAdminSettings { get; set; }
        public IQueryable<tb_ContentManagement> allContentManagement { get; set; }
        public List<tb_Compliance> complianceList { get; set; }
        public string adminNotes { get; set; }
        public List<Agreement> agreementList { get; set; }
        public IQueryable<FundRequest> fundRequestList { get; set; }

        public List<AgreementDetail> agreementDetailList { get; set; }

        public List<Blogs> blogList { get; set; }
        public List<HomeGraphic> homeGraphicList { get; set; }
        public List<tb_ProNonProAgreement> prononproAgreement { get; set; }


        public List<DemoAccountList> dAccountList { get; set; }
        public FundRequest editFundStatus { get; set; }

        public string SoftwareType { get; set; }
        public string ZeroProIds { get; set; }

        public string ZeroWebIds { get; set; }
        public int? TotalCost { get; set; }
        public int? FreeAfter { get; set; }
        public int? ZeroWebFreeAfter { get; set; }
        public int? ZeroWebTotal { get; set; }





        public List<tb_AddOn> addOnSelected;
        public string AddOnId;


        public string Initials;

        public string AgreementName { get; set; }
        public string AgreementId { get; set; }

        public Tuple<int, int, int, int> AccountEasyOfac;
        public string customerName { get; set; }
        public string message { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public bool agreementsEnabled { get; set; }
        public string searchFirstName { get; set; }
        public string searchLastName { get; set; }
        public string searchEmailAddress { get; set; }
        public string searchAccountNumber { get; set; }
        public string searchKeyword { get; set; }
        public int unsafeAccountCount { get; set; }
        public int searchAccountTypeId { get; set; }
        public int NotSafe { get; set; }
        public store_base activeStore { get; set; }
        public string activeURI { get; set; }
        public List<emailTemplate> emailTemplateList { get; set; }
        public List<EmailLog> emailLogList { get; set; }
        public emailTemplate editEmailTemplate { get; set; }
        public tb_blog editBlog { get; set; }
        public tb_PostNew editNews { get; set; }
        public tb_HomeGraphic editHomeGraphic { get; set; }
        public HomeGraphic objHomeGraphic { get; set; }
        public tb_blogmeta blogsMeta { get; set; }
        public bool IsSuperAdmin { get; set; }
        public tb_account account { get; set; }





        public string registrationType;
        public string responseMessage { get; set; }
        public List<GraphicImages> lstImages;
        public List<Blogs> popularBlogs;
        public string VirusDetected;
        public string InfetctedFileName;
        public bool SignupFinish;
        public string addRestrictEmailAddress;
        public int? ZeroProTotalCost;
        public int? ZeroProFreeAfter;
        public int? WebProTotalCost;
        public int? WebProFreeAfter;
    }
}