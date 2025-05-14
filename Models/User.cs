using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cloud_app_dev_exam_project.Models
{
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(50), Unique]
        public string Username { get; set; }

        [MaxLength(100)]
        public string Password { get; set; }

        [MaxLength(100)]
        public string Email { get; set; }

        [MaxLength(100)]
        public string FullName { get; set; }

        public string Role { get; set; } // "Customer", "Seller", "Supplier", "Admin"

        public int LocationId { get; set; } // fk

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
