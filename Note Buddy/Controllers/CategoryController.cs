using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Note_Buddy.Data;
using Note_Buddy.Models;
using Note_Buddy.Repositories;


namespace Note_Buddy.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryRepository _categoryRepository;
        private readonly UsersRepository _usersRepository;
        public CategoryController(ApplicationDbContext context)
        {
            _categoryRepository = new CategoryRepository(context);
            _usersRepository = new UsersRepository(context);
        }

        [HttpGet]
        public IActionResult Get()
        {
            var currentUser = GetCurrentUserProfile();
            return Ok(_categoryRepository.GetByUserProfileId(currentUser.Id));
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var category = _categoryRepository.GetById(id);
            var currentUser = GetCurrentUserProfile();
            if (category.UserId != currentUser.Id)
            {
                return Unauthorized();
            }
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }

        [HttpPost]
        public IActionResult Post(Category category)
        {
            var currentUser = GetCurrentUserProfile();
            category.UserId = currentUser.Id;
            _categoryRepository.Add(category);
            return CreatedAtAction("Get", new { id = category.Id }, category);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var category = _categoryRepository.GetById(id);
            var currentUser = GetCurrentUserProfile();
            if (category.UserId != currentUser.Id)
            {
                return Unauthorized();
            }
            _categoryRepository.Delete(id);
            return NoContent();
        }

        private Users GetCurrentUserProfile()
        {
            var firebaseUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return _usersRepository.GetByFirebaseUserId(firebaseUserId);
        }
    }
}