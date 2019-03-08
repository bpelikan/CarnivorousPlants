using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarnivorousPlants.Models.ProjectViewModel
{
    public class DetailsViewModel
    {
        public Project Project { get; set; }
        public string DomainName { get; set; }
        public IList<Tag> Tags { get; set; }
    }
}
