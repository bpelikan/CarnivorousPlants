using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CarnivorousPlants.Models.PlantsViewModels;
using CarnivorousPlants.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction.Models;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace CarnivorousPlants.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class PlantsController : Controller
    {
        private readonly IPhotoStorageService _photoStorageService;
        private readonly IConfiguration _configuration;
        private readonly string trainingKey;
        private readonly string predictionKey;
        private readonly Guid projectID;
        private readonly CustomVisionPredictionClient endpoint;
        //private string predictionEndpoint2;

        public PlantsController(IPhotoStorageService photoStorageService, IConfiguration configuration)
        {
            _photoStorageService = photoStorageService;
            _configuration = configuration;

            trainingKey = configuration["trainingKey"];
            predictionKey = configuration["predictionKey"];
            projectID = Guid.Parse(configuration["projectID"]);

            endpoint = new CustomVisionPredictionClient()
            {
                ApiKey = predictionKey,
                Endpoint = configuration["WestEuropeEndpoint"]
            };
        }

        //public IActionResult Index()
        //{
        //    return View();
        //}

        public async Task<IActionResult> SendPhoto()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendPhoto(IFormFile photo)
        {
            if (!ModelState.IsValid)
                return RedirectToAction(nameof(PlantsController.SendPhoto));

            try
            {
                using (var stream = photo.OpenReadStream())
                {
                    var photoId = await _photoStorageService.SavePhotoAsync(stream, photo.FileName);
                    return RedirectToAction(nameof(PlantsController.Recognize), new { photoId });
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [Route("{photoId}")]
        public async Task<IActionResult> Recognize(string photoId)
        {
            var photoUrl = _photoStorageService.UriFor(photoId);
            ImageUrl imgUrl = new ImageUrl { Url = photoUrl };

            RecognizeViewModel recognizeViewModel = new RecognizeViewModel()
            {
                PhotoURL = photoUrl,
                ImagePrediction = await endpoint.PredictImageUrlAsync(projectID, imgUrl)
            };

            return View(recognizeViewModel);
        }

        //try
        //{
        //    WebRequest request = WebRequest.Create(predictionEndpoint2);
        //    request.Headers["Prediction-Key"] = predictionKey;
        //    request.Headers["Content-Type"] = "application/json";
        //    //request.Credentials = CredentialCache.DefaultCredentials;
        //    request.Method = "POST";
        //    string postData = $"{{\"Url\": \"{photoUrl}\"}}";
        //    byte[] byteArray = Encoding.UTF8.GetBytes(postData);
        //    request.ContentLength = byteArray.Length;
        //    Stream dataStream = request.GetRequestStream();
        //    dataStream.Write(byteArray, 0, byteArray.Length);
        //    dataStream.Close();

        //    WebResponse response = request.GetResponse();
        //    //var statusCode = ((HttpWebResponse)response).StatusDescription;
        //    dataStream = response.GetResponseStream();
        //    StreamReader reader = new StreamReader(dataStream);
        //    string responseFromServer = reader.ReadToEnd();

        //    recognizeViewModel.ImagePrediction = JsonConvert.DeserializeObject<ImagePrediction>(responseFromServer);
        //    recognizeViewModel.PhotoURL = photoUrl;

        //    reader.Close();
        //    response.Close();
        //}
        //catch (WebException ex)
        //{
        //    TempData["Error"] = ex.Message;
        //}


        //ImageUrl imgUrl = new ImageUrl { Url = photoUrl };

        //var result = endpoint.PredictImageUrl(projectID, imgUrl);
        //var result = await endpoint.PredictImageUrlAsync(projectID, imgUrl);
        //ImagePrediction
        //var test = result.Predictions;

        //try
        //{
        //    WebRequest request = WebRequest.Create(url);
        //    request.Credentials = CredentialCache.DefaultCredentials;
        //    WebResponse response = request.GetResponse();

        //    var statusCode = ((HttpWebResponse)response).StatusDescription;

        //    Stream dataStream = response.GetResponseStream();
        //    StreamReader reader = new StreamReader(dataStream);
        //    string responseFromServer = reader.ReadToEnd();
        //    //appointmentExist = bool.Parse(responseFromServer);

        //    reader.Close();
        //    response.Close();
        //}
        //catch (WebException ex)
        //{
        //    log.LogError($"Message:{ex.Message}" +
        //                    $"\nURL:{url}");
        //}

        ////[ImportModelState]
        //[Route("{jobPositionId?}")]
        //public async Task<IActionResult> Recognize(string jobPositionId, string returnUrl = null)
        //{
        //    ViewData["ReturnUrl"] = returnUrl;

        //    var userId = _userManager.GetUserId(HttpContext.User);
        //    try
        //    {
        //        var vm = await _myApplicationService.GetApplyApplicationViewModel(jobPositionId, userId);
        //        return View(vm);
        //    }
        //    catch (CustomRecruiterException ex)
        //    {
        //        TempData["Error"] = ex.Message;
        //    }

        //    return RedirectToLocalOrToMyApplications(returnUrl);
        //}

        //[HttpPost]
        ////[ExportModelState]
        //[Route("{jobPositionId?}")]
        //public async Task<IActionResult> Recognize(string jobPositionId, IFormFile cv, ApplyApplicationViewModel applyApplicationViewModel, string returnUrl = null)
        //{
        //    if (!ModelState.IsValid)
        //        return RedirectToAction(nameof(MyApplicationController.Apply), new { jobPositionId, returnUrl });

        //    var userId = _userManager.GetUserId(HttpContext.User);
        //    try
        //    {
        //        var application = await _myApplicationService.ApplyMyApplication(cv, applyApplicationViewModel, userId);
        //        TempData["Success"] = _stringLocalizer["Successfully sended."].ToString();
        //        return RedirectToAction(nameof(MyApplicationController.MyApplicationDetails), new { applicationId = application.Id });
        //    }
        //    catch (CustomRecruiterException ex)
        //    {
        //        TempData["Error"] = ex.Message;
        //    }

        //    return RedirectToAction(nameof(MyApplicationController.Apply), new { jobPositionId, returnUrl });
        //}
    }
}