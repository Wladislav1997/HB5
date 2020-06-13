﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HB5.Models;

namespace HB5.VM.HomeVM
{
    public class P1PHomeVM
    {
        public IQueryable<P1> P1s { get; set; }
        public IQueryable<P> Ps { get; set; }
        public string Name { get; set; }
        public string Name1 { get; set; }
        public DateTime StData { get; set; }
        public DateTime FinData { get; set; }
        public string NameAct { get; set; }
        public int maxsum { get; set; }
        public int minsum { get; set; }
        public int? idoper { get; set; }
        public DateTime Data { get; set; }
        public DateTime DataPeriod { get; set; }
    }
}