using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
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

    public class SliderController : Controller
    {
        private readonly AppDbContext context;
        private readonly IWebHostEnvironment env;

        public SliderController(AppDbContext context, IWebHostEnvironment env)
        {
            this.context = context;
            this.env = env;
        }
        public IActionResult Index(int page=1)
        {
            ViewBag.CurrentPage = page;
            ViewBag.TotalPage = Math.Ceiling((decimal)context.Sliders.Count() / 3);
            List<Slider> sliders = context.Sliders.Skip((page-1)*3).Take(3).ToList();
            return View(sliders);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create(Slider slider)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (slider.ImageFile==null)
            {
                ModelState.AddModelError("ImageFile", "Image cannot be null");
                return View();
            }

            if (!slider.ImageFile.IsSizeOkay(2))
            {
                ModelState.AddModelError("ImageFile", "Image size cannot be bigger than 2MB");
                return View();

            }
            if (!slider.ImageFile.IsImage())
            {
                ModelState.AddModelError("ImageFile", "Select only image files!");
                return View();

            }

            slider.Image = slider.ImageFile.SaveImg(env.WebRootPath, "assets/images/slider");
            context.Sliders.Add(slider);
            context.SaveChanges();
            return RedirectToAction(nameof(Index));

        }

        public IActionResult Edit(int id)
        {
            Slider slider = context.Sliders.FirstOrDefault(s => s.Id == id);
            if (slider==null)
            {
                return NotFound();
            }

            return View(slider);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]

        public IActionResult Edit(Slider slider)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            Slider existed = context.Sliders.FirstOrDefault(s => s.Id == slider.Id);
            if (existed==null)
            {
                return NotFound();
            }

            if (slider.ImageFile!=null)
            {
                if (!slider.ImageFile.IsSizeOkay(2))
                {
                    ModelState.AddModelError("ImageFile", "Image size cannot be bigger than 2MB");
                    return View(existed);

                }
                if (!slider.ImageFile.IsImage())
                {
                    ModelState.AddModelError("ImageFile", "Select only image files!");
                    return View(existed);

                }

                Helpers.Helper.DeleteImg(env.WebRootPath, "assets/images/slider", existed.Image);
                existed.Image = slider.ImageFile.SaveImg(env.WebRootPath, "assets/images/slider");
            }
            existed.Link = slider.Link;
            existed.TextBig = slider.TextBig;
            existed.TextSmall = slider.TextSmall;
            context.SaveChanges();
            return RedirectToAction(nameof(Index));

        }

        public IActionResult Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            Slider existed = context.Sliders.FirstOrDefault(s => s.Id == id);
            if (existed == null)
            {
                return NotFound();
            }

            Helpers.Helper.DeleteImg(env.WebRootPath, "assets/images/slider", existed.Image);
            context.Sliders.Remove(existed);
            context.SaveChanges();


            return Json(new { status = 200 });
        }
    }
}
