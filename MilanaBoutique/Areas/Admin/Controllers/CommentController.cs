using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MilanaBoutique.DAL;
using MilanaBoutique.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MilanaBoutique.Areas.Admin
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class CommentController : Controller
    {
        private readonly AppDbContext _context;
        public CommentController(AppDbContext context)
        {
            _context = context;
        }

        

        public IActionResult Index(int productid ,int page = 1)
        {
            Product product = _context.Products.Include(c => c.Comments).FirstOrDefault(c => c.Id == productid);
            if (product==null)
            {
                return NotFound();
            }

          
            List<Comment> comments = _context.Comments.Include(c => c.AppUser).Include(c=>c.Product).Where(c=>c.ProductId == productid).OrderByDescending(c=>c.Id).Skip((page - 1) * 8).Take(8).ToList();
            ViewBag.CurrentPage = page;
            ViewBag.TotalPage = Math.Ceiling((decimal)_context.Comments.Include(c=>c.Product).Where(c=>c.ProductId==productid).Count() / 8);
            return View(comments);
        }

        public IActionResult CommentStatus(int id)
        {
            if (!_context.Comments.Any(c => c.Id == id))
            {
                return RedirectToAction("CourseComments", "Comment");
            }

            Comment comment = _context.Comments.Include(c=>c.Product).Include(c=>c.AppUser).FirstOrDefault(c => c.Id == id);
            comment.IsAccepted = comment.IsAccepted ? false : true;

        
            _context.SaveChanges();

            return RedirectToAction(nameof(Index),new {productid = comment.ProductId , page = 1 });



        }

        public IActionResult Blogcomments(int id, int page = 1)
        {
            if (_context.Blogs.FirstOrDefault(c => c.Id == id) == null)
            {
                return NotFound();
            }
            List<BlogComment> blogcomments = _context.BlogComments.Include(c => c.Blog).Include(c => c.AppUser).Where(c => c.BlogId == id).OrderByDescending(c=>c.Id).Skip((page-1)*8).Take(8).ToList();
            ViewBag.Currentpage = page;
            ViewBag.Totalpage = Math.Ceiling((decimal)_context.BlogComments.Where(c => c.BlogId == id).Count() / 8);
            return View(blogcomments);

        }

        public IActionResult Blogstatus(int id)
        {
            BlogComment blogComment = _context.BlogComments.Include(c=>c.AppUser).Include(c=>c.Blog).FirstOrDefault(c => c.Id == id);
            if (blogComment==null)
            {
                return NotFound();
            }

            blogComment.IsAccepted = blogComment.IsAccepted ? false : true;
            _context.SaveChanges();
            return RedirectToAction(nameof(Blogcomments), new { id = blogComment.Blog.Id, page = 1 });

        }

    }
}
