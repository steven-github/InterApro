//Tutorial
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace InterApro.Models
{
    public class MyDBContext : DbContext
    {
        //Tutorial
        public MyDBContext(DbContextOptions<MyDBContext> options) : base(options)
        {

        }
        public DbSet<User> User { get; set; }
        public DbSet<Requests> Requests { get; set; }
        public DbSet<Logs> Logs { get; set; }
    }

    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int Status { get; set; } = 0;
        public int Rol { get; set; } = 0;
        public int BossId { get; set; } = -1;
    }

    public class Requests
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public int AssigneeId { get; set; }
        public string AssigneeName { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public int OrderStatus { get; set; } = 0; //-1 Rejected - 0 Pending - 1 Approved
        public string OrderStatusDescription { get; set; } = "Assigned to Boss"; //0 Pending - 1 Approved by Boss - 2 Rejected by Boss - 3 Approved by FA - 4 Rejected by FA
    }

    public class Logs
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int AssigneeId { get; set; }
        public string Status { get; set; }
        public DateTime createdAt { get; set; } = new DateTime();
        public DateTime modifiedAt { get; set; }
    }
}

