using System.ComponentModel.DataAnnotations;

namespace HappyMeal_v3.ViewModels
{
    public class AccountViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Pseudo { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
