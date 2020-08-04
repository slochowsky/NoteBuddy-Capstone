using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Note_Buddy.Data;
using Note_Buddy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Note_Buddy.Repositories
{
    public class CategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Category GetById(int id)
        {
            return _context.Category.Include(p => p.User)
                .FirstOrDefault(c => c.Id == id);
        }

        public List<Category> GetByUserProfileId(int id)
        {
            return _context.Category.Include(p => p.User)// Using a .Include() method in the return to give us back the users category
                            .Where(p => p.UserId == id)
                            .ToList();
        }
        public void Add(Category category)
        {
            _context.Add(category);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var category = GetById(id);
            _context.Category.Remove(category);
            _context.SaveChanges();
        }
    }
}