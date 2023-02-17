using Microsoft.EntityFrameworkCore;

namespace Quiz_API.Entity
{
    public class AppDB : DbContext
    {
        public AppDB(DbContextOptions<AppDB> options) : base(options)
        { 
        }
        public DbSet<EasyQuestion> easyQuestions;
        public DbSet<MidQuestion> midQuestions;
        public DbSet<HardQuestion> hardQuestions;
        public DbSet<Answer> answers;
        public DbSet<Company> companys;
        public DbSet<CompanyUser> companyUsers;
        public DbSet<PrivateUser> privateUsers;
        public DbSet<Category> categories;
        public DbSet<Question> questions;
        public DbSet<User> users;

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
                eb.HasMany(u => u.questionsList)
                .WithMany(q => q.users);
            });
            modelBuilder.Entity<EasyQuestion>(eb =>
            {
                eb.Property(u => u.Points).HasDefaultValue(1);
            });
            modelBuilder.Entity<MidQuestion>(eb =>
            {
                eb.Property(u => u.Points).HasDefaultValue(2);
            });
            modelBuilder.Entity<HardQuestion>(eb =>
            {
                eb.Property(u => u.Points).HasDefaultValue(3);
            });
        }
    }
}
