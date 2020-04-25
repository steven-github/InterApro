using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using InterApro.Database.Tables;

namespace InterApro.Database
{
    public class InterAproDBContext : DbContext
    {
        public InterAproDBContext(DbContextOptions<InterAproDBContext> options) : base(options)
        {

        }

        public DbSet<Request> Request { get; set; }
        public DbSet<RequestStatus> RequestStatus { get; set; }
        public DbSet<Log> Log { get; set; }

        /// <summary>
        /// Inicializacion de valores al momento de crear el modelo de base de datos
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RequestStatus>().HasData(new RequestStatus { RequestStatusId = 1, Description = "New Request" });
            modelBuilder.Entity<RequestStatus>().HasData(new RequestStatus { RequestStatusId = 2, Description = "Rejected by Manager" });
            modelBuilder.Entity<RequestStatus>().HasData(new RequestStatus { RequestStatusId = 3, Description = "Approved by Manager" });
            modelBuilder.Entity<RequestStatus>().HasData(new RequestStatus { RequestStatusId = 4, Description = "Rejected by Finance" });
            modelBuilder.Entity<RequestStatus>().HasData(new RequestStatus { RequestStatusId = 5, Description = "Approved by Finance" });
        }
    }
}
