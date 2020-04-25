﻿using System;
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
    public class UsersControllerBK : Controller
    {
        public Models.MyDBContext db;
        public UsersControllerBK(Models.MyDBContext context)
        {
            db = context;
        }

        // GET: api/Users
        [HttpGet]
        public IEnumerable<UserViewModel> Get()
        {
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
                                            Status = d.Status,
                                            Rol = d.Rol.ToString()
                                        }).ToList();
            return list;
        }

        // POST: api/users/create
        [HttpPost("create")]
        public Response Create([FromBody] UserViewModel model)
        {
            Response response = new Response();

            try
            {
                Models.User newUser = new Models.User
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Username = model.Username,
                    Password = model.Password,
                    Rol = Int32.Parse(model.Rol)
                };
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

        // POST: api/users/login
        [HttpPost("login")]
        public Response Login([FromBody] UserViewModel model)
        {
            Response response = new Response();

            try
            {
                List<UserViewModelLogged> user = (from d in db.User
                                                  where d.Username == model.Username && d.Password == model.Password
                                                  orderby d.Id descending
                                                  select new UserViewModelLogged
                                                  {
                                                      FirstName = d.FirstName,
                                                      LastName = d.LastName,
                                                      Email = d.Email,
                                                      Username = d.Username,
                                                      Status = d.Status,
                                                      Rol = d.Rol
                                                  }).ToList();
                if (user == null)
                {
                    response.Success = 0;
                    response.Message = "User doesn't exists!";
                }
                else
                {
                    if (user.Count() == 0)
                    {
                        response.Success = 0;
                        response.Message = "Wrong credentials!";
                    }
                    else
                    {
                        response.Success = 1;
                        response.Message = "Access Granted!";
                        response.User = user;
                    }
                }
            }
            catch (Exception ex)
            {
                response.Success = 0;
                response.Message = ex.Message;
            }

            return response;
        }
        //private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] salt)
        //{
        //    using (var hmac = new System.Security.Cryptography.HMACSHA512(salt))
        //    {
        //        var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        //        for (int i = 0; i < computedHash.Length; i++)
        //        {
        //            if (computedHash[i] != passwordHash[i]) return false;
        //        }
        //    }
        //    return true;
        //}
    }

    public class Response2
    {
        public int Success { get; set; }
        public string Message { get; set; }
        public List<UserViewModelLogged> User { get; set; }
    }
}