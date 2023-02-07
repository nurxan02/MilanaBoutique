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

    public class CategoryController : Controller
    {
        private readonly AppDbContext context;
        private readonly IWebHostEnvironment env;

        public CategoryController(AppDbContext context, IWebHostEnvironment env)
        {
            this.context = context;
            this.env = env;
        }
        public IActionResult Index(int page = 1)
        {
          


            ViewBag.CurrentPage = page;
            ViewBag.TotalPage = Math.Ceiling((decimal)context.Categories.Count() / 5);
            List<Category> categories = context.Categories.Include(c=>c.Gender).Include(c=>c.Products).Include(c=>c.SubCategories).Skip((page - 1) * 5).Take(5).ToList();
            return View(categories);
        }

        public IActionResult Create()
        {
            ViewBag.Genders = context.Genders.ToList();
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create(Category category)
        {
            ViewBag.Genders = context.Genders.ToList();



            if (!ModelState.IsValid)
            {
                return View();
            }
            if (context.Categories.Any(c=>c.Name.Trim().ToLower() == category.Name.Trim().ToLower() && c.GenderId == category.GenderId))
            {
                ModelState.AddModelError("", "This category already exists");
                return View();
            }
            
            if (category.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "Select an image");
                return View();
            }
            if (category.SizeGuideFile == null)
            {
                ModelState.AddModelError("SizeGuideFile", "Select an image");
                return View();
            }
            if (category.GenderId == 0)
            {
                ModelState.AddModelError("", "Select an gender");
                return View();
            }

          

            if (!category.ImageFile.IsSizeOkay(2))
            {
                ModelState.AddModelError("ImageFile", "Image size must be less than 2mb!");
                return View();
            }
            if (!category.ImageFile.IsImage())
            {
                ModelState.AddModelError("ImageFile", "Select only image file!");
                return View();
            }

            if (!category.SizeGuideFile.IsSizeOkay(2))
            {
                ModelState.AddModelError("SizeGuideFile", "Image size must be less than 2mb!");
                return View();
            }
            if (!category.SizeGuideFile.IsImage())
            {
                ModelState.AddModelError("SizeGuideFile", "Select only image file!");
                return View();
            }
            category.SizeGuideImage = category.SizeGuideFile.SaveImg(env.WebRootPath, "assets/images/category");
            category.Image = category.ImageFile.SaveImg(env.WebRootPath, "assets/images/category");
            context.Categories.Add(category);
            context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)

        {
            ViewBag.Genders = context.Genders.ToList();

            Category category = context.Categories.Include(c =>c.Products).Include(c=>c.SubCategories).FirstOrDefault(c=> c.Id == id);
            if (category==null)
            {
                return NotFound();
            }
            return View(category);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Edit(Category category)
        {
            ViewBag.Genders = context.Genders.ToList();

            Category existed = context.Categories.FirstOrDefault(c => c.Id == category.Id);
            if (existed==null)
            {
                return NotFound();
            }


            if (!ModelState.IsValid)
            {
                return View(existed);
            }


            if (context.Categories.Any(c => c.Name.Trim().ToLower() == category.Name.Trim().ToLower() && c.GenderId == category.GenderId && c.Id != existed.Id))
            {
                ModelState.AddModelError("", "This category already exists");
                return View(existed);
            }


            if (category.ImageFile != null)
            {
                if (!category.ImageFile.IsSizeOkay(2))
                {
                    ModelState.AddModelError("ImageFile", "Image size must be less than 2mb!");
                    return View(existed);
                }
                if (!category.ImageFile.IsImage())
                {
                    ModelState.AddModelError("ImageFile", "Select only image file!");
                    return View(existed);
                }
                Helpers.Helper.DeleteImg(env.WebRootPath, "assets/images/category", existed.Image);
                existed.Image = category.ImageFile.SaveImg(env.WebRootPath, "assets/images/category");
            
            }


            if (category.SizeGuideFile != null)
            {
                if (!category.SizeGuideFile.IsSizeOkay(2))
                {
                    ModelState.AddModelError("SizeGuideFile", "Image size must be less than 2mb!");
                    return View(existed);
                }
                if (!category.SizeGuideFile.IsImage())
                {
                    ModelState.AddModelError("SizeGuideFile", "Select only image file!");
                    return View(existed);
                }
                Helpers.Helper.DeleteImg(env.WebRootPath, "assets/images/category", existed.SizeGuideImage);
                existed.SizeGuideImage = category.SizeGuideFile.SaveImg(env.WebRootPath, "assets/images/category");
            }


            if (category.GenderId==0)
            {
                ModelState.AddModelError("GenderId", "Select an gender");
                return View(existed);
            }
            existed.GenderId = category.GenderId;
            existed.Name = category.Name;
            context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }   

        public IActionResult Delete(int id)
        {
            ViewBag.Genders = context.Genders.ToList();
            Category existed = context.Categories.FirstOrDefault(c => c.Id == id);
            if (existed == null)
            {
                return NotFound();
            }
            Helpers.Helper.DeleteImg(env.WebRootPath, "assets/images/category",existed.Image);
            context.Categories.Remove(existed);
            context.SaveChanges();
            return Json(new { status = 200 });
        }

    }
}
