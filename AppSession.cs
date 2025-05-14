using cloud_app_dev_exam_project.Models;
using cloud_app_dev_exam_project.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cloud_app_dev_exam_project
{
    public static class AppSession
    {
        public static void Init(User user)
        {
            if (user == null) return;

            App.CurrentUser = user;
            App.CurrentUserRole = user.Role;

            SessionService.CurrentUserId = user.Id;
            SessionService.CurrentUserRole = user.Role;

            Console.WriteLine($"[AppSession] Initialized. Role: {user.Role}, UserId: {user.Id}");
        }
    }
}
