namespace cloud_app_dev_exam_project.Pages
{
    using cloud_app_dev_exam_project.Models;
    using cloud_app_dev_exam_project.Services;
    using CloudAppDevExamProject.Models;
    using System;
    using System.Threading.Tasks;

    public partial class CustomerRegistrationPage : ContentPage
    {
        private List<string> _cities = new List<string>();

        public CustomerRegistrationPage()
        {
            InitializeComponent();
            LoadCities();
        }

        private async void LoadCities()
        {
            var cities = await LocationService.GetKazakhstanCitiesAsync();
            LocationPicker.ItemsSource = cities;
        }

        private async void OnRegisterButtonClicked(object sender, EventArgs e)
        {
            var username = UsernameEntry.Text;
            var firstName = FirstNameEntry.Text;
            var lastName = LastNameEntry.Text;
            var email = EmailEntry.Text;
            var password = PasswordEntry.Text;
            var location = LocationPicker.SelectedItem as string;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(firstName) ||
                string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(email) ||
                string.IsNullOrEmpty(password) || string.IsNullOrEmpty(location))
            {
                await DisplayAlert("Validation Error", "Please fill in all fields", "OK");
                return;
            }

            var user = new User
            {
                Email = email,
                Password = password,
                Role = "customer"
            };

            var customerProfile = new CustomerProfile
            {
                Username = username,
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Location = location,
                DateCreated = DateTime.Now
            };

            var dbService = new DatabaseService(App.DatabasePath);

            await dbService.AddUserAsync(user); 
            await dbService.AddCustomerProfileAsync(customerProfile); 

            await Navigation.PushAsync(new LoginPage());
        }
    }
}
