using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using HardstyleFamily.Models;
using System.Linq;
using System.Threading.Tasks;


namespace HardstyleFamily.Data
{
    public class ApplicationDbContext : IdentityDbContext<Users>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<HardstyleFamily.Models.Music> Music { get; set; }
        public DbSet<HardstyleFamily.Models.Events> Events { get; set; }
        //public DbSet<HardstyleFamily.Models.Users> Users { get; set; }

    }
}
