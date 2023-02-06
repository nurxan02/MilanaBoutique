using MilanaBoutique.DAL;
using MilanaBoutique.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace c.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin,Admin")]

    public class SubcategoryController : Controller
    {
        private readonly AppDbContext context;
        private readonly IWebHostEnvironment env;

        public SubcategoryController(AppDbContext context, IWebHostEnvironment env)
        {
            this.context = context;
            this.env = env;
        }
        public IActionResult Index(int page=1)
        {
            ViewBag.CurrentPage = page;
            ViewBag.TotalPage = Math.Ceiling((decimal)context.SubCategories.Count() / 5);
            List<SubCategory> subCategories = context.SubCategories.Include(s=>s.Products).Include(s=>s.Category).ThenInclude(c=>c.Gender).Skip((page-1)*5).Take(5).ToList();
            return View(subCategories);
        }

        public IActionResult Create()
        {
            ViewBag.Categories = context.Categories.Include(c=>c.Gender).Include(c=>c.SubCategories).ToList();
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create(SubCategory subCategory)
        {
            ViewBag.Categories = context.Categories.Include(c => c.Gender).Include(c => c.SubCategories).ToList();


            if (!ModelState.IsValid)
            {
                return View();
            }
            if (subCategory.CategoryId == 0)
            {
                ModelState.AddModelError("CategoryId", "Select a category");
                return View();
            }
            context.SubCategories.Add(subCategory);
            context.SaveChanges();
            return RedirectToAction(nameof(Index));

        }

        public IActionResult Edit(int id)
        {
            ViewBag.Categories = context.Categories.Include(c => c.Gender).Include(c => c.SubCategories).ToList();


            SubCategory subcategory = context.SubCategories.FirstOrDefault(s => s.Id == id);
            if (subcategory==null)
            {
                return NotFound();
            }
            return View(subcategory);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Edit(SubCategory subCategory)
        {
            ViewBag.Categories = context.Categories.Include(c => c.Gender).Include(c => c.SubCategories).ToList();

            SubCategory existed = context.SubCategories.FirstOrDefault(s => s.Id == subCategory.Id);

            if (existed==null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return View(subCategory);
            }
            if (subCategory.CategoryId==0)
            {
                ModelState.AddModelError("CategoryId", "Select a category!");
                return View(subCategory);
            }
            existed.Name = subCategory.Name;
            existed.CategoryId = subCategory.CategoryId;
            context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            SubCategory existed = context.SubCategories.FirstOrDefault(s => s.Id == id);

            if (existed == null)
            {
                return NotFound();
            }

            context.SubCategories.Remove(existed);
            context.SaveChanges();
            return Json(new { status = 200 });
        }
    }
}
