using MilanaBoutique.DAL;
using MilanaBoutique.Extensions;
using MilanaBoutique.Models;
using MilanaBoutique.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MilanaBoutique.Controllers
{
    public class ContactController : Controller
    {
        private readonly AppDbContext context;
        private readonly IWebHostEnvironment env;

        public ContactController(AppDbContext context, IWebHostEnvironment env)
        {
            this.context = context;
            this.env = env;
        }
        public IActionResult Index()
        {
            ContactVM vm = new ContactVM
            {
                Contact = context.Contact.FirstOrDefault(),
                Stores = context.Stores.ToList()
            };
            

            return View(vm);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]

        public IActionResult Index(Questions question)
        {


            ContactVM vm = new ContactVM
            {
                Contact = context.Contact.FirstOrDefault(),
                Stores = context.Stores.ToList()
            };

            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            Questions questionnew = new Questions
            {
                IsAcces = false,
                Mail = question.Mail,
                Message = question.Message,
                Name = question.Name,
                Phone = question.Phone,
                Subject = question.Subject
            };

            context.Questions.Add(questionnew);
            context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }



    }
}
