using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Dziennik.Data;
using Dziennik.Models;

namespace Dziennik.Controllers
{
    public class PytaniaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PytaniaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Pytania
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Pytanie.Include(p => p.Test);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Pytania/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pytanie = await _context.Pytanie
                .Include(p => p.Test)
                .FirstOrDefaultAsync(m => m.PytanieId == id);
            if (pytanie == null)
            {
                return NotFound();
            }

            return View(pytanie);
        }

        // GET: Pytania/Create
        public IActionResult Create(int id)
        {
            ViewBag.TestId = id;
            //ViewData["TestId"] = new SelectList(_context.Test, "TestId", "TestId");
            return View();
        }

        // POST: Pytania/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PytanieId,tresc,odpA,odpB,odpC,odpD,poprawna,punkty,TestId")] Pytanie pytanie)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pytanie);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details","Testy",new { id=pytanie.TestId});
            }
            ViewData["TestId"] = new SelectList(_context.Test, "TestId", "TestId", pytanie.TestId);
            return View(pytanie);
        }

        // GET: Pytania/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pytanie = await _context.Pytanie.FindAsync(id);
            if (pytanie == null)
            {
                return NotFound();
            }
            ViewData["TestId"] = new SelectList(_context.Test, "TestId", "TestId", pytanie.TestId);
            return View(pytanie);
        }

        // POST: Pytania/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PytanieId,tresc,odpA,odpB,odpC,odpD,poprawna,punkty,TestId")] Pytanie pytanie)
        {
            if (id != pytanie.PytanieId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pytanie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PytanieExists(pytanie.PytanieId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["TestId"] = new SelectList(_context.Test, "TestId", "TestId", pytanie.TestId);
            return View(pytanie);
        }

        // GET: Pytania/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pytanie = await _context.Pytanie
                .Include(p => p.Test)
                .FirstOrDefaultAsync(m => m.PytanieId == id);
            if (pytanie == null)
            {
                return NotFound();
            }

            return View(pytanie);
        }

        // POST: Pytania/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pytanie = await _context.Pytanie.FindAsync(id);
            _context.Pytanie.Remove(pytanie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PytanieExists(int id)
        {
            return _context.Pytanie.Any(e => e.PytanieId == id);
        }
    }
}
