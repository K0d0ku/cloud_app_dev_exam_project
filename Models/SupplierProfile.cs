using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cloud_app_dev_exam_project.Models
{
    public class SupplierProfile
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }

        public string Location { get; set; }

        public string CountryCode { get; set; }  // +7
        public string ContactNumber { get; set; }  // 7011234567

        public string WarehouseName { get; set; }
        public string WarehouseOwnerName { get; set; }

        public DateTime DateCreated { get; set; }
    }
}

