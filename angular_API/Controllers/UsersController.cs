using angular_API.ModelsFromDB;
using angular_API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace angular_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        readonly ILogger<UsersController> _logger;
        readonly IUsersRepository _usersRepository;
        // GET: UsersController
        public UsersController(ILogger<UsersController> logger, IUsersRepository usersRepository)
        {
            _logger = logger;
            _usersRepository = usersRepository;
        }

        [HttpGet("GetAllUser")]
        public async Task<IActionResult> GetUsers()
        {
            //try
            //{
            //    var list = await _usersRepository.GetAllUser();               
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError(ex, Environment.StackTrace, ex.InnerException);
            //}
            var list = await _usersRepository.GetAllUser();
            return Ok(list);
        }

        [HttpGet("GetUser")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _usersRepository.GetUser(id);
            return Ok(user);
        }
    }
}
