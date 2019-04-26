using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CarnivorousPlants.Data;
using CarnivorousPlants.Models;
using CarnivorousPlants.Models.AdminViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CarnivorousPlants.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public AdminController(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context,
            IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return RedirectToAction(nameof(AdminController.UserManagement));
            //return View();
        }

        public IActionResult UserManagement()
        {
            var users = _userManager.Users.OrderBy(x => x.Email);
            return View(users);
        }

        public async Task<IActionResult> UserDetails(string id, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return RedirectToAction(nameof(AdminController.UserManagement));

            //var vm = _mapper.Map<ApplicationUser, UserDetailsViewModel>(user);
            var vm = new UserDetailsWithRolesViewModel()
            {
                User = _mapper.Map<ApplicationUser, UserDetailsViewModel>(user)
            };
            vm.User.CreatedAt = vm.User.CreatedAt.ToLocalTime();

            foreach (var role in _roleManager.Roles)
            {
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    vm.AddRole(role.Name, true);
                }
                else
                {
                    vm.AddRole(role.Name, false);
                }
            }
            return View(vm);
        }
    }
}