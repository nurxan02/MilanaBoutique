﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MilanaBoutique.DAL;
using MilanaBoutique.ViewModels;
using System.Linq;

namespace MilanaBoutique.Controllers
{
    public class AboutController : Controller
    {
        private readonly AppDbContext context;

        public AboutController(AppDbContext context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            AboutVM aboutVM = new AboutVM
            { Brands = context.Brands.ToList(),
                 AboutUs = context.AboutUs.FirstOrDefault(),
                Questions = context.Questions.Where(c => c.IsAcces == true).ToList(),
                Members = context.Members.Include(c=>c.SocialMedias).ToList()
            };
            return View(aboutVM);
        }
    }
}
