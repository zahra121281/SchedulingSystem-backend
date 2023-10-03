using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SchedualingSystem.Models.DTO;
using SchedualingSystem.Models.DTO.Request;
using SchedualingSystem.Models.DTO.Response;
using SchedualingSystem.Service;
using System.Diagnostics.Eventing.Reader;
using System.Net;

namespace SchedualingSystem.Controller
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private TaskService _taskService;
        private UserService _userService;
        public UserController(UserService userservice, TaskService taskservice)
        {
            _userService = userservice;
            _taskService = taskservice;
        }

        [HttpPost("[action]")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<UserResponse>> CreateUser([FromBody] UserRequest user)
        {
            if (ModelState.IsValid)
            {

                try
                {
                    var u = await _userService.AddUser(user);
                    return CreatedAtRoute("GetUserById", new { id = u.Id, createdTasks = new List<TaskResponse>() }, u);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            else
                return BadRequest(ModelState);

        }

        [HttpGet("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<UserGetResponse>>> GetUsers()
        {
            try
            {
                //_logger.Log("get all villas ", "");
                var users = await _userService.GetAllUsers();
                if (users.Count == 0)
                    return BadRequest("there is no users in data bases");
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("[action]/{id:guid}", Name = "GetUserById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserResponse>> GetUser(Guid id, string Name)
        {
            try
            {
                var createdTasks = await this._taskService.GetAllManagerTasks(id, Name);
                var user = await _userService.GetUserById(id, createdTasks);
                if (user == null)
                    return NotFound();
                if (!user.Name.Equals(Name))
                    return BadRequest("id and name did not match.");
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}");
            }
        }

        [HttpDelete("[action]/{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserResponse>> DeleteUserById(Guid id)
        {
            try
            {
                var b = await _userService.DeleteUser(id);
                if (!b)
                    return NotFound();
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}");
            }
        }

        [HttpPut("[action]/{id:guid}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserResponse>> UpdateUser(Guid id, [FromBody] UserRequest user)
        {
            try
            {
                var user_out = await _userService.UpdateUser(user, id);
                return Ok(user_out);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("[action]/{searchBy}/{searchItem}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<UserGetResponse>>> SearchUsers(string searchItem, string searchBy)
        {
            try
            {
                var matchedUsers = _userService.SearchUserBy(searchBy, searchItem);
                if (matchedUsers == null)
                    return NotFound();
                return Ok(matchedUsers);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
