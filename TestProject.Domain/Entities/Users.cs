using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace TestProjectDomain.Entities
{
    public class Users
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        public int UserId { get; set; }


        [StringLength(255, ErrorMessage = "Длина должна быть от 1 до 255 символов", MinimumLength =1)]
        [Required(ErrorMessage ="Поле обязательно для заполнения")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [StringLength(255, ErrorMessage = "Длина должна быть от 1 до 255 символов", MinimumLength = 1)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [EmailAddress(ErrorMessage ="Неверный формат эл. адреса")]
        [RegularExpression(@"^[a-zA-Z0-9.!#$%&’*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$", ErrorMessage = "Неправильный адрес эл. почты")]
        [StringLength(maximumLength:125)]
        [Display(Name = "Email")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [RegularExpression(@"^\+[1-9]{1}[0-9]{10,14}$", ErrorMessage = "Неверный телефонный номер")]
        public string Mobile { get; set; }
                
        public string CryptedPassword { get; set; }

        public string PasswordSalt { get; set; }

    }
}
