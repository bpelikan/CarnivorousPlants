using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarnivorousPlants.Shared
{
    public static class RoleCollection
    {
        public const string Administrator = "Administrator";
        public const string Moderator = "Moderator";
        public const string Supervisor = "Supervisor";
        public const string Provider = "Provider";

        public static HashSet<string> Roles = new HashSet<string>() {
                    Administrator,
                    Moderator,
                    Supervisor,
                    Provider
        };
    }
}
