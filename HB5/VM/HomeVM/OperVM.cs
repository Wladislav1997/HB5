﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HB5.Models;

namespace HB5.VM.HomeVM
{
    public class OperVM
    {
        public IQueryable<Operation> Operations { get; set; }
        public string Name { get; set; }
        public string NamePl { get; set; }
        public DateTime StData { get; set; }
        public DateTime FinData { get; set; }
        public string NameAct { get; set; }
        public int maxsum { get; set; }
        public int minsum { get; set; }
        public int maxsump { get; set; }
        public int minsump { get; set; }
        public int maxpr { get; set; }
        public int minpr { get; set; }
        public DateTime Date { get; set; }
        public DateTime DatePer { get; set; }
        public int? idplan { get; set; }
    }
}