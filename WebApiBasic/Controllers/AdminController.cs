using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiBasic.Data;
using WebApiBasic.Dtos.Account;
using WebApiBasic.Dtos.Role;
using WebApplication1.Models;

namespace WebApiBasic.Controllers
{
    [ApiController]
    [Authorize(Policy = "RolePolicy")]
    [Route("Admin")]
    public class AdminController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly DBContext _context;

        public AdminController(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager, DBContext context)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }
        [Route("/ListUsers")]
        [HttpGet]
        public IActionResult ListUser()
        {
            var users = _userManager.Users.Where(a => a.IsActive == true).AsNoTracking();
            return Ok(users);
        }

        [Route("/EditUser")]
        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> EditUserAsync(string userId, [FromBody] UpdateDto model)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                user.Email = model.Email;
                user.UserName = model.UserName;
                var result = await _userManager.UpdateAsync(user);
                foreach (var err in result.Errors)
                {
                    return BadRequest(err.Description);
                }
            }
            return Ok();
        }
        [Route("/EditUser")]
        [HttpDelete]
        public async Task<IActionResult> DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            user.IsActive = false;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                foreach (var err in result.Errors)
                {
                    return BadRequest(err.Description);
                }
            }
            return Ok();
        }
        [Route("/ListRoles")]
        [HttpGet]
        public IActionResult ListRoles()
        {
            var roles = _roleManager.Roles.AsNoTracking();
            return Ok(roles);
        }
        [Route("/CreateRole")]
        [HttpPost]
        public async Task<IActionResult> CreateRoleAsync([FromBody] CreateRoleDto model)
        {
            var identityRole = new IdentityRole()
            {
                Name = model.RoleName
            };
            var result = await _roleManager.CreateAsync(identityRole);
            if (!result.Succeeded)
            {
                foreach (var err in result.Errors)
                {
                    return BadRequest(err.Description);
                }
            }
            return StatusCode(201);
        }
        [Route("/EditRole")]
        [HttpPost]
        public async Task<IActionResult> EditRoleAsync([FromBody] EditRoleDto model)
        {
            var role = await _roleManager.FindByIdAsync(model.RoleId);
            if (role is null)
            {
                return BadRequest("this role not exists");
            }
            role.Name = model.RoleName;
            var result = await _roleManager.UpdateAsync(role);
            if (!result.Succeeded)
            {
                foreach (var err in result.Errors)
                {
                    return BadRequest(err.Description);
                }
            }
            return Ok("Success Change");
        }
        [Route("/DeleteRole")]
        [HttpPost]
        public async Task<IActionResult> DeleteRole(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role is null)
            {
                return BadRequest("This Id Not Role");
            }
            var result = await _roleManager.DeleteAsync(role);
            if (!result.Succeeded)
            {
                foreach (var err in result.Errors)
                {
                    return BadRequest(err.Description);
                }
            }
            return Ok();
        }
        [Route("/AddUserToRole")]
        [HttpPost]
        public async Task<IActionResult> AddUserToRole(string userId, [FromBody]List<UserRolesDto> model)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                return BadRequest("This Id Not User");
            }
            foreach (var role in model)
            {
                var userInRole = await _userManager.IsInRoleAsync(user, role.RoleName);
                if (userInRole)
                {
                    return BadRequest($"this {role.RoleName} in user");
                }
                var resulr = await _userManager.AddToRoleAsync(user, role.RoleName);
            }
            return Ok();
        }
        
       [Route("/DeleteUserInRole")]
        [HttpPost]
        public async Task<IActionResult> DeleteUserInRole(string userId, [FromBody] List<UserRolesDto> model)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                return BadRequest("This Id Not User");
            }
            var roles = model.Select(a => a.RoleName);
            var result = await _userManager.RemoveFromRolesAsync(user, roles);
            return Ok();
        }
    }
}
