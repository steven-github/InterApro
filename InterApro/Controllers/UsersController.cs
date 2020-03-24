using System;
using System.Collections.Generic;
using System.Linq;
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
            return await db.User.ToListAsync();
        }

        // GET: api/Users1/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await db.User.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // POST: api/Users
        [HttpPost("create")]
        public async Task<ActionResult<User>> PostCreate(User user)
        {
            db.User.Add(user);
            await db.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> Delete(int id)
        {
            var user = await db.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            db.User.Remove(user);
            await db.SaveChangesAsync();

            return user;
        }

        // POST: api/users/login
        [HttpPost("login")]
        public async Task<ActionResult<UserViewModelLogged>> PostLogin(User user)
        {
            var u = await db.User.Where(u => u.Username == user.Username && u.Password == user.Password).ToListAsync();

            if (u == null)
            {
                return NotFound(new { success = 0, message = "Not Found" });
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
                rol = Int32.Parse(u[0].Rol),
                status = Int32.Parse(u[0].Status),
                isLogged = true
            });;
        }
    }

    public class Response
    {
        public int Success { get; set; }
        public string Message { get; set; }
        public List<UserViewModelLogged> User { get; set; }
    }
}