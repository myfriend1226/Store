using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Store2.Data;
using Store2.Models;
using Store2.Models.ManageViewModels;
using Store2.Models.SyncfusionViewModels;

namespace Store2.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/ChangePassword")]
    public class ChangePasswordController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ChangePasswordController(ApplicationDbContext context,
                        UserManager<ApplicationUser> userManager,
                        RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: api/ChangePassword
        [HttpGet]
        public IActionResult GetChangePassword()
        {
            List<ApplicationUser> Items = new List<ApplicationUser>();
            Items = _context.Users.ToList();
            int Count = Items.Count();
            return Ok(new { Items, Count });
        }
        
        

        [HttpPost("[action]")]
        public async Task<IActionResult> Update([FromBody]CrudViewModel<ChangePasswordViewModel> payload)
        {
            ChangePasswordViewModel changePasswordViewModel = payload.value;
            var user = _context.Users.SingleOrDefault(x => x.Id.Equals(changePasswordViewModel.Id));

            if (user != null &&
                changePasswordViewModel.NewPassword.Equals(changePasswordViewModel.ConfirmPassword))
            {
                await _userManager.ChangePasswordAsync(user, changePasswordViewModel.OldPassword, changePasswordViewModel.NewPassword);
            }

            return Ok();
        }
        
    }
}