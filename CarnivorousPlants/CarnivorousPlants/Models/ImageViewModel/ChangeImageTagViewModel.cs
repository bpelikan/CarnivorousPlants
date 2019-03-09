using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarnivorousPlants.Models.ImageViewModel
{
    public class ChangeImageTagViewModel
    {
        public Guid ProjectId { get; set; }
        public Image Image { get; set; }

        [Required]
        public string TagId { get; set; }
        public SelectList TagsSelectList { get; set; }
    }
}
