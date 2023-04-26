using System.ComponentModel.DataAnnotations;

namespace GameTrip.API.Models.Auth
{
    public class ResetPasswordDTO
    {
        public ResetPasswordDTO(string email, string password, string passwordConfirmation, string token)
        {
            Email = email;
            Password = password;
            PasswordConfirmation = passwordConfirmation;
            Token = token;
        }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string PasswordConfirmation { get; }

        [Required]
        public string Token { get; set; }
    }
}