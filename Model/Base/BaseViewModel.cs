using Model.ApplicationDomainModels;
using Model.ViewModels;
using Model.ViewModels.SubmissionRule;
using Model.ViewModels.SystemSetting;

using System.Collections.Generic;

namespace Model.Base
{
    public class BaseViewModel
    {
        public string CurrentUserName { get; set; }
        public string GoogleRecaptchaResponse { get; set; }

        private MessageHandler _MessageHandler;
        public MessageHandler ActionMessageHandler
        {
            get
            {
                if (_MessageHandler == null)
                {
                    _MessageHandler = new MessageHandler();
                }

                return _MessageHandler;
            }
        }
        public string FormTitle { get; set; }
        public string CssClass { get; set; }

        public int LanguageId { get; set; }
        public LayoutViewModel Layout { get; set; }
        public Dictionary<string, string> LanguageDictionary { get; set; }
        public List<VmActiveLanguage> activeLanguageList;
        public string CurrentCultureName { get; set; }
        public string CurrentUserId { get; set; }
        public IEnumerable<string> CurrentUserRoles { get; set; }

        public string activeLanguageCommaSepatated;
        public bool MostSetWelcomeMessage = false;
        public bool UserEmailConfirmed;
        public string WelcomeMessage = "Welcome ";
        public string[] WelcomeMessageList =
            {
                "Hey ",
                "Good to see you here ",
                "It’s nice to have you here ",
                "How do you do ",
                "You make us SPECIAL just by being her ",
                "Smile, it's a wonderful start ",
                "It is a beautiful day, enjoy it ",
                "Let's just enjoy the moment ",
                "We hope you enjoy it here ",
                "We are happy to serve you "
            };
        public bool ReadOnly { get; set; }

        public BaseViewModel()
        {
        }
        public List<VmSystemSetting> SystemSettingList;

        public List<VmSubmissionRule> MenuSubmissionRuleList { get; set; }

        public string this[string refrenceWord]
        {
            get
            {
                try
                {
                    if (LanguageDictionary == null)
                        return refrenceWord;
                    return LanguageDictionary[refrenceWord];
                }
                catch
                {
                    return refrenceWord;
                }
            }
        }
    }
}