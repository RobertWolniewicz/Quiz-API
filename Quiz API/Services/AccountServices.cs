using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Quiz_API.Entity;
using Quiz_API.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Quiz_API.Services
{
    public interface IAccountServices
    {
        void RegisterUser<T>(RegisterUserDto dto) where T : User, new();
        string GenereteJwt(LoginDto dto, WebApplication builder);
    }

    public class AccountServices : IAccountServices
    {
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly AppDB _dbContext;
        public AccountServices(AppDB DbContext, IPasswordHasher<User> passwordHasher)
        {
            _passwordHasher = passwordHasher;
            _dbContext = DbContext;
        }
        public void RegisterUser<T>(RegisterUserDto dto) where T : User, new()
        {
            T newUser = new()
            {
                Name = dto.Name,
                EmailAddres = dto.EmailAddres,
                RoleId = dto.RoleId,
                DateOfBirth = dto.DateOfBirth
            };
            var hashedpassword = _passwordHasher.HashPassword(newUser, dto.Password);
            newUser.Password = hashedpassword;
            if (typeof(T) == typeof(CompanyUser))
            {
                var company = _dbContext.companys.FirstOrDefault(c => c.Name == dto.Company.Name);
                if (company == null)
                {
                    var newCompany = new Company()
                    {
                        Name=dto.Company.Name,
                        EmailAddres=dto.EmailAddres,
                    };
                    _dbContext.companys.Add(newCompany);
                    company = newCompany;
                }
                var companyUser = newUser as CompanyUser;
                companyUser.Company = company;
                _dbContext.users.Add(companyUser);
            }
            else
            {
                _dbContext.users.Add(newUser);
            }
            _dbContext.SaveChanges();
        }
        public string GenereteJwt(LoginDto dto, WebApplication builder)
        {
            var user = _dbContext.users.Include(u=>u.Role).FirstOrDefault(u => u.EmailAddres == dto.Email);
            if (user == null)
            {
                throw new BadHttpRequestException("Invalid username or password");
            }

           var result = _passwordHasher.VerifyHashedPassword(user, user.Password, dto.Password);
            if(result==PasswordVerificationResult.Failed)
            {
                throw new BadHttpRequestException("Invalid username or password");
            }
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Name,$"{user.Name}"),
                new Claim(ClaimTypes.Role,$"{user.Role}"),
                new Claim("DateOfBirth", user.DateOfBirth.ToString("dd-mm-yyyy"))
            };
            var token = new JwtSecurityToken
                (
                issuer: builder.Configuration["JwtIssuer"],
                audience: builder.Configuration["JwtIssuer"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(15),
                notBefore: DateTime.UtcNow,
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtKey"])), SecurityAlgorithms.HmacSha256)
                );
            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            return jwtToken;
        }
    }
}
