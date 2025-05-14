using cloud_app_dev_exam_project.Models;
using cloud_app_dev_exam_project.Services;
using System;
using System.Collections.Generic;

namespace cloud_app_dev_exam_project.Pages
{
    public partial class SupplierRegistrationPage : ContentPage
    {
        private List<string> _cities = new();
        private readonly List<string> _countryCodes = new() { "+7", "+996" };

        public SupplierRegistrationPage()
        {
            InitializeComponent();
            LoadCitiesAndCodes();
        }

        private async void LoadCitiesAndCodes()
        {
            _cities = await LocationService.GetKazakhstanCitiesAsync();
            LocationPicker.ItemsSource = _cities;

            CountryCodePicker.ItemsSource = _countryCodes;
            CountryCodePicker.SelectedItem = "+7";
        }

        private async void OnRegisterButtonClicked(object sender, EventArgs e)
        {
            var firstName = FirstNameEntry.Text;
            var lastName = LastNameEntry.Text;
            var email = EmailEntry.Text;
            var password = PasswordEntry.Text;
            var location = LocationPicker.SelectedItem as string;
            var countryCode = CountryCodePicker.SelectedItem as string;
            var contactNumber = ContactNumberEntry.Text;
            var warehouseName = WarehouseNameEntry.Text;
            var warehouseOwner = WarehouseOwnerNameEntry.Text;

            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) ||
                string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(location) || string.IsNullOrWhiteSpace(countryCode) ||
                string.IsNullOrWhiteSpace(contactNumber) || string.IsNullOrWhiteSpace(warehouseName) ||
                string.IsNullOrWhiteSpace(warehouseOwner))
            {
                await DisplayAlert("Validation Error", "Please fill in all fields", "OK");
                return;
            }

            var dbService = new DatabaseService(App.DatabasePath);

            var supplierProfile = new SupplierProfile
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Password = password,
                Location = location,
                CountryCode = countryCode,
                ContactNumber = contactNumber,
                WarehouseName = warehouseName,
                WarehouseOwnerName = warehouseOwner,
                DateCreated = DateTime.Now
            };

            await dbService.AddSupplierProfileAsync(supplierProfile);

            var user = new User
            {
                Email = email.Trim().ToLower(),
                Password = password,
                Role = "supplier"
            };

            await dbService.AddUserAsync(user);

            await DisplayAlert("Success", "Supplier registered!", "OK");
            await Navigation.PushAsync(new LoginPage());
        }

    }
}
