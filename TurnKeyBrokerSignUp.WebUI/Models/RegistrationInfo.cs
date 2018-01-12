
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TurnKeyBrokerSignUp.WebUI.Models;
using System.Xml;


namespace TurnKeyBrokerSignUp.WebUI.Models
{
    public class RegistrationInfo
    {
        
        public int ProNonProType { get; set; }
       
        public string Regtype;
        public int StepNo;
        public List<int> EmploymentTypes;
        public List<int> officers;
        public int officerId;
        public int AccountType;
        public string TradingType;
        public string EmailAddress;
        public string Password;
        public string NewPassword;
        public string WorkPhone;
        public string Address;
        public string City;
        public string State;
        public string ZipCode;
        public string IdentificationFile;
        public string AddressProof;
        public int? CountryId;
        public string AccKey;
        public int CitizenShipCountryId;
        public int AccountId;
        public string CustomerId;
        public bool CoApplicantInformation;
        public string pageContent;
        public IQueryable<tb_account_type_lu> allAccountTypes;
        public IQueryable<tb_initial_lu> allInitials;
        public IQueryable<tb_suffix_lu> allSuffixs;
        public IQueryable<tb_state> allStates;
        public IQueryable<tb_PostType> allPostType;
        public IQueryable<tb_country> allCountries;
        public IQueryable<tb_martial_status_lu> allMartialStatus;
        public IQueryable<tb_employment_status_lu> allEmploymentStatus;
        public IQueryable<tb_identification_type_lu> allIdentificationTypes;
        public IQueryable<tb_setting> allAdminSettings;
        public IQueryable<tb_ContentManagement> allContentManagement;
        
        public List<tb_Compliance> complianceList;
        public string adminNotes;
        public List<Agreement> agreementList;
        public IQueryable<FundRequest> fundRequestList;
        
        public List<AgreementDetail> agreementDetailList;
        
        public List<Blogs> blogList;
       
        public List<HomeGraphic> homeGraphicList;
        public List<tb_ProNonProAgreement> prononproAgreement;
        
        
        public List<DemoAccountList> dAccountList;
        public FundRequest editFundStatus;
       
        public string SoftwareType;
        public string ZeroProIds;
        
        public string ZeroWebIds;
        public int? TotalCost;
        public int? FreeAfter;
        public int? ZeroWebFreeAfter;
        public int? ZeroWebTotal;
        
        
       
        
       
        public List<tb_AddOn> addOnSelected;
        public string AddOnId;
        
       
        public string Initials;
       
        public string AgreementName;
        public string AgreementId;
        
        public Tuple<int, int, int,int> AccountEasyOfac;
        public string customerName;
        public string message;
        public string firstName;
        public string lastName;
        public bool agreementsEnabled;
        public string searchFirstName;
        public string searchLastName;
        public string searchEmailAddress;
        public string searchAccountNumber;
        public string searchKeyword;
        public int unsafeAccountCount;
        public int searchAccountTypeId;
        public int NotSafe;
        public store_base activeStore;
        public string activeURI;
        public List<emailTemplate> emailTemplateList;
        public List<EmailLog> emailLogList;
        public emailTemplate editEmailTemplate;
        public tb_blog editBlog;
        public tb_PostNew editNews;
        public tb_HomeGraphic editHomeGraphic;
        public HomeGraphic objHomeGraphic;
        public tb_blogmeta blogsMeta;
        public bool IsSuperAdmin;
        public tb_account account;
        public tb_address address;
        public tb_patriot pat;
        public tb_employementstatus empl;
        public tb_affiliation affi;
        public tb_financial_investment tfi;
        public tb_margin_account agrd;

        public string registrationType;
        public string responseMessage;
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

    public class GraphicImages
    {
        public string Name { get; set; }
    }

    public class RegistrationList
    {
        public int id;
        public string firstName;
        public string lastName;
        public string accountNo;
        public string accountType;
        public string tradingType;
        public string emailAddres;
        public string country;
        public string contactPhone;
        public bool? isUnsafe;
        public DateTime dtCreated;
        public string agreementSigned;
        public string lastStep;
        public string passedOFAC;
        public string complianceNotes;
        public bool? accountApproved;
        public DateTime? dtApproved;
        public string ApprovedBy;
        public string webTraderMessage;
        public string photoIDPath;
        public string identificationFilePath;
        public string photoFilePath;
        public bool docSigned;
        public int isSigned;
        public bool resendEmail;
        public int accountId;
        public bool isCoApplicant;
        public string additionalFile;
        public bool? sharedOnFB;
        public bool? sharedOnGP;
        public bool? sharedOnTW;
        public bool? freeCredits;
        public bool? fundEmail;
        public bool? addFundsEmail;
        public string Affiliate_Type;
        public bool? creditReversal;


    }

    public class DemoAccountList
    {
        public int id;
        public string firstName;
        public string lastName;
        public string phoneNo;
        public string eMail;
        public string software;
        public string comments;
        public DateTime createDate;
        public bool reminder1;
        public bool reminder2;
        public bool isExpire;
        public string userName;
        public bool isDemo;
        public bool isActive;
        public string numOfTraders;
        public string numOfTrades;
        public string subject;
        public string hostAddress;
        public string requestType;
        public int countryId;
        public string password;
        public string fullName;
        public bool? sharedOnFB;
        public bool? sharedOnGP;
        public bool? sharedOnTW;
        public string countryName;
        public bool? demoExtended;
        public string affiliateType;
        public string traders;
        public string trades;
    }

    public class NewCountries
    {
        public string country_code { get; set; }
        public string country { get; set; }
    }
    

    public class NewSates
    {
        public int country_id { get; set; }
        public string state { get; set; }
    }


  
    

}