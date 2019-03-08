using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarnivorousPlants.Models.TagViewModel
{
    public class CreateViewModel
    {
        public Guid ProjectId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string TagType { get; set; }
    }

    public static class TagTypes
    {
        public const string Regular = TagType.Regular;
        public const string Negative = TagType.Negative;
        public static HashSet<string> Types = new HashSet<string>() {
                    Regular,
                    Negative,
        };
    }
}
