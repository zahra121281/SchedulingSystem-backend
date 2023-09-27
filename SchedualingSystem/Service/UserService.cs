using SchedualingSystem.Models;
using SchedualingSystem.Models.DTO;
using SchedualingSystem.Models.DTO.Request;
using SchedualingSystem.Models.DTO.Response;
using Microsoft.EntityFrameworkCore;

namespace SchedualingSystem.Service
{
    public class UserService
    {
        private readonly AppDbContext _db ;
        // add users
        public UserService(AppDbContext dbcontext )
        {
            
            _db = dbcontext;
        }
        public async System.Threading.Tasks.Task<UserResponse> AddUser(UserRequest userRequest)
        {
            if ( userRequest == null ) 
            {
                throw new ArgumentNullException("user could not be null"); 
            }
            ValiadationModel.ValidateModel(userRequest);

            var user = userRequest.ToUser();
            user.Id = Guid.NewGuid(); 
            this._db.Users.Add(user);
            await _db.SaveChangesAsync();
            //await Users.Add( user );
            return user.ToUserResponse(new List<TaskResponse>() , new List<TaskResponse>()); 
        }
        public User InternallGetUserById(Guid id)
        {

            if (id != null)
            {
                User? user = this._db.Users.AsNoTracking().SingleOrDefault(x => x.Id == id);
                if (user == null)
                {
                    throw new InvalidOperationException("there is not such a user!");
                }
                return user;
            }
            else
                throw new InvalidOperationException("id could not be null or empty");
        }

            public async System.Threading.Tasks.Task<UserResponse> GetUserById(Guid id, List<TaskResponse> createdTasks)
            {
            if(id != null ) 
            {
                User? user = await _db.Users.Include(u => u.Tasks ).SingleAsync(u => u.Id == id ); 
                _db.Users.Attach(user);
                var tt = user.Tasks; 
                var tasks = user.Tasks.Select(t => t.ToTasksResponse(user.Name)).ToList(); 
                if(user == null )
                {
                    throw new InvalidOperationException("there is not such a user!"); 
                }
               
                return user.ToUserResponse(tasks , createdTasks); 
            }
            else
                throw new InvalidOperationException("id could not be null or empty");
        }
        //async System.Threading.Tasks.Task<bool> 
        public async Task<bool> DeleteUser(Guid userID)
        {
            if(userID == null )
            {
                throw new InvalidOperationException(nameof(userID));
            }
            var user = InternallGetUserById(userID);
            if (user != null)
            {
                this._db.Remove(user);
                await _db.SaveChangesAsync(); 
                return true; 
            }
            else
                return false; 
        }
        public async System.Threading.Tasks.Task<UserGetResponse> UpdateUser(UserRequest userRequest  , Guid id )
        {
            if (userRequest == null)
                throw new ArgumentNullException("requested user could not be null");

            ValiadationModel.ValidateModel(userRequest);

            //  get matching person object 
            var matchingUser = InternallGetUserById(id);
            if ( matchingUser == null ) 
            {
                throw new InvalidOperationException("there is not such a user!");
            }
            // update all details 
            this._db.Users.Attach(matchingUser);
            matchingUser.Name = userRequest.Name;
            matchingUser.Email = userRequest.Email;
            matchingUser.DateofBirth = userRequest.DateofBirth;
            matchingUser.Gender = userRequest.Gender.ToString();
            this._db.Users.Update(matchingUser);
            await this._db.SaveChangesAsync() ;
            return matchingUser.ToUserGetResponse(); 
        }
        public async System.Threading.Tasks.Task<List<UserGetResponse>> GetAllUsers() =>
            await this._db.Users.Select(u => u.ToUserGetResponse()).ToListAsync(); 

        public async System.Threading.Tasks.Task<List<UserGetResponse>> SearchUserBy(string Searchby , string SearchItem)
        {
             List<UserGetResponse> matchingUsers = null; 
            switch(Searchby)
            {
                case nameof(UserRequest.Name):
                    matchingUsers = await this._db.Users.Where(u => u.Name.Contains(SearchItem)).
                            Select(t => t.ToUserGetResponse()).ToListAsync(); 
                    break; 

                case nameof(UserRequest.Email):
                    matchingUsers =  await this._db.Users.Where(u => u.Email.Contains(SearchItem))
                            .Select( t=> t.ToUserGetResponse()).ToListAsync();
                    break;

                default:
                    throw new InvalidOperationException("there is no such a user");
            }
            return matchingUsers; 
        }

        public async Task<bool> IsUserValid(Guid UserId )=>
        
         await this._db.Users.AnyAsync(t => t.Id == UserId ) ; 
        

        public async System.Threading.Tasks.Task AssigneTaskToUsesr(SchedualingSystem.Models.Task task ,Guid userid )
        {
            var user = this._db.Users.Where(t => t.Id == userid).First();
            if (user != null)
            {
                this._db.Users.Attach(user);
                this._db.Tasks.Attach(task);
                user.Tasks.Add(task);
                await this._db.SaveChangesAsync();
            }
            else
                throw new InvalidOperationException("there is not such a user ");

        }

    }
}
