using Microsoft.EntityFrameworkCore;

namespace Quiz_API.Entity
{
    public class AppDB : DbContext
    {
        public AppDB(DbContextOptions<AppDB> options) : base(options)
        {
        }
        public DbSet<EasyQuestion> EasyQuestions { get; set; }
        public DbSet<MidQuestion> MidQuestions { get; set; }
        public DbSet<HardQuestion> HardQuestions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Company> Companys { get; set; }
        public DbSet<CompanyUser> CompanyUsers { get; set; }
        public DbSet<PrivateUser> PrivateUsers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<EmailParams> EmailParams { get; set; }

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
                eb.HasMany(U => U.QuestionsList)
               .WithMany(Q => Q.Users);
            });
            modelBuilder.Entity<Role>().HasData(
                new Role() { Id = 1, Name = "User" },
                new Role() { Id = 2, Name = "Admin" });
        }

    }
}
