using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MilanaBoutique.DAL;
using MilanaBoutique.Models;
using MilanaBoutique.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace MilanaBoutique.Controllers
{
    public class BasketController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContext;
        private readonly UserManager<AppUser> _userManager;

        public BasketController(AppDbContext context, IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager)
        {
            _context = context;
            _httpContext = httpContextAccessor;
            _userManager = userManager;
        }


        public async Task<IActionResult> SetBasket(int sizeId, int colorId, int productId, int quantity)
        {
          
            ProductSizeColor product = await _context.ProductSizeColors.Include(p => p.Product).FirstOrDefaultAsync(p => p.SizeId == sizeId && p.ColorId == colorId && p.ProductId == productId);

          
          
            if (User.Identity.IsAuthenticated  && User.IsInRole("Member"))
            {
                AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
                BasketItem basketItem = _context.BasketItems.FirstOrDefault(i => i.ProductSizeColorId == product.Id && i.AppUserId == user.Id);
                if (basketItem == null)
                {
                    basketItem = new BasketItem
                    {
                        ProductSizeColorId = product.Id,
                        AppUserId = user.Id,
                        Count = quantity

                    };
                    _context.BasketItems.Add(basketItem);


                
                }
                else
                {
                    basketItem.Count += quantity;
                }
                _context.SaveChanges();


                CheckoutVM checkout = new CheckoutVM
                {
                    
                    BasketItems = _context.BasketItems.Include(p => p.ProductSizeColor).ThenInclude(p => p.Product).ThenInclude(p => p.Gender).Include(p => p.ProductSizeColor).ThenInclude(p => p.Size).Include(p => p.ProductSizeColor).ThenInclude(p => p.Color).Where(b => b.AppUserId == user.Id).ToList()
                };


                return PartialView("_cartPartialView", checkout);


            }
            else
            {
                string basket = HttpContext.Request.Cookies["Basket"];

                if (basket == null)
                {

                    List<BasketCookieItemVM> basketCookieItems = new List<BasketCookieItemVM>();
                    basketCookieItems.Add(new BasketCookieItemVM
                    {
                        Count = quantity,
                        Id = product.Id
                    });

                    string basketStr = JsonConvert.SerializeObject(basketCookieItems);
                    HttpContext.Response.Cookies.Append("Basket", basketStr);
                }
                else
                {
                    List<BasketCookieItemVM> basketCookieItems = JsonConvert.DeserializeObject<List<BasketCookieItemVM>>(basket);
                    BasketCookieItemVM cookieItem = basketCookieItems.FirstOrDefault(p => p.Id == product.Id);
                    if (cookieItem == null)
                    {
                        cookieItem = new BasketCookieItemVM
                        {
                            Id = product.Id,
                            Count = quantity
                        };
                        basketCookieItems.Add(cookieItem);
                    }
                    else
                    {
                        cookieItem.Count += quantity;

                    }
                    string basketStr = JsonConvert.SerializeObject(basketCookieItems);
                    HttpContext.Response.Cookies.Append("Basket", basketStr);
                }

                return PartialView("_basketProductPartialView");


            }




        }


        public async Task<IActionResult> CartCounter(int sizeId, int colorId, int productId, int quantity)
        {
            ProductSizeColor product = await _context.ProductSizeColors.Include(p => p.Product).FirstOrDefaultAsync(p => p.SizeId == sizeId && p.ColorId == colorId && p.ProductId == productId);
                AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
                BasketItem basketItem = _context.BasketItems.FirstOrDefault(i => i.ProductSizeColorId == product.Id && i.AppUserId == user.Id);

                if (basketItem!=null)
                {
                    basketItem.Count = quantity;

                }


                _context.SaveChanges();


                CheckoutVM checkout = new CheckoutVM
                {

                    BasketItems = _context.BasketItems.Include(p => p.ProductSizeColor).ThenInclude(p => p.Product).ThenInclude(p => p.Gender).Include(p => p.ProductSizeColor).ThenInclude(p => p.Size).Include(p => p.ProductSizeColor).ThenInclude(p => p.Color).Where(b => b.AppUserId == user.Id).ToList()
                };


                return PartialView("_cartPartialView", checkout);


            
            
        }

        public async Task<IActionResult> CartPartial()
        {
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            CheckoutVM checkout = new CheckoutVM
            {

                BasketItems = _context.BasketItems.Include(p => p.ProductSizeColor).ThenInclude(p => p.Product).ThenInclude(p => p.Gender).Include(p => p.ProductSizeColor).ThenInclude(p => p.Size).Include(p => p.ProductSizeColor).ThenInclude(p => p.Color).Where(b => b.AppUserId == user.Id).ToList()
            };


            return PartialView("_cartPartialView", checkout);

        }
        public IActionResult GetPartial()
        {
            return PartialView("_basketProductPartialView");
        }

        public async Task<IActionResult> ForTotalCount()
        {

            if (User.Identity.IsAuthenticated)
            {
                AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
                List<BasketItem> item = _context.BasketItems.Where(b => b.AppUserId == user.Id).ToList();
                return Json(item.Count);
            }
            else
            {
                string basket = HttpContext.Request.Cookies["Basket"];
                List<BasketCookieItemVM> basketCookieItems = JsonConvert.DeserializeObject<List<BasketCookieItemVM>>(basket);
                return Json(basketCookieItems.Count);
            }

        }

        public async Task<IActionResult> GetTotalPrice()
        {
            string basket = _httpContext.HttpContext.Request.Cookies["Basket"];


            BasketVM basketData = new BasketVM
            {
                TotalPrice = 0,
                BasketItems = new List<BasketItemVM>(),
                Count = 0
            };
            if (_httpContext.HttpContext.User.Identity.IsAuthenticated)
            {
                AppUser user = await _userManager.FindByNameAsync(_httpContext.HttpContext.User.Identity.Name);
                List<BasketItem> basketItems = _context.BasketItems.Include(b => b.AppUser).Where(b => b.AppUserId == user.Id).ToList();
                foreach (BasketItem item in basketItems)
                {
                    ProductSizeColor prod = _context.ProductSizeColors.Include(p => p.ProductImages).Include(p => p.Product).ThenInclude(p => p.Gender).Include(p => p.Size).Include(p => p.Color).FirstOrDefault(p => p.Id == item.ProductSizeColorId);
                    if (prod != null)
                    {
                        BasketItemVM basketItemVM = new BasketItemVM
                        {
                            ProductSizeColor = prod,
                            Count = item.Count
                        };
                        basketItemVM.Price = prod.Product.Discount == null ? prod.Product.Price : prod.Product.Price - (prod.Product.Price * prod.Product.Discount / 100);
                        basketData.BasketItems.Add(basketItemVM);
                        basketData.Count++;
                        basketData.TotalPrice += basketItemVM.Price * basketItemVM.Count;
                    }
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(basket))
                {
                    List<BasketCookieItemVM> basketCookieItems = JsonConvert.DeserializeObject<List<BasketCookieItemVM>>(basket);

                    foreach (BasketCookieItemVM item in basketCookieItems)
                    {
                        ProductSizeColor prod = _context.ProductSizeColors.Include(p => p.ProductImages).Include(p => p.Product).ThenInclude(p => p.Gender).Include(p => p.Size).Include(p => p.Color).FirstOrDefault(p => p.Id == item.Id);
                        if (prod != null)
                        {
                            BasketItemVM basketItem = new BasketItemVM
                            {
                                ProductSizeColor = _context.ProductSizeColors.Include(p => p.ProductImages).Include(p => p.Product).Include(p => p.Size).Include(p => p.Color).FirstOrDefault(p => p.Id == item.Id),
                                Count = item.Count

                            };
                            basketItem.Price = prod.Product.Discount == null ? prod.Product.Price : prod.Product.Price - (prod.Product.Price * prod.Product.Discount / 100);
                            basketData.BasketItems.Add(basketItem);
                            basketData.Count++;
                            basketData.TotalPrice += basketItem.Price * basketItem.Count;
                        }
                    }


                }
            }

            return Json(basketData.TotalPrice);


        }


        public async Task<IActionResult> Remove(int id)
        {
            string basket = _httpContext.HttpContext.Request.Cookies["Basket"];

            if (User.Identity.IsAuthenticated)
            {
                AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
                List<BasketItem> item = _context.BasketItems.Where(b => b.AppUserId == user.Id && b.ProductSizeColorId == id).ToList();
                user.BasketItems.RemoveAll(p => item.Any(i => i.Id == p.Id));
                _context.BasketItems.RemoveRange(item);
                _context.SaveChanges();
            }
            else
            {
                if (!string.IsNullOrEmpty(basket))
                {
                    List<BasketCookieItemVM> basketCookieItems = JsonConvert.DeserializeObject<List<BasketCookieItemVM>>(basket);
                    List<BasketCookieItemVM> newlist = basketCookieItems.Where(p => p.Id == id).ToList();
                    basketCookieItems.RemoveAll(b => newlist.Any(p=>p.Id == b.Id));

                    string basketstr = JsonConvert.SerializeObject(basketCookieItems);
                    HttpContext.Response.Cookies.Append("Basket", basketstr);


                }
            }

            return Json(new {status=200 });
        }
    }
}
