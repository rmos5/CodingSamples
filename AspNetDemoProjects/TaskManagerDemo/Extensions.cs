using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace TaskManagerDemo
{
    public static class Extensions
    {
        public static async Task SendConfirmationEmail(this PageModel page, UserManager<IdentityUser> userManager, IUrlHelper urlHelper, IdentityUser user, string protocolScheme, IEmailSender emailSender, string email)
        {
            var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = urlHelper.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { userId = user.Id, code = code },
                protocol: protocolScheme);

            try
            {
                await emailSender.SendEmailAsync(email, "Confirm your email",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
                string message = $"E-mail has been sent to {email}. Please check your mail and confirm registration.";
                page.ModelState.AddModelError("", message);
            }
            catch (Exception ex)
            {
                page.ModelState.AddModelError("", ex.Message);
            }
        }
    }
}
