using Microsoft.EntityFrameworkCore;
using Note_Buddy.Data;
using Note_Buddy.Models;
using System.Linq;


namespace Note_Buddy.Repositories
{
    public class UsersRepository
    {
        private readonly ApplicationDbContext _context;

        public UsersRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Users GetByFirebaseUserId(string firebaseUserId)
        {
            return _context.Users
                .FirstOrDefault(up => up.FirebaseUserId == firebaseUserId);
        }

        public void Add(Users users)
        {
            _context.Add(users);
            _context.SaveChanges();
        }
    }
}