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
        public DbSet<Role> roles { get; set; }
        public DbSet<EmailParams> emailParams { get; set; }

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
                new Role() {Id=1, Name = "User"}, 
                new Role() { Id = 2, Name = "Admin" });
        }
        
    }
}
