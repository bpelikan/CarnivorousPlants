using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarnivorousPlants.Models.ImageViewModel;
using CarnivorousPlants.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training.Models;
using Microsoft.Extensions.Configuration;

namespace CarnivorousPlants.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class ImageController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IImageStorageService _imageStorageService;

        private readonly string trainingKey;
        private readonly CustomVisionTrainingClient trainingApi;

        public ImageController(IConfiguration configuration, IImageStorageService imageStorageService)
        {
            _configuration = configuration;
            _imageStorageService = imageStorageService;

            trainingKey = configuration["trainingKey"];
            trainingApi = new CustomVisionTrainingClient()
            {
                ApiKey = trainingKey,
                Endpoint = configuration["WestEuropeEndpoint"]
            };
        }


        //public IActionResult Index()
        //{
        //    return View();
        //}

        [Route("{projectId?}")]
        public IActionResult Create(Guid projectId)
        {
            CreateViewModel vm = new CreateViewModel() {
                ProjectId = projectId,
                Tags = trainingApi.GetTags(projectId),
            };
            return View(vm);
        }

        [HttpPost]
        [Route("{projectId?}")]
        public async Task<IActionResult> Create(Guid projectId, IFormFile image, CreateViewModel createViewModel)
        {
            if (!ModelState.IsValid)
            {
                if(createViewModel.TagId == null)
                    TempData["Warning"] = "You must choose a tag for the photo.";

                return RedirectToAction(nameof(ImageController.Create), new { projectId });
            }

            string imageId = null;
            try
            {
                if (image == null)
                    throw new Exception("Choose an image to send.");
                using (var stream = image.OpenReadStream())
                {
                    imageId = await _imageStorageService.SaveImageAsync(stream, image.FileName);
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(ImageController.Create), new { projectId });
            }

            var imageUrl = _imageStorageService.UriFor(imageId);
            ImageUrlCreateEntry imageUrlCreateEntry = new ImageUrlCreateEntry(imageUrl, new List<Guid>() { Guid.Parse(createViewModel.TagId) });
            IList<ImageUrlCreateEntry> imageUrlCreateEntries = new List<ImageUrlCreateEntry>() {imageUrlCreateEntry};
            ImageUrlCreateBatch url = new ImageUrlCreateBatch(imageUrlCreateEntries);

            trainingApi.CreateImagesFromUrls(projectId, url);

            TempData["Success"] = $"The image has been successfully uploaded.";

            return RedirectToAction(nameof(ProjectController.Details), "Project", new { projectId });
        }
    }
}