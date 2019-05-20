using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarnivorousPlants.Data;
using CarnivorousPlants.Models;
using CarnivorousPlants.Models.ImageViewModel;
using CarnivorousPlants.Services;
using CarnivorousPlants.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training.Models;
using Microsoft.Extensions.Configuration;

namespace CarnivorousPlants.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class ImageController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IImageStorageService _imageStorageService;

        private readonly string trainingKey;
        private readonly CustomVisionTrainingClient trainingApi;

        public ImageController(
            ApplicationDbContext context, 
            UserManager<ApplicationUser> userManager, 
            IConfiguration configuration, 
            IImageStorageService imageStorageService)
        {
            _context = context;
            _userManager = userManager;
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

        #region Moderator
        [Authorize(Roles = RoleCollection.Moderator)]
        [Route("{projectId?}")]
        public IActionResult Create(Guid projectId)
        {
            CreateViewModel vm = new CreateViewModel() {
                ProjectId = projectId,
                TagsSelectList = new SelectList(trainingApi.GetTags(projectId), "Id", "Name"),
                //Tags = trainingApi.GetTags(projectId),
            };
            return View(vm);
        }

        [Authorize(Roles = RoleCollection.Moderator)]
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

        [Authorize(Roles = RoleCollection.Moderator)]
        [Route("{projectId?}/{imageId?}")]
        public IActionResult Delete(Guid projectId, Guid imageId)
        {
            trainingApi.DeleteImages(projectId, new List<string>() { imageId.ToString() });
            return RedirectToAction(nameof(ProjectController.Details), "Project", new { projectId });
        }

        [Authorize(Roles = RoleCollection.Moderator)]
        [Route("{projectId?}/{imageId?}")]
        public IActionResult ChangeImageTag(Guid projectId, Guid imageId)
        {
            ChangeImageTagViewModel vm = new ChangeImageTagViewModel() {
                ProjectId = projectId,
                Image = trainingApi.GetImagesByIds(projectId, new List<string>() { imageId.ToString() }).FirstOrDefault(),
                TagsSelectList = new SelectList(trainingApi.GetTags(projectId), "Id", "Name"),
            };

            return View(vm);
        }

        [Authorize(Roles = RoleCollection.Moderator)]
        [HttpPost]
        [Route("{projectId?}/{imageId?}")]
        public async Task<IActionResult> ChangeImageTag(Guid projectId, Guid imageId, ChangeImageTagViewModel changeImageTagViewModel)
        {
            if (!ModelState.IsValid)
            {
                if (changeImageTagViewModel.TagId == null)
                    TempData["Warning"] = "You must choose a tag for the photo.";
                return RedirectToAction(nameof(ImageController.ChangeImageTag), new { projectId, imageId });
            }

            Image image = trainingApi.GetImagesByIds(projectId, new List<string>() { imageId.ToString() }).FirstOrDefault();
            IList<string> tagsList = image.Tags?.Select(x => x.TagId.ToString()).ToList();

            if(tagsList != null)
                trainingApi.DeleteImageTags(projectId, new List<string>() { imageId.ToString() }, tagsList);

            ImageTagCreateEntry imageTagCreateEntry = new ImageTagCreateEntry(imageId, Guid.Parse(changeImageTagViewModel.TagId));
            IList<ImageTagCreateEntry> imageTagCreateEntries = new List<ImageTagCreateEntry>() { imageTagCreateEntry };
            ImageTagCreateBatch createBatch = new ImageTagCreateBatch(imageTagCreateEntries);

            trainingApi.CreateImageTags(projectId, createBatch);

            return RedirectToAction(nameof(ProjectController.Details), "Project", new { projectId });
        }
        #endregion

        #region Provider
        [Authorize(Roles = RoleCollection.Provider)]
        public IActionResult ProvideLearningImage()
        {
            var defaultproject = _context.DefaultProjectHistories.OrderBy(x => x.SettingTime).FirstOrDefault();
            if (defaultproject == null)
            {
                TempData["Error"] = "Default project doesn't exist.";
                return RedirectToAction(nameof(PlantsController.SendPhoto), "Plants");
            }

            return RedirectToAction(nameof(ImageController.ProvideImage), "Image", new { projectId = defaultproject.MyProjectId });
        }

        [Authorize(Roles = RoleCollection.Provider)]
        [Route("{projectId?}")]
        public IActionResult ProvideImage(Guid projectId)
        {
            Project project = null;
            try
            {
                project = trainingApi.GetProject(projectId);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Something went wrong.";
            }

            if (project == null)
            {
                TempData["Error"] += $"<br />Project with ID:<b>{projectId}</b> doesn't exists.";
                return RedirectToAction(nameof(PlantsController.SendPhoto), "Plants");
            }

            ProvideImageViewModel vm = new ProvideImageViewModel()
            {
                ProjectId = projectId,
                ProjectName = project.Name,
                TagsSelectList = new SelectList(trainingApi.GetTags(projectId), "Id", "Name"),
                //Tags = trainingApi.GetTags(projectId),
            };
            return View(vm);
        }

        [Authorize(Roles = RoleCollection.Provider)]
        [HttpPost]
        [Route("{projectId?}")]
        public async Task<IActionResult> ProvideImage(Guid projectId, IFormFile image, ProvideImageViewModel provideImageViewModel)
        {
            if (!ModelState.IsValid)
            {
                if (provideImageViewModel.TagId == null)
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

            ImageWaitingToConfirm imageWaitingToConfirm = new ImageWaitingToConfirm()
            {
                ImageWaitingToConfirmId = Guid.NewGuid(),
                ImageId = imageId,
                MyTagId = Guid.Parse(provideImageViewModel.TagId),
                ProvidedBy = _userManager.GetUserId(HttpContext.User),
                SendTime = DateTime.UtcNow
            };

            _context.ImagesWaitingToConfirm.Add(imageWaitingToConfirm);
            _context.SaveChanges();

            TempData["Success"] = "Image send succesfully.";

            return RedirectToAction(nameof(PlantsController.SendPhoto), "Plants");
        }
        #endregion

    }
}