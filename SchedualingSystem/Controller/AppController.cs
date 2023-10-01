using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SchedualingSystem.Models.DTO;
using SchedualingSystem.Models.DTO.Request;
using SchedualingSystem.Models.DTO.Response;
using SchedualingSystem.Service;
using System.Diagnostics.Eventing.Reader;
using System.Net;

namespace SchedualingSystem.AppController
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SchedualingSystemController : ControllerBase
    {
        private TaskService _taskService; 
        private UserService _userService ;
        public SchedualingSystemController(UserService userservice , TaskService taskservice) 
        {
            _userService = userservice ;
            _taskService = taskservice ;
        }
        [HttpPost("[action]")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<UserResponse>> CreateUser([FromBody] UserRequest user)
        {
            if (ModelState.IsValid) { 

                try
                {
                    var u =  await _userService.AddUser(user);
                    return CreatedAtRoute("GetUserById", new { id = u.Id ,createdTasks = new List<TaskResponse>() }, u ); 
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
                var users =  await _userService.GetAllUsers();
                if (users.Count == 0 )
                    return BadRequest("there is no users in data bases"); 
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500 ,ex.Message ); 
            }
        }
        [HttpGet("[action]/{id:guid}" , Name = "GetUserById") ]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserResponse>> GetUser(Guid id , string Name)
        {
            try
            {
                var createdTasks = await this._taskService.GetAllManagerTasks(id , Name ); 
                var user = await _userService.GetUserById(id , createdTasks );
                if (user == null )
                    return NotFound();
                if (!user.Name.Equals(Name) )
                    return BadRequest("id and name did not match."); 
                return Ok(user);
            }
            catch(Exception ex)
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
                if(!b)
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
        public async Task<ActionResult<UserResponse>> UpdateUser(Guid id , [FromBody] UserRequest user)
        {
            try
            {
                var user_out = await _userService.UpdateUser(user , id ); 
                return Ok(user_out);
            }
            catch( Exception ex )
            {
                return BadRequest(ex.Message); 
            }

        }

        [HttpGet("[action]/{searchBy}/{searchItem}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<UserGetResponse>>> SearchUsers(string searchItem , string searchBy)
        {
            try
            {
                var matchedUsers = _userService.SearchUserBy(searchBy , searchItem);
                if(matchedUsers == null) 
                    return NotFound();
                return Ok(matchedUsers); 
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("[action]/{managerId:guid}/{managerName}")]
        //[Route("[action]")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TaskResponse>> CreateTask([FromBody] TaskRequest task 
                                                    , string managerName , Guid managerId)
        {
            if (ModelState.IsValid && await this._userService.IsUserValid(managerId ) )
            {
                try
                {
                   var Outtask =  await this._taskService.AddTask(task, managerName ,managerId );
                    return CreatedAtRoute("GetTaskByid" , new {taskId = Outtask.Id ,
                                                               managerName = managerName} , Outtask ); 
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message); 
                }
            }
            else
                return BadRequest(ModelState);

        }

        [HttpGet("[action]/{managerName}/{taskId:guid}" , Name = "GetTaskByid") ]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<TaskResponse>> GetTaskByid(Guid taskId , string managerName)
        {
            try
            {
                var outtask = await this._taskService.GetTaskByid(taskId, managerName);
                if (outtask == null)
                    return NotFound(); 
                return Ok(outtask);
            }
            catch( Exception ex )
            {
                return BadRequest(ex.Message );
            }
        }

        [HttpDelete("[action]/{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserResponse>> DeleteTaskById(Guid id)
        {
            try
            {
                var result = await _taskService.DeleteTask(id);
                if(!result)
                    return NotFound();
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}");
            }
        }

        [HttpDelete("[action]/{managerId:guid}/{managerName}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserResponse>> DeleteAllManagerTasks(Guid managerId , string managerName)
        {
            try
            {
                var result = await _taskService.DeleteManagerTasks(managerName , managerId );
                if (!result)
                    return NotFound();
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}");
            }
        }

        [HttpPut("[action]/{managername}/{taskid:guid}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserResponse>> UpdateTask(Guid taskid, [FromBody] TaskRequest task , string managerName)
        {
            try
            {
                var user_out = await _taskService.UpdateTask(task , managerName , taskid );
                return Ok(user_out);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("[action]/{managerId:guid}/{managerName}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<TaskResponse>>> GetAllManagerTasks(Guid managerId , string managerName)

        {
            try
            {
                var tasks = await this._taskService.GetAllManagerTasks(managerId, managerName);
                if (tasks.Count == 0)
                    return BadRequest("you have not created any task "); 
                return Ok(tasks);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("[action]/{managerId:guid}/{managerName}/{searchBy}/{searchItem}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<UserGetResponse>>> SearchTask(string searchItem, string searchBy,
                                                                                  string managerName , Guid ManagerId)
        {
            try
            {
                var matchedTasks = await this._taskService.SearchTaskBy(searchBy, searchItem 
                                                            , ManagerId , managerName);
                if(matchedTasks == null ) 
                    return NotFound();
                return Ok(matchedTasks);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("[action]/{userId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> AssigneTask(Guid userId,Guid taskId )
        {
            try
            {
                var task = this._taskService.InternallGetTaskByid(taskId);
                var user = this._userService.InternallGetUserById(userId);
                await _userService.AssigneTaskToUsesr(task, userId);
               // _taskService.AssigneUsesrToTask( user, taskId );
                return Ok(); 
            }
            catch(Exception ex)
            {
                return NotFound(ex.Message); 
            }
        }

        [HttpPut("[action]")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TaskResponse>> chnagePercentage([FromBody] double percentage , Guid taskId,
                                                                                              string managerName)
        {
            try
            {
                var t = await this._taskService.ChangePercentage(percentage, taskId, managerName);
                return Ok(t);
            }
            catch( Exception ex )
            {
                return BadRequest(ex.Message);
            }
        }

    }
}


//https://www.niceonecode.com/blog/85/authentication-and-authorization-in-asp.net-6.0-api-with-jwt-using-identity-framework