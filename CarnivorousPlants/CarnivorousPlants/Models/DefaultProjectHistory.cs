using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarnivorousPlants.Models
{
    public class DefaultProjectHistory
    {
        public Guid DefaultProjectHistoryId { get; set; }
        public Guid MyProjectId { get; set; }
        //public virtual MyProject MyProject { get; set; }
        public string SettedBy { get; set; }
        public DateTime SettingTime { get; set; }

    }
}
