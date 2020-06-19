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
                Plan pl = await db.Plans.FirstAsync(p => p.Id == idplan);
                oper.Operations = SearchOp(oper,1,operat);
                oper.Date = pl.Data;
                oper.DatePer = pl.DataPeriod;
                oper.idplan = idplan;
                return View(oper);

            }
            else
            {
                if (st == "home")
                {
                    operat = operat.Where(p => p.Plan.Data <= DateTime.Now && p.Plan.DataPeriod >= DateTime.Now);
                    oper.Operations = SearchOp(oper, 1, operat);
                    oper.St = st;
                    return View(oper);
                }
                else
                {
                    if (idp != null)
                    {
                        P p1= await db.Ps.FirstOrDefaultAsync(p => p.Id == idp);
                        operat = operat.Where(p => p.Id == p1.OperationId);
                        oper.Operations = SearchOp(oper, 1, operat);
                        return View(oper);
                    }
                    else
                    {
                        oper.Operations = SearchOp(oper, 1, operat); ;
                        return View(oper);
                    }

                }
            }
        }
        private IQueryable<Operation> SearchOp(OperVM op, int count, IQueryable<Operation> ops)
        {
            if (op.Name != null && count == 1)
            {
                ops = ops.Where(p => p.Name == op.Name);
                return SearchOp(op, 2, ops);
            }
            else if (op.NamePl != null && (count == 1 || count == 2))
            {
                ops = ops.Where(p => p.Plan.Name == op.NamePl);
                return SearchOp(op, 3, ops);
            }
            else if (op.StData != null && op.FinData != null && (count == 1 || count == 2 || count == 3))
            {
                ops = ops.Where(p => p.Plan.Data >= op.StData && p.Plan.DataPeriod <= op.FinData);
                return SearchOp(op, 4, ops);
            }
            else if (op.NameAct != null && (count == 1 || count == 2 || count == 3 || count == 4))
            {
                ops = ops.Where(p => p.NameAct == op.NameAct);
                return SearchOp(op, 5, ops);
            }
            else if (op.minsum != 0 && op.maxsum!=0 && (count == 1 || count == 2 || count == 3 || count == 4 || count == 5))
            {
                ops = ops.Where(p => p.Sum >= op.minsum && p.Sum <= op.maxsum);
                return SearchOp(op, 6, ops);
            }
            else if (op.minsump != 0 && op.maxsump != 0 && (count == 1 || count == 2 || count == 3 || count == 4 || count == 5 || count == 6))
            {
                ops = ops.Where(p => p.SumP >= op.minsump && p.SumP <= op.maxsump);
                return SearchOp(op, 7, ops);
            }
            else if (op.minpr != 0 && op.maxpr != 0 && (count == 1 || count == 2 || count == 3 || count == 4 || count == 5 || count == 6 || count == 7))
            {
                ops = ops.Where(p => p.Procent >= op.minpr && p.Procent <= op.maxpr);
                return SearchOp(op, 8, ops);
            }
            else
            {
                return ops;
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
                IQueryable<Operation> ops = db.Operations.Include(c => c.Plan);
                ops = ops.Where(p => p.Id == idoper);
                foreach (Operation p1 in ops)
                {
                    p1P.Data= p1.Plan.Data;
                    p1P.DataPeriod = p1.Plan.DataPeriod;
                    break;
                }
                p1P.Ps = SearchP(p1P,1,p);
                p1P.idoper = idoper;
                return View(p1P);
            }
            else
            {
                IQueryable<P1> p1 = db.P1s.Include(c => c.User);
                p1 = p1.Where(p => p.User.Email == User.Identity.Name);
                p1P.Ps = SearchP(p1P, 1, p);
                p1P.P1s = SearchP1(p1P, 1, p1);
                return View(p1P);
            }
        }
        private IQueryable<P1> SearchP1(P1PHomeVM p1, int count, IQueryable<P1> p1s)
        {
            if (p1.Name != null && count == 1)
            {
                p1s = p1s.Where(p => p.Name == p1.Name);
                return SearchP1(p1, 2, p1s);
            }
            else if (p1.NameAct!=null && (count == 1 || count == 2))
            {
                p1s = p1s.Where(p => p.NameAct ==p1.NameAct);
                return SearchP1(p1, 3, p1s);
            }
            else if (p1.StData != null && p1.FinData != null && (count == 1 || count == 2 || count == 3))
            {
                p1s = p1s.Where(p => p.Data >= p1.StData && p.Data <= p1.FinData);
                return SearchP1(p1, 4, p1s);
            }
            else if (p1.minsum!= 0 && p1.maxsum != 0 && (count == 1 || count == 2 || count == 3 || count == 4))
            {
                p1s = p1s.Where(p => p.Sum >= p1.minsum && p.Sum <= p1.maxsum);
                return SearchP1(p1, 5, p1s);
            }
            else
            {
                return p1s;
            }
        }
        private IQueryable<P> SearchP(P1PHomeVM p1, int count, IQueryable<P> p1s)
        {
            if (p1.Name != null && count == 1)
            {
                p1s = p1s.Where(p => p.Name == p1.Name);
                return SearchP(p1, 2, p1s);
            }
            else if (p1.NameAct != null && (count == 1 || count == 2))
            {
                p1s = p1s.Where(p => p.Operation.NameAct == p1.NameAct);
                return SearchP(p1, 3, p1s);
            }
            else if (p1.StData != null && p1.FinData != null && (count == 1 || count == 2 || count == 3))
            {
                p1s = p1s.Where(p => p.Data >= p1.StData && p.Data <= p1.FinData);
                return SearchP(p1, 4, p1s);
            }
            else if (p1.minsum != 0 && p1.maxsum != 0 && (count == 1 || count == 2 || count == 3 || count == 4))
            {
                p1s = p1s.Where(p => p.Sum >= p1.minsum && p.Sum <= p1.maxsum);
                return SearchP(p1, 5, p1s);
            }
            else
            {
                return p1s;
            }
        }
    }
}