using cloud_app_dev_exam_project.Models;
using cloud_app_dev_exam_project.Services;
using System;
using System.Threading.Tasks;

namespace cloud_app_dev_exam_project.Pages
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            string email = EmailEntry.Text?.Trim();
            string password = PasswordEntry.Text;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                await DisplayAlert("Error", "Please enter both email and password.", "OK");
                return;
            }

            var dbService = new DatabaseService(App.DatabasePath);
            var user = await dbService.GetUserByEmailAsync(email);

            if (user == null || user.Password != password) // TODO: Replace with secure hash check
            {
                await DisplayAlert("Login Failed", "Invalid email or password.", "OK");
                return;
            }

            AppSession.Init(user);


            switch (user.Role?.ToLowerInvariant())
            {
                case "customer":
                    Application.Current.MainPage = new Shell.CustomerShell();
                    break;

                case "seller":
                    Application.Current.MainPage = new Shell.SellerShell(); 
                    break;
                case "supplier":
                    Application.Current.MainPage = new Shell.SupplierShell();
                    break;

                default:
                    await DisplayAlert("Error", $"Unknown role: {user.Role}", "OK");
                    break;
            }
        }

        private async void OnRegisterButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegistrationRolePage());
        }
    }
}
