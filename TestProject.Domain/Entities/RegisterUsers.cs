using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProjectDomain.Entities;

namespace TestProjectDomain.Entities
{
    [NotMapped]
    public class RegisterUsers:Users
    {
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [DataType(DataType.Password)]
        [StringLength(maximumLength: 255)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage ="Введите одинаковые пароли")]
        public string RetypePassword { get; set; }
    }
}
