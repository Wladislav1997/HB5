using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace HB5.VM.HomeVM
{
    public class PlanHomeVM : IValidatableObject
    {
        public IQueryable<Models.Plan> pl { get; set; }
        public string Name { get; set; }
        public int mindoch { get; set; }
        public int maxdoch { get; set; }
        public int minras { get; set; }
        public int maxras { get; set; }
        public int minit { get; set; }
        public int maxit { get; set; }
        public int minpr { get; set; }
        public int maxpr { get; set; }
        public DateTime? Data { get; set; }
        public DateTime? DataPer { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errors = new List<ValidationResult>();

            if (Data >= DataPer)
            {
                errors.Add(new ValidationResult("Дата начала периода не может быть позже даты конца периода или ровняться ей!"));
            }
            if (mindoch > maxdoch)
            {
                errors.Add(new ValidationResult("Минимальный доход не может быть больше максимального!"));
            }
            if (minras > maxras)
            {
                errors.Add(new ValidationResult("Минимальный расход не может быть больше максимального!"));
            }
            if (minit > maxit)
            {
                errors.Add(new ValidationResult("Минимальный итог не может быть больше максимального!"));
            }
            if (minpr > maxpr)
            {
                errors.Add(new ValidationResult("Минимальный процент не может быть больше максимального!"));
            }
            if (mindoch > 100000000 || mindoch < 0)
            {
                errors.Add(new ValidationResult(" Сумма минимального дохода больше 100000000 или меньше 0!"));
            }
            if (maxdoch > 100000000 || maxdoch < 0)
            {
                errors.Add(new ValidationResult(" Сумма максимального дохода больше 100000000 или меньше 0!"));
            }
            if (maxras > 100000000 || maxras < 0)
            {
                errors.Add(new ValidationResult(" Сумма максимального расхода больше 100000000 или меньше 0!"));
            }
            if (minras > 100000000 || minras < 0)
            {
                errors.Add(new ValidationResult(" Сумма минимального расхода больше 100000000 или меньше 0!"));
            }
            if (maxit > 100000000 || maxit < 0)
            {
                errors.Add(new ValidationResult(" Сумма максимального итога больше 100000000 или меньше 0!"));
            }
            if (minit > 100000000 || minit < 0)
            {
                errors.Add(new ValidationResult(" Сумма минимального итога больше 100000000 или меньше 0!"));
            }
            if (maxpr > 100000000 || maxpr < 0)
            {
                errors.Add(new ValidationResult(" Сумма максимального процента больше 100000000 или меньше 0!"));
            }
            if (minpr > 100000000 || minpr < 0)
            {
                errors.Add(new ValidationResult(" Сумма минимального процента больше 100000000 или меньше 0!"));
            }
            return errors;
        }
    }
}