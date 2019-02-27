using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarnivorousPlants.Models.PlantsViewModels
{
    public class RecognizeViewModel
    {
        public string PhotoURL { get; set; }
        public ImagePrediction ImagePrediction { get; set; }
    }
}
