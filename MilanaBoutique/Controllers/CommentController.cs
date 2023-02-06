using MilanaBoutique.DAL;
using MilanaBoutique.Models;
using MilanaBoutique.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MilanaBoutique.Controllers
{
    public class CommentController : Controller
    {
        private readonly AppDbContext context;
        private readonly UserManager<AppUser> userManager;

        public CommentController(AppDbContext context, UserManager<AppUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }


        public async Task<IActionResult> AddComment(string Subject, string Message, int ProductId, short Star)
        {



            AppUser user = await userManager.FindByNameAsync(User.Identity.Name);
            if (!ModelState.IsValid)
            {
                return Json(new { status = 31 });
            }
            if (user == null)
            {
                return NotFound();
            }




            Product product = context.Products.Include(p => p.Comments).FirstOrDefault(p => p.Id == ProductId);

            if (product.Comments.Any(c=>c.AppUserId == user.Id))
            {
                return Content("exist");
            }

            if (product.Comments.Count == 0)
            {
                product.Rating = 0;
            }



            if (product.Rating == null)
            {
                product.Rating = 0;

            }
            if (context.Comments.Include(c => c.Product).Include(c => c.AppUser).Any(c => c.ProductId == ProductId && c.AppUserId == user.Id))
            {
                Star = 0;
            }


            product.Rating += Star;
            if (!context.Products.Any(p => p.Id == ProductId))
            {
                return NotFound();
            }
            Comment comment = new Comment
            {
                AppUserId = user.Id,
                CreatedTime = DateTime.Now,
                IsAccepted = true,
                Message = Message,
                ProductId = ProductId,
                Star = Star,
                Subject = Subject,

            };


            context.Comments.Add(comment);
            product.AvgStar = (short)Math.Ceiling((decimal)product.Rating / product.Comments.Where(c => c.Star != 0).Count());

            context.SaveChanges();

            ViewBag.Comment = context.Comments.FirstOrDefault(c => c.AppUserId == user.Id && c.ProductId == ProductId);
            ShopVM shopVM = new ShopVM
            {
                Comments = context.Comments.Include(c => c.Product).Include(c => c.AppUser).Where(c => c.ProductId == ProductId).ToList()

            };

            return PartialView("_commentPartialView", shopVM);
        }

        public IActionResult Startest()
        {
            Product product = context.Products.Include(p => p.Comments).FirstOrDefault(p => p.Id == 1027);

            return Json(Math.Ceiling((decimal)product.Rating / product.Comments.Count));
        }
        public IActionResult GetPartial(int id)
        {
            Product product = context.Products.Include(p => p.Brand).Include(p => p.Comments).Include(p => p.Category).Include(p => p.SubCategory).Include(p => p.ProductSizeColors).ThenInclude(p => p.ProductImages).Include(p => p.ProductColors).Include(p => p.ProductSizeColors).ThenInclude(p => p.Size).Include(p => p.ProductSizeColors).ThenInclude(p => p.Color).FirstOrDefault(p => p.Id == id);

            if (context.Comments.Where(c => c.ProductId == product.Id && c.IsAccepted == true) == null)
            {
                return Json(0);
            }
            else
            {

                return Json(context.Comments.Include(c => c.Product).Include(p => p.AppUser).Where(c => c.ProductId == product.Id && c.IsAccepted == true).Count());
            }

        }

        public IActionResult Count(int id)
        {
            var count = context.Comments.Include(p => p.Product).Where(p => p.ProductId == id).Count();

            return Json(count);
        }

        public async Task<IActionResult> AddBlogComment(string message, int blogid)
        {
            AppUser user = await userManager.FindByNameAsync(User.Identity.Name);
            if (!ModelState.IsValid)
            {
                return Json(new { status = 31 });
            }

            BlogComment comment = new BlogComment
            {
                BlogId = blogid,
                CreatedTime = DateTime.Now,
                Message = message,
                 AppUserId = user.Id,
                  IsAccepted = true
            };

            context.BlogComments.Add(comment);
            await context.SaveChangesAsync();

            BlogVM vM = new BlogVM
            {
                Blog = context.Blogs.Include(c=>c.Comments).ThenInclude(c=>c.AppUser).FirstOrDefault(c => c.Id == blogid)
            };

            return PartialView("_blogCommentPartial",vM);

        }

        public IActionResult Blogcountpartial(int blogid)
        {
            Blog blog = context.Blogs.Include(c=>c.Comments).FirstOrDefault(c => c.Id == blogid);
            return Json(blog.Comments.Where(c=>c.IsAccepted==true).Count());
        }

       
             


    }
}
