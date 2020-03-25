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
                firstname = user.FirstName,
                lastname = user.LastName,
                email = user.Email,
                username = user.Username,
                rol = user.Rol,
                status = user.Status
            });
        }

        // POST: api/Users
        [HttpPost("create")]
        public async Task<ActionResult<User>> PostCreate(User user)
        {
            var hashedPassword = HashPassword(user.Password);
            user.Password = hashedPassword;
            db.User.Add(user);
            await db.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<UserViewModelLogged>> Delete(int id)
        {
            var user = await db.User.FindAsync(id);
            if (user == null)
            {
                return NotFound(new { success = 0, message = "User Not Found" });
            }

            db.User.Remove(user);
            await db.SaveChangesAsync();

            return Ok(new
            {
                success = 1,
                message = "User Successfully Deleted",
                firstname = user.FirstName,
                lastname = user.LastName,
                email = user.Email,
                username = user.Username,
                rol = user.Rol,
                status = user.Status,
                isLogged = true
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

            return Ok(new { 
                success = 1, 
                message = "Access Granted", 
                firstname = u[0].FirstName,
                lastname = u[0].LastName,
                email = u[0].Email,
                username = u[0].Username,
                rol = u[0].Rol,
                status = u[0].Status,
                isLogged = true
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

            //If password is empty then keep the current password
            if (string.IsNullOrEmpty(user.Password))
            {
                var u = await db.User.FindAsync(user.Id);
                user.Password = u.Password;
            }

            db.Entry(user).State = EntityState.Modified;

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

        private bool UserExists(int id)
        {
            return db.User.Any(e => e.Id == id);
        }

        public static string HashPassword(string password, string algorithm = "sha256")
        {
            return Hash(Encoding.UTF8.GetBytes(password), algorithm);
        }

        private static string Hash(byte[] input, string algorithm = "sha256")
        {
            using (var hashAlgorithm = HashAlgorithm.Create(algorithm))
            {
                return Convert.ToBase64String(hashAlgorithm.ComputeHash(input));
            }
        }
    }

    public class Response
    {
        public int Success { get; set; }
        public string Message { get; set; }
        public List<UserViewModelLogged> User { get; set; }
    }
}