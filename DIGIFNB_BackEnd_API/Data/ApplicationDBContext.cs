using DIGIFNB_BackEnd_API.Models.Grab.Feedback;
using DIGIFNB_BackEnd_API.Models.Grab.Order;
using Microsoft.EntityFrameworkCore;

namespace DIGIFNB_BackEnd_API.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext()
        {

        }
        

        public DbSet<Preparing> Preparings { get; set; }
        public DbSet<Ready> Readys { get; set; }
        public DbSet<Upcoming> Upcomings { get; set; }
        public DbSet<History> Historys { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }

        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) { }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    var builder = new ConfigurationBuilder()
        //        .SetBasePath(Directory.GetCurrentDirectory())
        //        .AddJsonFile("appsettings.json", true, true);
        //    IConfigurationRoot configuration = builder.Build();
        //    optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        //}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    } 
}
