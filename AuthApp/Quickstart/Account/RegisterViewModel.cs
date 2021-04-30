using System.ComponentModel.DataAnnotations;

namespace AuthApp.Quickstart.Account
{
    public class RegisterViewModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [MinLength(6)]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{6,}$")]
        public string Password { get; set; }
        public string ReturnUrl = "/";
    }
}
