using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cloud_app_dev_exam_project.Services
{
    public static class LocationService
    {
        public static async Task<List<string>> GetKazakhstanCitiesAsync()
        {
            await Task.Delay(100);

            return new List<string>
            {
                "Almaty", "Astana", "Shymkent", "Karaganda", "Pavlodar", "Aktobe",
                "Oskemen", "Kostanay", "Kyzylorda", "Atyrau", "Petropavl", "Taraz"
            };
        }
    }
}