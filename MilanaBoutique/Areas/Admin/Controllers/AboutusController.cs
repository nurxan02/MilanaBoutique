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
using System.Linq;

namespace MilanaBoutique.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class AboutusController : Controller
    {
        private readonly AppDbContext context;
        private readonly IWebHostEnvironment env;

        public AboutusController(AppDbContext context, IWebHostEnvironment env)
        {
            this.context = context;
            this.env = env;
        }


        public IActionResult Index()
        {
            AboutUs aboutUs = context.AboutUs.FirstOrDefault();



            return View(aboutUs);
        }

        public IActionResult Edit(int id)
        {

            AboutUs aboutUs = context.AboutUs.FirstOrDefault(c=>c.Id==id);
            if (aboutUs==null)
            {
                return NotFound();
            }

            return View(aboutUs);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Edit(AboutUs aboutUs)
        {
            AboutUs exist = context.AboutUs.FirstOrDefault(c=>c.Id==aboutUs.Id);
            if (exist==null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return View(exist);
            }
            if (aboutUs.ImageFile != null)
            {
                if (!aboutUs.ImageFile.IsSizeOkay(2))
                {
                    ModelState.AddModelError("ImageFile", "Image size must be less than 2mb");
                    return View(exist);
                }
                if (!aboutUs.ImageFile.IsImage())
                {
                    ModelState.AddModelError("ImageFile", "Select an image file! ");
                    return View(exist);
                }
                Helpers.Helper.DeleteImg(env.WebRootPath, "assets/images", exist.Banner);
                exist.Banner = aboutUs.ImageFile.SaveImg(env.WebRootPath, "assets/images");
            }

            exist.Mission = aboutUs.Mission;
            exist.NewsLink = aboutUs.NewsLink;
            exist.Title = aboutUs.Title;
            exist.SubTitle = aboutUs.SubTitle;
            exist.Vision = aboutUs.Vision;

            context.SaveChanges();
            return RedirectToAction(nameof(Index));

        }

        public IActionResult Members(int page=1)
        {
            ViewBag.Currentpage = page;
            ViewBag.Totalpage = Math.Ceiling((decimal)context.Members.Count() / 6);
            List<Member> members = context.Members.Skip((page-1)*6).Take(6).ToList();
            return View(members);
        }

        public IActionResult CreateMember()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult CreateMember(Member member)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (member.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "Image cannot be null");
                return View();
            }

            if (!member.ImageFile.IsSizeOkay(2))
            {
                ModelState.AddModelError("ImageFile", "Image size cannot be bigger than 2MB");
                return View();

            }
            if (!member.ImageFile.IsImage())
            {
                ModelState.AddModelError("ImageFile", "Select only image files!");
                return View();

            }

            member.Image = member.ImageFile.SaveImg(env.WebRootPath, "assets/images/team");
            context.Members.Add(member);
            context.SaveChanges();
            return RedirectToAction(nameof(Members));
        }

        public IActionResult Editmember(int id)
        {
            Member member = context.Members.FirstOrDefault(c => c.Id == id);
            if (member==null)
            {
                return NotFound();
            }
            return View(member);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Editmember(Member member)
        {
            Member exist = context.Members.FirstOrDefault(c => c.Id == member.Id);
            if (exist==null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return View(exist);
            }

            if (member.ImageFile != null)
            {
                if (!member.ImageFile.IsSizeOkay(2))
                {
                    ModelState.AddModelError("ImageFile", "Image size cannot be bigger than 2MB");
                    return View(exist);

                }
                if (!member.ImageFile.IsImage())
                {
                    ModelState.AddModelError("ImageFile", "Select only image files!");
                    return View(exist);

                }

                Helpers.Helper.DeleteImg(env.WebRootPath, "assets/images/team", exist.Image);
                exist.Image = member.ImageFile.SaveImg(env.WebRootPath, "assets/images/team");
            }

            exist.Firstname = member.Firstname;
            exist.Lastname = member.Lastname;
            exist.Info = member.Info;
            exist.Speciality = member.Speciality;
            context.SaveChanges();
            return RedirectToAction(nameof(Members));
        }

        public IActionResult Deletemember(int id)
        {
            Member member = context.Members.FirstOrDefault(c => c.Id == id);
            if (member == null)
            {
                return NotFound();
            }

            context.Members.Remove(member);
            context.SaveChanges();
            return Json(new { status = 200 });
        }

        public IActionResult Socials(int id)
        {
           List<SocialMedia> socials  = context.SocialMedias.Include(c=>c.Member).Where(c => c.MemberId == id).ToList();
            return View(socials);

        }

        public IActionResult AddSocial(int id)
        {
            SocialCreateVM socialCreateVM = new SocialCreateVM
            {
                SocialMedia = new SocialMedia(),
                Member = context.Members.Include(c => c.SocialMedias).FirstOrDefault(c => c.Id == id)
            };
            if (socialCreateVM.Member == null)
            {
                return NotFound();
            }
            return View(socialCreateVM);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult AddSocial(SocialCreateVM socialMediaVm)
        {
            //return Json(socialMediaVm.Member.Id);
            //if (!ModelState.IsValid)
            //{
            //    return View();
            //}
            Member membercik = context.Members.FirstOrDefault(c => c.Id == socialMediaVm.Member.Id);


            socialMediaVm.SocialMedia.MemberId = membercik.Id;





            context.SocialMedias.Add(socialMediaVm.SocialMedia);
            context.SaveChanges();
            return RedirectToAction(nameof(Members));
        }

        public IActionResult EditSocial(int id)
        {
            SocialMedia social = context.SocialMedias.FirstOrDefault(c => c.Id == id);
            if (social==null)
            {
                return NotFound();
            }
            return View(social);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult EditSocial(SocialMedia socialMedia)
        {
            SocialMedia social = context.SocialMedias.Include(c=>c.Member).FirstOrDefault(c => c.Id == socialMedia.Id);
            if (social == null)
            {
                return NotFound();
            }
            social.Icon = socialMedia.Icon;
            social.Link = socialMedia.Link;
            context.SaveChanges();
            return RedirectToAction(nameof(Socials), new { id = social.MemberId });


        }

        public IActionResult Deletesocial(int id)
        {
            SocialMedia social = context.SocialMedias.FirstOrDefault(c => c.Id == id);
            if (social == null)
            {
                return NotFound();
            }

            context.SocialMedias.Remove(social);
            context.SaveChanges();
            return Json(new { status = 200 });

        }
    }
}
