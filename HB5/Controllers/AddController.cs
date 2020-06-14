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
                return RedirectToAction("PlanHome", "Home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> OperAdd(OperAddVM oper)
        {
            if (ModelState.IsValid)
            {
                Plan pl = await db.Plans.FirstOrDefaultAsync(u => u.Id == oper.idplan);
                db.Operations.Add(new Operation { Name = oper.Name, NameAct = oper.NameAct, Coment = oper.Coment, Sum = oper.Sum, Plan = pl });
                await db.SaveChangesAsync();
                return RedirectToAction("OperHome", "Home", new { idplan = oper.idplan });
            }
            return View(oper);
        }
        [HttpGet]
        public IActionResult P1PAdd(int? idoper)
        {
            if (idoper != null)
            {
                P1PAddVM p = new P1PAddVM();
                p.idoper = idoper;
                return View(p);
            }
            else
            {
                P1PAddVM p = new P1PAddVM();
                return View(p);
            }
        }
        [HttpPost]
        public async Task<IActionResult> P1PAdd(P1PAddVM p)
        {
            if (p.idoper != null)
            {
                if (ModelState.IsValid)
                {
                    IQueryable<Operation> ops = db.Operations.Include(c => c.Plan).Include(p => p.p);
                    ops = ops.Where(u => u.Id == p.idoper);
                    Operation o = new Operation();
                    foreach (Operation op in ops)
                    {
                        o = op;
                        break;
                    }
                    db.Ps.Add(new P { Name = p.Name, Data = DateTime.Now, Coment = p.Coment, Sum = p._Sum, Operation = o });
                    await db.SaveChangesAsync();
                    int pr1 = o.Sum / 100;
                    foreach (P p1 in o.p)
                    {
                        o.SumP += p1.Sum;// общая сумма всех соверш опер
                    }
                    o.Procent = o.SumP / pr1;
                    // считаем процент выполнения плана складываем проценты всех операций и делим на кол во опер
                    int count = 0;
                    int pr = 0;
                    foreach (Operation op1 in o.Plan.Operations)
                    {
                        pr += op1.Procent;
                        count++;
                    }
                    o.Plan.Procent = pr / count;
                    await db.SaveChangesAsync();
                    return RedirectToAction("P1PHome", "Home", new { idoper = p.idoper });
                }
                return View(p);

            }
            else
            {
                if (ModelState.IsValid)
                {
                    IQueryable<User> users = db.Users.Include(c => c.P1s);
                    users = users.Where(u => u.Email == User.Identity.Name);
                    User us = new User();
                    foreach(User u in users)
                    {
                        us = u;
                        break;
                    }
                    db.P1s.Add(new P1 { Name = p.Name, Data = DateTime.Now, Coment = p.Coment, Sum = p._Sum, NameAct = p.NameAct,User=us });
                    await db.SaveChangesAsync();
                    return RedirectToAction("P1PHome", "Home");
                }
                return View(p);
            }
        }
        
    }
}