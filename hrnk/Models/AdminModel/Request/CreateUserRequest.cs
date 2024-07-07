using System.ComponentModel.DataAnnotations;

namespace hrnk.Models.AdminModel.Request
{
    public class CreateUserRequest
    {
        [Required(ErrorMessage = "Username is required")]
        public String UserAccountName { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public String Password { get; set; }
        [Required(ErrorMessage = "Confirm password is required")]
        [Compare("Password", ErrorMessage = "Password and confirmation password do not match.")]
        public String ConfirmPassword { get; set; }
        [Required(ErrorMessage = "User Role is required")]
        public String UserRole { get; set; }
    }
}
