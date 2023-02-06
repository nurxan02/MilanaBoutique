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
    public class BlogController : Controller
    {
        private readonly AppDbContext context;

        public BlogController(AppDbContext context)
        {
            this.context = context;
        }
        public IActionResult Index(int tagid,int page=1)
        {
          


            if (tagid > 0)
            {

                BlogVM vM = new BlogVM
                {
                    Blogs = context.Blogs.Include(c => c.Comments).Include(b => b.BlogTags).ThenInclude(b => b.Tag).Where(c => c.BlogTags.Any(bt => bt.TagId == tagid)).Skip((page - 1) * 6).Take(6).ToList(),
                    Tags = context.Tags.Include(c => c.BlogTags).ThenInclude(c => c.Blog).ToList()
                };
                ViewBag.Currentpage = page;
                ViewBag.Totalpage = Math.Ceiling((decimal)vM.Blogs.Count / 6);
                return View(vM);

            }
            else
            {
                BlogVM vM = new BlogVM
                {
                    Blogs = context.Blogs.Include(c => c.Comments).Include(b => b.BlogTags).ThenInclude(b => b.Tag).Skip((page - 1) * 6).Take(6).ToList(),
                    Tags = context.Tags.Include(c => c.BlogTags).ThenInclude(c => c.Blog).ToList()
                };
                ViewBag.Currentpage = page;
                ViewBag.Totalpage = Math.Ceiling((decimal)vM.Blogs.Count / 6);
                return View(vM);

            }

        }

        public IActionResult Details(int id)
        {
            Blog blog = context.Blogs.Include(c => c.Comments).Include(b => b.BlogTags).ThenInclude(b => b.Tag).FirstOrDefault(c => c.Id == id);

            if (blog==null)
            {
                return NotFound();
            }

          

            BlogVM vM = new BlogVM
            {
                Blogs = context.Blogs.Include(c => c.Comments).Include(b => b.BlogTags).ThenInclude(b => b.Tag).ToList(),
                Tags = context.Tags.Include(c => c.BlogTags).ThenInclude(c => c.Blog).ToList(),
                Blog = context.Blogs.Include(c => c.Comments).ThenInclude(c=>c.AppUser).Include(b => b.BlogTags).ThenInclude(b => b.Tag).FirstOrDefault(c => c.Id == id)
        };
            return View(vM);
        }

      
    }
}
