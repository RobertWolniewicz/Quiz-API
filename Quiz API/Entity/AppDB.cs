using Microsoft.EntityFrameworkCore;

namespace Quiz_API.Entity
{
    public class AppDB : DbContext
    {
        public AppDB(DbContextOptions<AppDB> options) : base(options)
        { 
        }
        public DbSet<EasyQuestion> easyQuestions { get; set; }
        public DbSet<MidQuestion> midQuestions { get; set; }
        public DbSet<HardQuestion> hardQuestions { get; set; }
        public DbSet<Answer> answers { get; set; }
        public DbSet<Company> companys { get; set; }
        public DbSet<CompanyUser> companyUsers { get; set; }
        public DbSet<PrivateUser> privateUsers { get; set; }
        public DbSet<Category> categories { get; set; }
        public DbSet<Question> questions { get; set; }
        public DbSet<User> users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CompanyUser>(eb =>
            {
                eb.HasOne(cu => cu.Company)
                .WithMany(c => c.Users)
                .HasForeignKey(cu => cu.CompanyId);
            });
            modelBuilder.Entity<Question>(eb =>
            {
                eb.HasMany(q => q.Answers)
                .WithOne(a => a.Question)
                .HasForeignKey(a => a.QuestionId);
            });
            modelBuilder.Entity<Category>(eb =>
            {
                eb.HasMany(c => c.Questions)
                .WithMany(q => q.Categorys);
            });
            modelBuilder.Entity<User>(eb =>
            {
                eb.HasMany(u => u.QuestionsList)
                .WithMany(q => q.Users);
            });
            modelBuilder.Entity<EasyQuestion>(eb =>
            {
                eb.Property(u => u.Points).HasDefaultValue(1);
                eb.ToTable("easyQuestions");
            });
            modelBuilder.Entity<MidQuestion>(eb =>
            {
                eb.Property(u => u.Points).HasDefaultValue(2);
                eb.ToTable("midQuestions");
            });
            modelBuilder.Entity<HardQuestion>(eb =>
            {
                eb.Property(u => u.Points).HasDefaultValue(3);
                eb.ToTable("hardQuestions");
            });
        }
        
    }
}
