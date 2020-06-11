using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HB5.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace HB5.Controllers
{
    [Authorize]
    public class DelController : Controller
    {
        UserContext db;

        public DelController(UserContext user)
        {
            db = user;
        }
        [HttpGet]
        public async Task<IActionResult> PlanDel(int? id)
        {
            Plan p = await db.Plans.FirstAsync(p => p.Id == id);
            if (p != null)
            {
                return View(p);
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> PlanDel(Plan p)
        {
            db.Plans.Remove(p);
            await db.SaveChangesAsync();
            return RedirectToAction("PlanHome", "Home");
        }
        [HttpGet]
        public async Task<IActionResult> OperDel(int? id)
        {
            Plan p = await db.Plans.FirstAsync(p => p.Id == id);
            if (p != null)
            {
                return View(p);
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> OperDel(Operation op)
        {
            db.Operations.Remove(op);
            await db.SaveChangesAsync();
            return RedirectToAction("OperHome", "Home");
        }
    }
}