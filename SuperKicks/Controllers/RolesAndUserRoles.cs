using Microsoft.AspNetCore.Mvc;
using SuperKicks.Data.Models;
using SuperKicks.Repo;
using SuperKicks.Repo.Repository.Interface;
using SuperKicks.Repo.ViewModels;

namespace SuperKicks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesAndUserRoles(IRoleAndUserRoles roleAndUserRoles) : ControllerBase
    {
        private readonly IRoleAndUserRoles _roleAndUserRoles = roleAndUserRoles;
        [HttpGet(@"getRoles")]
        [ProducesResponseType(typeof(List<Role>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetRoles()
        {
            return Ok(_roleAndUserRoles.GetRoles());
        }
        [HttpPost(@"addUpdRole")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult AddUpdRole([FromBody] RoleViewModel viewModel)
        {
            string result = _roleAndUserRoles.AddUpdRole(viewModel);
            return result.StartsWith(StatusName.Failed) ? BadRequest(result.Replace(StatusName.Failed, "")) : Ok(result);
        }
        [HttpPost(@"activeInactiveRole/{flag:bool}/{id:guid}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult ActiveInactive(bool flag, Guid id)
        {
            string result = _roleAndUserRoles.ActiveInactiveRole(flag, id);
            if (result == StatusName.Success)
            {
                var activeInactive = flag ? "Role is activate" : "Role is inactivate";
                return Ok(activeInactive);
            }
            return BadRequest(result.Replace(StatusName.Failed, ""));
        }
        [HttpPost(@"assignUnAssignRole")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult AssignUnAssignRole([FromBody] UserRoleViewModel viewModel)
        {
            string result = _roleAndUserRoles.AssignUnAssignRolesToUser(viewModel);
            return result != StatusName.Success ? BadRequest(StatusName.SystemError) : Ok(result);
        }
    }
}
