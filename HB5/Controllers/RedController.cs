using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HB5.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using HB5.VM.RedVM;


namespace HB5.Controllers
{
    [Authorize]
    public class RedController : Controller
    {
        UserContext db;

        public RedController(UserContext user)
        {
            db = user;
        }
        [HttpGet]
        public async Task<IActionResult> PlanRed(int? id)
        {
            Plan pl = await db.Plans.FirstAsync(p => p.Id == id);
            PlanRedVM plan = new PlanRedVM();
            plan.Name = pl.Name;
            plan.Data = pl.Data;
            plan.DataPeriod = pl.DataPeriod;
            plan.idplan = id;
            return View(plan);
        }
        [HttpPost]
        public async Task<IActionResult> PlanRed(PlanRedVM plan)
        {
            if (ModelState.IsValid)
            {
                Plan pl = await db.Plans.FirstAsync(p => p.Id == plan.idplan);
                pl.Name = plan.Name;
                pl.Data = plan.Data;
                pl.DataPeriod = plan.DataPeriod;
                db.Plans.Update(pl);
                await db.SaveChangesAsync();
                return RedirectToAction("PlanHome", "Home");
            }
            return View(plan);
        }
        [HttpGet]
        public async Task<IActionResult> OperRed(int? id)
        {
            Operation op = await db.Operations.FirstOrDefaultAsync(p => p.Id == id);
            OperRedVM o = new OperRedVM();
            o.Name = op.Name;
            o.NameAct= op.NameAct;
            o.Sum = op.Sum;
            o.Coment = op.Coment;
            o.idplan = op.PlanId;
            o.idoper = id;
            return View(o);
        }
        [HttpPost]
        public async Task<IActionResult> OperRed(OperRedVM op)
        {
            if (ModelState.IsValid)
            {
                Operation o = await db.Operations.FirstOrDefaultAsync(p => p.Id == op.idoper);
                o.Name = op.Name;
                o.NameAct = op.NameAct;
                o.Sum = op.Sum;
                o.Coment = op.Coment;
                db.Operations.Update(o);
                await db.SaveChangesAsync();
                return RedirectToAction("OperHome", "Home",new { idplan = op.idplan });
            }
            return View(op);
        }
    }
}

