using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarnivorousPlants.Models.ProjectViewModel
{
    public class CreateViewModel
    {
        public string Name { get; set; }
        public virtual ICollection<Domain> Domains { get; set; }
    }
}
