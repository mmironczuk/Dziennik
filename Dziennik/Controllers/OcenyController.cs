using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Dziennik.Data;
using Dziennik.Models;
using System.Security.Claims;

namespace Dziennik.Controllers
{
    public class OcenyController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OcenyController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Oceny
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Ocena.Include(o => o.Nauczanie);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Oceny/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ocena = await _context.Ocena
                .Include(o => o.Nauczanie)
                .FirstOrDefaultAsync(m => m.OcenaId == id);
            if (ocena == null)
            {
                return NotFound();
            }

            return View(ocena);
        }

        // GET: Oceny/Create
        public IActionResult Create(int NauczanieId, string UczenId)
        {
            ViewBag.NauczanieId = NauczanieId;
            ViewBag.UczenId = UczenId;
            //ViewData["NauczanieId"] = new SelectList(_context.Nauczanie, "NauczanieId", "NauczanieId");
            return View();
        }

        // POST: Oceny/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OcenaId,wartosc,opis,NauczanieId,KontoId")] Ocena ocena)
        {
            ocena.koncowa = 1;
            ocena.data = DateTime.Now;
            if (ModelState.IsValid)
            {
                _context.Add(ocena);
                await _context.SaveChangesAsync();
                return RedirectToAction("DodajOceny","Nauczania",new { id=ocena.NauczanieId } );
            }
            ViewData["NauczanieId"] = new SelectList(_context.Nauczanie, "NauczanieId", "NauczanieId", ocena.NauczanieId);
            return View(ocena);
        }

        // GET: Oceny/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ocena = await _context.Ocena.FindAsync(id);
            if (ocena == null)
            {
                return NotFound();
            }
            ViewData["NauczanieId"] = new SelectList(_context.Nauczanie, "NauczanieId", "NauczanieId", ocena.NauczanieId);
            return View(ocena);
        }

        // POST: Oceny/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OcenaId,wartosc,opis,NauczanieId,KontoId,koncowa")] Ocena ocena)
        {
            if (id != ocena.OcenaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ocena);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OcenaExists(ocena.OcenaId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("DodajOceny", "Nauczania", new { id = ocena.NauczanieId });
            }
            ViewData["NauczanieId"] = new SelectList(_context.Nauczanie, "NauczanieId", "NauczanieId", ocena.NauczanieId);
            return View(ocena);
        }

        // GET: Oceny/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ocena = await _context.Ocena
                .Include(o => o.Nauczanie)
                .FirstOrDefaultAsync(m => m.OcenaId == id);
            if (ocena == null)
            {
                return NotFound();
            }

            return View(ocena);
        }

        // POST: Oceny/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ocena = await _context.Ocena.FindAsync(id);
            _context.Ocena.Remove(ocena);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> OcenyUcznia()
        {
            var UczenId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var applicationDbContext = _context.Ocena.Include(x => x.Nauczanie).Where(x => x.KontoId == UczenId);
            ViewBag.Przedmioty = _context.Przedmiot.ToList();
            return View(await applicationDbContext.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> OcenyUcznia(DateTime DateFirst, DateTime DateSecond)
        {
            var UczenId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var applicationDbContext = _context.Ocena.Include(x => x.Nauczanie).Where(x => x.KontoId == UczenId && x.data <= DateSecond && x.data >= DateFirst);
            ViewBag.Przedmioty = _context.Przedmiot.ToList();
            ViewBag.Message = $"Oceny z okresu od {DateFirst.ToShortDateString()} do {DateSecond.ToShortDateString()}";
            return View(await applicationDbContext.ToListAsync());
        }

        private bool OcenaExists(int id)
        {
            return _context.Ocena.Any(e => e.OcenaId == id);
        }
    }
}
