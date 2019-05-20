using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarnivorousPlants.Models.ImageViewModel
{
    public class AcceptLearningImageViewModel
    {
        public Guid ImageWaitingToConfirmId { get; set; }

        public Guid MyProjectId { get; set; }
        public Guid MyTagId { get; set; }
        public string MyTagName { get; set; }

        public string ImageId { get; set; }
        public string ImageUrl { get; set; }

        public SelectList TagsSelectList { get; set; }
        public string ProvidedBy { get; set; }
        public DateTime SendTime { get; set; }
    }
}
