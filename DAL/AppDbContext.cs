using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Ticket15.Models;

namespace Ticket15.DAL
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Agent> Agents { get; set; }
        public DbSet<Member> Members { get; set; }
    }
}
