using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cloud_app_dev_exam_project.Services
{
    public static class SessionService
    {
        public static int CurrentUserId { get; set; }
        public static string CurrentUserRole { get; set; }

        public static void LogOut()
        {
            CurrentUserId = 0;
            CurrentUserRole = null;
        }
    }
}
