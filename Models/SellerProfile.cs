using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cloud_app_dev_exam_project.Models
{
    public class SellerProfile
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; } 
        public string CompanyName { get; set; }
        public string Email { get; set; }      
        public string Password { get; set; }   
        public string Location { get; set; }   

        public string SocialMedia { get; set; } 
        public string SocialMediaLink { get; set; } 

        public DateTime DateCreated { get; set; } 
    }
}
