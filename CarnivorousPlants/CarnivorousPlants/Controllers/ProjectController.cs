using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarnivorousPlants.Models.ProjectViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training.Models;
using Microsoft.Extensions.Configuration;

namespace CarnivorousPlants.Controllers
{
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
            //TempData["Success"] = "Test";
            IList<Project> vm = trainingApi.GetProjects();
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
        public async Task<IActionResult> Create(CreateViewModel createViewModel)
        {
            var project = trainingApi.CreateProject(createViewModel.Name);

            //project.Settings.ClassificationType = "Multiclass";
            var test = project.Settings;
            var test2 = project.Settings.DomainId;
            TempData["Success"] = $"The project <b>{createViewModel.Name}</b> has been successfully created.";
            return RedirectToAction(nameof(ProjectController.Index));
        }

        //public IActionResult Create(string name)
        //{
        //    var project = trainingApi.CreateProject(name);

        //    return RedirectToAction(nameof(ProjectController.Index));
        //}

    }
}