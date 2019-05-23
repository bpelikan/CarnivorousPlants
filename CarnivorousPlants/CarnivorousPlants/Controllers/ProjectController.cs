using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarnivorousPlants.Data;
using CarnivorousPlants.Models;
using CarnivorousPlants.Models.ProjectViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Rest;

namespace CarnivorousPlants.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class ProjectController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly string trainingKey;
        private readonly CustomVisionTrainingClient trainingApi;

        public ProjectController(IConfiguration configuration, ApplicationDbContext context, UserManager<ApplicationUser> userManager)
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
                Iterations = trainingApi.GetIterations(projectId),
                ImagesTagged = trainingApi.GetTaggedImages(projectId),
                ImagesUntagged = trainingApi.GetUntaggedImages(projectId),
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
                            ClassificationType.Multiclass);
                            //createViewModel.ClassificationType);

            MyProject myProject = new MyProject()
            {
                MyProjectId = project.Id,
                CreatedBy = _userManager.GetUserId(HttpContext.User)
            };
            _context.MyProjects.Add(myProject);
            _context.SaveChanges();

            TempData["Success"] = $"The project <b>{createViewModel.Name}</b> has been successfully created.";

            return RedirectToAction(nameof(ProjectController.Index));
        }

        [Route("{projectId?}")]
        public IActionResult Delete(Guid projectId)
        {
            var projectName = trainingApi.GetProject(projectId).Name;
            trainingApi.DeleteProject(projectId);

            var project = _context.MyProjects.FirstOrDefault(x => x.MyProjectId == projectId);
            if (project != null)
            {
                _context.Remove(project);
                _context.SaveChanges();
            }

            TempData["Success"] = $"The project <b>{projectName}</b> has been successfully deleted.";

            return RedirectToAction(nameof(ProjectController.Index));
        }

        [Route("{projectId?}")]
        public IActionResult Train(Guid projectId)
        {
            var projectName = trainingApi.GetProject(projectId).Name;


            try
            {
                trainingApi.TrainProject(projectId);
                TempData["Success"] = $"The project <b>{projectName}</b> has been successfully trained.";
            }
            catch (HttpOperationException ex)
            {
                TempData["Error"] = ex.Response.Content;
            }

            //Iteration iter = trainingApi.TrainProject(projectId);
            //Guid iterationId = iter.Id;

            ////IList<Iteration> iterations = trainingApi.GetIterations(projectId);
            ////foreach (Iteration it in iterations)
            ////{
            ////    it.IsDefault = false;
            ////    trainingApi.UpdateIteration(projectId, it.Id, it);
            ////}

            //Iteration iteration = trainingApi.GetIteration(projectId, iterationId);
            //iteration.IsDefault = true;
            //trainingApi.UpdateIteration(projectId, iteration.Id, iteration);

            return RedirectToAction(nameof(ProjectController.Details), new { projectId });
        }

        [Route("{projectId?}/{iterationId?}")]
        public IActionResult SetDefaultIteration(Guid projectId, Guid iterationId)
        {
            var projectName = trainingApi.GetProject(projectId).Name;

            Iteration iteration = trainingApi.GetIteration(projectId, iterationId);
            iteration.IsDefault = true;
            trainingApi.UpdateIteration(projectId, iteration.Id, iteration);

            TempData["Success"] = $"The iteration <b>{iteration.Name}</b> has been successfully set as default.";

            return RedirectToAction(nameof(ProjectController.Details), new { projectId });
        }

        [Route("{projectId}")]
        public IActionResult SetDefaultProject(Guid projectId)
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
                return RedirectToAction(nameof(ProjectController.Index));
            }
            if (_context.DefaultProjectHistories.OrderByDescending(x => x.SettingTime).FirstOrDefault()?.MyProjectId == projectId)
            {
                TempData["Warning"] = $"Project with ID:<b>{projectId}</b> has already set as default.";
                return RedirectToAction(nameof(ProjectController.Details), new { projectId });
            }

            DefaultProjectHistory defaultProjectHistory = new DefaultProjectHistory()
            {
                DefaultProjectHistoryId = Guid.NewGuid(),
                MyProjectId = projectId,
                SettedBy = _userManager.GetUserId(HttpContext.User),
                SettingTime = DateTime.UtcNow
            };

            _context.DefaultProjectHistories.Add(defaultProjectHistory);
            _context.SaveChanges();

            TempData["Success"] = $"Project was successfully set as default.";

            return RedirectToAction(nameof(ProjectController.Details), new { projectId });
        }

        //public IActionResult Create(string name)
        //{
        //    var project = trainingApi.CreateProject(name);

        //    return RedirectToAction(nameof(ProjectController.Index));
        //}

    }
}