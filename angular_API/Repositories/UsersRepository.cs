using angular_API.DbContexts;
using angular_API.ModelsFromDB;
using Microsoft.EntityFrameworkCore;

namespace angular_API.Repositories
{
    public interface IUsersRepository
    {
        Task<IEnumerable<Appuser>> GetAllUser();
        Task<Appuser> GetUser(int id);
    }

    public class UsersRepository : IUsersRepository
    {
        readonly dating_appContext _dating_AppContext;
        readonly ILogger<UsersRepository> _logger;
        public UsersRepository(dating_appContext dating_AppContext, ILogger<UsersRepository> logger)
        {
            _dating_AppContext = dating_AppContext;
            _logger = logger;
        }

        public async Task<IEnumerable<Appuser>> GetAllUser()
        {
            try
            {
                return await _dating_AppContext.Appusers.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, Environment.StackTrace, ex.InnerException);
                return default;
            }
        }

        public async Task<Appuser> GetUser(int id)
        {
            try
            {
                return await _dating_AppContext.Appusers.Where(x => x.Id == id).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, Environment.StackTrace, ex.InnerException);
                return default;
            }

        }
    }
}
