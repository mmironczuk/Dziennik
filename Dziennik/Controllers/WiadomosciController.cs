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
    public class WiadomosciController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WiadomosciController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Wiadomosci
        public async Task<IActionResult> Index()
        {
            var KontoId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var WiadOdebrane = _context.Wiadomosc.Include(x=>x.Nadawca).Where(x => x.OdbiorcaId == KontoId).ToList();
            var WiadWyslane = _context.Wiadomosc.Include(x=>x.Odbiorca).Where(x => x.NadawcaId == KontoId).ToList();

            ViewBag.WiadOdebrane = WiadOdebrane;
            ViewBag.WiadWyslane = WiadWyslane;

            //var applicationDbContext = _context.Wiadomosc.Include(w => w.Nadawca).Include(w => w.Odbiorca);
            return View();
        }

        // GET: Wiadomosci/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wiadomosc = await _context.Wiadomosc
                .Include(w => w.Nadawca)
                .Include(w => w.Odbiorca)
                .FirstOrDefaultAsync(m => m.WiadomoscId == id);
            if (wiadomosc == null)
            {
                return NotFound();
            }

            return View(wiadomosc);
        }

        public IActionResult Odpowiedz(int id)
        {
            var wiad = _context.Wiadomosc.Include(x=>x.Nadawca).Where(x=>x.WiadomoscId==id).FirstOrDefault();
            wiad.czy_odczytana = 1;
            _context.Update(wiad);
            _context.SaveChanges();
            ViewBag.Odbiorca = wiad.Nadawca;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Odpowiedz([Bind("WiadomoscId,tytul,tresc,OdbiorcaId")] Wiadomosc wiadomosc)
        {
            var KontoId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            wiadomosc.NadawcaId = KontoId;
            wiadomosc.data = DateTime.Now;
            wiadomosc.czy_odczytana = 2;

            if (ModelState.IsValid)
            {
                _context.Add(wiadomosc);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            List<Konto> konta = new List<Konto>();
            if (User.IsInRole("Rodzic")) konta = _context.Konta.Where(x => x.typ_uzytkownika == 3).ToList();
            else konta = _context.Konta.Where(x => x.typ_uzytkownika == 4).ToList();

            ViewBag.Konta = konta;
            return View(wiadomosc);
        }

        // GET: Wiadomosci/Create
        public IActionResult Create()
        {
            //ViewData["NadawcaId"] = new SelectList(_context.Konta, "Id", "Id");
            //ViewData["OdbiorcaId"] = new SelectList(_context.Konta, "Id", "Id");

            List<Konto> konta = new List<Konto>();
            if (User.IsInRole("Rodzic")) konta = _context.Konta.Where(x => x.typ_uzytkownika == 3).ToList();
            else konta = _context.Konta.Where(x => x.typ_uzytkownika == 4).ToList();

            ViewBag.Konta = konta;

            return View();
        }

        // POST: Wiadomosci/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("WiadomoscId,tytul,tresc,OdbiorcaId")] Wiadomosc wiadomosc)
        {
            var KontoId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            wiadomosc.NadawcaId = KontoId;
            wiadomosc.data = DateTime.Now;
            wiadomosc.czy_odczytana = 2;

            if (ModelState.IsValid)
            {
                _context.Add(wiadomosc);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            List<Konto> konta = new List<Konto>();
            if (User.IsInRole("Rodzic")) konta = _context.Konta.Where(x => x.typ_uzytkownika == 3).ToList();
            else konta = _context.Konta.Where(x => x.typ_uzytkownika == 4).ToList();

            ViewBag.Konta = konta;
            return View(wiadomosc);
        }

        // GET: Wiadomosci/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wiadomosc = await _context.Wiadomosc.FindAsync(id);
            if (wiadomosc == null)
            {
                return NotFound();
            }
            ViewData["NadawcaId"] = new SelectList(_context.Konta, "Id", "Id", wiadomosc.NadawcaId);
            ViewData["OdbiorcaId"] = new SelectList(_context.Konta, "Id", "Id", wiadomosc.OdbiorcaId);
            return View(wiadomosc);
        }

        // POST: Wiadomosci/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("WiadomoscId,tytul,tresc,data,OdbiorcaId,NadawcaId,czy_odczytana")] Wiadomosc wiadomosc)
        {
            if (id != wiadomosc.WiadomoscId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(wiadomosc);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WiadomoscExists(wiadomosc.WiadomoscId))
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
            ViewData["NadawcaId"] = new SelectList(_context.Konta, "Id", "Id", wiadomosc.NadawcaId);
            ViewData["OdbiorcaId"] = new SelectList(_context.Konta, "Id", "Id", wiadomosc.OdbiorcaId);
            return View(wiadomosc);
        }

        // GET: Wiadomosci/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wiadomosc = await _context.Wiadomosc
                .Include(w => w.Nadawca)
                .Include(w => w.Odbiorca)
                .FirstOrDefaultAsync(m => m.WiadomoscId == id);
            if (wiadomosc == null)
            {
                return NotFound();
            }

            return View(wiadomosc);
        }

        // POST: Wiadomosci/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var wiadomosc = await _context.Wiadomosc.FindAsync(id);
            _context.Wiadomosc.Remove(wiadomosc);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WiadomoscExists(int id)
        {
            return _context.Wiadomosc.Any(e => e.WiadomoscId == id);
        }
    }
}
