using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SalaFinder.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SalaFinder.Models;
using System.Diagnostics;

namespace SalaFinder.DAO
{

    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }


        public DbSet<Space> Spaces { get; set; }

        public DbSet<Reservation> Reservations { get; set; }

        public DbSet<AuditLog> AuditLogs { get; set; }

        public DbSet<NoShow> NoShows { get; set; }
    }
}