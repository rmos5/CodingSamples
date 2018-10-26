using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerDemo.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        [BindProperty(Name = "g-recaptcha-response")]
        public string CapchaResponse { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            await ValidateRecapcha();

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);
                if (user == null)
                {
                    user = new IdentityUser { UserName = Input.Email, Email = Input.Email };
                    var result = await _userManager.CreateAsync(user, Input.Password);

                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User created a new account with password.");
                        await this.SendConfirmationEmail(_userManager, Url, user, Request.Scheme, _emailSender, Input.Email);
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
                else
                {
                    if (await _userManager.IsEmailConfirmedAsync(user) == false)
                    {
                        _logger.LogInformation($"Resending confirmation email to {Input.Email}.");
                        // resend confirmation email
                        await this.SendConfirmationEmail(_userManager, Url, user, Request.Scheme, _emailSender, user.Email);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private async Task ValidateRecapcha()
        {
            string msg = "Are you robot?";
            if (!string.IsNullOrWhiteSpace(CapchaResponse))
            {
                try
                {
                    string url = "https://www.google.com/recaptcha/api/siteverify";
                    WebClient client = new WebClient();
                    client.Headers[HttpRequestHeader.AcceptCharset] = "utf-8";
                    client.Headers[HttpRequestHeader.Accept] = "application/json";
                    NameValueCollection vals = new NameValueCollection
                    {
                        { "secret","6Lf57UYUAAAAAEyczSVQ0OYKHgejM6BbpvOq2pYq" },
                        { "response", CapchaResponse }
                    };
                    byte[] bresponse = await client.UploadValuesTaskAsync(url, vals);
                    string response = Encoding.UTF8.GetString(bresponse);
                    dynamic jobj = JsonConvert.DeserializeObject(response);
                    if (jobj?.success == true)
                        return;
                }
                catch (Exception ex)
                {
                    _logger.LogError($"{msg} {ex.Message}");
                }
            }

            ModelState.AddModelError(nameof(CapchaResponse), msg);
        }
    }
}
