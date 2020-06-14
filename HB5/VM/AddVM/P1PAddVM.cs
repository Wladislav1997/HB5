using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace HB5.VM.AddVM
{
    public class P1PAddVM : IValidatableObject
    {
        [Required(ErrorMessage = "Не указано имя")]
        public string Name { get; set; }
        public string NameAct { get; set; }
        [Required(ErrorMessage = "Не указана сумма")]
        public int _Sum { get; set; }
        public string Coment { get; set; }
        public int? idoper { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errors = new List<ValidationResult>();

            if (this._Sum<0&&this._Sum>100000000)
            {
                errors.Add(new ValidationResult("Некорректно задана сумма !"));
            }
            return errors;
        }
    }
}
