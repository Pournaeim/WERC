using Model.Base;
using Model.ViewModels.SystemSetting;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace WERC.Models
{
    public class ExternalLoginConfirmationViewModel : BaseViewModel
    {
        [Required]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel : BaseViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        public string Email { get; set; }
    }

    public class LoginViewModel : BaseViewModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string Url { get; set; }

        public bool RememberMe { get; set; }
        public bool ActiveRegisteration { get; internal set; }
    }

    public class RegisterViewModel : BaseViewModel
    {

        public string UserName { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        //[DataType(DataType.PhoneNumber)]
        //[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string RoleName { get; set; }
        public string ReturnUrl { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> UniversityList { get; set; }
        public int UniversityId { get; set; }
        public bool Sex { get; set; }
        public string WorkPhoneNumber { get; internal set; }
    }

    public class ResetPasswordViewModel : BaseViewModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel : BaseViewModel
    {
        //[Required]
        //[EmailAddress]
        //public string Email { get; set; }

        [Required]
        public string UserName { get; set; }
        public ForgotPasswordViewModel()
        { }
        public ForgotPasswordViewModel(string message)
        {
            Message = message;
        }

        public string Message { get; set; }
    }
    public class LoginPartialViewModel : BaseViewModel
    {
        public string UserName { get; set; }
        public LoginPartialViewModel()
        {

            UserName = HttpContext.Current.User.Identity.Name;

            //if (UserName.Contains("@"))
            //{
            //    UserName = UserName.Split('@')[0];
            //}
        }
    }
    public class ForgotPasswordConfirmationViewModel : BaseViewModel
    {

    }
}