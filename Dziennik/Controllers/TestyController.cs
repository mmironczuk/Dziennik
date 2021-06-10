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
    public class TestyController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TestyController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Testy
        public async Task<IActionResult> Index()
        {
            var NauczycielId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var applicationDbContext = _context.Test.Include(t => t.Nauczanie).Include(t=>t.Nauczanie.Klasa).Include(t=>t.Nauczanie.Przedmiot).Where(x=>x.Nauczanie.KontoId==NauczycielId);
            return View(await applicationDbContext.ToListAsync());
        }

        public IActionResult TestUcznia()
        {
            var UczenId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            Konto uczen = _context.Konta.Find(UczenId);
            var Testy = _context.Test.Include(x => x.Nauczanie).Include(x => x.Nauczanie.Przedmiot).Where(x => x.Nauczanie.KlasaId == uczen.KlasaId).ToList();
            //ViewBag.Testy = Testy;
            return View(Testy);
        }

        public IActionResult NastepnePytanie(int id, int points, int numer_pytania)
        {
            var Test = _context.Test.Include(x => x.Pytania).Where(x => x.TestId == id).FirstOrDefault();
            List<Pytanie> pytania = Test.Pytania.ToList();
            var pytanie = pytania[numer_pytania-1];

            PytanieViewModel p = new PytanieViewModel
            {
                PytanieId=pytanie.PytanieId,
                tresc = pytanie.tresc,
                odpA=pytanie.odpA,
                odpB=pytanie.odpB,
                odpC=pytanie.odpC,
                odpD=pytanie.odpD,
                poprawna=pytanie.poprawna,
                punkty=pytanie.punkty,
                TestId=pytanie.TestId,
                points=points,
                numer_pytania=numer_pytania
            };
            return View(p);

        }

        [HttpPost]
        public IActionResult NastepnePytanie([Bind("PytanieId,odp,points,numer_pytania,TestId")] PytanieViewModel pytanie)
        {
            var Test = _context.Test.Include(x => x.Pytania).Where(x => x.TestId == pytanie.TestId).FirstOrDefault();
            List<Pytanie> pytania = Test.Pytania.ToList();
            Pytanie x = _context.Pytanie.Where(x => x.PytanieId == pytanie.PytanieId).FirstOrDefault();
            if (pytanie.odp == x.poprawna) pytanie.points += x.punkty;
            if (pytanie.numer_pytania < pytania.Count)
            {
                return RedirectToAction("NastepnePytanie", new { id = pytanie.TestId, points = pytanie.points, numer_pytania = (pytanie.numer_pytania+1) });
            }
            else
            {
                double sum = 0;
                foreach(var p in pytania)
                {
                    sum += p.punkty;
                }

                double wynik = (Convert.ToDouble(pytanie.points) / sum) * 100;
                var UczenId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                Ocena ocena = new Ocena
                {
                    opis = "Ocena z testu: " + Test.nazwa,
                    data = DateTime.Now,
                    KontoId=UczenId,
                    NauczanieId=Test.NauczanieId,
                    koncowa=1
                };
                if (wynik < 30) ocena.wartosc = "1";
                else if (wynik < 50 && wynik >= 30) ocena.wartosc = "2";
                else if (wynik >= 50 && wynik < 70) ocena.wartosc = "3";
                else if (wynik >= 70 && wynik < 85) ocena.wartosc = "4";
                else ocena.wartosc = "5";
                _context.Ocena.Add(ocena);
                _context.SaveChanges();
                return RedirectToAction("WynikTestu", new { punkty = pytanie.points, wynik=wynik, ocena=ocena.wartosc });
            }

        }

        public IActionResult WynikTestu(int punkty, double wynik, string ocena)
        {
            ViewBag.punkty = punkty;
            ViewBag.wynik = wynik;
            ViewBag.ocena = ocena;
            return View();
        }

        // GET: Testy/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var test = await _context.Test
                .Include(t => t.Nauczanie).Include(t=>t.Pytania).Include(t=>t.Nauczanie.Przedmiot).Include(t=>t.Nauczanie.Klasa)
                .FirstOrDefaultAsync(m => m.TestId == id);
            if (test == null)
            {
                return NotFound();
            }

            return View(test);
        }

        // GET: Testy/Create
        public IActionResult Create()
        {
            //ViewData["NauczanieId"] = new SelectList(_context.Nauczanie, "NauczanieId", "NauczanieId");
            var NauczycielId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var Nauczania = _context.Nauczanie.Include(x => x.Klasa).Include(x => x.Przedmiot).Where(x => x.KontoId == NauczycielId).ToList();
            ViewBag.Nauczania = Nauczania;

            return View();
        }

        // POST: Testy/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TestId,nazwa,opis,czas_trwania,NauczanieId")] Test test)
        {
            if (ModelState.IsValid)
            {
                _context.Add(test);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["NauczanieId"] = new SelectList(_context.Nauczanie, "NauczanieId", "NauczanieId", test.NauczanieId);
            var NauczycielId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var Nauczania = _context.Nauczanie.Include(x => x.Klasa).Include(x => x.Przedmiot).Where(x => x.KontoId == NauczycielId).ToList();
            ViewBag.Nauczania = Nauczania;
            return View(test);
        }

        // GET: Testy/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var test = await _context.Test.FindAsync(id);
            if (test == null)
            {
                return NotFound();
            }
            //ViewData["NauczanieId"] = new SelectList(_context.Nauczanie, "NauczanieId", "NauczanieId", test.NauczanieId);
            var NauczycielId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var Nauczania = _context.Nauczanie.Include(x => x.Klasa).Include(x => x.Przedmiot).Where(x => x.KontoId == NauczycielId).ToList();
            ViewBag.Nauczania = Nauczania;
            return View(test);
        }

        // POST: Testy/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TestId,nazwa,opis,czas_trwania,NauczanieId")] Test test)
        {
            if (id != test.TestId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(test);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TestExists(test.TestId))
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
            //ViewData["NauczanieId"] = new SelectList(_context.Nauczanie, "NauczanieId", "NauczanieId", test.NauczanieId);
            var NauczycielId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var Nauczania = _context.Nauczanie.Include(x => x.Klasa).Include(x => x.Przedmiot).Where(x => x.KontoId == NauczycielId).ToList();
            ViewBag.Nauczania = Nauczania;
            return View(test);
        }

        // GET: Testy/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var test = await _context.Test
                .Include(t => t.Nauczanie)
                .FirstOrDefaultAsync(m => m.TestId == id);
            if (test == null)
            {
                return NotFound();
            }

            return View(test);
        }

        // POST: Testy/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var test = await _context.Test.FindAsync(id);
            _context.Test.Remove(test);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TestExists(int id)
        {
            return _context.Test.Any(e => e.TestId == id);
        }
    }
}
