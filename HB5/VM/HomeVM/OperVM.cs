using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HB5.Models;
using System.ComponentModel.DataAnnotations;


namespace HB5.VM.HomeVM
{
    public class OperVM : IValidatableObject
    {
        public string Name { get; set; }
        public string NamePl { get; set; }
        public DateTime? StData { get; set; }
        public DateTime? FinData { get; set; }
        public string NameAct { get; set; }
        public int maxsum { get; set; }
        public int minsum { get; set; }
        public int maxsump { get; set; }
        public int minsump { get; set; }
        public int maxpr { get; set; }
        public int minpr { get; set; }

        public IQueryable<Operation> Operations { get; set; }
        public DateTime Date { get; set; }
        public DateTime DatePer { get; set; }
        public int? idplan { get; set; }
        public string St { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errors = new List<ValidationResult>();

            if (StData >= FinData)
            {
                errors.Add(new ValidationResult("Дата начала периода не может быть позже даты конца периода или ровняться ей!"));
            }
            if (minsum > maxsum)
            {
                errors.Add(new ValidationResult("Минимальная плановая сумма не может быть больше максимальной!"));
            }
            if (minsump > maxsump)
            {
                errors.Add(new ValidationResult("Минимальный реальная сумма не может быть больше максимальной!"));
            }
            if (minpr > maxpr)
            {
                errors.Add(new ValidationResult("Минимальный процент не может быть больше максимального!"));
            }
            if (minsum > 100000000 || minsum < 0)
            {
                errors.Add(new ValidationResult(" Минимальная плановая сумма больше 100000000 или меньше 0!"));
            }
            if (maxsum > 100000000 || maxsum < 0)
            {
                errors.Add(new ValidationResult(" Максимальная плановая сумма больше 100000000 или меньше 0!"));
            }
            if (maxsump > 100000000 || maxsump < 0)
            {
                errors.Add(new ValidationResult(" Максимальная реальная сумма больше 100000000 или меньше 0!"));
            }
            if (minsump > 100000000 || minsump < 0)
            {
                errors.Add(new ValidationResult(" Минимальная реальная сумма больше 100000000 или меньше 0!"));
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