using System.ComponentModel.DataAnnotations;

namespace OtpServer.Request
{
    public class CreateUserRequest
    {
        [Required(ErrorMessage = "Username is required.")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 20 characters.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(25, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 25 characters.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [StringLength(20, ErrorMessage = "Email can be at most 20 characters long.")]
        public string Email { get; set; }
    }
}
