
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Note_Buddy.Data;
using Note_Buddy.Models;
using Note_Buddy.Repositories;

namespace Note_Buddy.Controllers
{
    [Authorize]
    [Route("api/user")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UsersRepository _usersRepository;
        public UsersController(ApplicationDbContext context)
        {
            _usersRepository = new UsersRepository(context);
        }

        [HttpGet("{firebaseUserId}")]
        public IActionResult GetByFirebaseUserId(string firebaseUserId)
        {
            var userProfile = _usersRepository.GetByFirebaseUserId(firebaseUserId);
            if (userProfile == null)
            {
                return NotFound();
            }
            return Ok(userProfile);
        }

        [HttpPost]
        public IActionResult Register(Users users)
        {
            // All newly registered users start out as a "user" user type (i.e. they are not admins)
            _usersRepository.Add(users);
            return CreatedAtAction(
                nameof(GetByFirebaseUserId), new { firebaseUserId = users.FirebaseUserId }, users);
        }
    }
}