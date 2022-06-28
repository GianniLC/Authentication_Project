using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Authentication.Models;

namespace Authentication.Data
{
    public class AuthenticationContext : DbContext
    {
        public AuthenticationContext (DbContextOptions<AuthenticationContext> options)
            : base(options)
        {

        }

        public DbSet<UserModel>? User { get; set; }
    }
}
