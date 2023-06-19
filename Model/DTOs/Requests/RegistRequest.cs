using System.ComponentModel.DataAnnotations;

namespace Mentore.Models.DTOs.Requests
{
    public class RegistRequest
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        /* [RegularExpression("/(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9]).{8}/g",
              ErrorMessage = "Password must meet requirements")]*/

        [Compare("Password", ErrorMessage = "Confirm password not match !")]
        public string ConfirmPassWord { get; set; }
    }
}