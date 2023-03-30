using EmployeeRecordsAdministration.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeRecordsAdministration.Controllers
{
    [Route("api/setup")]
    [ApiController]
    public class SetupContoller : ControllerBase
    {
        private readonly IProjectInfoRepository _projectInfoRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<SetupContoller> _logger;
        public SetupContoller(
            IProjectInfoRepository projectInfoRepository,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<SetupContoller> logger)
        {
            _projectInfoRepository = projectInfoRepository;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetAllRoles()
        {
            var roles = _roleManager.Roles.ToList();
            return Ok(roles);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(string name)
        {
            var roleExists = await _roleManager.RoleExistsAsync(name);

            if (!roleExists)
            {
                var newRole = await _roleManager.CreateAsync(new IdentityRole(name));

                if (newRole.Succeeded)
                {
                    _logger.LogInformation($"The role {name} has been created successfully");
                    return Ok(new
                    {
                        result = $"The role {name} has been created successfully"
                    });
                }
                else
                {
                    _logger.LogInformation($"Couldn't create role {name}");
                    return BadRequest(new
                    {
                        error = $"Couldn't create role {name}"
                    });
                }
            }

            return BadRequest(new 
                { 
                    error = "Role already exists" 
                });
        }

        [HttpGet]
        [Route("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            return Ok(users);
        }

        [HttpPost]
        [Route("AddUserToRole")]
        public async Task<IActionResult> AddUserToRole(string email, string roleName)
        {
            //check if user exists
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return BadRequest(new 
                    { 
                        error = "User doesn't exist" 
                    });
            }

            //check if role exists
            var roleExists = await _roleManager.RoleExistsAsync(roleName);

            if (!roleExists)
            {
                return BadRequest(new
                {
                    error = "Role doesn't exist"
                });
            }

            var result = await _userManager.AddToRoleAsync(user, roleName);

            if (result.Succeeded)
            {
                return Ok(new
                {
                    result = "User was added to the role"
                });
            }
            else
            {
                return BadRequest(new
                {
                    error = "User wasn't added to the role"
                });
            }

        }

        [HttpGet]
        [Route("GetUserRoles")]
        public async Task<IActionResult> GetUserRoles(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return BadRequest(new
                {
                    error = "User doesn't exist"
                });
            }

            var roles = await _userManager.GetRolesAsync(user);
            return Ok(roles);
        }

        [HttpPost]
        [Route("RemoveUserFromRole")]
        public async Task<IActionResult> RemoveUserFromRole(string email, string roleName)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return BadRequest(new
                {
                    error = "User doesn't exist"
                });
            }

            var roleExists = await _roleManager.RoleExistsAsync(roleName);

            if (!roleExists)
            {
                return BadRequest(new
                {
                    error = "Role doesn't exist"
                });
            }

            var result = await _userManager.RemoveFromRoleAsync(user, roleName);

            if (result.Succeeded)
            {
                return Ok(new {
                    result = $"User {email} has been removed from role {roleName}"
                });
            }

            return BadRequest(new
            {
                result = $"Unable to remove user {email} from role {roleName}"
            });

        }
    }
}
