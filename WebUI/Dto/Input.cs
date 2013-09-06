using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;
using Omu.AwesomeMvc;
using dFrontierAppWizard.Core.Model;
using dFrontierAppWizard.Core.Service;
using dFrontierAppWizard.Resources;


namespace dFrontierAppWizard.WebUI.Dto
{
    public class Input
    {
        public int Id { get; set; }
    }

    public class AdCreateInput : Input
    {
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(Mui))]
        [UIHint("AjaxDropdown")]
        [Display(ResourceType = typeof(Mui), Name = "ApplicationName")]
        public int? ApplicationId { get; set; }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(Mui))]
        [StrLen(250)]
        [UIHint("TextArea")]
        [Display(ResourceType = typeof(Mui), Name = "WebSiteReference")]
        public string WebSiteReference { get; set; }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(Mui))]
        [StrLen(50)]
        [Display(ResourceType = typeof(Mui), Name = "Banner")]
        public string Banner { get; set; }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(Mui))]
        [Display(ResourceType = typeof(Mui), Name = "Start")]
        public DateTime? Start { get; set; }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(Mui))]
        [Display(ResourceType = typeof(Mui), Name = "End")]
        public DateTime? End { get; set; }


    }

    public class ModulesInput : Input
    {
      public string Code { get; set; }
      public string Description { get; set; }
    }


    public class ConsumerFeedbackInput : Input
    {
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(Mui))]
        [StrLen(500)]
        [AdditionalMetadata("DontSurround", true)]
        [UIHint("CKEditor")]
        public string Comments { get; set; }
        public string Status { get; set; }
        public DateTime DatePosted { get; set; }

        
    }

    public class FileUploadInput : Input
    {
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(Mui))]
        [StrLen(500)]
        public string FileName { get; set; }

    }

    public class FeedbackInput : Input
    {
        public FeedbackInput()
        {
            DatePosted = DateTime.UtcNow;
        }
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(Mui))]
        [StrLen(500)]
        [AdditionalMetadata("DontSurround", true)]
        [UIHint("CKEditor")]
        public string Comments { get; set; }

        public DateTime DatePosted { get; set; }
    }



    public class TermsAndConditionsInput : Input
    {
        [Display(ResourceType = typeof(Mui), Name = "Empty")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(Mui))]
        [StrLen(500)]
        [UIHint("TextArea")]
        public string Terms { get; set; }

        [Display(ResourceType = typeof(Mui), Name = "IHaveReadTheTermsAndConditions")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(Mui))]
        public bool IsAgreed { get; set; }
        
    }



    
    public class ApplicationFeedbackInput : Input
    {
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(Mui))]
        [StrLen(500)]
        [AdditionalMetadata("DontSurround", true)]
        [UIHint("CKEditor")]
        public string Comments { get; set; }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(Mui))]
        [StrLen(100)]
        [Display(ResourceType = typeof(Mui), Name = "Name")]
        public string Name { get; set; }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(Mui))]
        [StrLen(100)]
        [Display(ResourceType = typeof(Mui), Name = "Email")]
        public string Email { get; set; }


    }

    public class ApplicationInput : Input
    {
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(Mui))]
        [StrLen(100)]
        [Display(ResourceType = typeof(Mui), Name = "ApplicationName")]
        public string ApplicationName { get; set; }

        public int UserId { get; set; }
        public string AppStartUpMediaType { get; set; }
        public string AppLogo { get; set; }
        public string AppVideo { get; set; }
        public int AppLogoId { get; set; }
        public int AppVideoId { get; set; }
        public string MediaTypeId { get; set; }
        public string MediaLinkImage { get; set; }
        public string MediaLinkVideo { get; set; }
     //   [UIHint("AjaxDropdown")]
        public int MediaLinkImageId { get; set; }
     //   [UIHint("AjaxDropdown")]
        public int MediaLinkVideoId { get; set; }
        public int MainAddress { get; set; }
        public int LoyaltyGiftImageId { get; set; }
        [Display(ResourceType = typeof(Mui), Name = "Link1")]
        public string SocialMedia1Link { get; set; }

        public string SocialMedia1Image { get; set; }

        [Display(ResourceType = typeof(Mui), Name = "Link2")]
        public string SocialMedia2Link { get; set; }

        public string SocialMedia2Image { get; set; }

        [Display(ResourceType = typeof(Mui), Name = "Link3")]
        public string SocialMedia3Link { get; set; }

        public string SocialMedia3Image { get; set; }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(Mui))]
        [UIHint("TextArea")]
        [Display(ResourceType = typeof(Mui), Name = "VoucherMessage")]
        public string VoucherMessage { get; set; }

        public DateTime? VourcherMessageStart { get; set; }
        public DateTime? VourcherMessageEnd { get; set; }
        public string VoucherMessageFrequency { get; set; }
        [StrLen(200)]
        public string ApiKey { get; set; }
        [Display(ResourceType = typeof(Mui), Name = "TriggerRadius")]
        public Decimal VourhcerMessageTriggerPoint { get; set; }
        public string TermsAndConditions { get; set; }
        public bool IsLoyaltyVisible { get; set; }
        public bool IsFeedbackVisible { get; set; }
        public bool IsPaymentVisible { get; set; }
        public bool IsTermandConditionsAgreed { get; set; }
        public string LoyaltyTermsAndConditions { get; set; }
        public string LoyaltyGiftImage { get; set; }
        public string LoyaltyPassword { get; set; }
        public string FeedbackGiftImage { get; set; }
        public int FeedbackGiftImageId { get; set; }
        public string TabBackgroundColor { get; set; }
        public string TabFontColor { get; set; }
         [UIHint("AjaxDropdown")]
        public int TabFont { get; set; }

        [Display(ResourceType = typeof(Mui), Name = "TabFontSize")]
        [UIHint("AjaxDropdown")]
        public int TabFontSize { get; set; }

        [Display(ResourceType = typeof(Mui), Name = "BackGroundColor")]
        public string AppBackGroundColor { get; set; }

        [Display(ResourceType = typeof(Mui), Name = "MainTitleFontSize")]
        [UIHint("AjaxDropdown")]
        public int AppMainTitleFontSize { get; set; }

        [Display(ResourceType = typeof(Mui), Name = "MainTitleColor")]
        public string AppMainTitleColor { get; set; }

        [Display(ResourceType = typeof(Mui), Name = "MainTitleFont")]
        [UIHint("AjaxDropdown")]
        public int AppMainTitleFont { get; set; }

       
        [UIHint("AjaxDropdown")]
        public int AppHeaderFontSize { get; set; }
        [UIHint("AjaxDropdown")]
        public int AppHeaderFont { get; set; }
         [UIHint("AjaxDropdown")]
        public int AppDataFont { get; set; }
        [UIHint("AjaxDropdown")]
        public int AppDataFontSize { get; set; }
        public string AppButtonColor { get; set; }
        [UIHint("AjaxDropdown")]
        public int AppButtonFont { get; set; }
        [UIHint("AjaxDropdown")]
        public int AppButtonFontSize { get; set; }
        public string AppTextBoxColor { get; set; }
        [UIHint("AjaxDropdown")]
        public int AppTextBoxFontSize { get; set; }
        [UIHint("AjaxDropdown")]
        public int AppTextBoxFont { get; set; }
        public int TriggerRadiusId { get; set; }
    }

    public class PushNotificationRegUserInput : Input
    {
        public int UserId { get; set; }
        public string DeviceId { get; set; }
        public DateTime CreatedTime { get; set; }
    }

    public class IndustryInput : Input
    {
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(Mui))]
        [StrLen(50)]
        [Display(ResourceType = typeof(Mui), Name = "Description")]
        public string Description { get; set; }

    }

    public class OrderInput : Input
    {
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(Mui))]
        public int ConsumerId { get; set; }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(Mui))]
        public int PromotionId { get; set; }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(Mui))]
        public int TransactionId { get; set; }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(Mui))]
        public DateTime DateOfOrder { get; set; }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(Mui))]
        public int Quantity { get; set; }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(Mui))]
        public Decimal Total { get; set; }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(Mui))]
        public DateTime DateCreated { get; set; }
    }

    public class TransactionInput : Input
    {
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(Mui))]
        public int PaymentModeId { get; set; }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(Mui))]
        public DateTime DateofTransaction { get; set; }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(Mui))]
        public string PaymentReference { get; set; }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(Mui))]
        public Decimal TotalAmount { get; set; }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(Mui))]
        public Decimal DiscountOfTotalPercent { get; set; }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(Mui))]
        public Decimal FinalAmount { get; set; }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(Mui))]
        public string Status { get; set; }

 
    }

    
    public class LoyaltyInput : Input
    {
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(Mui))]
        [StrLen(200)]
        [Display(ResourceType = typeof(Mui), Name = "CaptureLoyaltyImage")]
        public string LoyaltyImage { get; set; }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(Mui))]
        [UIHint("TextArea")]
        [Display(ResourceType = typeof(Mui), Name = "LoyaltyTermsAndConditions")]
        public string LoyaltyTermsAndConditions { get; set; }

    }

    public class RetailerLinksInput : Input
    {
   
        [StrLen(15)]
        public string LinkType { get; set; }

        [StrLen(250)]
        public string VideoOrImageLink { get; set; }

        [StrLen(250)]
        public string SocialNetworkLink1 { get; set; }

        [StrLen(250)]
        public string SocialNetworkLink2{ get; set; }

        [StrLen(250)]
        public string SocialNetworkLink3 { get; set; }

         [UIHint("TextArea")]
        public string Location { get; set; }
    }

    public class PushNotificationInput : Input
    {
        [UIHint("TextArea")]
        public string VoucherMessage { get; set; }

        [StrLen(20)]
        public string StartDate { get; set; }

        [StrLen(20)]
        public string EndDate { get; set; }

        [StrLen(20)]
        public string Frequency { get; set; }

        [StrLen(20)]
        public string TriggerPoint { get; set; }

    } 

    public class UserCreateInput : Input
    {
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(Mui))]
        [StrLen(15)]
        [LoginUnique]
        public string Login { get; set; }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(Mui))]
        [StrLen(20)]
        [UIHint("password")]
        public string Password { get; set; }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(Mui))]
        [StringLength(50, ErrorMessageResourceName = "strlen", ErrorMessageResourceType = typeof(Mui), MinimumLength = 6)]
        [UIHint("password")]
        [System.Web.Mvc.Compare("Password")]
        [Display(ResourceType = typeof(Mui), Name = "Confirm_Password")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(Mui))]
        [StrLen(100)]
        [Display(ResourceType = typeof(Mui), Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(Mui))]
        [StrLen(50)]
        [Display(ResourceType = typeof(Mui), Name = "Company")]
        public string Company { get; set; }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(Mui))]
        [StrLen(20)]
        [Display(ResourceType = typeof(Mui), Name = "Phone")]
        public string Phone { get; set; }
        
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(Mui))]
        [StrLen(100)]
        [System.Web.Mvc.Compare("Email")]
        [Display(ResourceType = typeof(Mui), Name = "ConfirmEmail")]
        public string ConfirmEmail { get; set; }

 
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(Mui))]
        [Display(ResourceType = typeof(Mui), Name = "TermsOfService")]
        public bool TermsOfService { get; set; }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(Mui))]
        [UIHint("MultiLookup")]
        public IEnumerable<int> Industries { get; set; }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(Mui))]
        [UIHint("MultiLookup")]
        public IEnumerable<int> Roles { get; set; }

        [Display(ResourceType = typeof(Mui), Name = "Address")]
        [UIHint("TextArea")]
        public string Address { get; set; }
    }

    public class ConsumerInput : Input
    {
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(Mui))]
        [StrLen(50)]
        public string Name { get; set; }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(Mui))]
        public int Age { get; set; }

        [Display(ResourceType = typeof(Mui), Name = "Gender")]
        public char Gender { get; set; }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(Mui))]
        [Display(ResourceType = typeof(Mui), Name = "Date")]
        [AdditionalMetadata("Placeholder", "please pick date")]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(Mui))]
        [StrLen(100)]
        [Display(ResourceType = typeof(Mui), Name = "Phone")]
        public string Phone { get; set; }
        
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(Mui))]
        [StrLen(100)]
        [Display(ResourceType = typeof(Mui), Name = "Email")]
        public string Email { get; set; }
        
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(Mui))]
        [UIHint("TextArea")]
        public string Address { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

    }

    public class PaymentInput : Input
    {
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(Mui))]
        [Display(ResourceType = typeof(Mui), Name = "NameOnCard")]
        [StrLen(30)]
        public string NameOnCard { get; set; }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(Mui))]
        [Display(ResourceType = typeof(Mui), Name = "CreditCardNumber")]
        [StrLen(16)]
        public string CreditCardNumber { get; set; }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(Mui))]
        [Display(ResourceType = typeof(Mui), Name = "SecurityCode")]
        [StrLen(8)]
        public string SecurityCode { get; set; }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(Mui))]
        [Display(ResourceType = typeof(Mui), Name = "ExpiryMonth")]
        [StrLen(3)]
        public string ExpiryMonth{ get; set; }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(Mui))]
        [Display(ResourceType = typeof(Mui), Name = "ExpiryYear")]
        [StrLen(4)]
        public string ExpiryYear { get; set; }

    }

    public class UserEditInput : Input
    {
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(Mui))]
        [UIHint("MultiLookup")]
        public IEnumerable<int> Roles { get; set; }
    }

    public class MediaDocumentInput : Input
    {
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(Mui))]
        [Display(ResourceType = typeof(Mui), Name = "DocName")]
        public string DocName { get; set; }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(Mui))]
        [Display(ResourceType = typeof(Mui), Name = "DateCreated")]
        public DateTime DateCreated { get; set; }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(Mui))]
        [Display(ResourceType = typeof(Mui), Name = "MediaType")]
        public string MediaType { get; set; }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(Mui))]
        [Display(ResourceType = typeof(Mui), Name = "Module")]
        public string Module { get; set; }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(Mui))]
        [Display(ResourceType = typeof(Mui), Name = "Application")]
        public int ApplicationId { get; set; }
    }




    public class FrequencyInput : Input
    {
        public int ApplicationId { get; set; }
        public string Description { get; set; }
    }

    public class UserBillingInformationEditInput : Input
    {
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(Mui))]
        [UIHint("MultiLookup")]
        public IEnumerable<int> Roles { get; set; }
    }

    public class ChangePasswordInput : Input
    {
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(Mui))]
        [StrLen(20)]
        [UIHint("password")]
        public string Password { get; set; }
    }

    public class CountryInput : Input
    {
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(Mui))]
        [StrLen(20)]
        [Display(ResourceType = typeof(Mui), Name = "Name")]
        public string Name { get; set; }
    }

    public class SignInInput
    {
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(Mui))]
        // [Display(ResourceType = typeof(Mui), Name = "Login")]
        public string Login { get; set; }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(Mui))]
        [UIHint("Password")]
        //   [Display(ResourceType = typeof(Mui), Name = "Password")]
        public string Password { get; set; }

        [Display(ResourceType = typeof(Mui), Name = "Remember")]
        public bool Remember { get; set; }
    }

    public class BranchInput : Input
    {
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(Mui))]
        [StrLen(15)]
        [Display(ResourceType = typeof(Mui), Name = "Branch")]
        public string BranchName { get; set; }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(Mui))]
        [StrLen(250)]
        [Display(ResourceType = typeof(Mui), Name = "Address")]
        public string Address { get; set; }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(Mui))]
        [UIHint("AjaxDropdown")]
        [Display(ResourceType = typeof(Mui), Name = "Country")]
        public int CountryId { get; set; }

        public int UserId { get; set; }

        [Display(ResourceType = typeof(Mui), Name = "ZipCode")]
        public string ZipCode { get; set; }
    }

    
    public class PaymentGatewayInput : Input
    {
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(Mui))]
        [StrLen(150)]
        [Display(ResourceType = typeof(Mui), Name = "ApiLogin")]
        public string ApiLogin { get; set; }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(Mui))]
        [StrLen(150)]
        [Display(ResourceType = typeof(Mui), Name = "TransactionKey")]
        public string TransactionKey { get; set; }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(Mui))]
        [StrLen(150)]
        [Display(ResourceType = typeof(Mui), Name = "Type")]
        public string Type { get; set; }
        
        public int UserId { get; set; }
    }

    public class LeadInput : Input
    {
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(Mui))]
        [StrLen(50)]
        [Display(ResourceType = typeof(Mui), Name = "First_Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(Mui))]
        [StrLen(50)]
        [Display(ResourceType = typeof(Mui), Name = "Last_Name")]
        public string LastName { get; set; }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(Mui))]
        [StrLen(50)]
        [Display(ResourceType = typeof(Mui), Name = "Email")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(Mui))]
        [StrLen(50)]
        [Display(ResourceType = typeof(Mui), Name = "Address")]
        public string Address { get; set; }
        
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(Mui))]
        [StrLen(50)]
        [Display(ResourceType = typeof(Mui), Name = "ReferredBy")]
        public string ReferredBy { get; set; }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(Mui))]
        [StrLen(50)]
        [Display(ResourceType = typeof(Mui), Name = "Mobile")]
        public string Mobile { get; set; }
       
    }

    public class ProductCategoryInput : Input
    {
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(Mui))]
        [StrLen(15)]
        [Display(ResourceType = typeof(Mui), Name = "Description")]
        public string Description { get; set; }

        public int UserId { get; set; }

    }

    public class NotificationInput : Input
    {
        public int ApplicationId { get; set; }
        public int BranchId { get; set; }
        public string MessageTitle { get; set; }
        public string Message { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsActive { get; set; }

    }

    public class ProductInput : Input
    {
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(Mui))]
        [StrLen(50)]
        [Display(ResourceType = typeof(Mui), Name = "Name")]
        public string Name { get; set; }

        [Display(ResourceType = typeof(Mui), Name = "Description")]
        [UIHint("TextArea")]
        public string Description { get; set; }
        
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(Mui))]
        [Display(ResourceType = typeof(Mui), Name = "Price")]
        public Decimal Price { get; set; }

        public int UserId { get; set; }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(Mui))]
        [UIHint("AjaxDropdown")]
        [Display(ResourceType = typeof(Mui), Name = "Category")]
        public int ProductCategoryId { get; set; }
        
    }

    public class UserBillingInformationInput : Input
    {
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(Mui))]
        public int UserId { get; set; }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(Mui))]
        [StrLen(50)]
        [Display(ResourceType = typeof(Mui), Name = "First_Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(Mui))]
        [StrLen(50)]
        [Display(ResourceType = typeof(Mui), Name = "Last_Name")]
        public string LastName { get; set; }

        [Display(ResourceType = typeof(Mui), Name = "BillingContactEmail")]
        [StrLen(50)]
        public string BillingContactEmail { get; set; }

        [Display(ResourceType = typeof(Mui), Name = "BillingAddress")]
        [UIHint("TextArea")]
        public string BillingAddress { get; set; }

        [Display(ResourceType = typeof(Mui), Name = "Country")]
        public string Country { get; set; }

        [Display(ResourceType = typeof(Mui), Name = "City")]
        public string City { get; set; }

        [Display(ResourceType = typeof(Mui), Name = "BillingZipCode")]
        [StrLen(50)]
        public string BillingZipCode { get; set; }

        [Display(ResourceType = typeof(Mui), Name = "BillingContactNumber")]
        [StrLen(50)]
        public string BillingContactNumber { get; set; }

        [Display(ResourceType = typeof(Mui), Name = "CreditCardNumber")]
        [StrLen(50)]
        public string CreditCardNumber { get; set; }

        [Display(ResourceType = typeof(Mui), Name = "CardCode")]
        [StrLen(3)]
        public string CardCode { get; set; }

        [Display(ResourceType = typeof(Mui), Name = "ExpiryDate")]
        [StrLen(10)]
        public string ExpiryDate { get; set; }

        [Display(ResourceType = typeof(Mui), Name = "Subscription")]
        [StrLen(20)]
        public string Subscription { get; set; }
    }

    public class ConsumerBillingInformationInput : Input
    {
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(Mui))]
        public int ConsumerId { get; set; }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(Mui))]
        [StrLen(50)]
        [Display(ResourceType = typeof(Mui), Name = "First_Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(Mui))]
        [StrLen(50)]
        [Display(ResourceType = typeof(Mui), Name = "Last_Name")]
        public string LastName { get; set; }

        [Display(ResourceType = typeof(Mui), Name = "BillingContactEmail")]
        [StrLen(50)]
        public string BillingContactEmail { get; set; }

        [Display(ResourceType = typeof(Mui), Name = "BillingAddress")]
        [UIHint("TextArea")]
        public string BillingAddress { get; set; }

      

        [Display(ResourceType = typeof(Mui), Name = "BillingZipCode")]
        [StrLen(50)]
        public string BillingZipCode { get; set; }

        [Display(ResourceType = typeof(Mui), Name = "BillingContactNumber")]
        [StrLen(50)]
        public string BillingContactNumber { get; set; }

        [Display(ResourceType = typeof(Mui), Name = "CreditCardNumber")]
        [StrLen(50)]
        public string CreditCardNumber { get; set; }

        [Display(ResourceType = typeof(Mui), Name = "CardCode")]
        [StrLen(3)]
        public string CardCode { get; set; }

        [Display(ResourceType = typeof(Mui), Name = "ExpiryDate")]
        [StrLen(10)]
        public string ExpiryDate { get; set; }

        [Display(ResourceType = typeof(Mui), Name = "Subscription")]
        [StrLen(20)]
        public string Subscription { get; set; }

         
    }
    public class ConsumerBillingInformationEditInput : Input
    {
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(Mui))]
        [UIHint("MultiLookup")]
        public IEnumerable<int> Roles { get; set; }
    }



    public class PromotionInput : Input
    {
        private DateTime _date = DateTime.Now;
        
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(Mui))]
        [Display(ResourceType = typeof(Mui), Name = "Start")]
        public DateTime Start
        {
            get { return _date; }
            set { _date = value; }
        }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(Mui))]
        [Display(ResourceType = typeof(Mui), Name = "End")]
        public DateTime End
        {
            get { return _date; }
            set { _date = value; }
        }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(Mui))]
        [UIHint("MultiLookup")]
        [AdditionalMetadata("DragAndDrop",true)]
        [AdditionalMetadata("PopupClass","productsLookup")]
        [MultiLookup(Fullscreen = false)]
        [Display(ResourceType = typeof(Mui), Name = "Products")]
        public IEnumerable<int> Products { get; set; }
        
        [Display(ResourceType = typeof(Mui), Name = "Price")]
        public Decimal Price { get; set; }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(Mui))]
        [Display(ResourceType = typeof(Mui), Name = "Description")]
        [UIHint("TextArea")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(Mui), Name = "DiscountPercentage")]
        public int DiscountPercentage { get; set; }

        [Display(ResourceType = typeof(Mui), Name = "OriginalPrice")]
        public Decimal OriginalPrice { get; set; }

        public bool IsActive { get; set; }
        public bool IsPublished { get; set; }
    }

    public class DelBtn
    {
        public int Id { get; set; }
        public string Controller { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class CropInput
    {
        public int ImageHeight { get; set; }
        public int ImageWidth { get; set; }
        public int Id { get; set; }
        public string FileName { get; set; }
    }
}