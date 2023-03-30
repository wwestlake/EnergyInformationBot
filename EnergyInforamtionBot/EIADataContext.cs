using Microsoft.EntityFrameworkCore;

namespace EnergyInforamtionBot
{
    /// <summary>
    /// Entity Framework Data Context
    /// </summary>
    public class EIADataContext : DbContext
    {
        /// <summary>
        /// Createds a EIADataContext
        /// </summary>
        /// <param name="options">The DBContectOptions</param>
        public EIADataContext(DbContextOptions<EIADataContext> options)  : base(options)
        {
        }

        /// <summary>
        /// Called when updatying the database schema
        /// </summary>
        /// <param name="optionsBuilder">Opions for building database</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Trust Server Certificate=true;Integrated Security=True;Persist Security Info=False;Initial Catalog=eiadatabase;Data Source=BILLS_GAMING_MA\SQLEXPRESS");
        }

        /// <summary>
        /// The series data from EIA
        /// </summary>
        public DbSet<EIASeriesItem> SeriesItems { get; set; }
    }
}
