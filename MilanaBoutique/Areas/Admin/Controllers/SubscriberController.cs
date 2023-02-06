using MilanaBoutique.DAL;
using MilanaBoutique.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MilanaBoutique.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="SuperAdmin,Admin")]
    public class SubscriberController : Controller
    {
        private readonly AppDbContext context;

        public SubscriberController(AppDbContext context)
        {
            this.context = context;
        }
        public IActionResult Index(int page=1)
        {
            ViewBag.Currentpage = page;
            ViewBag.TotalPage = Math.Ceiling((decimal)context.Subscribers.Count() / 6);

            List<Subscriber> subscribers = context.Subscribers.Skip((page-1)*6).Take(6).ToList();
            return View(subscribers);
        }
    }
}
