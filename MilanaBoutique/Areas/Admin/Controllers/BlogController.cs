using MilanaBoutique.DAL;
using MilanaBoutique.Extensions;
using MilanaBoutique.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MilanaBoutique.Areas.Admin.Controllers
{
    [Authorize(Roles ="Admin,SuperAdmin")]
    [Area("Admin")]
    public class BlogController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        private readonly AppDbContext _context;
        private IWebHostEnvironment _env;
        public BlogController(AppDbContext context, IWebHostEnvironment env, UserManager<AppUser> userManager)
        {
            _context = context;
            _env = env;
            _userManager = userManager;
        }
        public IActionResult Index(int page = 1)
        {
            ViewBag.CurrentPage = page;
            ViewBag.TotalPage = Math.Ceiling((decimal)_context.Blogs.Count() / 6);
            List<Blog> blogs = _context.Blogs.Skip((page - 1) * 6).Take(6).ToList();
            return View(blogs);
        }

        [Authorize(Roles ="SuperAdmin")]
        public IActionResult Create()
        {
            ViewBag.Tags = _context.Tags.ToList();
            return View()
;        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(Blog blog)
        {
            ViewBag.Tags = _context.Tags.ToList();
            if (!ModelState.IsValid)
            {
                return View();
            }
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);


            if (blog.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "image cannot be null");
                return View();
            }
            if (!blog.ImageFile.IsSizeOkay(2))
            {
                ModelState.AddModelError("ImageFile", "image size must be less than 2mb");
                return View();
            }
            if (!blog.ImageFile.IsImage())
            {
                ModelState.AddModelError("ImageFile", "only image files");
                return View();
            }

            blog.Image = blog.ImageFile.SaveImg(_env.WebRootPath, "assets/images/blog");

            blog.BlogTags = new List<BlogTag>();
            foreach (var item in blog.TagIds)
            {
                BlogTag blogTag = new BlogTag
                {
                    Blog = blog,
                    TagId = item
                };
                blog.BlogTags.Add(blogTag);
            }
            blog.CreatedTime = DateTime.Now;
            blog.Author = user.UserName;
            _context.Blogs.Add(blog);
           await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));



        }
        public IActionResult Edit(int id)
        {
            ViewBag.Tags = _context.Tags.ToList();

            Blog blog = _context.Blogs.Include(c=>c.BlogTags).FirstOrDefault(b => b.Id == id);
            if (blog == null)
            {
                return NotFound();
            }
            return View(blog);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Edit(Blog blog)
        {


            ViewBag.Tags = _context.Tags.ToList();
            Blog exist = _context.Blogs.Include(c => c.BlogTags).FirstOrDefault(c => c.Id == blog.Id);
          

            if (!ModelState.IsValid)
            {
                return View(exist);
            }

            if (blog.ImageFile != null)
            {
                if (!blog.ImageFile.IsSizeOkay(2))
                {
                    ModelState.AddModelError("ImageFile", "image file must be less than 2mb");
                    return View(blog);
                }
                if (!blog.ImageFile.IsImage())
                {
                    ModelState.AddModelError("ImageFile", "select an image file");
                    return View(blog);
                }
                Helpers.Helper.DeleteImg(_env.WebRootPath, "assets/images/blog", exist.Image);
                exist.Image = blog.ImageFile.SaveImg(_env.WebRootPath, "assets/images/blog");

            }

            exist.BlogTags = new List<BlogTag>();

            foreach (var item in blog.TagIds)
            {
                BlogTag blogTag = new BlogTag
                {
                    BlogId = exist.Id,
                    TagId = item
                };

                exist.BlogTags.Add(blogTag);
            }

         
            exist.Name = blog.Name;
            exist.BlackQuote = blog.BlackQuote;
            exist.Description = blog.Description;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            Blog blog = _context.Blogs.FirstOrDefault(c => c.Id == id);
            if (blog==null)
            {
                return NotFound();
            }

            _context.Blogs.Remove(blog);
            _context.SaveChanges();
            return Json(new {status=200 });
        }
    }
}
