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
        public async Task<IActionResult> OperHome(OperVM oper, string st, int? idplan, int? idoper)
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
                return View(oper1);

            }
            else
            {
                if (st == "home")
                {
                    operat = operat.Where(p => p.Plan.Data <= DateTime.Now && p.Plan.DataPeriod >= DateTime.Now);
                    OperVM op2 = new OperVM();
                    op2.Operations = operat;
                    return View(op2);
                }
                else
                {
                    if (idoper != null)
                    {
                        operat = operat.Where(p => p.Id == idoper);
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
        public IActionResult PlanHome(PlanHomeVM plan, int idplan)
        {
            IQueryable<Models.Plan> pl = db.Plans.Include(c => c.User).Include(u => u.Operations).ThenInclude(u => u.p);
            pl = pl.Where(p => p.User.Email == User.Identity.Name);

            if (idplan != 0)
            {
                pl = pl.Where(p => p.Id == idplan);
                PlanHomeVM plan1 = new PlanHomeVM();
                plan1.pl = pl;
                return View(plan1);
            }
            else
            {
                PlanHomeVM plan1 = new PlanHomeVM();
                plan1.pl = pl;
                return View(plan1);
            }

        }
        public IActionResult P1PHome(P1PHomeVM p1P, int? idoper)
        {
            IQueryable<P> p = db.Ps.Include(c => c.Operation).ThenInclude(u => u.Plan).ThenInclude(u => u.User);
            IQueryable<P1> p1 = db.P1s.Include(c => c.User);
            p = p.Where(p => p.Operation.Plan.User.Email == User.Identity.Name);
            p1 = p1.Where(p => p.User.Email == User.Identity.Name);

            if (idoper != null)
            {
                p = p.Where(p => p.Id == idoper);
                P1PHomeVM plan1 = new P1PHomeVM();
                plan1.Ps = p;
                return View(plan1);
            }
            else
            {
                P1PHomeVM plan1 = new P1PHomeVM();
                plan1.P1Ps = (p, p1);
                return View(plan1);
            }
        }
    }
}