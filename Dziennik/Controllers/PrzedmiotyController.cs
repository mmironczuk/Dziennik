using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Dziennik.Data;
using Dziennik.Models;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace Dziennik.Controllers
{
    public class PrzedmiotyController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PrzedmiotyController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Przedmioty
        public async Task<IActionResult> Index()
        {
            return View(await _context.Przedmiot.ToListAsync());
        }

        // GET: Przedmioty/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var przedmiot = await _context.Przedmiot.Include(x=>x.Pliki)
                .FirstOrDefaultAsync(m => m.PrzedmiotId == id);
            if (przedmiot == null)
            {
                return NotFound();
            }

            return View(przedmiot);
        }

        // GET: Przedmioty/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Przedmioty/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PrzedmiotId,nazwa,dziedzina,tresci")] Przedmiot przedmiot)
        {
            if (ModelState.IsValid)
            {
                _context.Add(przedmiot);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(przedmiot);
        }

        // GET: Przedmioty/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var przedmiot = await _context.Przedmiot.FindAsync(id);
            if (przedmiot == null)
            {
                return NotFound();
            }
            return View(przedmiot);
        }

        // POST: Przedmioty/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PrzedmiotId,nazwa,dziedzina,tresci")] Przedmiot przedmiot)
        {
            if (id != przedmiot.PrzedmiotId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(przedmiot);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PrzedmiotExists(przedmiot.PrzedmiotId))
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
            return View(przedmiot);
        }

        // GET: Przedmioty/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var przedmiot = await _context.Przedmiot
                .FirstOrDefaultAsync(m => m.PrzedmiotId == id);
            if (przedmiot == null)
            {
                return NotFound();
            }

            return View(przedmiot);
        }

        // POST: Przedmioty/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var przedmiot = await _context.Przedmiot.FindAsync(id);
            _context.Przedmiot.Remove(przedmiot);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult DeletePlik(int id, int PrzedmiotId)
        {
            var plik = _context.Plik.Find(id);
            _context.Plik.Remove(plik);
            _context.SaveChanges();
            return RedirectToAction("Details", new { id = PrzedmiotId });
        }

        public IActionResult PobierzPlik(int id)
        {
            var plik = _context.Plik.Find(id);
            return File(plik.plik, "application/octet-stream", plik.nazwa);
        }

        public IActionResult DodaniePliku(IFormFile files, int id)
        {
            if (files != null)
            {
                if (files.Length > 0)
                {
                    //Getting FileName
                    var fileName = Path.GetFileName(files.FileName);
                    //Getting file Extension
                    var fileExtension = Path.GetExtension(fileName);
                    // concatenating  FileName + FileExtension
                    //var newFileName = String.Concat(Convert.ToString(Guid.NewGuid()), fileExtension);
                    //var newFileName = String.Concat(fileName, fileExtension);

                    var objfiles = new Plik()
                    {
                        nazwa = fileName,
                        typ = fileExtension,
                        utworzenie = DateTime.Now,
                        PrzedmiotId=id
                    };

                    using (var target = new MemoryStream())
                    {
                        files.CopyTo(target);
                        objfiles.plik = target.ToArray();
                    }

                    _context.Plik.Add(objfiles);
                    _context.SaveChanges();

                }
            }
            return RedirectToAction("Details", new { id=id });
        }

        private bool PrzedmiotExists(int id)
        {
            return _context.Przedmiot.Any(e => e.PrzedmiotId == id);
        }
    }
}
