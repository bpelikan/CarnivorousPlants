using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarnivorousPlants.Models.ProjectViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training.Models;
using Microsoft.Extensions.Configuration;

namespace CarnivorousPlants.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class ProjectController : Controller
    {
        private readonly IConfiguration _configuration;

        private readonly string trainingKey;
        private readonly CustomVisionTrainingClient trainingApi;

        public ProjectController(IConfiguration configuration)
        {
            _configuration = configuration;

            trainingKey = configuration["trainingKey"];
            trainingApi = new CustomVisionTrainingClient()
            {
                ApiKey = trainingKey,
                Endpoint = configuration["WestEuropeEndpoint"]
            };
        }

        public IActionResult Index()
        {
            IList<Project> vm = trainingApi.GetProjects();

            return View(vm);
        }

        [Route("{projectId?}")]
        public IActionResult Details(Guid projectId)
        {
            Project project = trainingApi.GetProject(projectId);
            //trainingApi.GetTaggedImages();
            DetailsViewModel vm = new DetailsViewModel() {
                Project = project,
                DomainName = trainingApi.GetDomain(project.Settings.DomainId).Name,
                Tags = trainingApi.GetTags(project.Id),
                Images = trainingApi.GetTaggedImages(projectId),
            };

            return View(vm);
        }

        public IActionResult Create()
        {
            CreateViewModel vm = new CreateViewModel() {
                Domains = trainingApi.GetDomains()
            };

            return View(vm);
        }

        [HttpPost]
        public IActionResult Create(CreateViewModel createViewModel)
        {
            var project = trainingApi.CreateProject(
                            createViewModel.Name, 
                            createViewModel.Description, 
                            createViewModel.DomainId,
                            createViewModel.ClassificationType);
            
            TempData["Success"] = $"The project <b>{createViewModel.Name}</b> has been successfully created.";

            return RedirectToAction(nameof(ProjectController.Index));
        }

        [Route("{projectId?}")]
        public IActionResult Delete(Guid projectId)
        {
            var projectName = trainingApi.GetProject(projectId).Name;
            trainingApi.DeleteProject(projectId);

            TempData["Success"] = $"The project <b>{projectName}</b> has been successfully deleted.";

            return RedirectToAction(nameof(ProjectController.Index));
        }

        //public IActionResult Create(string name)
        //{
        //    var project = trainingApi.CreateProject(name);

        //    return RedirectToAction(nameof(ProjectController.Index));
        //}

    }
}