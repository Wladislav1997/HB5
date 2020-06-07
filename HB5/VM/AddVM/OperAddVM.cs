using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HB5.VM.AddVM
{
    public class OperAddVM
    {
        public string Name { get; set; }
        public string NameAct { get; set; } // доход расход
        public string Coment { get; set; }
        public int Sum { get; set; }
        public int? idplan { get; set; }
    }
}
