﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HB5.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using HB5.VM.DetVM;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HB5.Controllers
{
    [Authorize]
    public class DetController : Controller
    {
        UserContext db;

        public DetController(UserContext user)
        {
            db = user;
        }
        [HttpGet]
        public async Task<IActionResult> PlanDet(int id)
        {
            Plan pl = await db.Plans.FirstAsync(p => p.Id == id);
            return View(pl);
        }
        [HttpPost]
        public IActionResult PlanDet()
        {
            return RedirectToAction("PlanHome", "Home");
        }
        [HttpGet]
        public async Task<IActionResult> OperDet(int id)
        {
            Operation op = await db.Operations.FirstAsync(p => p.Id == id);
            OperDetVM oper = new OperDetVM();
            oper.op = op;
            oper.Id = op.Plan.Id;
            return View(oper);
        }
        [HttpPost]
        public IActionResult OperDet(OperDetVM oper)
        { 
            return RedirectToAction("OperHome", "Home",new { planid=oper.Id});
        }
        public async Task<IActionResult> PDet(int id)
        {
            P p = await db.Ps.FirstAsync(p => p.Id == id);
            return View(p);
        }
        public async Task<IActionResult> P1Det(int id)
        {
            P1 p1 = await db.P1s.FirstAsync(p => p.Id == id);
            return View(p1);
        }
    }
}