using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarnivorousPlants.Models
{
    public class ImageWaitingToConfirm
    {
        public Guid ImageWaitingToConfirmId { get; set; }

        public Guid MyProjectId { get; set; }
        public Guid MyTagId { get; set; }

        public string ImageId { get; set; }
        public string ProvidedBy { get; set; }
        public DateTime SendTime { get; set; }
    }
}
