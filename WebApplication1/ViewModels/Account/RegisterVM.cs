using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.ViewModels.Account
{
    public class RegisterVM
    {
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Username { get; set; }
        [Required,DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required,DataType(DataType.Password)]
        public string Password { get; set; }

        [Required, DataType(DataType.Password),Compare(nameof(Password),ErrorMessage ="Please make sure your passwords match.")]
        public string CheckPassword { get; set; }
    }
}
