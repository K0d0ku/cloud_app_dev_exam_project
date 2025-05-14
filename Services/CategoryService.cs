using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cloud_app_dev_exam_project.Services
{
    public static class CategoryService
    {
        public static async Task<List<string>> GetHardwareCategoriesAsync()
        {
            await Task.Delay(100);

            return new List<string>
            {
                "CPU", "GPU", "Motherboard", "RAM", "SSD", "HDD", "PSU",
                "Cooling", "Case", "Monitors", "Mouses", "Keyboards",
                "Cables", "Accessories", "Others"
            };
        }
    }
}
