using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Note_Buddy.Repositories;
using Note_Buddy.Models;
using Note_Buddy.Data;

namespace Tabloid.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class NoteController : ControllerBase
    {
        private readonly NoteRepository _noteRepository;
        private readonly UsersRepository _usersRepository;

        public NoteController(ApplicationDbContext context)
        {
            _noteRepository = new NoteRepository(context);
            _usersRepository = new UsersRepository(context);
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_noteRepository.GetAll());
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

        [HttpGet("getbyuser/{id}")]
        public IActionResult GetByUser(int id)
        {
            return Ok(_noteRepository.GetByUserProfileId(id));
        }

        [HttpPost]
        public IActionResult Post(Note note)
        {
            _noteRepository.Add(note);
            return CreatedAtAction("Get", new { id = note.Id }, note);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, Note note)
        {
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