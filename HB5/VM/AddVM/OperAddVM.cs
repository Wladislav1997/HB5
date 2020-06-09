using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace HB5.VM.AddVM
{
    public class OperAddVM : IValidatableObject
    {
        [Required(ErrorMessage = "Не указано имя")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Не указано действие")]
        public string NameAct { get; set; } // доход расход
        [StringLength(50,MinimumLength =5,ErrorMessage ="Длина коментария минимум 5 знаков, максимум 50!")]
        public string Coment { get; set; }
        [Required(ErrorMessage = "Не указана сумма")]
        public int Sum { get; set; }
        public int? idplan { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errors = new List<ValidationResult>();
            
            if (this.Sum > 100000000||this.Sum<0)
            {
                errors.Add(new ValidationResult(" Сумма операции больше 100000000!"));
            }
            
            return errors;
        }
    }
}
