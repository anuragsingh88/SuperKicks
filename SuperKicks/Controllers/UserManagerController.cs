using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuperKicks.Data.Models;
using SuperKicks.Repo;
using SuperKicks.Repo.Repository.Interface;
using SuperKicks.Repo.ViewModels;

namespace SuperKicks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserManagerController(IUserManagerRepository userManagerRepository) : ControllerBase
    {
        private readonly IUserManagerRepository _userManagerRepository = userManagerRepository;

        [HttpGet(@"getUsers")]
        [ProducesResponseType(typeof(List<User>),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetUsers()
        {
            return Ok(_userManagerRepository.GetUsers());
        }
        [AllowAnonymous]
        [HttpGet(@"validateUser/{validateBy}/{value}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult ValidateUser(string validateBy, string value)
        {
            var result = _userManagerRepository.ValidateCredential(validateBy, value);
            return result == "Success" ? Ok(result) : BadRequest(result);
        }

        [AllowAnonymous]
        [HttpPost(@"createUser")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreateUser([FromBody] UserViewModel viewModel)
        {
            string result = _userManagerRepository.CreateUser(viewModel);
            return result == StatusName.Success
                        ? Ok($"user with {viewModel.UserName} is created successfully.")
                        : BadRequest($"user with {viewModel.UserName} email is alreay exists!");
        }

        [AllowAnonymous]
        [HttpPost(@"login")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Login([FromBody] LoginViewModel viewModel)
        {
            string result = _userManagerRepository.Login(viewModel);
            return result.StartsWith(StatusName.Success)
                ? Ok($"Login successfull\n{result.Replace(StatusName.Success, "")}")
                : BadRequest(result);
        }

        [HttpPost(@"changePassword")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult ChangePassword([FromBody] LoginViewModel viewModel)
        {
            string result = _userManagerRepository.ChangePassword(viewModel);
            return result == StatusName.Success ? Ok("Password changed successfully.") : BadRequest("Please Enter correct password!");
        }
    }
}