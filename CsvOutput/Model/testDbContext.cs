namespace CsvOutput.Model
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class testDbContext : DbContext
    {
        public testDbContext()
            : base("name=testDbContext")
        {
        }

        public virtual DbSet<m_persons> m_persons { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
