using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cloud_app_dev_exam_project.Models
{
    public class ListableItem
    {
        [PrimaryKey, AutoIncrement]
        public int ItemId { get; set; } 
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string CoverImagePath { get; set; }
        public string ImagePathsSerialized { get; set; }

        // this is the property we want to deserialize into from the serialized string
        [Ignore] // i mark it with [Ignore] to prevent SQLite from trying to store this in the database
        public List<string> ImagePaths { get; set; } = new();

        public string Specs { get; set; }
        public string PublisherName { get; set; }
        public bool IsListedBySeller { get; set; }
        public string ListedByUserId { get; set; } 
        public int? AvailableAmount { get; set; } 

        public string AvailableLocationsSerialized { get; set; }

        [Ignore]
        public List<string> AvailableLocations { get; set; } = new();

        /*customer specific*/
        public bool IsFavourite { get; set; } = false;
        public bool IsInCart { get; set; } = false;
        public string? CustomerId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
