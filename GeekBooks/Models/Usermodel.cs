using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace GeekBooks.Models
{
    public class Usermodel
    {
        

        [Required(AllowEmptyStrings = false, ErrorMessage = "Username is required")]
        [Display(Name = "Username: ")]
        public string Username { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "First Name is required")]
        [Display(Name = "First Name: ")]
        public string UserFirst { get; set; }

        
        [Display(Name = "Middle Name: ")]
        public string UserMiddle { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Last Name is required")]
        [Display(Name = "Last Name: ")]
        public string UserLast { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Email is required")]
        [Display(Name = "Email: ")]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required")]
        [Display(Name = "Password: ")]
        [DataType(DataType.Password)]
        public string UserPassword { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please confirm password")]
        [Display(Name = "Confirm Password: ")]
        [DataType(DataType.Password)]
        [Compare("UserPassword", ErrorMessage = "Password must match")]
        public string ConfirmPassword { get; set; }
  

        public string Success { get; set; }
    }
}