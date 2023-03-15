using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Quiz_API.Entity;
using Quiz_API.Exceptions;
using Quiz_API.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Quiz_API.Services
{
    public interface IAccountServices
    {
        Task RegisterUser<T>(RegisterUserDto dto) where T : User, new();
        Task<string> GenereteJwt(LoginDto dto, WebApplication builder);
        Task Delete(int id);
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
        public async Task RegisterUser<T>(RegisterUserDto dto) where T : User, new()
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
                var company = await _dbContext.Companys.FirstOrDefaultAsync(c => c.Name == dto.Company.Name);
                if (company == null)
                {
                    var newCompany = new Company()
                    {
                        Name = dto.Company.Name,
                        EmailAddres = dto.EmailAddres,
                    };
                    _dbContext.Companys.Add(newCompany);
                    company = newCompany;
                }
                var companyUser = newUser as CompanyUser;
                companyUser.Company = company;
                _dbContext.Users.Add(companyUser);
            }
            else
            {
                _dbContext.Users.Add(newUser);
            }
            await _dbContext.SaveChangesAsync();
        }
        public async Task<string> GenereteJwt(LoginDto dto, WebApplication builder)
        {
            var user = await _dbContext.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.EmailAddres == dto.Email);
            if (user == null)
            {
                throw new BadRequestException("Invalid username or password");
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, dto.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                throw new BadRequestException("Invalid username or password");
            }
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Role,$"{user.Role.Name}"),
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
        public async Task Delete(int id)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }
            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();
        }
    }
}
