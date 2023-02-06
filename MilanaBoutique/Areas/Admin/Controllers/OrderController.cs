using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MilanaBoutique.DAL;
using MilanaBoutique.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MilanaBoutique.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin,Admin")]

    public class OrderController : Controller
    {
        private readonly AppDbContext context;
        private readonly UserManager<AppUser> _userManager;

        public OrderController(AppDbContext context, UserManager<AppUser> userManager)
        {
            this.context = context;
            _userManager = userManager;
        }
        public IActionResult Index(int page = 1)
        {
            ViewBag.Status = context.Statuses.Include(s => s.Orders).ToList();
            ViewBag.CurrentPage = page;
            ViewBag.TotalPage = Math.Ceiling((decimal)context.Orders.Count() / 5);
            List<Order> orders = context.Orders.OrderByDescending(p => p.Id).Include(b => b.OrderItems).ThenInclude(o => o.ProductSizeColor).Include(p => p.AppUser).Skip((page - 1) * 5).Take(5).ToList();
            return View(orders);
        }

        public IActionResult Edit(int id)
        {

            ViewBag.Status = context.Statuses.Include(s => s.Orders).ThenInclude(o => o.OrderItems).ToList();

            Order order = context.Orders.Include(o => o.Status).Include(o => o.OrderItems).ThenInclude(o => o.ProductSizeColor).ThenInclude(p => p.Size).Include(o => o.OrderItems).ThenInclude(o => o.ProductSizeColor).ThenInclude(p => p.Color).Include(o => o.AppUser).FirstOrDefault(o => o.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Edit(Order order)
        {
            ViewBag.Status = context.Statuses.Include(s => s.Orders).ThenInclude(o => o.OrderItems).ToList();

            Order exist = context.Orders.Include(o => o.Status).Include(o => o.OrderItems).ThenInclude(o => o.ProductSizeColor).ThenInclude(p => p.Size).Include(o => o.OrderItems).ThenInclude(o => o.ProductSizeColor).ThenInclude(p => p.Color).Include(o => o.AppUser).FirstOrDefault(o => o.Id == order.Id);



            //if (!ModelState.IsValid)
            //{
            //    return View(exist);
            //}


            exist.MessageToUser = order.MessageToUser;
            exist.StatusId = order.StatusId;


            if (exist.StatusId == 2)
            {
                foreach (OrderItem item in exist.OrderItems)
                {
                    ProductSizeColor product = context.ProductSizeColors.Include(p => p.Product).FirstOrDefault(p => p.Id == item.ProductSizeColor.Id);
                    product.Product.TotalSold += item.Count;
                    product.TotalSold += item.Count;
                    product.TotalStock -= item.Count;

                }
            }


            context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Dailycount()
        {


            List<Order> orders = context.Orders.Where(c => c.Date.Day == DateTime.Now.Day).ToList();
            if (orders == null)
            {
                return Json(0);
            }
            else
            {
                return Json(orders.Count);

            }


        }

        public IActionResult Pendingcount()
        {
            List<Order> orders = context.Orders.Include(c => c.Status).Where(c => c.StatusId == 1).ToList();
            if (orders == null)
            {
                return Json(0);
            }
            else
            {
                return Json(orders.Count);

            }
        }

        public IActionResult Dailyrevenue()
        {
            double? revenue = 0;
            List<Order> orders = context.Orders.Include(c => c.Status).Where(c => c.Date.Day == DateTime.Now.Day && c.StatusId == 2).ToList();
            if (orders == null)
            {
                return Json(0);
            }
            else
            {
                foreach (var item in orders)
                {
                    revenue += item.TotalPrice;
                };
                return Json(revenue);

            }
        }

        public IActionResult Monthlyrevenue()
        {
            double? revenue = 0;
            List<Order> orders = context.Orders.Include(c => c.Status).Where(c => c.Date.Month == DateTime.Now.Month && c.StatusId == 2).ToList();
            if (orders == null)
            {
                return Json(0);
            }
            else
            {
                foreach (var item in orders)
                {
                    revenue += item.TotalPrice;
                };
                return Json(revenue);

            }
        }
        public IActionResult OrderedMan()
        {
            int totalsold = 0;
            List<ProductSizeColor> productSizeColors = context.ProductSizeColors.Include(c => c.Product).Where(c => c.Product.GenderId == 1).ToList();

            if (productSizeColors != null)
            {
                foreach (var item in productSizeColors)
                {
                    totalsold += item.TotalSold;
                }
                return Json(totalsold);


            }
            else
            {
                return Json(0);
            }

        }
        public IActionResult OrderedWoman()
        {
            int totalsold = 0;
            List<ProductSizeColor> productSizeColors = context.ProductSizeColors.Include(c => c.Product).Where(c => c.Product.GenderId == 2).ToList();

            if (productSizeColors != null)
            {
                foreach (var item in productSizeColors)
                {
                    totalsold += item.TotalSold;
                }
                return Json(totalsold);


            }
            else
            {
                return Json(0);
            }

        }

        public IActionResult Improving()
        {
            List<OrderItem> dailyOrderItems = context.OrderItems.Include(c => c.Order).ThenInclude(c => c.Status).Include(c => c.ProductSizeColor).ThenInclude(c => c.Product).Where(c => c.Order.Date.Day == DateTime.Now.Day && c.Order.StatusId == 2).ToList();
            //foreach (OrderItem item in dailyOrderItems)
            //{

            //    ProductSizeColor product = context.ProductSizeColors.FirstOrDefault(c => c.Id == item.ProductSizeColorId);

            //    product.DailySoldCount = 0;

            //}

            foreach (var item in context.ProductSizeColors)
            {
                item.DailySoldCount = 0;
            }

            foreach (OrderItem item in dailyOrderItems)
            {

                ProductSizeColor product = context.ProductSizeColors.FirstOrDefault(c => c.Id == item.ProductSizeColorId);

                product.DailySoldCount += item.Count;

            }
            context.SaveChanges();

            if (dailyOrderItems.Count == 0)
            {
                return Json(new { status = 201 });
            }

            return Json(dailyOrderItems.Count);

        }

        public IActionResult PendingPartial()
        {
            List<Order> pendings = context.Orders.Include(c => c.Status).Include(c => c.AppUser).Where(c => c.StatusId == 1).OrderByDescending(c => c.Id).Take(3).ToList();

            return PartialView("_pendingPartial", pendings);
        }

        public IActionResult Brandtest()
        {

            foreach (var item in context.Brands)
            {
                item.Totalsold = 0;
            }



            foreach (var item in context.Products.Include(c => c.Brand))
            {
                item.Brand.Totalsold += item.TotalSold;
            }

            //foreach (var item in context.Brands)
            //{
            //    return Json(new { brand = item.Name, count = item.Totalsold });
            //}

            var customers = from o in context.Brands
                            select new { brand = o.Name, count = o.Totalsold };

            context.SaveChanges();
            return Json(customers.ToList());
            //return Content("tam");
        }

        //public IActionResult DailyBestseller()
        //{
        //    List<Order> orders = context.Orders.Where(c=>c.Date.Day == DateTime.Now.Day).ToList();




        //    return Json(order.ProductSizeColor.Product.Name);
        //        //ProductSizeColor product = context.ProductSizeColors.FirstOrDefault(c=>c.or)
        //}
    }
}
