using Dziennik.Data;
using Dziennik.Models;
using EmailService;
using Microsoft.EntityFrameworkCore;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dziennik.Sender
{
    public class Job_SendMail : IJob
    {
        private readonly IEmailSender _emailSender;
        private readonly ApplicationDbContext _context;
        public Job_SendMail(IEmailSender emailSender, ApplicationDbContext context)
        {
            _emailSender = emailSender;
            _context = context;
        }

        public Task Execute(IJobExecutionContext context)
        {
            //Message message = new Message(new string [] { "michaszek1212@wp.pl"}, "Test tego", "Tresc maila");
            //_emailSender.SendEmail(message);
            Send();
            return Task.CompletedTask;
        }

        public void Send()
        {
            var Uczniowie = _context.Konta.Include(x=>x.Rodzic).Where(x => x.RodzicId != null).ToList();
            var Przedmioty = _context.Przedmiot.ToList();
            var Oceny = _context.Ocena.Include(x=>x.Nauczanie).Where(x => x.data.Month == DateTime.Now.Month).ToList();
            string tresc = "";
            Message message;
            foreach (Konto u in Uczniowie)
            {
                tresc = $"<p><b>Zestaw ocen za miesiąc {DateTime.Today.Month}:</b></p>";
                foreach (Przedmiot p in Przedmioty)
                {
                    tresc += $"<p>{p.nazwa}: ";
                    foreach(Ocena o in Oceny)
                    {
                        if (o.KontoId == u.Id && o.Nauczanie.PrzedmiotId == p.PrzedmiotId) tresc += $" {o.wartosc}, ";
                    }
                    tresc += "</p>";
                }
                message = new Message(new string[] { u.Rodzic.Email},"Miesięczne zestawienie ocen dziecka", tresc);
                _emailSender.SendEmail(message);
            }
        }
    }
}
