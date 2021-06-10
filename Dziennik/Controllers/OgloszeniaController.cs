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
using System.Security.Claims;

namespace Dziennik.Controllers
{
    public class OgloszeniaController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _emailSender;

        public OgloszeniaController(ApplicationDbContext context, IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
        }

        // GET: Ogloszenia
        public async Task<IActionResult> Index()
        {
            //var applicationDbContext = _context.Ogloszenie.Include(o => o.Nauczania);
            var Ogloszenia = _context.Ogloszenie.Include(x => x.Nauczania).Include(x => x.Nauczania.Konto).Where(x => x.akceptacja == 2).ToList();
            ViewBag.Ogloszenia = Ogloszenia;
            return View();
        }

        public async Task<IActionResult> Akceptacja(int? id)
        {
            Ogloszenie ogloszenie = _context.Ogloszenie.Find(id);
            ogloszenie.akceptacja = 1;
            _context.Update(ogloszenie);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // GET: Ogloszenia/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ogloszenie = await _context.Ogloszenie
                .Include(o => o.Nauczania)
                .FirstOrDefaultAsync(m => m.OgloszenieId == id);
            if (ogloszenie == null)
            {
                return NotFound();
            }

            return View(ogloszenie);
        }

        // GET: Ogloszenia/Create
        public IActionResult Create()
        {
            ViewData["NauczanieId"] = new SelectList(_context.Nauczanie, "NauczanieId", "NauczanieId");
            return View();
        }

        // POST: Ogloszenia/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OgloszenieId,NauczanieId,nazwa,opis")] Ogloszenie ogloszenie)
        {
            ogloszenie.data = DateTime.Now;
            if(ogloszenie.NauczanieId==0)
            {
                ogloszenie.wszyscy = 1;
                ogloszenie.akceptacja = 2;
                ogloszenie.NauczanieId = 1;
            }
            else
            {
                ogloszenie.wszyscy = 2;
                ogloszenie.akceptacja = 1;

                var NauczycielId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                Klasa k = _context.Klasa.Where(x => x.KontoId == NauczycielId).FirstOrDefault();

                Nauczanie n = _context.Nauczanie.Where(x => x.KlasaId == k.KlasaId && x.KontoId==NauczycielId).FirstOrDefault();
                if(n!=null)
                {
                    ogloszenie.NauczanieId = n.NauczanieId;
                    var Uczniowie = _context.Konta.Include(x => x.Rodzic).Where(x => x.KlasaId == n.KlasaId).ToList();
                    foreach(var u in Uczniowie)
                    {
                        if(u.Rodzic!=null)
                        {
                            var message = new Message(new string[] { u.Rodzic.Email }, ogloszenie.nazwa, ogloszenie.opis);
                            _emailSender.SendEmail(message);
                        }
                    }
                }
                else
                {
                    ViewBag.Message = "Brak klasy wychowawczej";
                    return View(ogloszenie);
                }
            }
            if (ModelState.IsValid)
            {
                _context.Add(ogloszenie);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index","Home");
            }
            ViewData["NauczanieId"] = new SelectList(_context.Nauczanie, "NauczanieId", "NauczanieId", ogloszenie.NauczanieId);
            return View(ogloszenie);
        }

        // GET: Ogloszenia/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ogloszenie = await _context.Ogloszenie.FindAsync(id);
            if (ogloszenie == null)
            {
                return NotFound();
            }
            ViewData["NauczanieId"] = new SelectList(_context.Nauczanie, "NauczanieId", "NauczanieId", ogloszenie.NauczanieId);
            return View(ogloszenie);
        }

        // POST: Ogloszenia/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OgloszenieId,NauczanieId,nazwa,opis,data")] Ogloszenie ogloszenie)
        {
            if (id != ogloszenie.OgloszenieId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ogloszenie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OgloszenieExists(ogloszenie.OgloszenieId))
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
            ViewData["NauczanieId"] = new SelectList(_context.Nauczanie, "NauczanieId", "NauczanieId", ogloszenie.NauczanieId);
            return View(ogloszenie);
        }

        // GET: Ogloszenia/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ogloszenie = await _context.Ogloszenie
                .Include(o => o.Nauczania)
                .FirstOrDefaultAsync(m => m.OgloszenieId == id);
            if (ogloszenie == null)
            {
                return NotFound();
            }

            return View(ogloszenie);
        }

        // POST: Ogloszenia/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ogloszenie = await _context.Ogloszenie.FindAsync(id);
            _context.Ogloszenie.Remove(ogloszenie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> OgloszeniaAkceptacja()
        {
            
            
            return View();
        }

        private bool OgloszenieExists(int id)
        {
            return _context.Ogloszenie.Any(e => e.OgloszenieId == id);
        }
    }
}
