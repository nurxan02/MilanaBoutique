using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MilanaBoutique.DAL;
using MilanaBoutique.Extensions;
using MilanaBoutique.Models;
using MilanaBoutique.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace MilanaBoutique.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin,Admin")]

    public class ProductController : Controller
    {
        private readonly AppDbContext context;
        private readonly IWebHostEnvironment env;

        public ProductController(AppDbContext context, IWebHostEnvironment env)
        {
            this.context = context;
            this.env = env;
        }

        public IActionResult Index(int page = 1)
        {
            ViewBag.CurrentPage = page;
            ViewBag.TotalPage = Math.Ceiling((decimal)context.Products.Count() / 10);
            List<Product> products = context.Products.Include(p => p.SubCategory).Include(p => p.Category).Include(p => p.Gender).Include(p => p.Brand).Include(p => p.ProductColors).Include(p => p.ProductSizeColors).Skip((page - 1) * 10).Take(10).ToList();
            return View(products);
        }
        public IActionResult ColorSizes(int id,int page = 1)
        {
         
            List<ProductSizeColor> products = context.ProductSizeColors.Include(p => p.Product).ThenInclude(p => p.SubCategory).ThenInclude(p => p.Category).ThenInclude(c => c.Gender).Include(c => c.Color).Include(p => p.Size).Where(c=>c.ProductId == id).Skip((page - 1) * 10).Take(10).ToList();
           
            ViewBag.CurrentPage = page;
            ViewBag.TotalPage = Math.Ceiling((decimal)context.ProductSizeColors.Include(c=>c.Product).Where(c=>c.ProductId==id).Count()/ 10);
            return View(products);
        }



        public IActionResult Create()
        {
            ViewBag.Genders = context.Genders.ToList();
            ViewBag.Categories = context.Categories.Include(c => c.Gender).Include(c => c.SubCategories).ToList();
            ViewBag.Subcategories = context.SubCategories.Include(s => s.Category).ThenInclude(c => c.Gender).ToList();
            ViewBag.Brands = context.Brands.Include(c => c.Products).ToList();
            ViewBag.Colors = context.Colors.ToList();
            ViewBag.Sizes = context.Sizes.ToList();
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(ProdCreateVM prodVM)
        {
            ViewBag.Genders = context.Genders.Include(g => g.Categories).ThenInclude(c => c.SubCategories).ToList();
            ViewBag.Categories = context.Categories.Include(c => c.Gender).Include(c => c.SubCategories).ToList();
            ViewBag.Subcategories = context.SubCategories.Include(s => s.Category).ThenInclude(c => c.Gender).ToList();
            ViewBag.Brands = context.Brands.Include(c => c.Products).ToList();
            ViewBag.Colors = context.Colors.Include(s => s.ProductSizeColors).ToList();
            ViewBag.Sizes = context.Sizes.Include(s => s.ProductSizeColors).ToList();
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (prodVM.Product.GenderId == 0)
            {
                ModelState.AddModelError("", "Select a gender");
                return View();
            }
            if (prodVM.Product.CategoryId == 0)
            {
                ModelState.AddModelError("", "Select a category");
                return View();
            }
            if (prodVM.Product.SubCategoryId == 0)
            {
                ModelState.AddModelError("", "Select a subcategory!");
                return View();
            }
            if (prodVM.Product.BrandId == 0)
            {
                ModelState.AddModelError("", "Select a brand");
                return View();
            }
            if (prodVM.ProductSizeColor.ColorId == 0)
            {
                ModelState.AddModelError("", "Select a color");
                return View();
            }
            if (prodVM.ProductSizeColor.SizeId == 0)
            {
                ModelState.AddModelError("", "Select a Size");
                return View();
            }
            if (prodVM.ProductSizeColor.MainImageFile == null)
            {
                ModelState.AddModelError("", "Select a main image!");
                return View();
            }
            if (!prodVM.ProductSizeColor.MainImageFile.IsImage())
            {
                ModelState.AddModelError("", "Choose only image file!");
                return View();
            }
            if (!prodVM.ProductSizeColor.MainImageFile.IsSizeOkay(2))
            {
                ModelState.AddModelError("", "Image size must be less than 2MB");
                return View();
            }
            if (prodVM.ProductSizeColor.ImageFiles == null)
            {
                ModelState.AddModelError("", "Other images cannot be null");
                return View();
            }
            if (prodVM.ProductSizeColor.ImageFiles.Count < 3)
            {
                ModelState.AddModelError("", "You cant select less than 3 images!");
                return View();
            }
            if (prodVM.ProductSizeColor.ImageFiles.Count() > 3)
            {
                ModelState.AddModelError("", "You can choose only 3 images");
                return View();
            }
            ProductSizeColor productSizeColor = new ProductSizeColor();

            productSizeColor.ProductImages = new List<ProductImage>();

            prodVM.Product.ProductColors = new List<ProductColor>();

            foreach (var item in prodVM.ProductSizeColor.ImageFiles)
            {
                if (!item.IsImage())
                {
                    ModelState.AddModelError("", "Choose only image file!");
                    return View();
                }
                if (!item.IsSizeOkay(2))
                {
                    ModelState.AddModelError("", "Image size must be less than 2MB");
                    return View();
                }

                ProductImage productImage = new ProductImage
                {
                    Image = item.SaveImg(env.WebRootPath, "assets/images/products"),
                    ProductSizeColor = productSizeColor
                };
                productSizeColor.ProductImages.Add(productImage);
            }

            productSizeColor.SizeId = prodVM.ProductSizeColor.SizeId;
            productSizeColor.ColorId = prodVM.ProductSizeColor.ColorId;
            productSizeColor.TotalStock = prodVM.ProductSizeColor.TotalStock;
            productSizeColor.TotalSold = 0;
            productSizeColor.MainImage = prodVM.ProductSizeColor.MainImageFile.SaveImg(env.WebRootPath, "assets/images/products");

            productSizeColor.ProductId = prodVM.Product.Id;
            prodVM.Product.ProductSizeColors = new List<ProductSizeColor>();

            prodVM.Product.ProductSizeColors.Add(productSizeColor);
            prodVM.Product.CreatedDate = DateTime.Now;

            ProductColor productColor = new ProductColor
            {
                ColorId = prodVM.ProductSizeColor.ColorId,
                ProductId = prodVM.Product.Id
            };
            prodVM.Product.ProductColors.Add(productColor);
            context.Products.Add(prodVM.Product);
            context.SaveChanges();
            //string prodlink = $"https://localhost:44388/{prodVM.Product.Gender.Name}/Details/" + $"{productSizeColor.Id}";
            string prodlink = $"http://nurxan02-001-site1.atempurl.com/{prodVM.Product.Gender.Name}/Details/" + $"{productSizeColor.Id}";
            MailMessage mail = new MailMessage();
            byte[] imageArray = System.IO.File.ReadAllBytes($"wwwroot/assets/images/products/{productSizeColor.MainImage}");
            string base64ImageRepresentation = Convert.ToBase64String(imageArray);
            string body = string.Empty;
            using (StreamReader reader = new StreamReader("wwwroot/assets/template/NewProduct.html"))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{link}", prodlink);


            mail.From = new MailAddress("finalprojectcodeacademy@gmail.com", "Milana Boutique");

            mail.Subject = "New product!";

            mail.Body = body;
            foreach (var item in context.Subscribers)
            {

                mail.To.Add(new MailAddress(item.Email));


            }
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.EnableSsl = true;

            smtp.Credentials = new NetworkCredential("finalprojectcodeacademy@gmail.com", "shldhrtkghufzhmg");
            smtp.Send(mail);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult AddColorSize(int id)
        {
            ViewBag.Genders = context.Genders.Include(g => g.Categories).ThenInclude(c => c.SubCategories).ToList();
            ViewBag.Categories = context.Categories.Include(c => c.Gender).Include(c => c.SubCategories).ToList();
            ViewBag.Subcategories = context.SubCategories.Include(s => s.Category).ThenInclude(c => c.Gender).ToList();
            ViewBag.Brands = context.Brands.Include(c => c.Products).ToList();
            ViewBag.Colors = context.Colors.Include(s => s.ProductSizeColors).ToList();
            ViewBag.Sizes = context.Sizes.Include(s => s.ProductSizeColors).ToList();


            ProdCreateVM prodCreateVM = new ProdCreateVM
            {

                Product = context.Products.Include(p => p.ProductColors).Include(p => p.ProductSizeColors).FirstOrDefault(p => p.Id == id),
                ProductSizeColor = new ProductSizeColor()

            };


            return View(prodCreateVM);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult AddColorSize(ProdCreateVM prodVM)
        {
            ViewBag.Genders = context.Genders.Include(g => g.Categories).ThenInclude(c => c.SubCategories).ToList();
            ViewBag.Categories = context.Categories.Include(c => c.Gender).Include(c => c.SubCategories).ToList();
            ViewBag.Subcategories = context.SubCategories.Include(s => s.Category).ThenInclude(c => c.Gender).ToList();
            ViewBag.Brands = context.Brands.Include(c => c.Products).ToList();
            ViewBag.Colors = context.Colors.Include(s => s.ProductSizeColors).ToList();
            ViewBag.Sizes = context.Sizes.Include(s => s.ProductSizeColors).ToList();
          
            Product existed = context.Products.Include(p => p.ProductSizeColors).Include(p => p.ProductColors).FirstOrDefault(p => p.Id == prodVM.Product.Id);




            if (prodVM.ProductSizeColor.SizeId == 0)
            {
                ModelState.AddModelError("", "Select a Size");
                return View();
            }
            if (prodVM.ProductSizeColor.ColorId == 0)
            {
                ModelState.AddModelError("", "Select a color");
                return View();
            }
            if (prodVM.ProductSizeColor.MainImageFile == null)
            {
                ModelState.AddModelError("", "Select a main image!");
                return View();
            }
            if (!prodVM.ProductSizeColor.MainImageFile.IsImage())
            {
                ModelState.AddModelError("", "Choose only image file!");
                return View();
            }
            if (!prodVM.ProductSizeColor.MainImageFile.IsSizeOkay(2))
            {
                ModelState.AddModelError("", "Image size must be less than 2MB");
                return View();
            }
            if (prodVM.ProductSizeColor.ImageFiles == null)
            {
                ModelState.AddModelError("", "Other images cannot be null");
                return View();
            }
            if (prodVM.ProductSizeColor.ImageFiles.Count < 3)
            {
                ModelState.AddModelError("", "You cant select less than 3 images!");
                return View();
            }
            if (prodVM.ProductSizeColor.ImageFiles.Count() > 3)
            {
                ModelState.AddModelError("", "You can choose only 3 images");
                return View();
            }

            ProductSizeColor productSizeColor = new ProductSizeColor();

            productSizeColor.ProductImages = new List<ProductImage>();

            foreach (var item in prodVM.ProductSizeColor.ImageFiles)
            {
                if (!item.IsImage())
                {
                    ModelState.AddModelError("", "Choose only image file!");
                    return View();
                }
                if (!item.IsSizeOkay(2))
                {
                    ModelState.AddModelError("", "Image size must be less than 2MB");
                    return View();
                }

                ProductImage productImage = new ProductImage
                {
                    Image = item.SaveImg(env.WebRootPath, "assets/images/products"),
                    ProductSizeColor = productSizeColor
                };
                productSizeColor.ProductImages.Add(productImage);
            }


            productSizeColor.SizeId = prodVM.ProductSizeColor.SizeId;
            productSizeColor.ColorId = prodVM.ProductSizeColor.ColorId;
            productSizeColor.TotalStock = prodVM.ProductSizeColor.TotalStock;
            productSizeColor.TotalSold = 0;
            productSizeColor.MainImage = prodVM.ProductSizeColor.MainImageFile.SaveImg(env.WebRootPath, "assets/images/products");

            productSizeColor.Product = existed;

            productSizeColor.Product.Id = existed.Id;

            if (!existed.ProductColors.Any(p => p.ColorId == prodVM.ProductSizeColor.ColorId))
            {
                ProductColor productColor1 = new ProductColor();


                productColor1.ProductId = existed.Id;
                productColor1.ColorId = prodVM.ProductSizeColor.ColorId;

                existed.ProductColors.Add(productColor1);

            }




            if (existed.ProductSizeColors.Any(p => p.ColorId == prodVM.ProductSizeColor.ColorId && p.SizeId == prodVM.ProductSizeColor.SizeId))
            {

                Color color = existed.ProductSizeColors.FirstOrDefault(p => p.ColorId == prodVM.ProductSizeColor.ColorId).Color;
                Size size = existed.ProductSizeColors.FirstOrDefault(p => p.SizeId == prodVM.ProductSizeColor.SizeId).Size;
                string existColor = color.Name;

                ModelState.AddModelError("", $"{existed.Name} product already has option with COLOR : {existColor} SIZE : {size.Name} ");
                return View();
            }


            existed.ProductSizeColors.Add(productSizeColor);
          
            context.SaveChanges();


            return RedirectToAction(nameof(Index));

        }

        public IActionResult Edit(int id)
        {
            Product product = context.Products.Include(p => p.SubCategory).ThenInclude(s=>s.Category).Include(p => p.Category).ThenInclude(c=>c.Gender).Include(p => p.Gender).Include(p => p.Brand).Include(p => p.ProductColors).Include(p => p.ProductSizeColors).FirstOrDefault(p => p.Id == id);

            ViewBag.Genders = context.Genders.Include(g => g.Categories).ThenInclude(c => c.SubCategories).ToList();
            ViewBag.Categories = context.Categories.Include(c => c.Gender).Include(c => c.SubCategories).ToList();
            ViewBag.Subcategories = context.SubCategories.Include(s => s.Category).ThenInclude(c => c.Gender).ToList();
            ViewBag.Brands = context.Brands.Include(c => c.Products).ToList();
            ViewBag.Colors = context.Colors.Include(s => s.ProductSizeColors).ToList();
            ViewBag.Sizes = context.Sizes.Include(s => s.ProductSizeColors).ToList();
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Edit(Product prod)
        {

            ViewBag.Genders = context.Genders.Include(g => g.Categories).ThenInclude(c => c.SubCategories).ToList();
            ViewBag.Categories = context.Categories.Include(c => c.Gender).Include(c => c.SubCategories).ToList();
            ViewBag.Subcategories = context.SubCategories.Include(s => s.Category).ThenInclude(c => c.Gender).ToList();
            ViewBag.Brands = context.Brands.Include(c => c.Products).ToList();
            ViewBag.Colors = context.Colors.Include(s => s.ProductSizeColors).ToList();
            ViewBag.Sizes = context.Sizes.Include(s => s.ProductSizeColors).ToList();
       
            

            Product exist = context.Products.Include(p => p.SubCategory).Include(p => p.Category).Include(p => p.Gender).Include(p => p.Brand).Include(p => p.ProductColors).Include(p => p.ProductSizeColors).FirstOrDefault(p => p.Id == prod.Id);

            if (exist == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return View(prod);
            }
            if (prod.GenderId == 0)
            {
                ModelState.AddModelError("GenderId", "Select a gender");
                return View(prod);
            }
            if (prod.CategoryId == 0)
            {
                ModelState.AddModelError("CategoryId", "Select a category");
                return View(prod);
            }
            if (prod.SubCategoryId == 0)
            {
                ModelState.AddModelError("SubCategoryId", "Select a subcategory!");
                return View(prod);
            }
            if (prod.BrandId == 0)
            {
                ModelState.AddModelError("BrandId", "Select a brand");
                return View(prod);
            }

            Category exstCat = context.Categories.Include(c=>c.Gender).FirstOrDefault(p => p.Id == prod.CategoryId);
            SubCategory exsSub = context.SubCategories.FirstOrDefault(s=>s.Id == prod.SubCategoryId);
            Gender prodsgend = context.Genders.FirstOrDefault(g => g.Id == prod.GenderId);

            Gender reverse = context.Genders.FirstOrDefault(g => g.Id != exstCat.GenderId);

            if (prod.GenderId != exstCat.Gender.Id)
            {
                ModelState.AddModelError("", $"You cant add {prodsgend.Name} gender product to {exstCat.Gender.Name} category!");
                return View(prod);
            }
            if (prod.CategoryId != exsSub.CategoryId)
            {
                ModelState.AddModelError("", $"You cant add {exstCat.Gender.Name} category product to {reverse.Name} subcategory!");
                return View(prod);
            }

            exist.Name = prod.Name;
            exist.Price = prod.Price;
            exist.Description = prod.Description;
            exist.Discount = prod.Discount;
            exist.Subtitle =prod.Subtitle;
            exist.GenderId = prod.GenderId;
            exist.CategoryId =prod.CategoryId;
            exist.SubCategoryId = prod.SubCategoryId;
            exist.BrandId = prod.BrandId;
            context.SaveChanges();
            return RedirectToAction(nameof(Index));


        }

        public IActionResult Test()
        {
            Product prod = context.Products.Include(p=>p.ProductColors).FirstOrDefault(p => p.Id == 1027);
            return Json(prod.ProductColors.Count);

        }
        public IActionResult DeleteSizeColor(int id)
        {
            ProductSizeColor product = context.ProductSizeColors.Include(p => p.Color).Include(p => p.Product).FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            Product pro = context.Products.Include(p => p.ProductSizeColors).Include(p => p.ProductColors).FirstOrDefault(p => p.Id == product.ProductId);
            if (pro.ProductSizeColors.Where(p => p.ColorId == product.ColorId).Count() == 1)
            {
                ProductColor productColor = context.ProductColors.FirstOrDefault(p => p.ProductId == pro.Id && p.ColorId == product.ColorId);

                context.ProductColors.Remove(productColor);
            }
            if (pro.ProductSizeColors.Count() == 1)
            {
                context.Products.Remove(pro);
                context.ProductSizeColors.Remove(product);

                context.SaveChanges();

                return Json(new {status=201 });
            }

            context.ProductSizeColors.Remove(product);

            context.SaveChanges();
            return Json(new { status = 200 });


        }

        public IActionResult Delete(int id)
        {
            Product exist = context.Products.Include(p => p.SubCategory).Include(p => p.Category).Include(p => p.Gender).Include(p => p.Brand).Include(p => p.ProductColors).Include(p => p.ProductSizeColors).FirstOrDefault(p => p.Id == id);

            if (exist == null)
            {
                return NotFound();
            }
            List<ProductSizeColor> sizeColors = context.ProductSizeColors.Where(p => p.ProductId == exist.Id).ToList();
            context.Products.Remove(exist);
            context.ProductSizeColors.RemoveRange(exist.ProductSizeColors);
           
            context.SaveChanges();
            return Json(new { status = 200 });
        }

        public IActionResult EditColorSize(int id)
        {
            ViewBag.Colors = context.Colors.Include(s => s.ProductSizeColors).ToList();
            ViewBag.Sizes = context.Sizes.Include(s => s.ProductSizeColors).ToList();
            ProductSizeColor product = context.ProductSizeColors.Include(p=>p.ProductImages).Include(p=>p.Size).Include(p => p.Color).Include(p => p.Product).Include(p => p.Product).ThenInclude(p => p.SubCategory).ThenInclude(p => p.Category).ThenInclude(c => c.Gender).FirstOrDefault(p => p.Id == id);


          


            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult EditColorSize(ProductSizeColor prod)
        {

            ViewBag.Colors = context.Colors.Include(s => s.ProductSizeColors).ToList();
            ViewBag.Sizes = context.Sizes.Include(s => s.ProductSizeColors).ToList();
            ProductSizeColor exist = context.ProductSizeColors.Include(p => p.ProductImages).Include(p => p.Size).Include(p => p.Color).Include(p => p.Product).ThenInclude(p => p.Gender).Include(p=>p.Product).ThenInclude(p => p.SubCategory).ThenInclude(p => p.Category).ThenInclude(c => c.Gender).FirstOrDefault(p => p.Id == prod.Id);
            Product product = context.Products.Include(p=>p.ProductColors).Include(p=>p.ProductSizeColors).ThenInclude(p=>p.Size).Include(p=>p.ProductSizeColors).ThenInclude(p=>p.Color).FirstOrDefault(p => p.Id == exist.ProductId);



            var clrName = context.Colors.FirstOrDefault(c => c.Id == prod.ColorId);
            var size = context.Sizes.FirstOrDefault(s => s.Id == prod.SizeId);


            if (exist == null)
            {
                return NotFound();
            }
            if (prod.SizeId == 0)
            {
                ModelState.AddModelError("", "Select a Size");
                return View(exist);
            }
            if (prod.ColorId == 0)
            {
                ModelState.AddModelError("ColorId", "Select a color");
                return View(exist);
            }
            if (product.ProductSizeColors.Any(p=>p.SizeId == prod.SizeId && p.ColorId == prod.ColorId && p.Id != prod.Id))
            {
                ModelState.AddModelError("", $"{product.Name} already exists with {size.Name} size , {clrName.Name} color");
                return View(exist);
            }
            if (prod.MainImageFile != null)
            {
                if (!prod.MainImageFile.IsImage())
                {
                    ModelState.AddModelError("MainImageFile", "Select a image file");
                    return View(exist);
                }
                if (!prod.MainImageFile.IsSizeOkay(2))
                {
                    ModelState.AddModelError("MainImageFile", "Image size must be less than 2MB");
                    return View(exist);
                }
                Helpers.Helper.DeleteImg(env.WebRootPath, "assets/images/products", exist.MainImage);
                exist.MainImage = prod.MainImageFile.SaveImg(env.WebRootPath, "assets/images/products");

            }
            
            if (prod.ImageIds == null && prod.ImageFiles == null)
            {
                ModelState.AddModelError("", "images cannot be empty");
                return View(exist);
            }
            if (prod.ImageIds == null && prod.ImageFiles.Count > 3)
            {
                ModelState.AddModelError("", "select 3 images maximum");
                return View(exist);
            }
            if (prod.ImageIds == null && prod.ImageFiles.Count < 3)
            {
                ModelState.AddModelError("", "select 3 images minimum");
                return View(exist);
            }

            if (prod.ImageIds != null)
            {
                if (prod.ImageIds.Count() != 3 && prod.ImageFiles == null)
                {
                    ModelState.AddModelError("", $"select {(3 - prod.ImageIds.Count)} images!");
                    return View(exist);
                }

                if (prod.ImageIds.Count() == 3 && prod.ImageFiles != null)
                {
                    ModelState.AddModelError("", "You dont have any free image space");
                    return View(exist);
                }

                if (prod.ImageIds.Count() != 3 && prod.ImageFiles != null)
                {
                    if (prod.ImageIds.Count + prod.ImageFiles.Count > 3)
                    {
                        ModelState.AddModelError("", "total image count cant be more than 3!");
                        return View(exist);
                    }
                    if (prod.ImageIds.Count + prod.ImageFiles.Count < 3)
                    {
                        ModelState.AddModelError("", "total image count cant be less than 3!");
                        return View(exist);
                    }
                }
                if (prod.ImageIds == null && prod.ImageFiles.Count < 3)
                {
                    ModelState.AddModelError("", "select 3 images!");
                    return View(exist);
                }

                if (prod.ImageFiles != null)
                {

                    if (prod.ImageFiles.Count > 3)
                    {
                        ModelState.AddModelError("", "you can choose maximum 3 images");
                        return View(exist);
                    }
                    if (prod.ImageIds.Count == 3)
                    {
                        ModelState.AddModelError("", "you already have 3 images!");
                        return View(exist);
                    }
                    if (prod.ImageIds.Count == 2 && prod.ImageFiles.Count > 1)
                    {
                        ModelState.AddModelError("", "you have only 1 empty image space!");
                        return View(exist);
                    }
                    if (prod.ImageIds.Count == 1 && prod.ImageFiles.Count > 2)
                    {
                        ModelState.AddModelError("", "you have only 2 empty image space!");
                        return View(exist);
                    }
                    if (prod.ImageIds == null && prod.ImageFiles.Count < 3)
                    {
                        ModelState.AddModelError("", "select 3 images!");
                        return View(exist);
                    }

                    foreach (var item in prod.ImageFiles)
                    {
                        if (!item.IsImage())
                        {
                            ModelState.AddModelError("ImageFiles", "Select a image file");
                            return View(exist);
                        }
                        if (!item.IsSizeOkay(2))
                        {
                            ModelState.AddModelError("ImageFiles", "Image size must be less than 2MB");
                            return View(exist);
                        }

                    }

                    List<ProductImage> removableImages = exist.ProductImages.Where(p => !prod.ImageIds.Contains(p.Id)).ToList();
                    exist.ProductImages.RemoveAll(p => removableImages.Any(ri => ri.Id == p.Id));
                    foreach (var item in removableImages)
                    {
                        Helpers.Helper.DeleteImg(env.WebRootPath, "assets/images/products", item.Image);
                    }

                    foreach (var item in prod.ImageFiles)
                    {
                        ProductImage productImage = new ProductImage
                        {
                            Image = item.SaveImg(env.WebRootPath, "assets/images/products"),
                            ProductSizeColorId = exist.Id
                        };
                        exist.ProductImages.Add(productImage);
                    }
                }

            }

            if (prod.ImageIds == null && prod.ImageFiles.Count == 3)
            {
                foreach (var item in prod.ImageFiles)
                {
                    if (!item.IsImage())
                    {
                        ModelState.AddModelError("ImageFiles", "Select a image file");
                        return View(exist);
                    }
                    if (!item.IsSizeOkay(2))
                    {
                        ModelState.AddModelError("ImageFiles", "Image size must be less than 2MB");
                        return View(exist);
                    }

                }

                exist.ProductImages.RemoveAll(p=>p.ProductSizeColorId == exist.Id);
                foreach (var item in exist.ProductImages)
                {
                    Helpers.Helper.DeleteImg(env.WebRootPath, "assets/images/products", item.Image);
                }

                foreach (var item in prod.ImageFiles)
                {
                    ProductImage productImage = new ProductImage
                    {
                        Image = item.SaveImg(env.WebRootPath, "assets/images/products"),
                        ProductSizeColorId = exist.Id
                    };
                    exist.ProductImages.Add(productImage);
                }
            }
           



            if (!product.ProductColors.Any(p=>p.ColorId == prod.ColorId))
            {
                ProductColor color = new ProductColor
                {
                    ColorId = prod.ColorId,
                    ProductId = product.Id
                };
                product.ProductColors.Add(color);
            }
            if (prod.ColorId != exist.ColorId)
            {
                if (product.ProductSizeColors.Where(p=>p.ColorId == exist.ColorId).Count() == 1)
                {
                    ProductColor removable = context.ProductColors.FirstOrDefault(p => p.ProductId == product.Id && p.ColorId == exist.ColorId);
                    product.ProductColors.Remove(removable);
                }
            }
         
          
          

            exist.ColorId = prod.ColorId;
            exist.SizeId = prod.SizeId;
            exist.TotalStock = prod.TotalStock;
            context.SaveChanges();
           
            return RedirectToAction(nameof(ColorSizes),new {id = product.Id, page = 1 });

        }

        public IActionResult Dealoftheday(int id)
        {
            Product product = context.Products.FirstOrDefault(c => c.Id == id);
            if (product==null)
            {
                return NotFound();
            }

            if (product.DealOfTheDay)
            {
                product.DealOfTheDay = false;
                context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                foreach (var item in context.Products)
                {
                    item.DealOfTheDay = false;
                }

                product.DealOfTheDay = true;
                context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }


          
        }
    }
}
