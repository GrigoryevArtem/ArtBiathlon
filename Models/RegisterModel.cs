using System.ComponentModel.DataAnnotations;
using ArtBiathlon.Enums;

namespace ArtBiathlon.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Укажите логин")]
        [Display(Name = nameof(Login))]
        [MinLength(8, ErrorMessage = "Логин должен иметь длину не менее 8 символов")]
        [MaxLength(50, ErrorMessage = "Логин должен иметь длину не более 50 символов")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Укажите пароль")]
        [DataType(DataType.Password)]
        [Display(Name = nameof(Password))]
        [MinLength(8, ErrorMessage = "Пароль должен иметь длину не менее 8 символов")]
        [MaxLength(50, ErrorMessage = "Пароль должен иметь длину не более 50 символов")]
        public string Password { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Необходимо ввести адрес почты")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Необходимо ввести имя")]
        public string FIO { get; set; }

        [Required(ErrorMessage = "Необходимо выбрать пол")]
        public Gender Gender { get; set; }
        
        [Required(ErrorMessage = "Необходимо выбрать дату")]
        public DateTime Date { get; set; }
        
        [Required(ErrorMessage = "Необходимо выбрать ранг")]
        public Rank Rank { get; set; }
        
        [Required(ErrorMessage = "Необходимо выбрать статус")]
        public Role Role { get; set; }
    }
}
