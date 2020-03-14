using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InterApro.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

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
        public IEnumerable<UserViewModel> Get()
        {
            //List<Models.User> list = db.User.ToList();
            //return Json(list);
            List<UserViewModel> list = (from d in db.User
                                        orderby d.Id descending
                                        select new UserViewModel
                                        {
                                            Id = d.Id,
                                            FirstName = d.FirstName,
                                            LastName = d.LastName,
                                            Email = d.Email,
                                            Username = d.Username,
                                            Password = d.Password,
                                            Rol = d.Rol
                                        }).ToList();
            return list;
        }

        // POST: api/Users
        [HttpPost]
        public Response Post([FromBody] UserViewModel model)
        {
            Response response = new Response();

            try
            {
                Models.User newUser = new Models.User();
                newUser.FirstName = model.FirstName;
                newUser.LastName = model.LastName;
                newUser.Email = model.Email;
                newUser.Username = model.Username;
                newUser.Password = model.Password;
                newUser.Rol = model.Rol;
                db.User.Add(newUser);
                db.SaveChanges();
                response.Success = 1;
            }
            catch (Exception ex)
            {
                response.Success = 0;
                response.Message = ex.Message;
            }

            return response;
        }
    }

    public class Response
    {
        public int Success { get; set; }
        public string Message { get; set; }
    }
}