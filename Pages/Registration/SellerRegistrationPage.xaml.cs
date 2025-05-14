using cloud_app_dev_exam_project.Models;
using cloud_app_dev_exam_project.Services;
using System;
using System.Collections.Generic;

namespace cloud_app_dev_exam_project.Pages
{
    public partial class SellerRegistrationPage : ContentPage
    {
        private List<string> _cities = new();
        private List<string> _socials = new() { "None", "Instagram", "TikTok" };

        public SellerRegistrationPage()
        {
            InitializeComponent();
            LoadCities();
            SocialsPicker.ItemsSource = _socials;
        }

        private async void LoadCities()
        {
            _cities = await LocationService.GetKazakhstanCitiesAsync();
            LocationPicker.ItemsSource = _cities;
        }

        private void OnSocialsPickerChanged(object sender, EventArgs e)
        {
            var selectedSocial = SocialsPicker.SelectedItem as string;
            SocialLinkEntry.IsVisible = selectedSocial == "Instagram" || selectedSocial == "TikTok";
        }

        private async void OnRegisterButtonClicked(object sender, EventArgs e)
        {
            var firstName = FirstNameEntry.Text;
            var lastName = LastNameEntry.Text;
            var companyName = CompanyNameEntry.Text;
            var email = EmailEntry.Text;
            var password = PasswordEntry.Text;
            var location = LocationPicker.SelectedItem as string;
            var selectedSocial = SocialsPicker.SelectedItem as string;
            var socialLink = SocialLinkEntry.Text;

            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) ||
                string.IsNullOrWhiteSpace(companyName) || string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(location))
            {
                await DisplayAlert("Validation Error", "Please fill in all fields", "OK");
                return;
            }

            if (selectedSocial != "None" && string.IsNullOrWhiteSpace(socialLink))
            {
                await DisplayAlert("Validation Error", "Please provide a social media link", "OK");
                return;
            }

            var sellerProfile = new SellerProfile
            {
                FirstName = firstName,
                LastName = lastName,
                CompanyName = companyName,
                Email = email,
                Password = password,
                Location = location,
                SocialMedia = selectedSocial == "None" ? null : selectedSocial,
                SocialMediaLink = string.IsNullOrWhiteSpace(socialLink) ? null : socialLink,
                DateCreated = DateTime.Now
            };

            var dbService = new DatabaseService(App.DatabasePath);
            await dbService.AddSellerProfileAsync(sellerProfile);
            
            var user = new User
            {
                Email = email.Trim().ToLower(),
                Password = password,
                Role = "seller"
            };

            await dbService.AddUserAsync(user);

            await DisplayAlert("Success", "Seller registered!", "OK");
            await Navigation.PushAsync(new LoginPage());
        }
    }
}