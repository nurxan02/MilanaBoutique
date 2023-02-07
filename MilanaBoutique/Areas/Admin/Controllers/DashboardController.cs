using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MilanaBoutique.DAL;
using MilanaBoutique.Models;
using MilanaBoutique.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MilanaBoutique.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin,Admin")]

    public class DashboardController : Controller
    {
        private readonly AppDbContext context;

        public DashboardController(AppDbContext context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            ProductSizeColor todays = context.ProductSizeColors.Include(c => c.Product).ThenInclude(c => c.Brand).Include(c => c.ProductImages).OrderByDescending(c => c.DailySoldCount).FirstOrDefault();


            BestsellerVM bestsellerVM = new BestsellerVM
            {
                Daily = context.ProductSizeColors.Include(c=>c.Size).Include(c=>c.Color).Include(c => c.Product).ThenInclude(c => c.Brand).Include(c => c.ProductImages).OrderByDescending(c=>c.DailySoldCount).FirstOrDefault(),
                //Monthly = context.ProductSizeColors.Include(c => c.Product).ThenInclude(c => c.Brand).Include(c => c.ProductImages).FirstOrDefault(c => c.Id == monthly.ProductSizeColorId),
            };
            double? startPrice = 0;
            
            List<OrderItem> orderItems = context.OrderItems.Include(c => c.ProductSizeColor).Where(c => c.Order.Date.Day == DateTime.Now.Day && c.ProductSizeColorId == todays.Id && c.Order.StatusId == 2).ToList();
            

            if (bestsellerVM.Daily != null)
            {
                foreach (var item in orderItems)
                {
                    startPrice += item.Price;
                }
                ViewBag.Daily = startPrice;
                return View(bestsellerVM);

            }
            else
            {
                return View();
            }

        }

        public IActionResult Test()
        {
            return View();
        }
    }
}
