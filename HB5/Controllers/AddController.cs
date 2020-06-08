using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HB5.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using HB5.VM.AddVM;
using System.Linq.Expressions;

namespace HB5.Controllers
{
    [Authorize]
    public class AddController : Controller
    {
        UserContext db;

        public AddController(UserContext user)
        {
            db = user;
        }
        [HttpGet]
        public IActionResult PlanAdd()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> PlanAdd(PlanAddVM plan)
        {
            if (ModelState.IsValid)
            {
                User user = await db.Users.FirstOrDefaultAsync(u => u.Email == User.Identity.Name);
                db.Plans.Add(new Plan { Name = plan.Name, Data = plan.Data, DataPeriod = plan.DataPeriod, User = user });
                await db.SaveChangesAsync();
                return RedirectToAction("PlanHome", "Home");
            }
            return View(plan);
        }
        [HttpGet]
        public IActionResult OperAdd(int? idplan)
        {
            if (idplan != null)
            {
                OperAddVM p = new OperAddVM();
                p.idplan = idplan;
                return View(p);
            }
            else
            {
                return RedirectToAction("OperHome", "Home",new {idplan=idplan});
            }
        }
        [HttpPost]
        public async Task<IActionResult> OperAdd(OperAddVM oper)
        {
            Plan pl = await db.Plans.FirstOrDefaultAsync(u => u.Id == oper.idplan);
            if (pl.DataPeriod < DateTime.Now)
            {
                db.Operations.Add(new Operation { Name = oper.Name, NameAct = oper.NameAct, Coment = oper.Coment, Sum = oper.Sum, Plan = pl });
                await db.SaveChangesAsync();
                return RedirectToAction("OperHome", "Home", new { idplan = oper.idplan });
            }
            return View(oper);
        }
        [HttpGet]
        public IActionResult PAdd(int? idoper)
        {
            if (idoper != null)
            {
                PAddVM p = new PAddVM();
                p.idoper = idoper;
                return View(p);
            }
            else
            {
                return RedirectToAction("OperHome", "Home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> PAdd(PAddVM p)
        {
            Operation op = await db.Operations.FirstOrDefaultAsync(u => u.Id == p.idoper);
            if (op.Plan.Data < DateTime.Now && op.Plan.DataPeriod > DateTime.Now)
            {
                db.Ps.Add(new P { Name = p.Name, Data = p.Data, Coment = p.Coment, Sum = p.Sum, Operation = op });
                // считаем общую сумму для операции и процент её выполнения
                foreach (P p1 in op.p)
                {
                    op.SumP += p1.Sum;// общая сумма всех соверш опер
                }
                op.Procent = op.SumP / (op.Sum / 100);
                // считаем процент выполнения плана складываем проценты всех операций и делим на кол во опер
                int count = 0;
                int pr = 0;
                foreach (Operation op1 in op.Plan.Operations)
                {
                    pr += op1.Procent;
                    count++;
                }
                op.Plan.Procent = pr / count;
                await db.SaveChangesAsync();
                return RedirectToAction("P1PHome", "Home");
            }
            return View(p);
        }
        [HttpGet]
        public IActionResult P1Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> P1Add(P1AddVM p)
        {
            db.P1s.Add(new P1 { Name = p.Name, Data = p.Data, Coment = p.Coment, Sum = p.Sum, NameAct = p.NameAct });
            await db.SaveChangesAsync();
            return RedirectToAction("P1PHome", "Home");
        }
    }
}