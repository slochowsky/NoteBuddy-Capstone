using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System;
using Note_Buddy.Data;
using Note_Buddy.Models;

namespace Note_Buddy.Repositories
{
    public class NoteRepository
    {
        private readonly ApplicationDbContext _context;

        public NoteRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Note> GetAll()
        {
            return _context.Notes
                .Include(p => p.User)
                .Include(p => p.Category)
                .ToList();

        }

        public Note GetById(int id)
        {
            return _context.Notes.Include(p => p.User)
                                .Include(p => p.Category)
                                .FirstOrDefault(p => p.Id == id);
        }

        public List<Note> GetByUserProfileId(int id)
        {
            return _context.Notes.Include(p => p.User)
                             .Where(p => p.UserId == id)
                            .Include(p => p.Category)
                            .ToList();
        }

        public void Add(Note note)
        {

            _context.Add(note);
            _context.SaveChanges();
        }

        public void Update(Note note)
        {
            _context.Entry(note).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var note = GetById(id);
            _context.Notes.Remove(note);
            _context.SaveChanges();
        }
    }
}