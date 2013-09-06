using System;
using System.Collections.Generic;

namespace dFrontierAppWizard.Core.Model
{
    public class Application : DelEntity
    {
      public string ApplicationName { get; set; }
      public int UserId { get; set; }
      public int MainAddress { get; set; }
      public string MediaTypeId { get; set; }
      public string AppStartUpMediaType { get; set; }
      public string AppLogo { get; set; }
      public string AppVideo { get; set; }
      public int AppLogoId { get; set; }
      public int AppVideoId { get; set; }
      public int MediaLinkImageId { get; set; }
      public int MediaLinkVideoId { get; set; }
      public string MediaLinkImage { get; set; }
      public string MediaLinkVideo { get; set; }
      public string SocialMedia1Link { get; set; }
      public string SocialMedia1Image { get; set; }
      public string SocialMedia2Link { get; set; }
      public string SocialMedia2Image { get; set; }
      public string SocialMedia3Link { get; set; }
      public string SocialMedia3Image { get; set; }
      public string VoucherMessage { get; set; }
      public DateTime? VourcherMessageStart { get; set; }
      public DateTime?  VourcherMessageEnd { get; set; }
      public string  VoucherMessageFrequency { get; set; }
      public Decimal VourhcerMessageTriggerPoint { get; set; }
      public string ApiKey { get; set; }
      public string TermsAndConditions { get; set; }
      public bool IsLoyaltyVisible { get; set; }
      public string LoyaltyTermsAndConditions { get; set; }
      public string FeedbackGiftImage { get; set; }
      public bool IsFeedbackVisible { get; set; }
      public bool IsPaymentVisible { get; set; }
      public bool IsTermandConditionsAgreed { get; set; }
      public string LoyaltyGiftImage { get; set; }
      public string LoyaltyPassword { get; set; }
      public string TabBackgroundColor { get; set; }
      public string TabFontColor { get; set; }
      public int TabFont { get; set; }
      public int TabFontSize { get; set; }
      public string AppBackGroundColor { get; set; }
      public int AppMainTitleFontSize { get; set; }
      public string AppMainTitleColor { get; set; }
      public int AppMainTitleFont { get; set; }
      public int AppHeaderFontSize { get; set; }
      public int AppHeaderFont { get; set; }
      public int AppDataFont { get; set; }
      public int AppDataFontSize { get; set; }

        public string AppButtonColor { get; set; }
        public int AppButtonFont { get; set; }
        public int AppButtonFontSize { get; set; }
        public string AppTextBoxColor { get; set; }
        public int AppTextBoxFontSize { get; set; }
        public int AppTextBoxFont { get; set; }
        public int LoyaltyGiftImageId { get; set; }
        public int FeedbackGiftImageId { get; set; }
        public int TriggerRadiusId { get; set; }
        
    }
}
