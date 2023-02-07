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
    [Authorize(Roles ="SuperAdmin,Admin")]
    public class ContactController : Controller
    {
        private readonly AppDbContext context;
        private readonly IWebHostEnvironment env;

        public ContactController(AppDbContext context, IWebHostEnvironment env)
        {
            this.context = context;
            this.env = env;
        }
        public IActionResult Index()
        {
            Contact contact = context.Contact.FirstOrDefault();
            return View(contact);
        }

        public IActionResult Edit(int id)
        {
            Contact contact = context.Contact.FirstOrDefault(c=>c.Id == id);

            return View(contact);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Edit(Contact contact)
        {
            Contact exist = context.Contact.FirstOrDefault(c => c.Id == contact.Id);
            if (!ModelState.IsValid)
            {
                return View(contact);

            }

            if (contact.BannerFile!=null)
            {
                if (!contact.BannerFile.IsImage())
                {
                    ModelState.AddModelError("BannerFile", "Select only image file!");
                    return View(exist);
                }
                if (!contact.BannerFile.IsSizeOkay(2))
                {
                    ModelState.AddModelError("BannerFile", "Image size must be less than 2mb!");
                    return View(exist);
                }
                Helpers.Helper.DeleteImg(env.WebRootPath, "assets/images", exist.BannerImage);
                exist.BannerImage = contact.BannerFile.SaveImg(env.WebRootPath, "assets/images");
            }

            exist.BannerDesc = contact.BannerDesc;
            exist.BannerTitle = contact.BannerTitle;
            exist.CloseTime = contact.CloseTime;
            exist.ContactInformation = contact.ContactInformation;
            exist.LocationIcon = contact.LocationIcon;
            exist.OfficeLocation = contact.OfficeLocation;
            exist.OpenTime = contact.OpenTime;
            exist.Phone = contact.Phone;
            exist.PhoneIcon = contact.PhoneIcon;
            exist.Mail = contact.Mail;
            exist.MailIcon = contact.MailIcon;
            exist.OclockIcon = contact.OclockIcon;
            context.SaveChanges();
            return RedirectToAction(nameof(Index));


        }

        public IActionResult Stores()
        {
            List<Store> stores = context.Stores.ToList();

            return View(stores);
        }

        public IActionResult Createstore()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Createstore(Store store)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (store.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "Image cannot be null");
                return View();
            }

            if (!store.ImageFile.IsSizeOkay(2))
            {
                ModelState.AddModelError("ImageFile", "Image size cannot be bigger than 2MB");
                return View();

            }
            if (!store.ImageFile.IsImage())
            {
                ModelState.AddModelError("ImageFile", "Select only image files!");
                return View();

            }

          store.StoreImage =   store.ImageFile.SaveImg(env.WebRootPath, "assets/images/stores");

            context.Stores.Add(store);
            context.SaveChanges();
            return RedirectToAction(nameof(Index));

        }

        public IActionResult Editstore(int id)
        {
            Store store = context.Stores.FirstOrDefault(c => c.Id == id);
            if (store==null)
            {
                return RedirectToAction(nameof(Stores));
            }

            return View(store);


        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]

        public IActionResult Editstore(Store store)
        {
            if (store==null)
            {
                return RedirectToAction(nameof(Stores));
            }

            Store exist = context.Stores.FirstOrDefault(c => c.Id == store.Id); 
            if (!ModelState.IsValid)
            {
                return View(exist);
            }

            if (store.ImageFile!=null)
            {
                if (!store.ImageFile.IsImage())
                {
                    ModelState.AddModelError("ImageFile","Select image only");
                    return View(exist);
                }
                if (!store.ImageFile.IsSizeOkay(2))
                {
                    ModelState.AddModelError("ImageFile", "Image size must be than 2mb");
                    return View(exist);
                }
                Helpers.Helper.DeleteImg(env.WebRootPath, "assets/images/stores",exist.StoreImage); ;
                exist.StoreImage = store.ImageFile.SaveImg(env.WebRootPath, "assets/images/stores");

            }

            exist.Mail = store.Mail;
            exist.Name = store.Name;
            exist.Phone = store.Phone;
            exist.StoreCloseTime = store.StoreCloseTime;
            exist.StoreOpenTime = store.StoreOpenTime;
            exist.Address = store.Address;
            exist.StoreLink = store.StoreLink;

            context.SaveChanges();
            return RedirectToAction(nameof(Stores));





        }

        public IActionResult Deletestore(int id)
        {
            Store store = context.Stores.FirstOrDefault(c => c.Id == id);
            if (store == null)
            {
                return RedirectToAction(nameof(Stores));
            }

            context.Stores.Remove(store);
            context.SaveChanges();
            return Json(new { status = 200 });


        }
        public IActionResult Questions(int page = 1) {

            ViewBag.Currentpage = 1;
            ViewBag.totalpage = Math.Ceiling((decimal)context.Questions.Count() / 10);
            List<Questions> questions = context.Questions.OrderByDescending(c=>c.Id).Skip((page-1)*10).Take(10).ToList();
            return View(questions);
        }

        //[HttpPost]
        //[AutoValidateAntiforgeryToken]
        public IActionResult Questionstatus(int id)
        {
            Questions question = context.Questions.FirstOrDefault(c => c.Id == id);
          
            if (question.IsAcces == true)
            {
                question.IsAcces = false;
                context.SaveChanges();
                return Json(new { status = 200 });
            }
            
            else
            {
                question.IsAcces = true;
                context.SaveChanges();
                return Json(new { status = 200 });

            }
           
        }
        public IActionResult Deletequestion(int id)
        {
            Questions question = context.Questions.FirstOrDefault(c => c.Id == id);
            if (question==null)
            {
                return Json(new { status = 500 });
            }

            context.Questions.Remove(question);
            context.SaveChanges();
            return Json(new { status = 200 });
        }
    }
}
