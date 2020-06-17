using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HB5.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using HB5.VM.HomeVM;

namespace HB5.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        UserContext db;

        public HomeController(UserContext user)
        {
            db = user;
        }
        public async Task<IActionResult> OperHome(OperVM oper, string st, int? idplan, int? idp)
        {
            IQueryable<Operation> operat = db.Operations.Include(p => p.Plan).Include(c => c.p);
            operat = operat.Where(p => p.Plan.User.Email == User.Identity.Name);
            if (idplan != null)
            {
                operat = operat.Where(p => p.PlanId == idplan);
                OperVM oper1 = new OperVM();
                Plan pl = await db.Plans.FirstAsync(p => p.Id == idplan);
                oper1.Operations = operat;
                oper1.Date = pl.Data;
                oper1.DatePer = pl.DataPeriod;
                oper1.idplan = idplan;
                return View(oper1);

            }
            else
            {
                if (st == "home")
                {
                    operat = operat.Where(p => p.Plan.Data <= DateTime.Now && p.Plan.DataPeriod >= DateTime.Now);
                    OperVM op2 = new OperVM();
                    op2.Operations = operat;
                    op2.St = st;
                    return View(op2);
                }
                else
                {
                    if (idp != null)
                    {
                        P p1= await db.Ps.FirstOrDefaultAsync(p => p.Id == idp);
                        operat = operat.Where(p => p.Id == p1.OperationId);
                        OperVM oper1 = new OperVM();
                        oper1.Operations = operat;
                        return View(oper1);
                    }
                    else
                    {
                        OperVM op2 = new OperVM();
                        op2.Operations = operat;
                        return View(op2);
                    }

                }
            }
        }
        public async Task<IActionResult> PlanHome(PlanHomeVM plan, int? idoper)
        {
            IQueryable<Models.Plan> pl = db.Plans.Include(c => c.User).Include(u => u.Operations).ThenInclude(u => u.p);
            pl = pl.Where(p => p.User.Email == User.Identity.Name);
            if (idoper != null)
            {
                Operation op = await db.Operations.FirstOrDefaultAsync(p => p.Id == idoper);
                pl = pl.Where(p => p.Id == op.PlanId);
                plan.pl = SearchPlan(plan, 1, pl);
                return View(plan);
            }
            else
            {
                PlanHomeVM plan1 = new PlanHomeVM();
                plan.pl = SearchPlan(plan, 1, pl);
                return View(plan);
            }

        }
        private IQueryable<Plan> SearchPlan(PlanHomeVM plan, int count, IQueryable<Plan> plans)
        {
            if (plan.Name != null && count==1)
            {
                plans = plans.Where(p => p.Name == plan.Name);
                return SearchPlan(plan, 2, plans);
            }
            else if (plan.maxpr != 0 && (count == 1 || count == 2))
            {
                plans = plans.Where(p => p.Procent >= plan.minpr && p.Procent <= plan.maxpr);
                return SearchPlan(plan, 3, plans);
            }
            else if (plan.Data != null && plan.DataPer != null && (count == 1 || count == 2 || count == 3))
            {
                plans = plans.Where(p => p.Data >= plan.Data && p.DataPeriod <= plan.DataPer);
                return SearchPlan(plan, 4, plans);
            }
            else if (plan.maxdoch != 0 && (count == 1 || count == 2 || count == 3 || count == 4))
            {
                plans = plans.Where(p => p.DochMonth >= plan.mindoch && p.DochMonth <= plan.maxdoch);
                return SearchPlan(plan, 5, plans);
            }
            else if (plan.maxras != 0 && (count == 1 || count == 2 || count == 3 || count == 4 || count == 5))
            {
                plans = plans.Where(p => p.RasMonth >= plan.minras && p.RasMonth <= plan.maxras);
                return SearchPlan(plan, 6, plans);
            }
            else if (plan.maxit != 0 && (count == 1 || count == 2 || count == 3 || count == 4 || count == 5 || count == 6))
            {
                plans = plans.Where(p => p.RaznDochRas >= plan.minit && p.RaznDochRas <= plan.maxit);
                return SearchPlan(plan, 7, plans);
            }
            else
            {
                return plans;
            }
        }
        public IActionResult P1PHome(P1PHomeVM p1P, int? idoper)
        {
            IQueryable<P> p = db.Ps.Include(c => c.Operation).ThenInclude(u => u.Plan).ThenInclude(u => u.User);
            p = p.Where(p => p.Operation.Plan.User.Email == User.Identity.Name);
            if (idoper != null)
            {
                p = p.Where(p => p.OperationId == idoper);
                P1PHomeVM plan1 = new P1PHomeVM();
                IQueryable<Operation> ops = db.Operations.Include(c => c.Plan);
                ops = ops.Where(p => p.Id == idoper);
                foreach (Operation p1 in ops)
                {
                    plan1.Data= p1.Plan.Data;
                    plan1.DataPeriod = p1.Plan.DataPeriod;
                    plan1.Name1 = p1.Name;
                    break;
                }
                plan1.Ps = p;
                plan1.idoper = idoper;
                return View(plan1);
            }
            else
            {
                IQueryable<P1> p1 = db.P1s.Include(c => c.User);
                p1 = p1.Where(p => p.User.Email == User.Identity.Name);
                P1PHomeVM plan1 = new P1PHomeVM();
                plan1.Ps = p;
                plan1.P1s = p1;
                return View(plan1);
            }
        }
    }
}