using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IranOtaku.Core.ViewModels
{
    public class SignUpAndSignInViewModel
    {
        

        #region Register
        [MaxLength(150, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        [Display(Name = "نام کاربری")]
        public string UserName { get; set; }


        [MaxLength(150, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        [EmailAddress(ErrorMessage = "لطفا ایمیل معتبری وارد کنید")]
        [Display(Name = "ایمیل")]
        public string Email { get; set; }

        [MaxLength(150, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        [MinLength(8,ErrorMessage = "رمز عبور باید حداقل هشت کاراکتر باشد")]
        [Display(Name = "رمز عبور")]
        [DataType(DataType.Password)]
        public string RegisterPassword { get; set; }
        [MaxLength(150, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        [Display(Name = "تکرار رمز عبور")]
        [DataType(DataType.Password)]
        [Compare(nameof(RegisterPassword), ErrorMessage = "رمز عبور با تکرار رمز عبور همخوانی ندارد")]
        public string ConfirmPassword { get; set; }

        #endregion

        #region Login
        [MaxLength(150, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        [Display(Name = "نام کاربری یا ایمیل")]
        public string UserNameOrEmail { get; set; }

        [MaxLength(150, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        [MinLength(8,ErrorMessage = "رمز عبور باید حداقل هشت کاراکتر باشد")]
        [Display(Name = "رمز عبور")]
        [DataType(DataType.Password)]
        public string LoginPassword { get; set; }

        [Display(Name = "منو یادت باشه")]
        public bool RememberMe { get; set; }

        public AuthenticationScheme ExternalLogin { get; set; }
        #endregion
    }
    public class ForgotPasswordViewModel
    {
        [MaxLength(150, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        [EmailAddress]
        [Display(Name = "ایمیل")]
        public string Email { get; set; }
    }
    public class ResetPasswordViewModel
    {
        [DataType(DataType.Password)]
        [Display(Name = "رمز عبور")]
        public string Password { get; set; }

        [Display(Name = "تکرار رمز عبور")]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }

        public string Email { get; set; }
        public string Token { get; set; }

    }

}
