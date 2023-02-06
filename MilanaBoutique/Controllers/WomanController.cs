using MilanaBoutique.DAL;
using MilanaBoutique.Models;
using MilanaBoutique.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MilanaBoutique.Controllers
{
    public class WomanController : Controller
    {
        private readonly AppDbContext context;

        public WomanController(AppDbContext context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            CountVM vM = new CountVM
            {
                Categories = context.Categories.Include(c => c.Products).ThenInclude(p => p.ProductSizeColors).Include(c => c.SubCategories).Where(c => c.GenderId == 2).ToList(),
                Colors = context.ProductColors.Where(pc => pc.Product.GenderId == 2).ToList()
            };

            return View(vM);
        }

        public IActionResult Sorting(int catid, int sortval = 1, int page = 1)
        {
            Category category = context.Categories.Include(c => c.Products).Include(c => c.Gender).Include(c => c.SubCategories).ThenInclude(s => s.Products).FirstOrDefault(c => c.Id == catid);

            if (sortval == 1)
            {
                ShopVM shopVM = new ShopVM
                {
                    Brands = context.Brands.Include(b => b.Products).ToList(),
                    Category = context.Categories.Include(c => c.Products).Include(c => c.Gender).Include(c => c.SubCategories).ThenInclude(s => s.Products).FirstOrDefault(c => c.Id == catid),
                    Products = context.Products.Include(p => p.ProductColors).Include(p => p.Category).Include(p => p.ProductSizeColors).ThenInclude(p => p.Color).Include(p => p.ProductSizeColors).ThenInclude(p => p.Size).Include(p => p.Brand).Include(p => p.SubCategory).Include(p => p.Gender).Where(p => p.CategoryId == catid && p.ProductSizeColors.Count > 0).ToList(),
                    ProductSizeColors = context.ProductSizeColors.Include(p => p.Color).Include(p => p.Size).Include(p => p.Product).ThenInclude(p => p.SubCategory).Include(p => p.ProductImages).Where(p => p.Product.CategoryId == catid).ToList(),
                    ProductColors = context.ProductColors.Include(p => p.Product).ThenInclude(p => p.Comments).Include(p => p.Product).ThenInclude(p => p.Category).ThenInclude(c => c.Gender).Include(p => p.Product).ThenInclude(p => p.ProductSizeColors).Where(p => p.Product.CategoryId == catid && p.Product.GenderId == 2).OrderByDescending(p => p.Id).Skip((page - 1) * 6).Take(6).ToList(),
                    ProductColors2 = context.ProductColors.Include(p => p.Product).ThenInclude(p => p.Comments).Include(p => p.Product).ThenInclude(p => p.Comments).Include(p => p.Product).ThenInclude(p => p.Category).ThenInclude(c => c.Gender).Include(p => p.Product).ThenInclude(p => p.ProductSizeColors).Where(p => p.Product.CategoryId == catid && p.Product.GenderId == 2).ToList(),
                };

                return PartialView("_productPartialView", shopVM);
            }
            else
            {
                if (sortval == 2)
                {
                    ShopVM shopVM = new ShopVM
                    {
                        Brands = context.Brands.Include(b => b.Products).ToList(),
                        Category = context.Categories.Include(c => c.Products).Include(c => c.Gender).Include(c => c.SubCategories).ThenInclude(s => s.Products).FirstOrDefault(c => c.Id == catid),
                        Products = context.Products.Include(p => p.ProductColors).Include(p => p.Category).Include(p => p.ProductSizeColors).ThenInclude(p => p.Color).Include(p => p.ProductSizeColors).ThenInclude(p => p.Size).Include(p => p.Brand).Include(p => p.SubCategory).Include(p => p.Gender).Where(p => p.CategoryId == catid && p.ProductSizeColors.Count > 0).ToList(),
                        ProductSizeColors = context.ProductSizeColors.Include(p => p.Color).Include(p => p.Size).Include(p => p.Product).ThenInclude(p => p.SubCategory).Include(p => p.ProductImages).Where(p => p.Product.CategoryId == catid).ToList(),
                        ProductColors = context.ProductColors.Include(p => p.Product).ThenInclude(p => p.Comments).Include(p => p.Product).ThenInclude(p => p.Category).ThenInclude(c => c.Gender).Include(p => p.Product).ThenInclude(p => p.ProductSizeColors).Where(p => p.Product.CategoryId == catid && p.Product.GenderId == 2).OrderBy(p => p.Product.Price).Skip((page - 1) * 6).Take(6).ToList(),
                        ProductColors2 = context.ProductColors.Include(p => p.Product).ThenInclude(p => p.Comments).Include(p => p.Product).ThenInclude(p => p.Comments).Include(p => p.Product).ThenInclude(p => p.Category).ThenInclude(c => c.Gender).Include(p => p.Product).ThenInclude(p => p.ProductSizeColors).Where(p => p.Product.CategoryId == catid && p.Product.GenderId == 2).ToList(),
                    };

                    return PartialView("_productPartialView", shopVM);
                }
                else
                {
                    if (sortval == 3)
                    {
                        ShopVM shopVM = new ShopVM
                        {
                            Brands = context.Brands.Include(b => b.Products).ToList(),
                            Category = context.Categories.Include(c => c.Products).Include(c => c.Gender).Include(c => c.SubCategories).ThenInclude(s => s.Products).FirstOrDefault(c => c.Id == catid),
                            Products = context.Products.Include(p => p.ProductColors).Include(p => p.Category).Include(p => p.ProductSizeColors).ThenInclude(p => p.Color).Include(p => p.ProductSizeColors).ThenInclude(p => p.Size).Include(p => p.Brand).Include(p => p.SubCategory).Include(p => p.Gender).Where(p => p.CategoryId == catid && p.ProductSizeColors.Count > 0).ToList(),
                            ProductSizeColors = context.ProductSizeColors.Include(p => p.Color).Include(p => p.Size).Include(p => p.Product).ThenInclude(p => p.SubCategory).Include(p => p.ProductImages).Where(p => p.Product.CategoryId == catid).ToList(),
                            ProductColors = context.ProductColors.Include(p => p.Product).ThenInclude(p => p.Comments).Include(p => p.Product).ThenInclude(p => p.Category).ThenInclude(c => c.Gender).Include(p => p.Product).ThenInclude(p => p.ProductSizeColors).Where(p => p.Product.CategoryId == catid && p.Product.GenderId == 2).OrderByDescending(p => p.Product.Price).Skip((page - 1) * 6).Take(6).ToList(),
                            ProductColors2 = context.ProductColors.Include(p => p.Product).ThenInclude(p => p.Comments).Include(p => p.Product).ThenInclude(p => p.Comments).Include(p => p.Product).ThenInclude(p => p.Category).ThenInclude(c => c.Gender).Include(p => p.Product).ThenInclude(p => p.ProductSizeColors).Where(p => p.Product.CategoryId == catid && p.Product.GenderId == 2).ToList(),
                        };

                        return PartialView("_productPartialView", shopVM);
                    }
                    else
                    {
                        if (sortval == 4)
                        {
                            ShopVM shopVM = new ShopVM
                            {
                                Brands = context.Brands.Include(b => b.Products).ToList(),
                                Category = context.Categories.Include(c => c.Products).Include(c => c.Gender).Include(c => c.SubCategories).ThenInclude(s => s.Products).FirstOrDefault(c => c.Id == catid),
                                Products = context.Products.Include(p => p.ProductColors).Include(p => p.Category).Include(p => p.ProductSizeColors).ThenInclude(p => p.Color).Include(p => p.ProductSizeColors).ThenInclude(p => p.Size).Include(p => p.Brand).Include(p => p.SubCategory).Include(p => p.Gender).Where(p => p.CategoryId == catid && p.ProductSizeColors.Count > 0).ToList(),
                                ProductSizeColors = context.ProductSizeColors.Include(p => p.Color).Include(p => p.Size).Include(p => p.Product).ThenInclude(p => p.SubCategory).Include(p => p.ProductImages).Where(p => p.Product.CategoryId == catid).ToList(),
                                ProductColors = context.ProductColors.Include(p => p.Product).ThenInclude(p => p.Comments).Include(p => p.Product).ThenInclude(p => p.Category).ThenInclude(c => c.Gender).Include(p => p.Product).ThenInclude(p => p.ProductSizeColors).Where(p => p.Product.CategoryId == catid && p.Product.GenderId == 2).OrderBy(p => p.Product.Name).Skip((page - 1) * 6).Take(6).ToList(),
                                ProductColors2 = context.ProductColors.Include(p => p.Product).ThenInclude(p => p.Comments).Include(p => p.Product).ThenInclude(p => p.Comments).Include(p => p.Product).ThenInclude(p => p.Category).ThenInclude(c => c.Gender).Include(p => p.Product).ThenInclude(p => p.ProductSizeColors).Where(p => p.Product.CategoryId == catid && p.Product.GenderId == 2).ToList(),
                            };

                            return PartialView("_productPartialView", shopVM);
                        }
                        else
                        {
                            if (sortval == 5)
                            {
                                ShopVM shopVM = new ShopVM
                                {
                                    Brands = context.Brands.Include(b => b.Products).ToList(),
                                    Category = context.Categories.Include(c => c.Products).Include(c => c.Gender).Include(c => c.SubCategories).ThenInclude(s => s.Products).FirstOrDefault(c => c.Id == catid),
                                    Products = context.Products.Include(p => p.ProductColors).Include(p => p.Category).Include(p => p.ProductSizeColors).ThenInclude(p => p.Color).Include(p => p.ProductSizeColors).ThenInclude(p => p.Size).Include(p => p.Brand).Include(p => p.SubCategory).Include(p => p.Gender).Where(p => p.CategoryId == catid && p.ProductSizeColors.Count > 0).ToList(),
                                    ProductSizeColors = context.ProductSizeColors.Include(p => p.Color).Include(p => p.Size).Include(p => p.Product).ThenInclude(p => p.SubCategory).Include(p => p.ProductImages).Where(p => p.Product.CategoryId == catid).ToList(),
                                    ProductColors = context.ProductColors.Include(p => p.Product).ThenInclude(p => p.Comments).Include(p => p.Product).ThenInclude(p => p.Category).ThenInclude(c => c.Gender).Include(p => p.Product).ThenInclude(p => p.ProductSizeColors).Where(p => p.Product.CategoryId == catid && p.Product.GenderId == 2).OrderByDescending(p => p.Product.Name).Skip((page - 1) * 6).Take(6).ToList(),
                                    ProductColors2 = context.ProductColors.Include(p => p.Product).ThenInclude(p => p.Comments).Include(p => p.Product).ThenInclude(p => p.Comments).Include(p => p.Product).ThenInclude(p => p.Category).ThenInclude(c => c.Gender).Include(p => p.Product).ThenInclude(p => p.ProductSizeColors).Where(p => p.Product.CategoryId == catid && p.Product.GenderId == 2).ToList(),
                                };

                                return PartialView("_productPartialView", shopVM);
                            }
                        }
                    }
                }
            }

            ViewBag.Sortdata = sortval;

            return Json(new { status = 200 });
        }

        public IActionResult Testing(int id, string[] catvals,  string[] brandvals, string[] colorvals, int min, int max, string[] sizevals,  int page = 1)
        {

          

            if (catvals.Length != 0 && max != 0 && brandvals.Length != 0 && colorvals.Length != 0)
            {
                ViewBag.CurrentPages = page;
                ViewBag.TotalPages = Math.Ceiling((decimal)context.ProductColors.Where(p => p.Product.CategoryId == id && p.Product.GenderId == 2).Count() / 6);

                ShopVM shopVM = new ShopVM
                {
                    ProductColors = context.ProductColors.Include(p => p.Product).ThenInclude(p => p.Comments).Include(p => p.Product).ThenInclude(p => p.Comments).Include(p => p.Product).ThenInclude(p => p.Category).ThenInclude(c => c.Gender).Include(p => p.Product).ThenInclude(p => p.ProductSizeColors).Include(p => p.Product).ThenInclude(p => p.SubCategory).Where(p => p.Product.CategoryId == id && p.Product.GenderId == 2 && catvals.Contains(p.Product.SubCategoryId.ToString()) && p.Product.Price <= max && p.Product.Price >= min && brandvals.Contains(p.Product.BrandId.ToString()) && colorvals.Contains(p.ColorId.ToString())).Skip((page - 1) * 6).Take(6).ToList(),
                    

                    ProductColors2 = context.ProductColors.Include(p => p.Product).ThenInclude(p => p.Comments).Include(p => p.Product).ThenInclude(p => p.Comments).Include(p => p.Product).ThenInclude(p => p.Category).ThenInclude(c => c.Gender).Include(p => p.Product).ThenInclude(p => p.ProductSizeColors).Include(p => p.Product).ThenInclude(p => p.SubCategory).Where(p => p.Product.CategoryId == id && p.Product.GenderId == 2 && catvals.Contains(p.Product.SubCategoryId.ToString()) && p.Product.Price <= max && p.Product.Price >= min ).ToList()
                };

                return PartialView("_productPartialView", shopVM);
            }
            
            else
            {
                if (catvals.Length == 0 && max != 0 && brandvals.Length != 0 && colorvals.Length != 0)
                {
                    ViewBag.CurrentPages = page;
                    ViewBag.TotalPages = Math.Ceiling((decimal)context.ProductColors.Where(p => p.Product.CategoryId == id && p.Product.GenderId == 2).Count() / 6);

                    ShopVM shopVM = new ShopVM
                    {
                        ProductColors = context.ProductColors.Include(p => p.Product).ThenInclude(p => p.Comments).Include(p => p.Product).ThenInclude(p => p.Comments).Include(p => p.Product).ThenInclude(p => p.Category).ThenInclude(c => c.Gender).Include(p => p.Product).ThenInclude(p => p.ProductSizeColors).Include(p => p.Product).ThenInclude(p => p.SubCategory).Where(p => p.Product.CategoryId == id && p.Product.GenderId == 2 && p.Product.Price <= max && p.Product.Price >= min && brandvals.Contains(p.Product.BrandId.ToString()) && colorvals.Contains(p.ColorId.ToString())).Skip((page - 1) * 6).Take(6).ToList(),


                        ProductColors2 = context.ProductColors.Include(p => p.Product).ThenInclude(p => p.Comments).Include(p => p.Product).ThenInclude(p => p.Comments).Include(p => p.Product).ThenInclude(p => p.Category).ThenInclude(c => c.Gender).Include(p => p.Product).ThenInclude(p => p.ProductSizeColors).Include(p => p.Product).ThenInclude(p => p.SubCategory).Where(p => p.Product.CategoryId == id && p.Product.GenderId == 2 ).ToList()
                    };

                    return PartialView("_productPartialView", shopVM);
                }
                else
                {
                    if (catvals.Length != 0 && max != 0 && brandvals.Length == 0 && colorvals.Length != 0)
                    {
                        ViewBag.CurrentPages = page;
                        ViewBag.TotalPages = Math.Ceiling((decimal)context.ProductColors.Where(p => p.Product.CategoryId == id && p.Product.GenderId == 2).Count() / 6);

                        ShopVM shopVM = new ShopVM
                        {
                            ProductColors = context.ProductColors.Include(p => p.Product).ThenInclude(p => p.Comments).Include(p => p.Product).ThenInclude(p => p.Comments).Include(p => p.Product).ThenInclude(p => p.Category).ThenInclude(c => c.Gender).Include(p => p.Product).ThenInclude(p => p.ProductSizeColors).Include(p => p.Product).ThenInclude(p => p.SubCategory).Where(p => p.Product.CategoryId == id && p.Product.GenderId == 2 && p.Product.Price <= max && p.Product.Price >= min && colorvals.Contains(p.ColorId.ToString()) && catvals.Contains(p.Product.SubCategoryId.ToString())).Skip((page - 1) * 6).Take(6).ToList(),


                            ProductColors2 = context.ProductColors.Include(p => p.Product).ThenInclude(p => p.Comments).Include(p => p.Product).ThenInclude(p => p.Comments).Include(p => p.Product).ThenInclude(p => p.Category).ThenInclude(c => c.Gender).Include(p => p.Product).ThenInclude(p => p.ProductSizeColors).Include(p => p.Product).ThenInclude(p => p.SubCategory).Where(p => p.Product.CategoryId == id && p.Product.GenderId == 2).ToList()
                        };

                        return PartialView("_productPartialView", shopVM);
                    }
                    else
                    {
                        if (catvals.Length != 0 && max != 0 && brandvals.Length != 0 && colorvals.Length == 0)
                        {
                            ViewBag.CurrentPages = page;
                            ViewBag.TotalPages = Math.Ceiling((decimal)context.ProductColors.Where(p => p.Product.CategoryId == id && p.Product.GenderId == 2).Count() / 6);

                            ShopVM shopVM = new ShopVM
                            {
                                ProductColors = context.ProductColors.Include(p => p.Product).ThenInclude(p => p.Comments).Include(p => p.Product).ThenInclude(p => p.Comments).Include(p => p.Product).ThenInclude(p => p.Category).ThenInclude(c => c.Gender).Include(p => p.Product).ThenInclude(p => p.ProductSizeColors).Include(p => p.Product).ThenInclude(p => p.SubCategory).Where(p => p.Product.CategoryId == id && p.Product.GenderId == 2 && p.Product.Price <= max && p.Product.Price >= min && catvals.Contains(p.Product.SubCategoryId.ToString()) && brandvals.Contains(p.Product.BrandId.ToString())).Skip((page - 1) * 6).Take(6).ToList(),


                                ProductColors2 = context.ProductColors.Include(p => p.Product).ThenInclude(p => p.Comments).Include(p => p.Product).ThenInclude(p => p.Comments).Include(p => p.Product).ThenInclude(p => p.Category).ThenInclude(c => c.Gender).Include(p => p.Product).ThenInclude(p => p.ProductSizeColors).Include(p => p.Product).ThenInclude(p => p.SubCategory).Where(p => p.Product.CategoryId == id && p.Product.GenderId == 2).ToList()
                            };

                            return PartialView("_productPartialView", shopVM);
                        }
                        else
                        {
                            if (catvals.Length == 0 && max != 0 && brandvals.Length == 0 && colorvals.Length != 0)
                            {
                                ViewBag.CurrentPages = page;
                                ViewBag.TotalPages = Math.Ceiling((decimal)context.ProductColors.Where(p => p.Product.CategoryId == id && p.Product.GenderId == 2).Count() / 6);

                                ShopVM shopVM = new ShopVM
                                {
                                    ProductColors = context.ProductColors.Include(p => p.Product).ThenInclude(p => p.Comments).Include(p => p.Product).ThenInclude(p => p.Comments).Include(p => p.Product).ThenInclude(p => p.Category).ThenInclude(c => c.Gender).Include(p => p.Product).ThenInclude(p => p.ProductSizeColors).Include(p => p.Product).ThenInclude(p => p.SubCategory).Where(p => p.Product.CategoryId == id && p.Product.GenderId == 2 && p.Product.Price <= max && p.Product.Price >= min && colorvals.Contains(p.ColorId.ToString())).Skip((page - 1) * 6).Take(6).ToList(),


                                    ProductColors2 = context.ProductColors.Include(p => p.Product).ThenInclude(p => p.Comments).Include(p => p.Product).ThenInclude(p => p.Comments).Include(p => p.Product).ThenInclude(p => p.Category).ThenInclude(c => c.Gender).Include(p => p.Product).ThenInclude(p => p.ProductSizeColors).Include(p => p.Product).ThenInclude(p => p.SubCategory).Where(p => p.Product.CategoryId == id && p.Product.GenderId == 2 ).ToList()
                                };

                                return PartialView("_productPartialView", shopVM);
                            }
                            else
                            {
                                if (catvals.Length != 0 && max != 0 && brandvals.Length == 0 && colorvals.Length == 0)
                                {
                                    ViewBag.CurrentPages = page;
                                    ViewBag.TotalPages = Math.Ceiling((decimal)context.ProductColors.Where(p => p.Product.CategoryId == id && p.Product.GenderId == 2).Count() / 6);

                                    ShopVM shopVM = new ShopVM
                                    {
                                        ProductColors = context.ProductColors.Include(p => p.Product).ThenInclude(p => p.Comments).Include(p => p.Product).ThenInclude(p => p.Comments).Include(p => p.Product).ThenInclude(p => p.Category).ThenInclude(c => c.Gender).Include(p => p.Product).ThenInclude(p => p.ProductSizeColors).Include(p => p.Product).ThenInclude(p => p.SubCategory).Where(p => p.Product.CategoryId == id && p.Product.GenderId == 2 && p.Product.Price <= max && p.Product.Price >= min && catvals.Contains(p.Product.SubCategoryId.ToString())).Skip((page - 1) * 6).Take(6).ToList(),


                                        ProductColors2 = context.ProductColors.Include(p => p.Product).ThenInclude(p => p.Comments).Include(p => p.Product).ThenInclude(p => p.Comments).Include(p => p.Product).ThenInclude(p => p.Category).ThenInclude(c => c.Gender).Include(p => p.Product).ThenInclude(p => p.ProductSizeColors).Include(p => p.Product).ThenInclude(p => p.SubCategory).Where(p => p.Product.CategoryId == id && p.Product.GenderId == 2).ToList()
                                    };

                                    return PartialView("_productPartialView", shopVM);
                                }
                                else
                                {
                                    if (catvals.Length == 0 && max != 0 && brandvals.Length != 0 && colorvals.Length == 0)
                                    {
                                        ViewBag.CurrentPages = page;
                                        ViewBag.TotalPages = Math.Ceiling((decimal)context.ProductColors.Where(p => p.Product.CategoryId == id && p.Product.GenderId == 2).Count() / 6);

                                        ShopVM shopVM = new ShopVM
                                        {
                                            ProductColors = context.ProductColors.Include(p => p.Product).ThenInclude(p => p.Comments).Include(p => p.Product).ThenInclude(p => p.Comments).Include(p => p.Product).ThenInclude(p => p.Category).ThenInclude(c => c.Gender).Include(p => p.Product).ThenInclude(p => p.ProductSizeColors).Include(p => p.Product).ThenInclude(p => p.SubCategory).Where(p => p.Product.CategoryId == id && p.Product.GenderId == 2 && p.Product.Price <= max && p.Product.Price >= min && brandvals.Contains(p.Product.BrandId.ToString())).Skip((page - 1) * 6).Take(6).ToList(),


                                            ProductColors2 = context.ProductColors.Include(p => p.Product).ThenInclude(p => p.Comments).Include(p => p.Product).ThenInclude(p => p.Comments).Include(p => p.Product).ThenInclude(p => p.Category).ThenInclude(c => c.Gender).Include(p => p.Product).ThenInclude(p => p.ProductSizeColors).Include(p => p.Product).ThenInclude(p => p.SubCategory).Where(p => p.Product.CategoryId == id && p.Product.GenderId == 2).ToList()
                                        };

                                        return PartialView("_productPartialView", shopVM);
                                    }
                                }
                            }
                        }
                    }
                }


                ViewBag.CurrentPages = page;
                ViewBag.TotalPages = Math.Ceiling((decimal)context.ProductColors.Where(p => p.Product.CategoryId == id && p.Product.GenderId == 2).Count() / 6);

                ShopVM shopVMi = new ShopVM
                {
                    ProductColors = context.ProductColors.Include(p => p.Product).ThenInclude(p => p.Comments).Include(p => p.Product).ThenInclude(p => p.Comments).Include(p => p.Product).ThenInclude(p => p.Category).ThenInclude(c => c.Gender).Include(p => p.Product).ThenInclude(p => p.ProductSizeColors).Include(p => p.Product).ThenInclude(p => p.SubCategory).Where(p => p.Product.CategoryId == id && p.Product.GenderId == 2 && p.Product.Price <= max && p.Product.Price >= min).Skip((page - 1) * 6).Take(6).ToList(),


                    ProductColors2 = context.ProductColors.Include(p => p.Product).ThenInclude(p => p.Comments).Include(p => p.Product).ThenInclude(p => p.Comments).Include(p => p.Product).ThenInclude(p => p.Category).ThenInclude(c => c.Gender).Include(p => p.Product).ThenInclude(p => p.ProductSizeColors).Include(p => p.Product).ThenInclude(p => p.SubCategory).Where(p => p.Product.CategoryId == id && p.Product.GenderId == 2).ToList()
                };

                return PartialView("_productPartialView", shopVMi);
            }



        }




        public IActionResult Shop(int id, int page = 1, string[] catvals=null)
        {
            if (context.Categories.FirstOrDefault(c => c.Id == id) == null)
            {
                return NotFound();
            }
            if (context.Categories.FirstOrDefault(c => c.Id == id).GenderId != 2)
            {
                return NotFound();
            }

            ViewBag.Sizes = context.Sizes.ToList();
            ViewBag.Colors = context.Colors.ToList();


            ViewBag.CurrentPage = page;
            ViewBag.TotalPage = Math.Ceiling((decimal)context.ProductColors.Where(p => p.Product.CategoryId == id && p.Product.GenderId == 2).Count() / 6);
            ShopVM shopVM = new ShopVM
            {
                Brands = context.Brands.Include(b => b.Products).ToList(),
                Category = context.Categories.Include(c => c.Products).Include(c => c.Gender).Include(c => c.SubCategories).ThenInclude(s => s.Products).FirstOrDefault(c => c.Id == id),
                Products = context.Products.Include(p => p.ProductColors).Include(p => p.Category).Include(p => p.ProductSizeColors).ThenInclude(p => p.Color).Include(p => p.ProductSizeColors).ThenInclude(p => p.Size).Include(p => p.Brand).Include(p => p.SubCategory).Include(p => p.Gender).Where(p => p.CategoryId == id && p.ProductSizeColors.Count > 0).ToList(),
                ProductSizeColors = context.ProductSizeColors.Include(p => p.Color).Include(p => p.Size).Include(p => p.Product).ThenInclude(p => p.SubCategory).Include(p => p.ProductImages).Where(p => p.Product.CategoryId == id).ToList(),


                ProductColors = context.ProductColors.Include(p => p.Product).ThenInclude(p => p.Comments).Include(p => p.Product).ThenInclude(p => p.Comments).Include(p => p.Product).ThenInclude(p => p.Category).ThenInclude(c => c.Gender).Include(p => p.Product).ThenInclude(p => p.ProductSizeColors).Where(p => p.Product.CategoryId == id && p.Product.GenderId == 2).OrderByDescending(c => c.Id).Skip((page - 1) * 6).Take(6).ToList(),


                ProductColors2 = context.ProductColors.Include(p => p.Product).ThenInclude(p => p.Comments).Include(p => p.Product).ThenInclude(p => p.Comments).Include(p => p.Product).ThenInclude(p => p.Category).ThenInclude(c => c.Gender).Include(p => p.Product).ThenInclude(p => p.ProductSizeColors).Where(p => p.Product.CategoryId == id && p.Product.GenderId == 2).ToList()
            };

            return View(shopVM);






        }




        public IActionResult Details(int id)
        {
            if (context.ProductSizeColors.FirstOrDefault(p => p.Id == id) == null)
            {
                return NotFound();
            }

            ProductSizeColor product = context.ProductSizeColors.Include(p => p.Color).Include(p => p.Size).Include(p => p.ProductImages).Include(p => p.Product).ThenInclude(p => p.SubCategory).ThenInclude(s => s.Category).Include(p => p.Product).ThenInclude(p => p.Brand).Include(p => p.Product).ThenInclude(p => p.Gender).Include(p => p.Product).ThenInclude(p => p.ProductSizeColors).ThenInclude(p => p.Size).FirstOrDefault(p => p.Id == id);
            if (product.Product.GenderId != 2)
            {
                return NotFound();
            }

            ProductSizeColor forcolor = context.ProductSizeColors.Include(p => p.Color).Include(p => p.Size).Include(p => p.ProductImages).Include(p => p.Product).ThenInclude(p => p.SubCategory).ThenInclude(s => s.Category).Include(p => p.Product).ThenInclude(p => p.Brand).Include(p => p.Product).ThenInclude(p => p.Gender).Include(p => p.Product).ThenInclude(p => p.ProductSizeColors).ThenInclude(p => p.Size).Include(p => p.Product).ThenInclude(p => p.ProductColors).Include(c=>c.Product).ThenInclude(c=>c.Category).FirstOrDefault(p => p.Id == id);

            ShopVM shopVM = new ShopVM
            {
                ProductSizeColor = context.ProductSizeColors.Include(p => p.Color).Include(p => p.Size).Include(p => p.ProductImages).Include(p => p.Product).ThenInclude(p => p.SubCategory).ThenInclude(s => s.Category).Include(p => p.Product).ThenInclude(p => p.Brand).Include(p => p.Product).ThenInclude(p => p.Gender).Include(p => p.Product).ThenInclude(p => p.ProductSizeColors).ThenInclude(p => p.Size).Include(p => p.Product).ThenInclude(p => p.ProductColors).FirstOrDefault(p => p.Id == id),
                Products = context.Products.Include(p => p.Brand).Include(p => p.Category).Include(p => p.SubCategory).Include(p => p.ProductSizeColors).ThenInclude(p => p.ProductImages).Include(p => p.ProductColors).Include(p => p.ProductSizeColors).ThenInclude(p => p.Size).Include(p => p.ProductSizeColors).ThenInclude(p => p.Color).Where(p => p.CategoryId == product.Product.CategoryId && p.Id != product.ProductId).ToList(),
                Product = context.Products.Include(p => p.Brand).Include(p => p.Comments).Include(p => p.Category).Include(p => p.SubCategory).Include(p => p.ProductSizeColors).ThenInclude(p => p.ProductImages).Include(p => p.ProductColors).Include(p => p.ProductSizeColors).ThenInclude(p => p.Size).Include(p => p.ProductSizeColors).ThenInclude(p => p.Color).FirstOrDefault(p => p.Id == product.ProductId),
                ProductColors = context.ProductColors.Include(p => p.Product).ThenInclude(p => p.Comments).Where(p => p.Product.CategoryId == product.Product.CategoryId && p.ProductId != forcolor.ProductId).ToList(),
                Comments = context.Comments.Include(c => c.Product).Include(p => p.AppUser).Where(c => c.ProductId == product.ProductId).ToList(),
                Category = context.Categories.FirstOrDefault(c=>c.Id == forcolor.Product.CategoryId)

            };
            if (context.Comments.Where(c => c.ProductId == product.Id && c.IsAccepted == true) == null)
            {
                ViewBag.Comments = 0;
            }
            else
            {
                ViewBag.Comments = context.Comments.Include(c => c.Product).Include(p => p.AppUser).Where(c => c.ProductId == product.ProductId && c.IsAccepted == true).Count();
            }



            return View(shopVM);
        }

        public IActionResult ChangeColor(int id)
        {
            if (context.ProductSizeColors.FirstOrDefault(p => p.Id == id) == null)
            {
                return NotFound();
            }
            ProductSizeColor product = context.ProductSizeColors.Include(p => p.Color).Include(p => p.Size).Include(p => p.ProductImages).Include(p => p.Product).ThenInclude(p => p.SubCategory).ThenInclude(s => s.Category).Include(p => p.Product).ThenInclude(p => p.Brand).Include(p => p.Product).ThenInclude(p => p.Gender).Include(p => p.Product).ThenInclude(p => p.ProductSizeColors).ThenInclude(p => p.Size).FirstOrDefault(p => p.Id == id);

            ProductSizeColor forcolor = context.ProductSizeColors.Include(p => p.Color).Include(p => p.Size).Include(p => p.ProductImages).Include(p => p.Product).ThenInclude(p => p.SubCategory).ThenInclude(s => s.Category).Include(p => p.Product).ThenInclude(p => p.Brand).Include(p => p.Product).ThenInclude(p => p.Gender).Include(p => p.Product).ThenInclude(p => p.ProductSizeColors).ThenInclude(p => p.Size).Include(p => p.Product).ThenInclude(p => p.ProductColors).FirstOrDefault(p => p.Id == id);

            ShopVM shopVM = new ShopVM
            {
                ProductSizeColor = context.ProductSizeColors.Include(p => p.Color).Include(p => p.Size).Include(p => p.ProductImages).Include(p => p.Product).ThenInclude(p => p.SubCategory).ThenInclude(s => s.Category).Include(p => p.Product).ThenInclude(p => p.Brand).Include(p => p.Product).ThenInclude(p => p.Gender).Include(p => p.Product).ThenInclude(p => p.ProductSizeColors).ThenInclude(p => p.Size).Include(p => p.Product).ThenInclude(p => p.ProductColors).FirstOrDefault(p => p.Id == id),
                Products = context.Products.Include(p => p.Brand).Include(p => p.Category).Include(p => p.SubCategory).Include(p => p.ProductSizeColors).ThenInclude(p => p.ProductImages).Include(p => p.ProductColors).Include(p => p.ProductSizeColors).ThenInclude(p => p.Size).Include(p => p.ProductSizeColors).ThenInclude(p => p.Color).Where(p => p.CategoryId == product.Product.CategoryId && p.Id != product.ProductId).ToList(),
                Product = context.Products.Include(p => p.Brand).Include(p => p.Category).Include(p => p.SubCategory).Include(p => p.ProductSizeColors).ThenInclude(p => p.ProductImages).Include(p => p.ProductColors).Include(p => p.ProductSizeColors).ThenInclude(p => p.Size).Include(p => p.ProductSizeColors).ThenInclude(p => p.Color).FirstOrDefault(p => p.Id == id),
                ProductColors = context.ProductColors.Where(p => p.Product.CategoryId == product.Product.CategoryId && p.ProductId != forcolor.ProductId).ToList()

            };

            return PartialView("_productDetailPartialView", shopVM);

        }
    }
}
