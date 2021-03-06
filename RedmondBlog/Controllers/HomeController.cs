using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RedmondBlog.Data;
using RedmondBlog.Models;
using RedmondBlog.Services;
using RedmondBlog.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace RedmondBlog.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBlogEmailSender _emailSender;
        private readonly ApplicationDbContext _context;
        private readonly IImageService _imageService;

        public HomeController(ILogger<HomeController> logger,
                              IBlogEmailSender emailSender,
                              ApplicationDbContext context,
                              IImageService imageService)
        {
            _logger = logger;
            _emailSender = emailSender;
            _context = context;
            _imageService = imageService;
        }

        public async Task<IActionResult> Index(int? page)
        {
            var pageNumber = page ?? 1;
            var pageSize = 6;


            var blogs = _context.Blogs
                .Include(b => b.Author)
                .OrderByDescending(b => b.Created)
                .ToPagedListAsync(pageNumber, pageSize);

            var defaultBackgroundImage = _imageService.EncodeImageAsync($"defaultBackground.jpg");
            var defaultBackgroundType = "jpg";

            ViewData["MainText"] = "Still under construction. Please check back soon.";
            ViewData["SubText"] = "";
            ViewData["HeaderImage"] = _imageService.DecodeImage(await defaultBackgroundImage, defaultBackgroundType);
            return View(await blogs);
        }

        public async Task<IActionResult> About()
        {
            var aboutMeImage = _imageService.EncodeImageAsync($"aboutMe.jpg");
            var aboutMeType = "jpg";

            ViewData["MainText"] = "About Me";
            ViewData["SubText"] = "A little bit about me.";
            ViewData["HeaderImage"] = _imageService.DecodeImage(await aboutMeImage, aboutMeType);
            return View();
        }

        public async Task<IActionResult> Contact()
        {
            var contactMeImage = _imageService.EncodeImageAsync($"contactMe.jpg");
            var contactMeType = "jpg";

            ViewData["MainText"] = "Contact Me";
            ViewData["SubText"] = "Please feel free to reach out. I would love to connect.";
            ViewData["HeaderImage"] = _imageService.DecodeImage(await contactMeImage, contactMeType);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contact(ContactMe model)
        {
            //This is where we will be emailing..
            model.Message = $"{model.Message} <hr/> Phone: {model.Phone}";
            await _emailSender.SendContactEmailAsync(model.Email, model.Name, model.Subject, model.Message);
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
