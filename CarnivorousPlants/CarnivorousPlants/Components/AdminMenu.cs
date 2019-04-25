using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarnivorousPlants.Components
{
    public class AdminMenu : ViewComponent
    {
        public AdminMenu()
        {
        }

        public IViewComponentResult Invoke()
        {
            var menuItems = new List<AdminMenuItem> {
                new AdminMenuItem(){
                    DisplayValue = "User management",
                    ActionValue = "UserManagement"
                },
                new AdminMenuItem(){
                    DisplayValue = "Role management",
                    ActionValue = "RoleManagement"
                }};

            return View(menuItems);
        }
    }

    public class AdminMenuItem
    {
        public string DisplayValue { get; set; }
        public string ActionValue { get; set; }
    }
}
