using Dziennik.Data;
using Dziennik.Models;
using Dziennik.Sender;
using EmailService;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Dziennik.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IEmailSender _emailSender;
        private readonly SignInManager<Konto> _SignInManager;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, IEmailSender emailSender, SignInManager<Konto> SignInManager, ApplicationDbContext context)
        {
            _logger = logger;
            _emailSender = emailSender;
            _SignInManager = SignInManager;
            _context = context;
        }

        public IActionResult Index()
        {
            if (!_SignInManager.IsSignedIn(User))
            {
                var Ogloszenia = _context.Ogloszenie.Where(x => x.akceptacja == 1 && x.wszyscy == 1).ToList();
                ViewBag.Ogloszenia = Ogloszenia;
            }
            else if(User.IsInRole("Uczen"))
            {
                var UczenId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                Konto uczen = _context.Konta.Find(UczenId);
                var Ogloszenia = _context.Ogloszenie.Include(x => x.Nauczania).Where(x => x.Nauczania.KlasaId == uczen.KlasaId || (x.akceptacja == 1 && x.wszyscy == 1)).OrderBy(x=>x.data).ToList();
                ViewBag.Ogloszenia = Ogloszenia;
            }
            else if(User.IsInRole("Rodzic"))
            {
                var RodzicId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                Konto uczen = _context.Konta.Where(x => x.RodzicId == RodzicId).FirstOrDefault();
                var Ogloszenia = _context.Ogloszenie.Include(x => x.Nauczania).Where(x => x.Nauczania.KlasaId == uczen.KlasaId || (x.akceptacja == 1 && x.wszyscy == 1)).OrderBy(x => x.data).ToList();
                ViewBag.Ogloszenia = Ogloszenia;
            }
            else if(User.IsInRole("Nauczyciel"))
            {
                var NauczycielId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                Konto nauczyciel = _context.Konta.Find(NauczycielId);
                var Ogloszenia = _context.Ogloszenie.Include(x => x.Nauczania).Where(x => x.Nauczania.KontoId == nauczyciel.Id || (x.akceptacja == 1 && x.wszyscy == 1)).OrderBy(x => x.data).ToList();
                ViewBag.Ogloszenia = Ogloszenia;
            }
            else
            {
                var Ogloszenia = _context.Ogloszenie.Where(x => x.akceptacja == 1).ToList();
                ViewBag.Ogloszenia = Ogloszenia;
            }
            return View();
        }
        public void MaileOcen()
        {

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
