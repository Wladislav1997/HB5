﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HB5.Models
{
    public class User
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public int? Id { get; set; }


        public List<P1> P1s { get; set; }
        public List<Plan> Plans { get; set; }
    }
}