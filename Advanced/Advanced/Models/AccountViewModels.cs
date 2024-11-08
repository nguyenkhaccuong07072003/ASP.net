using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Advanced.Models;

namespace Advanced.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
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

        [Display(Name = "Remember this browser")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Display(Name = "Username")]
        [Required]
        public string UserName { get; set; }
        [Display(Name = "Email")]
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        [Display(Name = "Role name")]
        [Required]
        public string RoleId { get; set; }
    }
    public class EditUserViewModel
    {
        public string UserId { get; set; }
        [Display(Name = "Username")]
        [Required]
        public string UserName { get; set; }
        [Display(Name = "Fullname")]
        public string FullName { get; set; }
        [Display(Name = "Age")]
        public string Age { get; set; }
        [Display(Name = "Main subject")]
        public string Main_subject { get; set; }
        [Display(Name = "Birth Date")]
        public DateTime? Birthdate { get; set; }
        [Display(Name = "Address")]
        public string address { get; set; }
        [Display(Name = "Phone number")]
        public string Phone_number { get; set; }
        [Display(Name = "Two or three words about you")]
        public string ShortDesc { get; set; }
        public string Image { get; set; }
    }
    public class ChangeEmail
    {
        public string UserId { get; set; }
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }
    }
    public class ResetPasswordViewModel
    {
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
    public class CustomPasswordValidator : IIdentityValidator<string>
    {
        public int MinLength { get; set; }
        //While Creating CustomPasswordValidator Instance we need to pass the Minimum Length of the Password
        public CustomPasswordValidator(int minLength)
        {
            MinLength = minLength;
        }
        // Validate Password: count how many types of characters exists in the password  
        // Provide Implementation for the ValidateAsync method of IIdentityValidator Interface
        public Task<IdentityResult> ValidateAsync(string password)
        {
            //First Check the Minimum Length Validator
            if (string.IsNullOrEmpty(password) || password.Length < MinLength)
            {
                return Task.FromResult(IdentityResult.Failed($"Password Too Short, Minimum {MinLength} Character Required"));
            }
            int counter = 0;
            //Create a List of String to store the different patterns to be checked in the password
            List<string> patterns = new List<string>
            {
                @"[a-z]", // Lowercase  
                @"[A-Z]", // Uppercase  
                @"[0-9]", // Digits  
                @"[!@#$%^&*\(\)_\+\-\={}<>,\.\|""'~`:;\\?\/\[\]]" // Special Symbols
            };
            // Count Type of Different Chars present in the Password  
            foreach (string p in patterns)
            {
                if (Regex.IsMatch(password, p))
                {
                    counter++;
                }
            }
            //If the counter is less than or equals to 3, means password doesnot contain all the required patterns
            if (counter <= 3)
            {
                return Task.FromResult(IdentityResult.Failed("Please Use a Combination of Lowercase, Uppercase, Digits, Special Symbols Characters"));
            }
            return Task.FromResult(IdentityResult.Success);
        }
    }
    public class RegisterRoleViewModel
    {
        [Display(Name = "Role name")]
        [Required]
        public string RoleName { get; set; }
    }
    public class EditRoleViewModel
    {
        [Required]
        public string RoleId { get; set; }
        [Display(Name = "Role name")]
        [Required]
        public string RoleName { get; set; }
    }
    public class DeleteRoleViewModel
    {
        [Required]
        public string RoleId { get; set; }
    }
    public class UserList
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Fullname { get; set; }
        public string Phonenumber { get; set; }
        public string Age { get; set; }
        public string Address { get; set; }
        public string ShortDesc { get; set; }
        public string RoleName { get; set; }
        public string Image {  get; set; }
    }
    public class ClassDetailViewModel
    {
        public Lophoc Lophoc { get; set; }
        public IEnumerable<Lichhoc> Lichhocs { get; set; }
        public IEnumerable<ClassMember> ClassMembers { get; set; }
    }
    public class EditDetailViewModel
    {
        public Lophoc Lophoc { get; set; }
        public Lichhoc Lichhocs { get; set; }
        public IEnumerable<ClassMember> ClassMembers { get; set; }
    }
    public class RollOutDetailViewModel
    {
        public IEnumerable<Lophoc> Lophoc { get; set; }
        public IEnumerable<Lichhoc> Lichhocs { get; set; }
        public IEnumerable<ClassMember> ClassMembers { get; set; }
    }
    public class PostModel
    {
        public string UserId { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string Fullname { get; set; }
        public DateTime? public_date { get; set; }

    }
    public class TrueBlog
    {
        public Post_Post Post_s { get; set; }
        public IEnumerable<Main_Comment> main_Comments { get; set; }
        public IEnumerable<Sub_Comment> sub_Comments { get; set; }
        public string NewComment { get; set; }
    }
    public class RollOutModel
    {
        public List<Lophoc> Lophoc { get; set; }
        public Lichhoc Lichhoc { get; set; }
        public List<ClassMember> ClassMembers { get; set; }
    }
    public class AbsenceRecord
    {
        public DateTime Date { get; set; }
        public bool Excused { get; set; }
    }
    public class ListAbsentModel
    {
        public string class_name { get; set; }
        public string user_name { get; set; }
        public List<AbsenceRecord> AbsenceRecords { get; set; }
        public int Total { get; set; }
    }
    public class MarkModel
    {
        public List<Lophoc> Lophoc { get; set; }
        public List<ClassMember> ClassMembers { get; set; }
    }
    public class ReportModel
    {
        public List<Mark> Marks { get; set; }
        public List<Lophoc> Lophocs { get; set; }
    }
    public class MyCourseViewModel
    {
        public DateTime NgayMua { get; set; }
        public float TongTien { get; set; }
        public string KhoaHocName { get; set; }
        public string User { get; set; }
        public Boolean TrangThai { get; set; }
    }
}
