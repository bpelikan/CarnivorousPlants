using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarnivorousPlants.Models.ImageViewModel
{
    public class CreateViewModel
    {
        public Guid ProjectId { get; set; }

        [Required]
        public string TagId { get; set; }
        //public IList<Tag> Tags { get; set; }
        public SelectList TagsSelectList { get; set; }
    }
}
