using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MilanaBoutique.DAL;
using MilanaBoutique.Models;
using MilanaBoutique.ViewModels;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MilanaBoutique.Services
{
    public class LayoutService
    {

        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContext;
        private readonly UserManager<AppUser> _userManager;

        public LayoutService(AppDbContext context, IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager)
        {
            _context = context;
            _httpContext = httpContextAccessor;
            _userManager = userManager;
        }

        public Settings getSettingDatas()
        {
            Settings data = _context.Settings.FirstOrDefault();
            return data;
        }
        public async Task<BasketVM> ShowBasket()
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
                    ProductSizeColor prod = _context.ProductSizeColors.Include(p => p.ProductImages).Include(p=>p.Product).ThenInclude(p=>p.Gender).Include(p=>p.Size).Include(p=>p.Color).FirstOrDefault(p => p.Id == item.ProductSizeColorId);
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
                        ProductSizeColor prod = _context.ProductSizeColors.Include(p => p.ProductImages).Include(p => p.Product).ThenInclude(p=>p.Gender).Include(p => p.Size).Include(p => p.Color).FirstOrDefault(p => p.Id == item.Id);
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

            return basketData;

        }
    }
}
