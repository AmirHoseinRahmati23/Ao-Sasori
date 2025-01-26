using IranOtaku.Core.Utilities;
using IranOtaku.Core.ViewModels;
using IranOtaku.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ZibalIPGRequests;
using FluentFTP;
using System.Threading;
using IranOtaku.Core.UnitOfWork.Services;
using IranOtaku.Core.UnitOfWork.Repositories;
using System.Security.Claims;

namespace IranOtaku.Web.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private IEmailSender _sender;
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IEmailSender sender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _sender = sender;
        }


        [Route("Register")]
        public async Task<IActionResult> RegisterAndLogin(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(returnUrl)) ViewData["returnURL"] = returnUrl;

            var model = new SignUpAndSignInViewModel()
            {
                ExternalLogin = (await _signInManager.GetExternalAuthenticationSchemesAsync()).First()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(SignUpAndSignInViewModel sign)
        {
            ViewBag.Message = "نکته: رمز عبور باید حداقل 8 حرف و دارای حداقل 1 عدد باشد";
            #region Validation
            if (!ModelState.IsValid) return View("RegisterAndLogin", sign);
            if (string.IsNullOrEmpty(sign.UserName) || string.IsNullOrEmpty(sign.Email))
            {
                ModelState.AddModelError("", "لطفا نام کاربری و ایمیل را وارد کنید");
                return View("RegisterAndLogin", sign);
            }
            if (string.IsNullOrEmpty(sign.RegisterPassword) || string.IsNullOrEmpty(sign.ConfirmPassword))
            {
                ModelState.AddModelError("", "رمز عبور و تکرار رمز عبور را وارد کنید");
                return View("RegisterAndLogin", sign);
            }
            #endregion


            sign.ExternalLogin = (await _signInManager.GetExternalAuthenticationSchemesAsync()).First();
            var registeredUser = await _userManager.FindByEmailAsync(sign.Email) ??
                await _userManager.FindByNameAsync(sign.UserName);

            if(registeredUser != null)
            {
                ViewBag.Message += "حساب کاربری با این نام کاربری یا ایمیل از قبل موجود است";
                return View("RegisterAndLogin", sign);
            }
            #region Register
            //Login User
            var user = new User()
            {
                UserName = sign.UserName,
                Email = sign.Email,
                EmailConfirmed = true
            };
            var result = await _userManager.CreateAsync(user, sign.RegisterPassword);

            // Show Errors(if errors exist)
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("" , error.Description);
                }
                ViewBag.Message = "توجه کنید : رمز عبور باید حداقل 8 حرف و دارای حرف و عدد باشه";
                return View("RegisterAndLogin", sign);
            }

            var roleResult = await _userManager.AddToRoleAsync(user, "User");

            if (!roleResult.Succeeded)
            {
                foreach (var error in roleResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View("RegisterAndLogin", sign);
            }
            #endregion


            //Everything is Ok :)       Go To Success Page
            return RedirectToAction("SuccessRegister");
        }
        [HttpGet]
        public IActionResult SuccessRegister()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(SignUpAndSignInViewModel login, string returnUrl = null)
        {
            ViewBag.Message = " ";
            login.ExternalLogin = (await _signInManager.GetExternalAuthenticationSchemesAsync()).First();
            #region Validation
            if (!ModelState.IsValid) return View("RegisterAndLogin", login);
            if (string.IsNullOrEmpty(login.UserNameOrEmail) || string.IsNullOrEmpty(login.LoginPassword))
            {
                ModelState.AddModelError("", "لطفا نام کاربری/ایمیل و رمز عبور را وارد کنید");
                return View("RegisterAndLogin", login);
            }
            #endregion

            #region Login

            //get user by UserName OR Email
            var user = await _userManager.FindByEmailAsync(login.UserNameOrEmail) ??
                await _userManager.FindByNameAsync(login.UserNameOrEmail);
            if (user == null)
            {
                ModelState.AddModelError("", "لطفا اطلاعات معتبر وارد کنید");
                return View("RegisterAndLogin", login);
            }

            //Sign in User
            var result = await
                _signInManager.PasswordSignInAsync(user, login.LoginPassword, login.RememberMe, true);

            // return Error if Login Failed
            if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "به دلیل پنج بار اشتباه وارد کردن رمز عبور حساب کاربری شما به مدت پنج دقیقه قفل شد");
                return View("RegisterAndLogin", login);
            }
            if (!result.Succeeded)
            {
                return View("RegisterAndLogin", login);
            }
            #endregion


            //Everything is OK :)     return to HOME PAGE Or RETURN URL
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Index", "Home");

        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult ExternalLogin(string provider)
        {
            var returnUrl = Url.Action("ExternalLoginCallback", "Account");

            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider , returnUrl);

            return new ChallengeResult(provider, properties);
        }

        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null,string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");


            if(remoteError != null)
            {
                ViewBag.Message = $"مشکلی به وجود آمد: {remoteError}";
                return RedirectToAction(nameof(RegisterAndLogin));
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if(info == null)
            {
                ViewBag.Message = $"مشکلی به وجود آمد";
                return RedirectToAction(nameof(RegisterAndLogin));
            }

            var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, true);
            if (!signInResult.Succeeded)
            {
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);

                if (email != null)
                {
                    var user = await _userManager.FindByEmailAsync(email);

                    if (user == null)
                    {
                        user = new User()
                        {
                            UserName = info.Principal.FindFirstValue(ClaimTypes.Email),
                            Email = info.Principal.FindFirstValue(ClaimTypes.Email),
                            Wallet = 0
                        };

                        await _userManager.CreateAsync(user);

                        await _userManager.AddToRoleAsync(user, "User");
                    }

                    await _userManager.AddLoginAsync(user, info);
                    await _signInManager.SignInAsync(user, true);
                }
            }

            return LocalRedirect(returnUrl);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangeData(string firstName, string lastName, IFormFile imageFile)
        {

            var user = await _userManager.GetUserAsync(User);

            #region Delete And Add Image File


            if (!string.IsNullOrEmpty(user.Image) && imageFile != null)
            {
                var directoryPath = Directory.GetCurrentDirectory() + "/wwwroot" +
                       "/UserImages" + "/" + user.Image;

                System.IO.File.Delete(directoryPath);
            }

            if (imageFile != null)
            {
                var fileName = NameGenerator.CreateName() + Path.GetExtension(imageFile.FileName);
                var path = Directory.GetCurrentDirectory() + "/wwwroot" +
                "/UserImages" + "/" + fileName;
                using (var stream = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    await imageFile.CopyToAsync(stream);
                }


                using (var client = new FtpClient("171.22.24.61", "pz13195", "x7JdBaZH"))
                {
                    var token = new CancellationToken();
                    await client.ConnectAsync(token);

                    var result = await client
                        .UploadFileAsync(path, "/public_html/UserImages/" + fileName, FtpRemoteExists.NoCheck, true);

                    if (result == FtpStatus.Failed)
                    {
                        System.IO.File.Delete(path);
                        return RedirectToAction(nameof(Index), "UserPanel");
                    }
                    user.Image = fileName;
                }

                System.IO.File.Delete(path);
            }

            #endregion



            if (!string.IsNullOrEmpty(firstName)) user.Name = firstName;
            if (!string.IsNullOrEmpty(lastName)) user.Family = lastName;

            await _userManager.UpdateAsync(user);

            return RedirectToAction(nameof(Index), "UserPanel");
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var link = Url.Action(nameof(ResetPassword), "Account",
                    new { email = user.Email, token = token }, Request.Scheme);

                string emailText
                    = $" سلام کاربر گرامی، برای تغییر رمز عبور خود به <a href=\" {link} \"> این لینک</a> مراجعه کنید";


                await _sender.SendEmailAsync(user.Email, "فراموشی رمز عبور", emailText, true);

                ViewBag.Message = "ایمیل تغییر رمز برای شما ارسال شد";
            }
            else
            {
                ViewBag.Message = "عملیات با شکست مواجه شد، لطفا از درستی ایمیل خود اطمینان حاصل کنید";
            }

            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword(string email , string token)
        {
            if(string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
            {
                return NotFound();
            }

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if(user != null)
                {
                    var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);

                    if(result.Succeeded)
                    {
                        ViewBag.Message = "تغییر رمز با موفقیت انجام شد";
                        return RedirectToAction(nameof(RegisterAndLogin));
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            return View(model);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Charge()
        {
            ViewBag.Error = false;
            return View();
        }


        [Authorize]
        [HttpPost]
        public IActionResult Charge(int amount)
        {
            if (!(amount >= 1000))
            {
                ViewBag.Error = true;
                return View();
            }

            string url = "https://gateway.zibal.ir/v1/request"; // url
            Zibal.makeRequest Request = new Zibal.makeRequest(); // define Request

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "merchant.txt");
            var merchant = System.IO.File.ReadAllText(path);
            Request.merchant = merchant; // String
            
            Request.amount = amount * 10; //Integer
            Request.callbackUrl = Url.ActionLink(nameof(VerifyPayment), "Account"); //String
            var httpResponse = Zibal.HttpRequestToZibal(url, JsonConvert.SerializeObject(Request));  // get Response
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream())) // make stream reader
            {
                var responseText = streamReader.ReadToEnd(); // read Response
                Zibal.makeRequest_response item = JsonConvert.DeserializeObject<Zibal.makeRequest_response>(responseText);

                return Redirect($"https://gateway.zibal.ir/start/{item.trackId}/direct");
            }

        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> VerifyPayment(string trackId)
        {

            string url = "https://gateway.zibal.ir/v1/verify"; // url
            Zibal.verifyRequest Request = new Zibal.verifyRequest(); // define Request

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "merchant.txt");
            var merchant = System.IO.File.ReadAllText(path);
            Request.merchant = merchant; // String
            
            Request.trackId = trackId; // String 
            var httpResponse = Zibal.HttpRequestToZibal(url, JsonConvert.SerializeObject(Request));  // get Response
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream())) // make stream reader
            {
                var responseText = streamReader.ReadToEnd(); // read Response
                Zibal.verifyResponse item = JsonConvert.DeserializeObject<Zibal.verifyResponse>(responseText);
                if (item.result == 100.ToString())
                {
                    var user = await _userManager.GetUserAsync(User);
                    user.Wallet += int.Parse(item.amount);

                    await _userManager.UpdateAsync(user);
                }
                else
                {
                    return RedirectToAction(nameof(PaymentResult), "Account", new { success = false });
                }

            }
            return RedirectToAction(nameof(PaymentResult), "Account", new { success = true });
        }


        [HttpGet]
        [Authorize]
        public IActionResult PaymentResult(bool success)
        {
            return View(success);
        }


        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult ChangeMerchant()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangeMerchant(string merchant)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "merchant.txt");
            await System.IO.File.WriteAllTextAsync(path , merchant);


            return Redirect("/");
        }
    }
}

