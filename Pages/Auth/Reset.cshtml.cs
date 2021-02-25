using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using GaryPortalAPI.Services;
using GaryPortalAPI.Services.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GaryPortalAPI.Pages.Auth
{

    public class ResetInputModelView
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "New Password", Prompt = "New Password")]
        public string NewPassword { get; set; }


        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password", Prompt = "Confirm Password")]
        public string ConfirmPassword { get; set; }
    }

    public class ResetModel : PageModel
    {
        private readonly IAuthenticationService _authService;
        public bool success = false;

        [BindProperty]
        public ResetInputModelView ResetInput { get; set; }

        public ResetModel(IAuthenticationService authService)
        {
            _authService = authService;
        }

        public IActionResult OnGet(string token = "")
        {
            if (string.IsNullOrEmpty(token))
            {
                Console.WriteLine("null token");
                return Redirect("https://garyportal.tomk.online");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            string token = HttpContext.Request.Query["token"];
            string newPassword = ResetInput.NewPassword;
            string newConf = ResetInput.ConfirmPassword;
            if (ModelState.IsValid)
            {
                if (newPassword.Trim() != newConf.Trim() || string.IsNullOrWhiteSpace(newPassword) || string.IsNullOrWhiteSpace(newConf))
                {
                    Console.WriteLine("Error no amtch");
                    ModelState.AddModelError("", "Passwords do not match");
                    return Page();
                }

                Console.WriteLine(token);
                Console.WriteLine(newPassword);
                if (await _authService.ResetPassword(token, newPassword, default))
                {
                    Console.WriteLine("reset");
                    this.success = true;
                    return Page();
                }

                Console.WriteLine("No success");

                return Page();
            }
            else
            {
                Console.WriteLine("Invalid");
                return Page();
            }


            //Console.WriteLine("ON POST ASYNC");
            //if (newPassword != newConf)
            //{
            //    Console.WriteLine($"Does not match {newPassword} vs {newConf}");
            //    ModelState.AddModelError("", "Passwords did not match");
            //    return Page();
            //}
            //Console.WriteLine("End");
            //Console.WriteLine("TOKEN:::");
            //Console.WriteLine(token);
            //if (await _authService.ResetPassword(token, newPassword, default))
            //{
            //    Console.WriteLine("reset");
            //    this.success = true;
            //    return Page();
            //}
            //else
            //{
            //    Console.WriteLine("No sucess");
            //    return Redirect("https://garyportal.tomk.online");
            //}
        }
    }
}
