using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarnivorousPlants.Models
{
    public class MyTag
    {
        public Guid MyTagId { get; set; }

        public Guid MyProjectId { get; set; }
        public virtual MyProject MyProject { get; set; }

        public string CreatedBy { get; set; }

    }
}
