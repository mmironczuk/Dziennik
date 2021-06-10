using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Dziennik.Data;
using Dziennik.Models;
using EmailService;

namespace Dziennik.Controllers
{
    public class KlasyController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _emailSender;

        public KlasyController(ApplicationDbContext context, IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
        }

        // GET: Klasy
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Klasa.Include(k => k.Wychowawca);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Klasy/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var klasa = await _context.Klasa
                .Include(k => k.Wychowawca)
                .Include(k => k.Uczniowie)
                .FirstOrDefaultAsync(m => m.KlasaId == id);

            var uczniowie = _context.Konta.Where(x => x.KlasaId == null&&x.typ_uzytkownika==2).ToList();
            ViewBag.UczniowieBezKlasy = uczniowie;
            if (klasa == null)
            {
                return NotFound();
            }

            return View(klasa);
        }

        // GET: Klasy/Create
        public IActionResult Create()
        {
            var nauczyciele = _context.Konta.Where(x => x.typ_uzytkownika == 3 && x.Wychowankowie == null).ToList();
            ViewData["Konta"] = nauczyciele;
            return View();
        }

        // POST: Klasy/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("KlasaId,nazwa,KontoId")] Klasa klasa)
        {
            if (ModelState.IsValid)
            {
                _context.Add(klasa);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["KontoId"] = new SelectList(_context.Set<Konto>(), "Id", "Id", klasa.KontoId);
            return View(klasa);
        }

        // GET: Klasy/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var klasa = await _context.Klasa.FindAsync(id);
            if (klasa == null)
            {
                return NotFound();
            }
            var nauczyciele = _context.Konta.Where(x => x.typ_uzytkownika == 3 && x.Wychowankowie == null).ToList();
            ViewData["Konta"] = nauczyciele;
            return View(klasa);
        }

        // POST: Klasy/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("KlasaId,nazwa,KontoId")] Klasa klasa)
        {
            if (id != klasa.KlasaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(klasa);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KlasaExists(klasa.KlasaId))
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
            ViewData["KontoId"] = new SelectList(_context.Set<Konto>(), "Id", "Id", klasa.KontoId);
            return View(klasa);
        }

        // GET: Klasy/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var klasa = await _context.Klasa
                .Include(k => k.Wychowawca)
                .FirstOrDefaultAsync(m => m.KlasaId == id);
            if (klasa == null)
            {
                return NotFound();
            }

            return View(klasa);
        }

        // POST: Klasy/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var klasa = await _context.Klasa.FindAsync(id);
            _context.Klasa.Remove(klasa);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DeleteClass(string id, int ClassId)
        {
            var konto = await _context.Konta.Where(x => x.Id == id).FirstOrDefaultAsync();
            konto.KlasaId = null;
            _context.Update(konto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = ClassId });
        }

        public async Task<IActionResult> AddToClass(string id, int ClassId)
        {
            var konto = await _context.Konta.Where(x => x.Id == id).FirstOrDefaultAsync();
            konto.KlasaId = ClassId;
            _context.Update(konto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = ClassId });
        }

        private bool KlasaExists(int id)
        {
            return _context.Klasa.Any(e => e.KlasaId == id);
        }
    }
}
