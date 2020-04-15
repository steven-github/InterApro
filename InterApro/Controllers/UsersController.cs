using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using InterApro.Models;
using InterApro.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InterApro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        public Models.MyDBContext db;
        public UsersController(Models.MyDBContext context)
        {
            db = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            //return await db.User.ToListAsync();

            var users = await db.User.ToListAsync();
            if (users == null)
            {
                return NotFound(new { success = 0, message = "Not Found" });
            }

            if (users.Count() == 0)
            {
                return Unauthorized(new { success = 0, message = "Unauthorized Account" });
            }

            return Ok(new
            {
                success = 1,
                message = "Users Fethed Successfully",
                users
            });
        }

        // GET: api/users/requests-by-id/5
        [HttpGet("requests-assigned-to-user/{id}")]
        public async Task<ActionResult<User>> GetRequestsAssignedToUser(int id)
        {
            var requests = await db.Requests.Where(u => u.AssigneeId == id).ToListAsync();

            if (requests == null)
            {
                return NotFound(new { success = 0, message = "Requests Not Found" });
            }

            // return request;
            return Ok(new
            {
                success = 1,
                message = "Requests Successfully Found",
                requests
            });
        }

        // GET: api/Users/5
        [HttpGet("request/{id}")]
        public async Task<ActionResult<User>> GetRequest(int id)
        {
            var request = await db.Requests.FindAsync(id);

            if (request == null)
            {
                return NotFound(new { success = 0, message = "Request Not Found" });
            }

            // return request;
            return Ok(new
            {
                success = 1,
                message = "Request Successfully Found",
                id = request.Id,
                userId = request.UserId,
                firstName = request.FirstName,
                lastName = request.LastName,
                email = request.Email,
                username = request.Username,
                assigneeId = request.AssigneeId,
                assigneeName = request.AssigneeName,
                price = request.Price,
                description = request.Description
            });
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await db.User.FindAsync(id);

            if (user == null)
            {
                return NotFound(new { success = 0, message = "User Not Found" });
            }

            // return user;
            return Ok(new
            {
                success = 1,
                message = "User Successfully Found",
                userId = user.Id,
                firstName = user.FirstName,
                lastName = user.LastName,
                email = user.Email,
                username = user.Username,
                rol = user.Rol,
                status = user.Status
            });
        }

        // GET: api/Users/assignee/5
        [HttpGet("assignee/{id}")]
        public async Task<ActionResult<UserViewModelLogged>> GetAssignee(int id)
        {
            var user = await db.User.Where(u => u.Id == id).ToListAsync();
            if (user == null)
            {
                return NotFound(new { success = 0, message = "Not Found" });
            }

            if (user.Count() == 0)
            {
                return NotFound(new { success = 0, message = "Not Assignee Found" });
            }

            return Ok(new
            {
                success = 1,
                message = "User Successfully Found",
                firstName = user[0].FirstName,
                lastName = user[0].LastName
            });
        }

        // GET: api/Users/Boss/5
        [HttpGet("boss/{id}")]
        public async Task<ActionResult<User>> GetBoss(int id)
        {
            var user = await db.User.FindAsync(id);

            if (user == null)
            {
                return NotFound(new { success = 0, message = "Boss Not Found" });
            }

            // return user;
            return Ok(new
            {
                success = 1,
                message = "Boss Successfully Found",
                userId = user.Id,
                firstName = user.FirstName,
                lastName = user.LastName,
                email = user.Email,
                username = user.Username,
                rol = user.Rol,
                status = user.Status
            });
        }

        // GET: api/users/bosses
        [HttpGet("bosses")]
        public async Task<ActionResult<IEnumerable<UserViewModelLogged>>> GetBosses()
        {
            var bosses = await db.User.Where(u => u.Rol == 1).ToListAsync();
            if (bosses == null)
            {
                return NotFound(new { success = 0, message = "Not Found" });
            }

            if (bosses.Count() == 0)
            {
                return NotFound(new { success = 0, message = "You need at least 1 boss profile" });
            }

            return Ok(new
            {
                success = 1,
                message = "Bosses Fethed Successfully",
                bosses
            });
        }

        // GET: api/users/requests
        [HttpGet("requests")]
        public async Task<ActionResult<IEnumerable<Requests>>> GetRequests()
        {

            var requests = await db.Requests.ToListAsync();
            if (requests == null)
            {
                return NotFound(new { success = 0, message = "Not Found" });
            }

            if (requests.Count() == 0)
            {
                return Ok(new { success = 1, message = "Not Requests Found", requests });
            }

            //var index = 0;
            foreach (var request in requests)
            {
                var assigneeId = await db.User.FindAsync(request.AssigneeId);
                //requests[index]. = assigneeId.FirstName + assigneeId.LastName;
            }

            return Ok(new
            {
                success = 1,
                message = "Requests Fethed Successfully",
                requests
            });
        }

        // POST: api/users/create-user
        [HttpPost("create-user")]
        public async Task<ActionResult<User>> PostCreateUser(User user)
        {
            var hashedPassword = HashPassword(user.Password);
            user.Password = hashedPassword;
            db.User.Add(user);
            await db.SaveChangesAsync();

            return CreatedAtAction("PostCreateUser", new { id = user.Id }, user);
        }

        // POST: api/users/create-request
        [HttpPost("create-request")]
        public async Task<ActionResult<Requests>> PostCreateRequest(Requests request)
        {
            db.Requests.Add(request);
            await db.SaveChangesAsync();

            return CreatedAtAction("PostCreateRequest", new { id = request.Id }, request);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<UserViewModelLogged>> DeleteUser(int id)
        {
            var user = await db.User.FindAsync(id);
            var requests = await db.Requests.Where(u => u.AssigneeId == id).ToListAsync();
            if (user == null)
            {
                return NotFound(new { success = 0, message = "User Not Found" });
            }

            if (requests.Count() > 0)
            {
                return Conflict(new { success = 0, message = "Users with active request can't be deleted!" });
            }

            db.User.Remove(user);
            await db.SaveChangesAsync();

            return Ok(new
            {
                success = 1,
                message = "User Successfully Deleted",
                firstName = user.FirstName,
                lastName = user.LastName,
                email = user.Email,
                username = user.Username,
                rol = user.Rol,
                status = user.Status,
                isLogged = true
            });
        }

        // DELETE: api/Users/delete-request/5
        [HttpDelete("delete-request-by-id/{id}")]
        public async Task<ActionResult<UserViewModelLogged>> DeleteRequestById(int id)
        {
            var request = await db.Requests.FindAsync(id);
            if (request == null)
            {
                return NotFound(new { success = 0, message = "Request Not Found" });
            }

            db.Requests.Remove(request);
            await db.SaveChangesAsync();

            return Ok(new
            {
                success = 1,
                message = "Request Successfully Deleted"
            });
        }

        // POST: api/users/login
        [HttpPost("login")]
        public async Task<ActionResult<UserViewModelLogged>> PostLogin(User user)
        {
            var hashedPassword = HashPassword(user.Password);
            var u = await db.User.Where(u => u.Username == user.Username && u.Password == hashedPassword).ToListAsync();

            if (u == null)
            {
                return NotFound(new { success = 0, message = "User Not Found" });
            }

            if (u.Count() == 0)
            {
                return Unauthorized(new { success = 0, message = "Unauthorized Account" });
            }

            return Ok(new
            {
                success = 1,
                message = "Access Granted",
                userId = u[0].Id,
                firstName = u[0].FirstName,
                lastName = u[0].LastName,
                email = u[0].Email,
                username = u[0].Username,
                bossId = u[0].BossId,
                rol = u[0].Rol,
                status = u[0].Status,
                isLogged = true,
                isAdmin = u[0].Rol == -1 ? 1 : 0
            });
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            var localU = await db.User.FindAsync(user.Id);
            localU.FirstName = user.FirstName;
            localU.LastName = user.LastName;
            localU.Email = user.Email;
            localU.Username = user.Username;
            localU.Status = user.Status;
            localU.Rol = user.Rol;

            //If password is empty then keep the current password
            if (!string.IsNullOrEmpty(user.Password))
            {
                localU.Password = HashPassword(user.Password);
            }

            db.Entry(localU).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(new { success = 1, message = "User Successfully Updated" });
        }

        // PUT: api/users/edit-requet/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("edit-request-by-creator/{id}")]
        public async Task<IActionResult> EditRequestByCreator(int id, Requests request)
        {
            if (id != request.Id)
            {
                return BadRequest();
            }

            var localR = await db.Requests.FindAsync(request.Id);
            localR.Price = request.Price;
            localR.Description = request.Description;

            db.Entry(localR).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RequestExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(new { success = 1, message = "Request Successfully Updated" });
        }

        // PUT: api/users/edit-requet/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("edit-request-by-internal/{id}")]
        public async Task<IActionResult> EditRequestByInternal(int id, Request request)
        {

            var localR = await db.Requests.FindAsync(id);
            var localU = await db.User.FindAsync(request.AssigneeId);

            localR.OrderStatus = -1;
            var status = "Rejected by";

            switch (localU.Rol)
            {
                case 2:
                    localR.OrderStatusDescription += " financial approver 1";
                    break;
                case 3:
                    localR.OrderStatusDescription += " financial approver 2";
                    break;
                case 4:
                    localR.OrderStatusDescription += " financial approver 3";
                    break;
                default:
                    localR.OrderStatusDescription += " boss";
                    break;
            }

            if (request.Approve)
            {
                if (localU.Rol > 1)
                {
                    localR.OrderStatus = 1;
                    status = "Approved by";
                }
                else
                {
                    localR.OrderStatus = 0;
                    status = "Assigned to";
                }
                var rol = 0;

                if (localR.Price > 0 && localR.Price < 100000)
                {
                    rol = 2;
                    localR.OrderStatusDescription += " financial approver 1";
                }
                else if (localR.Price > 100000 && localR.Price < 1000000)
                {
                    rol = 3;
                    localR.OrderStatusDescription += " financial approver 2";
                }
                else if (localR.Price > 1000000)
                {
                    rol = 4;
                    localR.OrderStatusDescription += " financial approver 3";
                }

                var localA = await db.User.Where(u => u.Rol == rol).ToListAsync();

                if (localA.Count() == 0)
                {
                    return NotFound(new { success = 0, message = "Admin needs to create a" + localR.OrderStatusDescription + " account!" });
                }

                localR.AssigneeId = localA[0].Id;
                localR.AssigneeName = localA[0].FirstName + " " + localA[0].LastName;
                localR.OrderStatusDescription = status + localR.OrderStatusDescription;
            }

            db.Entry(localR).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RequestExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(new { success = 1, message = "Request Successfully Updated", localR });
        }

        private bool UserExists(int id)
        {
            return db.User.Any(e => e.Id == id);
        }

        private bool RequestExists(int id)
        {
            return db.Requests.Any(e => e.Id == id);
        }

        public static string HashPassword(string password, string algorithm = "sha256")
        {
            return Hash(Encoding.UTF8.GetBytes(password), algorithm);
        }

        private static string Hash(byte[] input, string algorithm = "sha256")
        {
            using var hashAlgorithm = HashAlgorithm.Create(algorithm);
            return Convert.ToBase64String(hashAlgorithm.ComputeHash(input));
        }
    }

    public class Response
    {
        public int Success { get; set; }
        public string Message { get; set; }
        public List<UserViewModelLogged> User { get; set; }
    }

    public class Request
    {
        public Boolean Approve { get; set; }
        public int AssigneeId { get; set; }
    }
}