using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Dziennik.Data;
using Dziennik.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Dziennik.Controllers
{
    public class NauczaniaController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Konto> _userManager;

        public NauczaniaController(ApplicationDbContext context, UserManager<Konto> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Nauczania
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Nauczanie.Include(n => n.Klasa).Include(n => n.Przedmiot).Include(n => n.Konto).OrderBy(x => x.KlasaId);
            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> WyborKlasy()
        {
            var NauczycielId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var applicationDbContext = _context.Nauczanie.Include(n => n.Klasa).Include(n => n.Przedmiot).Include(n => n.Konto).Where(x => x.KontoId == NauczycielId.ToString()).OrderBy(x => x.KlasaId);
            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> DodajOceny(int id)
        {
            Nauczanie Nauczanie = _context.Nauczanie.Include(x => x.Klasa).Include(x => x.Ocena).Where(x => x.NauczanieId == id).FirstOrDefault();
            var Uczniowie = await _context.Konta.Where(x => x.KlasaId == Nauczanie.KlasaId&&x.typ_uzytkownika==2).ToListAsync();
            ViewBag.Uczniowie = Uczniowie;
            ViewBag.NauczanieId = Nauczanie.NauczanieId;
            ViewBag.Oceny = Nauczanie.Ocena.ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DodajOceny(DateTime DateFirst, DateTime DateSecond, int id)
        {
            Nauczanie Nauczanie = _context.Nauczanie.Include(x => x.Klasa).Include(x => x.Ocena).Where(x => x.NauczanieId == id).FirstOrDefault();
            var Uczniowie = await _context.Konta.Where(x => x.KlasaId == Nauczanie.KlasaId && x.typ_uzytkownika == 2).ToListAsync();
            ViewBag.Uczniowie = Uczniowie;
            ViewBag.NauczanieId = Nauczanie.NauczanieId;
            ViewBag.Oceny = _context.Ocena.Where(x => x.NauczanieId == id && x.data <= DateSecond && x.data >= DateFirst).ToList();
            ViewBag.Message = $"Oceny z okresu od {DateFirst.ToShortDateString()} do {DateSecond.ToShortDateString()}";
            return View();
        }


        // GET: Nauczania/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nauczanie = await _context.Nauczanie
                .Include(n => n.Klasa)
                .Include(n => n.Przedmiot)
                .FirstOrDefaultAsync(m => m.NauczanieId == id);
            if (nauczanie == null)
            {
                return NotFound();
            }

            return View(nauczanie);
        }

        // GET: Nauczania/Create
        public IActionResult Create()
        {
            ViewData["KlasaId"] = new SelectList(_context.Klasa, "KlasaId", "nazwa");
            ViewData["PrzedmiotId"] = new SelectList(_context.Przedmiot, "PrzedmiotId", "nazwa");
            var nauczyciele = _context.Konta.Where(x => x.typ_uzytkownika == 3).ToList();
            ViewBag.Nauczyciele = nauczyciele;
            return View();
        }

        // POST: Nauczania/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NauczanieId,KlasaId,PrzedmiotId,KontoId")] Nauczanie nauczanie)
        {
            Nauczanie n = _context.Nauczanie.Where(x => x.KlasaId == nauczanie.KlasaId && x.KontoId == nauczanie.KontoId && x.PrzedmiotId == nauczanie.PrzedmiotId).FirstOrDefault();
            ViewData["KlasaId"] = new SelectList(_context.Klasa, "KlasaId", "nazwa", nauczanie.KlasaId);
            ViewData["PrzedmiotId"] = new SelectList(_context.Przedmiot, "PrzedmiotId", "nazwa", nauczanie.PrzedmiotId);
            var nauczyciele = _context.Konta.Where(x => x.typ_uzytkownika == 3).ToList();
            ViewBag.Nauczyciele = nauczyciele;
            if (n!=null)
            {
                ViewBag.Fail = "Takie zestawienie już istnieje!";
                return View(nauczanie);
            }
            if (ModelState.IsValid)
            {
                _context.Add(nauczanie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(nauczanie);
        }

        // GET: Nauczania/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nauczanie = await _context.Nauczanie.FindAsync(id);
            if (nauczanie == null)
            {
                return NotFound();
            }
            ViewData["KlasaId"] = new SelectList(_context.Klasa, "KlasaId", "nazwa", nauczanie.KlasaId);
            ViewData["PrzedmiotId"] = new SelectList(_context.Przedmiot, "PrzedmiotId", "nazwa", nauczanie.PrzedmiotId);
            var nauczyciele = _context.Konta.Where(x => x.typ_uzytkownika == 3).ToList();
            ViewBag.Nauczyciele = nauczyciele;
            return View(nauczanie);
        }

        // POST: Nauczania/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NauczanieId,KlasaId,PrzedmiotId")] Nauczanie nauczanie)
        {
            if (id != nauczanie.NauczanieId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nauczanie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NauczanieExists(nauczanie.NauczanieId))
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
            ViewData["KlasaId"] = new SelectList(_context.Klasa, "KlasaId", "nazwa", nauczanie.KlasaId);
            ViewData["PrzedmiotId"] = new SelectList(_context.Przedmiot, "PrzedmiotId", "nazwa", nauczanie.PrzedmiotId);
            var nauczyciele = _context.Konta.Where(x => x.typ_uzytkownika == 3).ToList();
            ViewBag.Nauczyciele = nauczyciele;
            return View(nauczanie);
        }

        // GET: Nauczania/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nauczanie = await _context.Nauczanie
                .Include(n => n.Klasa)
                .Include(n => n.Przedmiot)
                .FirstOrDefaultAsync(m => m.NauczanieId == id);
            if (nauczanie == null)
            {
                return NotFound();
            }

            return View(nauczanie);
        }

        // POST: Nauczania/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var nauczanie = await _context.Nauczanie.FindAsync(id);
            _context.Nauczanie.Remove(nauczanie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NauczanieExists(int id)
        {
            return _context.Nauczanie.Any(e => e.NauczanieId == id);
        }
    }
}
