using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using hrnk.Models;

namespace hrnk.Data
{
    public class hrnkContext : DbContext
    {
        public hrnkContext(DbContextOptions<hrnkContext> options) : base(options)
        {
        }

        public DbSet<User> User { get; set; }

        public DbSet<hrnk.Models.Role> Role { get; set; }
    }
}
