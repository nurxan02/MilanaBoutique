using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MilanaBoutique.DAL;
using MilanaBoutique.Extensions;
using MilanaBoutique.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MilanaBoutique.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin,Admin")]

    public class BrandController : Controller
    {
        private readonly AppDbContext context;
        private readonly IWebHostEnvironment env;

        public BrandController(AppDbContext context, IWebHostEnvironment env)
        {
            this.context = context;
            this.env = env;
        }
        public IActionResult Index(int page=1)
        {
            ViewBag.CurrentPage = page;
            ViewBag.TotalPage = Math.Ceiling((decimal)context.Brands.Count() / 5);
            List<Brand> brands = context.Brands.Include(b=>b.Products).Skip((page-1)*5).Take(5).ToList();
            return View(brands);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create(Brand brand)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (brand.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "Image cannot be null");
                return View();
            }

            if (!brand.ImageFile.IsSizeOkay(2))
            {
                ModelState.AddModelError("ImageFile", "Image size cannot be bigger than 2MB");
                return View();

            }
            if (!brand.ImageFile.IsImage())
            {
                ModelState.AddModelError("ImageFile", "Select only image files!");
                return View();

            }

            brand.Image = brand.ImageFile.SaveImg(env.WebRootPath, "assets/images/logos");

            context.Brands.Add(brand);
            context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            Brand brand = context.Brands.Include(b => b.Products).FirstOrDefault(b => b.Id == id);
            if (brand==null)
            {
                return NotFound();
            }
            return View(brand);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Edit(Brand brand)
        {
            Brand existed = context.Brands.Include(b => b.Products).FirstOrDefault(b => b.Id == brand.Id);
            if (existed == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return View(existed);
            }

            if (brand.ImageFile != null)
            {
                if (!brand.ImageFile.IsSizeOkay(2))
                {
                    ModelState.AddModelError("ImageFile", "Image size cannot be bigger than 2MB");
                    return View(existed);

                }
                if (!brand.ImageFile.IsImage())
                {
                    ModelState.AddModelError("ImageFile", "Select only image files!");
                    return View(existed);

                }

                Helpers.Helper.DeleteImg(env.WebRootPath, "assets/images/logos", existed.Image);
                existed.Image = brand.ImageFile.SaveImg(env.WebRootPath, "assets/images/logos");
            }

            existed.Name = brand.Name;
            context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            Brand brand = context.Brands.Include(b => b.Products).FirstOrDefault(b => b.Id == id);
            if (brand == null)
            {
                return NotFound();
            }
            Helpers.Helper.DeleteImg(env.WebRootPath, "assets/images/slider", brand.Image);

            context.Brands.Remove(brand);
            context.SaveChanges();
            return Json(new { status = 200 });
        }
    }
}
