using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarnivorousPlants.Data;
using CarnivorousPlants.Models;
using CarnivorousPlants.Models.TagViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training;
using Microsoft.Extensions.Configuration;

namespace CarnivorousPlants.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class TagController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly string trainingKey;
        private readonly CustomVisionTrainingClient trainingApi;

        public TagController(IConfiguration configuration, ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;

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
            };

            return View(vm);
        }

        [HttpPost]
        [Route("{projectId?}")]
        public IActionResult Create(Guid projectId, CreateViewModel createViewModel)
        {
            var projectTag = trainingApi.CreateTag(
                createViewModel.ProjectId,
                createViewModel.Name,
                createViewModel.Description,
                createViewModel.TagType
            );

            MyTag myTag = new MyTag()
            {
                MyTagId = projectTag.Id,
                MyProjectId = createViewModel.ProjectId,
                CreatedBy = _userManager.GetUserId(HttpContext.User)
            };
            _context.MyTags.Add(myTag);
            _context.SaveChanges();

            TempData["Success"] = $"The project tag <b>{createViewModel.Name}</b> has been successfully created.";

            return RedirectToAction(nameof(ProjectController.Details), "Project", new { projectId = createViewModel.ProjectId });
        }

        [Route("{projectId?}/{tagId?}")]
        public IActionResult Delete(Guid projectId, Guid tagId)
        {
            var tagName = trainingApi.GetTag(projectId, tagId).Name;
            trainingApi.DeleteTag(projectId, tagId);

            var tag = _context.MyTags.FirstOrDefault(x => x.MyTagId == tagId);
            if (tag != null)
            {
                _context.Remove(tag);
                _context.SaveChanges();
            }

            TempData["Success"] = $"The tag <b>{tagName}</b> has been successfully deleted.";

            return RedirectToAction(nameof(ProjectController.Details), "Project", new { projectId });
        }
    }
}