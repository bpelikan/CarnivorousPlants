using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarnivorousPlants.Models.ImageViewModel
{
    public class ProvideImageViewModel
    {
        public Guid ProjectId { get; set; }
        public string ProjectName { get; set; }

        [Required]
        public string TagId { get; set; }
        //public IList<Tag> Tags { get; set; }
        public SelectList TagsSelectList { get; set; }
    }
}
