using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace HB5.VM.RedVM
{
    
    public class PlanRedVM : IValidatableObject
    {
        [Required(ErrorMessage = "Не указано имя")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Не указана дата начала периода")]
        public DateTime Data { get; set; }
        [Required(ErrorMessage = "Не указано дата конца периода")]
        public DateTime DataPeriod { get; set; }
        public int? idplan { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errors = new List<ValidationResult>();

            if (this.Data < DateTime.Now)
            {
                errors.Add(new ValidationResult("Дата начала периода не может находится в прошедшем времени!"));
            }
            if (this.Data >= this.DataPeriod)
            {
                errors.Add(new ValidationResult("Дата начала периода не может находится позже даты конца !"));
            }
            return errors;
        }
    }
}