using System.ComponentModel.DataAnnotations;
using ArtBiathlon.DataEntity;

namespace ArtBiathlon.Models
{
    public class AuthorizationModel
    {
        [Required(ErrorMessage = "Укажите логин")]
        [Display(Name = "Login")]
        [MinLength(3, ErrorMessage = "Логин должен иметь длину не менее 3 символов")]
        [MaxLength(50, ErrorMessage = "Логин должен иметь длину не более 50 символов")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Укажите пароль")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [MinLength(6, ErrorMessage = "Пароль должен иметь длину не менее 6 символов")]
        [MaxLength(50, ErrorMessage = "Пароль должен иметь длину не более 50 символов")]
        public string Password { get; set; }
    }
}
