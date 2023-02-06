using MilanaBoutique.DAL;
using MilanaBoutique.Models;
using MilanaBoutique.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace MilanaBoutique.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext context;

        public HomeController(AppDbContext context)
        {
            this.context = context;
        }
        
        public IActionResult Index()
        {
            HomeVM home = new HomeVM
            {
                Sliders = context.Sliders.ToList(),
                Brands = context.Brands.Include(b => b.Products).ToList(),
               
   
                ProductColors = context.ProductColors.Include(p => p.Product).ThenInclude(p => p.Comments).Include(p=>p.Product).ThenInclude(p=>p.Category).ThenInclude(c=>c.Gender).Include(p=>p.Product).ThenInclude(p=>p.ProductSizeColors).OrderBy(pc=>pc.Product.Name).ToList(),
                ProductColors2 = context.ProductColors.Include(p => p.Product).ThenInclude(p => p.Comments).ToList(),
                Product = context.Products.Include(p=>p.ProductSizeColors).Include(p=>p.Gender).FirstOrDefault(p=>p.DealOfTheDay==true),
                Settings = context.Settings.FirstOrDefault(),
                Blogs = context.Blogs.Include(c=>c.Comments).ToList()
                
            };

            return View(home);
        }

        public IActionResult Subscribe(string email)
        {
            Subscriber subscriber = context.Subscribers.FirstOrDefault(c => c.Email.Trim().ToLower() == email.Trim().ToLower());
            if (subscriber==null)
            {

                Subscriber news = new Subscriber
                {
                    Email = email
                };

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("finalprojectcodeacademy@gmail.com", "Milana Boutique");
                mail.To.Add(new MailAddress(email));
                mail.Subject = "Thanks for subscribing!";
                string body = string.Empty;
                string link = "sa";
                using (StreamReader reader = new StreamReader("wwwroot/assets/template/SubscribedEmail.html"))
                {
                    body = reader.ReadToEnd();
                }

                mail.Body = body.Replace("{{link}}", link);
                mail.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;

                smtp.Credentials = new NetworkCredential("finalprojectcodeacademy@gmail.com", "shldhrtkghufzhmg");
                smtp.Send(mail);


                context.Subscribers.Add(news);
                context.SaveChanges();


                return Json(new { status=200});

            }
            else
            {
                return Json(new { status = 500 });

            }



        }
      
    }
}
