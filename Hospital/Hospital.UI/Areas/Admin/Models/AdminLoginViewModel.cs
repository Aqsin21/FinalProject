using System.ComponentModel.DataAnnotations;

namespace Hospital.UI.Areas.Admin.Models
{
    public class AdminLoginViewModel
    {

        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
