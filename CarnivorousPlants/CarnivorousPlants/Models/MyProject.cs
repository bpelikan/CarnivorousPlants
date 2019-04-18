using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarnivorousPlants.Models
{
    public class MyProject
    {
        public Guid MyProjectId { get; set; }
        //public string Name { get; set; }
        //public string Description { get; set; }
        //public DateTime Created { get; set; }
        //public DateTime LastModified { get; set; }
        //public string ThumbnailUri { get; set; }
        public string CreatedBy { get; set; }

        public virtual ICollection<MyTag> MyTags { get; set; }

    }
}
