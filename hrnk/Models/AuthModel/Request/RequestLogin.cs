using System.ComponentModel.DataAnnotations;

namespace hrnk.Models.AuthModel.Request
{
    public class RequestLogin
    {
        [Required(ErrorMessage = "Username is required")]
        public String UserAccountName {  get; set; }
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public String Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }

    }
}
