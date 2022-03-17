using Microsoft.AspNetCore.Mvc;
using RedmondBlog.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedmondBlog.Controllers
{
    [Route("ErrorPage/{statusCode}")]
    public class ErrorPageController : Controller
    {
        private readonly IImageService _imageService;

        public ErrorPageController(IImageService imageService)
        {
            _imageService = imageService;
        }

        public async Task<IActionResult> Index(int statusCode)
        {
            switch (statusCode)
            {
                case 404:
                    ViewData["Error"] = "Page Not Found";
                    break;
                default:
                    break;
            }

            var errorImage = _imageService.EncodeImageAsync($"404.jpg");
            var errorImageType = "jpg";

            ViewData["HeaderImage"] = _imageService.DecodeImage(await errorImage, errorImageType);

            return View("ErrorPage");
        }
    }
}
