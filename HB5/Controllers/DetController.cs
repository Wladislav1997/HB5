using System;
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
        public async Task<IActionResult> OperDet(int id, string st)
        {
            Operation op = await db.Operations.FirstAsync(p => p.Id == id);
            OperDetVM oper = new OperDetVM();
            oper.op = op;
            oper.Id = op.PlanId;
            oper.st = st;
            return View(oper);
        }
        [HttpPost]
        public IActionResult OperDet(OperDetVM oper)
        {
            if (oper.st==null)
            {
                return RedirectToAction("OperHome", "Home",new { st = oper.st });
            }
            return RedirectToAction("OperHome", "Home",new { idplan=oper.Id});
        }
        [HttpGet]
        public async Task<IActionResult> PDet(int id)
        {
            P p = await db.Ps.FirstAsync(p => p.Id == id);
            Operation op = await db.Operations.FirstAsync(u => u.Id == p.OperationId);
            PDetVM pDet = new PDetVM();
            pDet.p = p;
            pDet.NameAct = op.NameAct;
            pDet.idoper = p.OperationId;
            return View(pDet);
        }
        [HttpPost]
        public IActionResult PDet(PDetVM pDet)
        {
            return RedirectToAction("P1PHome", "Home", new { idoper = pDet.idoper });
        }
        [HttpGet]
        public async Task<IActionResult> P1Det(int id)
        {
            P1 p1 = await db.P1s.FirstAsync(p => p.Id == id);
            return View(p1);
        }
        [HttpPost]
        public IActionResult P1Det()
        {
            return RedirectToAction("P1PHome", "Home");
        }
    }
}