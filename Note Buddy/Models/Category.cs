using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Note_Buddy.Models
{
    public class Category
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public Users User { get; set; }
        public string Name { get; set; }
}
}
