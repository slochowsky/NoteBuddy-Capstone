using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Note_Buddy.Repositories;
using Note_Buddy.Models;
using Note_Buddy.Data;

namespace Note_Buddy.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class NoteController : ControllerBase
    {
        private readonly NoteRepository _noteRepository;
        private readonly UsersRepository _usersRepository;
        private readonly CategoryRepository _categoryRepository;


        public NoteController(ApplicationDbContext context)
        {
            _noteRepository = new NoteRepository(context);
            _usersRepository = new UsersRepository(context);
            _categoryRepository = new CategoryRepository(context);

        }

        [HttpGet]
        public IActionResult Get()
        {
            var currentUser = GetCurrentUserProfile();
            return Ok(_noteRepository.GetByUserProfileId(currentUser.Id));
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var post = _noteRepository.GetById(id);
            if (post == null)
            {
                return NotFound();
            }
            return Ok(post);
        }

        [HttpPost]
        public IActionResult Post(Note note)
        {
            var currentUser = GetCurrentUserProfile();
            var category = _categoryRepository.GetById((int)note.CategoryId);
            if (category.UserId != currentUser.Id)
            {
                return BadRequest();
            }
            note.UserId = currentUser.Id;
            _noteRepository.Add(note);
            return CreatedAtAction("Get", new { id = note.Id }, note);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, Note note)
        {
            var currentUser = GetCurrentUserProfile();
            if (note.UserId != currentUser.Id)
            {
                return Unauthorized();
            }
            if (id != note.Id)
            {
                return BadRequest();
            }
            _noteRepository.Update(note);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var user = GetCurrentUserProfile();
            var post = _noteRepository.GetById(id);
            if (user.Id != post.UserId)
            {
                return Forbid();
            }

            _noteRepository.Delete(id);
            return NoContent();
        }

        private Users GetCurrentUserProfile()
        {
            var firebaseUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return _usersRepository.GetByFirebaseUserId(firebaseUserId);
        }
    }
}