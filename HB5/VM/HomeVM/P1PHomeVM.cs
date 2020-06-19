using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HB5.Models;
using System.ComponentModel.DataAnnotations;



namespace HB5.VM.HomeVM
{
    public class P1PHomeVM : IValidatableObject
    {
        public IQueryable<P1> P1s { get; set; }
        public IQueryable<P> Ps { get; set; }

        public string Name { get; set; }
        public DateTime? StData { get; set; }
        public DateTime? FinData { get; set; }
        public string NameAct { get; set; }
        public int maxsum { get; set; }
        public int minsum { get; set; }

        public int? idoper { get; set; }
        public DateTime Data { get; set; }
        public DateTime DataPeriod { get; set; }
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
            if (minsum > 100000000 || minsum < 0)
            {
                errors.Add(new ValidationResult(" Минимальная плановая сумма больше 100000000 или меньше 0!"));
            }
            if (maxsum > 100000000 || maxsum < 0)
            {
                errors.Add(new ValidationResult(" Максимальная плановая сумма больше 100000000 или меньше 0!"));
            }
            return errors;
        }
    }
}