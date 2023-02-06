using MilanaBoutique.DAL;
using MilanaBoutique.Models;
using MilanaBoutique.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MilanaBoutique.Controllers
{
    public class WishlistController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContext;
        private readonly UserManager<AppUser> _userManager;

        public WishlistController(AppDbContext context, IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager)
        {
            _context = context;
            _httpContext = httpContextAccessor;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            string wishlists = HttpContext.Request.Cookies["Wishlists"];

            WishlistVM wishlist = new WishlistVM
            {
                WishlistItems = new List<WishlistItemVM>()
            };

            if (User.Identity.IsAuthenticated && User.IsInRole("Member"))
            {
                AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);

                List<WishlistItem> wishlistItems = _context.WishlistItems.Include(w => w.ProductColor).ThenInclude(c => c.Product).ThenInclude(p => p.ProductSizeColors).Include(p => p.ProductColor).ThenInclude(p => p.Product).ThenInclude(p => p.Gender).Where(c => c.AppUserId == user.Id).ToList();


               foreach (var item in wishlistItems)
                {
                    ProductColor productColor = _context.ProductColors.Include(c => c.Product).ThenInclude(p => p.ProductSizeColors).Include(p => p.Product).ThenInclude(p => p.Gender).FirstOrDefault(c => c.Id == item.ProductColorId);

                    if (productColor!=null)
                    {

                        WishlistItemVM wishlistItemVM = new WishlistItemVM
                        {
                            ProductColor = productColor
                        };

                        wishlist.WishlistItems.Add(wishlistItemVM);

                    }

                }
                return View(wishlist);

                //return PartialView("_wishPartialView", wishlist);
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(wishlists))
                {
                    List<WishlistCookieItemVM> wishlistCookieItems = JsonConvert.DeserializeObject<List<WishlistCookieItemVM>>(wishlists);



                    foreach (var item in wishlistCookieItems)
                    {

                        ProductColor productColor = _context.ProductColors.Include(c => c.Product).ThenInclude(p => p.ProductSizeColors).Include(p => p.Product).ThenInclude(p => p.Gender).FirstOrDefault(c => c.Id == item.Id);

                        if (productColor != null)
                        {

                            WishlistItemVM wishlistItemVM = new WishlistItemVM
                            {
                                ProductColor = productColor
                            };

                            wishlist.WishlistItems.Add(wishlistItemVM);

                        }


                    }

                    return View(wishlist);
                }

                return View(wishlist);

            }


        }

    



        public async Task<IActionResult> Test()
        {
            string wishlists = HttpContext.Request.Cookies["Wishlists"];

            if (User.Identity.IsAuthenticated)
            {
                AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
                List<WishlistItem> list = _context.WishlistItems.Include(c => c.ProductColor).ThenInclude(c => c.Product).Where(w => w.AppUserId == user.Id).ToList();

                //WishlistVM wishlistVM = 


                return Json(list.Count);
            }
            else
            {
                if (!string.IsNullOrEmpty(wishlists))
                {
                    List<WishlistCookieItemVM> wishlistCookieItems = JsonConvert.DeserializeObject<List<WishlistCookieItemVM>>(wishlists);

                    return Json(wishlistCookieItems.Count());
                }
                else
                {
                    return Json(0);
                }
             
            }
          


        }

        public async Task<IActionResult> SetWishlist(int id)
        {

            ProductColor product = _context.ProductColors.FirstOrDefault(p => p.Id == id);

          
          
            if (User.Identity.IsAuthenticated  && User.IsInRole("Member"))
            {
                AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
                WishlistItem wishlistItem = _context.WishlistItems.FirstOrDefault(i => i.ProductColorId == product.Id && i.AppUserId == user.Id);
                if (wishlistItem == null)
                {
                    wishlistItem = new WishlistItem
                    {
                        ProductColorId = product.Id,
                        AppUserId = user.Id,

                    };
                    _context.WishlistItems.Add(wishlistItem);

                    _context.SaveChanges();

                    return Json(_context.WishlistItems.Where(c => c.AppUserId == user.Id).Count());

                }
                else
                {
                    return Json(new { status = 500 });
                }
               
              


            }
            else
            {
                string wishlists = HttpContext.Request.Cookies["Wishlists"];

                if (wishlists == null)
                {

                    List<WishlistCookieItemVM> wishlistCookieItems = new List<WishlistCookieItemVM>();
                    wishlistCookieItems.Add(new WishlistCookieItemVM
                    {
                       
                        Id = product.Id
                    });

                    string wishlistStr = JsonConvert.SerializeObject(wishlistCookieItems);
                    HttpContext.Response.Cookies.Append("Wishlists", wishlistStr);
                    return Json(1);
                }
                else
                {
                    List<WishlistCookieItemVM> wishlistCookieItems = JsonConvert.DeserializeObject<List<WishlistCookieItemVM>>(wishlists);
                    WishlistCookieItemVM cookieItem = wishlistCookieItems.FirstOrDefault(p => p.Id  == product.Id) ;
                    if (cookieItem == null)
                    {
                        cookieItem = new WishlistCookieItemVM
                        {
                           Id = product.Id
                        };
                        wishlistCookieItems.Add(cookieItem);
                        string wishlistStr = JsonConvert.SerializeObject(wishlistCookieItems);
                        HttpContext.Response.Cookies.Append("Wishlists", wishlistStr);


                        return Json(wishlistCookieItems.Count());
                    }
                    else
                    {
                        return Json(new { status = 500 });
                    }
                   
                   
                }



            }



            //return RedirectToAction("Index", "Home");
        }

       
        public async Task<IActionResult> Remove(int id)
        {
            WishlistVM wishlistVM = new WishlistVM
            {
                WishlistItems = new List<WishlistItemVM>()
            };
            string wishlists = HttpContext.Request.Cookies["Wishlists"];
            if (User.Identity.IsAuthenticated && User.IsInRole("Member"))
            {
                AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
                //ProductColor product = _context.ProductColors.FirstOrDefault(c => c.Id == id);
                WishlistItem removable = _context.WishlistItems.Include(c=>c.AppUser).FirstOrDefault(c => c.ProductColorId == id && c.AppUserId == user.Id);

                //List<WishlistItem> wishlistItems = _context.WishlistItems.Where(c => c.AppUserId == user.Id).ToList();

                //wishlistItems.Remove(removable);
                user.WishlistItems.Remove(removable);
                _context.WishlistItems.Remove(removable);
                _context.SaveChanges();

                List<WishlistItem> wishlistItems = _context.WishlistItems.Include(w => w.ProductColor).ThenInclude(c => c.Product).ThenInclude(p => p.ProductSizeColors).Include(p => p.ProductColor).ThenInclude(p => p.Product).ThenInclude(p => p.Gender).Where(c => c.AppUserId == user.Id).ToList();


                foreach (var item in wishlistItems)
                {
                    ProductColor productColor = _context.ProductColors.Include(c => c.Product).ThenInclude(p => p.ProductSizeColors).Include(p => p.Product).ThenInclude(p => p.Gender).FirstOrDefault(c => c.Id == item.ProductColorId);

                    if (productColor != null)
                    {

                        WishlistItemVM wishlistItemVM = new WishlistItemVM
                        {
                            ProductColor = productColor
                        };

                        wishlistVM.WishlistItems.Add(wishlistItemVM);

                    }

                }

               

                return PartialView("_wishPartialView",wishlistVM);

            }
            else
            {
                List<WishlistCookieItemVM> wishlistCookieItems = JsonConvert.DeserializeObject<List<WishlistCookieItemVM>>(wishlists);
                //WishlistItem removable = _context.WishlistItems.FirstOrDefault(c => c.ProductColorId == id);

                WishlistCookieItemVM removable = wishlistCookieItems.FirstOrDefault(c => c.Id == id);

                wishlistCookieItems.Remove(removable);




                string wishlistStr = JsonConvert.SerializeObject(wishlistCookieItems);
                HttpContext.Response.Cookies.Append("Wishlists", wishlistStr);

                foreach (var item in wishlistCookieItems)
                {
                    ProductColor productColor = _context.ProductColors.Include(c => c.Product).ThenInclude(p => p.ProductSizeColors).Include(p => p.Product).ThenInclude(p => p.Gender).FirstOrDefault(c => c.Id == item.Id);

                    if (productColor != null)
                    {

                        WishlistItemVM wishlistItemVM = new WishlistItemVM
                        {
                            ProductColor = productColor
                        };

                        wishlistVM.WishlistItems.Add(wishlistItemVM);

                    }

                }



                return PartialView("_wishPartialView", wishlistVM);

            }
        }


        public async Task<IActionResult> GetPartial()
        {
            WishlistVM wishlistVM = new WishlistVM
            {
                WishlistItems = new List<WishlistItemVM>()
            };
            string wishlists = HttpContext.Request.Cookies["Wishlists"];
            if (User.Identity.IsAuthenticated)
            {
                AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);


                List<WishlistItem> wishlistItems = _context.WishlistItems.Include(w => w.ProductColor).ThenInclude(c => c.Product).ThenInclude(p => p.ProductSizeColors).Include(p => p.ProductColor).ThenInclude(p => p.Product).ThenInclude(p => p.Gender).Where(c => c.AppUserId == user.Id).ToList();

                foreach (var item in wishlistItems)
                {
                    ProductColor productColor = _context.ProductColors.Include(c => c.Product).ThenInclude(p => p.ProductSizeColors).Include(p => p.Product).ThenInclude(p => p.Gender).FirstOrDefault(c => c.Id == item.ProductColorId);

                    if (productColor != null)
                    {

                        WishlistItemVM wishlistItemVM = new WishlistItemVM
                        {
                            ProductColor = productColor
                        };

                        wishlistVM.WishlistItems.Add(wishlistItemVM);

                    }

                }

                if (wishlistVM.WishlistItems.Count != 0)
                {
                    return PartialView("_wishPartialView", wishlistVM);

                }
                else
                {
                    return Content("Your wishlist is empty!");
                }


            }
            else
            {
                if (!string.IsNullOrEmpty(wishlists))
                {
                    List<WishlistCookieItemVM> wishlistCookieItems = JsonConvert.DeserializeObject<List<WishlistCookieItemVM>>(wishlists);






                    //string wishlistStr = JsonConvert.SerializeObject(wishlistCookieItems);
                    //HttpContext.Response.Cookies.Append("Wishlists", wishlistStr);

                    foreach (var item in wishlistCookieItems)
                    {
                        ProductColor productColor = _context.ProductColors.Include(c => c.Product).ThenInclude(p => p.ProductSizeColors).Include(p => p.Product).ThenInclude(p => p.Gender).FirstOrDefault(c => c.Id == item.Id);

                        if (productColor != null)
                        {

                            WishlistItemVM wishlistItemVM = new WishlistItemVM
                            {
                                ProductColor = productColor
                            };

                            wishlistVM.WishlistItems.Add(wishlistItemVM);

                        }

                    }




                    return PartialView("_wishPartialView", wishlistVM);
                }
                return Content("Your wishlist is empty!");
            }
        }

        //public IActionResult EmptyWIsh()
        //{
        //    return View();
        //}

        //public async Task<IActionResult> Testo()
        //{
        //    AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
        //    return Json(_context.WishlistItems.Where(c => c.AppUserId == user.Id).Count());


        //}
    }
}
