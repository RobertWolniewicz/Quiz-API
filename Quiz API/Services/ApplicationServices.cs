using Microsoft.EntityFrameworkCore;
using Quiz_API.Entity;

namespace Quiz_API.Services
{
    public interface IApplicationServices
    {
        Task SetEmailParams(EmailParams emailParams);
        Task<EmailParams> GetEmailParams();
    }
    public class ApplicationServices : IApplicationServices
    {
        private readonly AppDB _dbContext;
        public ApplicationServices(AppDB DbContext)
        {
            _dbContext = DbContext;
        }
        public async Task SetEmailParams(EmailParams emailParams)
        {
            _dbContext.EmailParams.RemoveRange(_dbContext.EmailParams);
            _dbContext.EmailParams.Add(emailParams);
            await _dbContext.SaveChangesAsync();
        }
        public async Task<EmailParams> GetEmailParams()
        {
            return await _dbContext.EmailParams.FirstAsync();
        }
    }
}
