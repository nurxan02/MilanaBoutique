using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MilanaBoutique.DAL;
using MilanaBoutique.Models;
using MilanaBoutique.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MilanaBoutique.Controllers
{
    public class OrderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContext;
        private readonly UserManager<AppUser> _userManager;

        public OrderController(AppDbContext context, IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager)
        {
            _context = context;
            _httpContext = httpContextAccessor;
            _userManager = userManager;
        }


        [Authorize]
        public async Task<IActionResult> Checkout()
        {
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);

            CheckoutVM checkout = new CheckoutVM
            {
                Address = user.Address,
                City = user.City,
                Country = user.Country,
                Email = user.Email,
                Firstname = user.Firstname,
                Phone = user.Phone,
                Surname = user.Surname,
                Zip = user.Zip,
                 BasketItems = _context.BasketItems.Include(p=>p.ProductSizeColor).ThenInclude(p=>p.Product).ThenInclude(p=>p.Gender).Include(p=>p.ProductSizeColor).ThenInclude(p=>p.Size).Include(p=>p.ProductSizeColor).ThenInclude(p=>p.Color).Where(b=>b.AppUserId == user.Id).ToList()
            };

            return View(checkout);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Checkout(CheckoutVM checkoutVm)
        {
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);

            CheckoutVM checkout = new CheckoutVM
            {
                Address = checkoutVm.Address,
                City = checkoutVm.City,
                Country = checkoutVm.Country,
                Email = checkoutVm.Email,
                Firstname = checkoutVm.Firstname,
                Phone = checkoutVm.Phone,
                Surname = checkoutVm.Surname,
                Zip = checkoutVm.Zip,
                BasketItems = _context.BasketItems.Include(p => p.ProductSizeColor).ThenInclude(p => p.Product).ThenInclude(p => p.Gender).Include(p => p.ProductSizeColor).ThenInclude(p => p.Size).Include(p => p.ProductSizeColor).ThenInclude(p => p.Color).Where(b => b.AppUserId == user.Id).ToList(),
                MessagesToAdmin = checkoutVm.MessagesToAdmin
            };
            if (!ModelState.IsValid) return View(checkout);

            TempData["Succeeded"] = false;
            if (checkout.BasketItems.Count == 0) return RedirectToAction("Index", "Home");

            Order order = new Order
            {
                Address = checkout.Address,
                AppUserId = user.Id,
                City = checkout.City,
                Country = checkout.Country,
                Date = DateTime.Now,
                Phone = checkout.Phone,
                Zip = checkout.Zip,
                TotalPrice = 0,
                 Firstname = checkout.Firstname,
                  Lastname = checkout.Surname,
                  Email = checkout.Email,
                   MessageToAdmin = checkoutVm.MessagesToAdmin,
                   StatusId = 1
                 
            };


            foreach (BasketItem item in checkout.BasketItems)
            {
                order.TotalPrice += (item.ProductSizeColor.Product.Discount == null ? item.Count * item.ProductSizeColor.Product.Price : item.Count * (item.ProductSizeColor.Product.Price - (item.ProductSizeColor.Product.Price * item.ProductSizeColor.Product.Discount / 100)));

                OrderItem orderItem = new OrderItem
                {
                    AppUserId = user.Id,
                    Order = order,
                    Name = item.ProductSizeColor.Product.Name,
                    ProductSizeColorId = item.ProductSizeColor.Id,
                    Price = (item.ProductSizeColor.Product.Discount == null ? item.Count * item.ProductSizeColor.Product.Price : item.Count * (item.ProductSizeColor.Product.Price - (item.ProductSizeColor.Product.Price * item.ProductSizeColor.Product.Discount / 100))),
                     Count = item.Count
                };
                _context.OrderItems.Add(orderItem);
            }
            _context.BasketItems.RemoveRange(checkout.BasketItems);
            _context.Orders.Add(order);
            _context.SaveChanges();
            TempData["Succeeded"] = true;

            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles ="Member")]
        public async Task<IActionResult> Cart()
        {
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);

            CheckoutVM checkout = new CheckoutVM
            {
              
                BasketItems = _context.BasketItems.Include(p => p.ProductSizeColor).ThenInclude(p => p.Product).ThenInclude(p => p.Gender).Include(p => p.ProductSizeColor).ThenInclude(p => p.Size).Include(p => p.ProductSizeColor).ThenInclude(p => p.Color).Where(b => b.AppUserId == user.Id).ToList()
            };

            return View(checkout);

        }

    }
}
