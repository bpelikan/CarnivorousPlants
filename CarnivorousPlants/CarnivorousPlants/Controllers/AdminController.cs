﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CarnivorousPlants.Data;
using CarnivorousPlants.Models;
using CarnivorousPlants.Models.AdminViewModels;
using CarnivorousPlants.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CarnivorousPlants.Controllers
{
    [Authorize(Roles = RoleCollection.Administrator)]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public AdminController(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context,
            ILogger<AdminController> logger,
            IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return RedirectToAction(nameof(AdminController.UserManagement));
            //return View();
        }

        #region UserManagement
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

        public IActionResult AddUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(AddUserViewModel addUserViewModel)
        {
            if (!ModelState.IsValid)
                return View(addUserViewModel);

            var user = new ApplicationUser()
            {
                UserName = addUserViewModel.Email,
                Email = addUserViewModel.Email,
                EmailConfirmed = addUserViewModel.EmailConfirmed,
                PhoneNumber = addUserViewModel.PhoneNumber,
                CreatedAt = DateTime.UtcNow
            };

            IdentityResult result = await _userManager.CreateAsync(user, addUserViewModel.Password);

            if (result.Succeeded)
            {
                TempData["Success"] = "User added successfully.";
                _logger.LogInformation("User added successfully.");
                return RedirectToAction(nameof(AdminController.UserManagement));
            }

            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(addUserViewModel);
        }

        public async Task<IActionResult> EditUser(string id, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return RedirectToAction(nameof(AdminController.UserManagement));

            var vm = _mapper.Map<ApplicationUser, EditUserViewModel>(user);

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel editUserViewModel, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (!ModelState.IsValid)
                return View(editUserViewModel);
            
            var user = await _userManager.FindByIdAsync(editUserViewModel.Id);

            if (user != null)
            {
                user.UserName = editUserViewModel.Email.Normalize().ToUpper();
                user.Email = editUserViewModel.Email;
                user.PhoneNumber = editUserViewModel.PhoneNumber;

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    TempData["Success"] = "User changed successfully.";
                    _logger.LogInformation("User changed successfully.");
                    //return RedirectToAction(nameof(AdminController.UserDetails), "Admin", new { id = user.Id });
                    return RedirectToLocal(returnUrl);
                }

                ModelState.AddModelError("", "User not updated, something went wrong.");

                return View(editUserViewModel);
            }

            return RedirectToAction(nameof(AdminController.UserManagement));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user != null)
            {
                IdentityResult result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    TempData["Success"] = "User deleted successfully.";
                    _logger.LogInformation("User deleted successfully.");
                    return RedirectToAction(nameof(AdminController.UserManagement));
                }
                else
                {
                    ModelState.AddModelError("", "Something went wrong while deleting this user.");
                }
            }
            else
            {
                ModelState.AddModelError("", "This user can't be found.");
            }
            return View(nameof(AdminController.UserManagement), _userManager.Users);
        }
        #endregion

        #region Users in roles
        public async Task<IActionResult> AddUserToRole(string id, string roleName, string returnUrl = null)
        {
            var user = await _userManager.FindByIdAsync(id);
            var role = await _roleManager.FindByNameAsync(roleName);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            var result = await _userManager.AddToRoleAsync(user, role.Name);

            if (!result.Succeeded)
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return RedirectToLocal(returnUrl);
        }

        public async Task<IActionResult> DeleteUserFromRole(string id, string roleName, string returnUrl = null)
        {
            var user = await _userManager.FindByIdAsync(id);
            var role = await _roleManager.FindByNameAsync(roleName);

            if (user == null)
            {
                //throw new Exception($"User with id {id} not found. ({_userManager.GetUserId(HttpContext.User)})");
                return NotFound("User not found.");
            }

            var result = await _userManager.RemoveFromRoleAsync(user, role.Name);

            if (!result.Succeeded)
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return RedirectToLocal(returnUrl);
        }
        #endregion

        #region Helpers

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (returnUrl != null && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        #endregion
    }
}