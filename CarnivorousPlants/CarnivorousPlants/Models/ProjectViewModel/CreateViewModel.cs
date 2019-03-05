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
        public string Description { get; set; }
        public Guid DomainId { get; set; }
        public string ClassificationType { get; set; }
        public virtual ICollection<Domain> Domains { get; set; }
    }

    public static class ClassificationType
    {
        public const string Multiclass = "Multiclass";
        public const string Multilabel = "Multilabel";
        public static HashSet<string> Types = new HashSet<string>() {
                    Multiclass,
                    Multilabel,
        };
    }
}
