﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarnivorousPlants.Models.AdminViewModels
{
    public class UserDetailsWithRolesViewModel
    {
        public UserDetailsWithRolesViewModel()
        {
            Roles = new List<RoleInUserDetails>();
        }

        public UserDetailsViewModel User { get; set; }

        [Display(Name = "Roles")]
        public List<RoleInUserDetails> Roles { get; }

        public void AddRole(string name, bool isInRole)
        {
            Roles.Add(new RoleInUserDetails()
            {
                Name = name,
                IsInRole = isInRole
            });
        }

        public class RoleInUserDetails
        {
            public string Name { get; set; }
            public bool IsInRole { get; set; }
        }
    }
}
