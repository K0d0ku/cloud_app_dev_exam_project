using System;
using cloud_app_dev_exam_project.Shell;
using cloud_app_dev_exam_project.Pages;
using cloud_app_dev_exam_project.Pages.SellerPages;
using cloud_app_dev_exam_project.Pages.CustomerPages;
using cloud_app_dev_exam_project.Pages.SupplierPages;
using Microsoft.Maui.Controls;
using cloud_app_dev_exam_project.Services;

namespace cloud_app_dev_exam_project.Pages.SellerPages
{
    public partial class SellerMainMenuPage : ContentPage
    {
        private int _carouselIndex = 0;
        private IDispatcherTimer _carouselTimer;

        public SellerMainMenuPage()
        {
            InitializeComponent();

            _carouselTimer = Dispatcher.CreateTimer();
            _carouselTimer.Interval = TimeSpan.FromSeconds(5);
            _carouselTimer.Tick += (s, e) =>
            {
                _carouselIndex = (_carouselIndex + 1) % 3;
                carousel.Position = _carouselIndex;
            };
            _carouselTimer.Start();
        }

        private async void OnButtonClicked_Shop(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ShopPage("Seller"));
        }

        private async void OnButtonClicked_Cart(object sender, EventArgs e) =>
            await CustomerShell.Current.GoToAsync("Cart");

        private async void OnButtonClicked_AddItem(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddSupplierItemPage("Seller"));
        }

        private async void OnButtonClicked_Store(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SupplierWarehousePage("Seller"));
        }

        private async void OnButtonClicked_Bank(object sender, EventArgs e) =>
            await SellerShell.Current.GoToAsync("BankPage");

        private async void OnButtonClicked_Stats(object sender, EventArgs e) =>
            await SupplierShell.Current.GoToAsync("Stats");

        private async void OnButtonClicked_Announcements(object sender, EventArgs e) =>
            await DisplayAlert("Announcements", "📢 Announcements page is not implemented yet.", "OK");

        private async void OnButtonClicked_Settings(object sender, EventArgs e) =>
            await DisplayAlert("Settings", "⚙️ Settings page is not implemented yet.", "OK");
        private async void OnButtonClicked_Profile(object sender, EventArgs e) =>
            await SellerShell.Current.GoToAsync("ProfilePage");
        private async void OnButtonClicked_Socials(object sender, EventArgs e)
        {
            try
            {
                var userId = SessionService.CurrentUserId;

                if (userId == 0)
                {
                    await DisplayAlert("Error", "User session not found.", "OK");
                    return;
                }

                var db = new DatabaseService(App.DatabasePath);

                var user = await db.GetUserByIdAsync(userId);
                if (user == null)
                {
                    await DisplayAlert("Error", "User not found.", "OK");
                    return;
                }

                var sellerProfiles = await db.GetAllSellerProfilesAsync();
                var seller = sellerProfiles.FirstOrDefault(s =>
                    s.Email.Equals(user.Email, StringComparison.OrdinalIgnoreCase));

                if (seller == null)
                {
                    await DisplayAlert("Error", "Seller profile not found.", "OK");
                    return;
                }

                if (string.IsNullOrWhiteSpace(seller.SocialMedia) || string.IsNullOrWhiteSpace(seller.SocialMediaLink))
                {
                    await DisplayAlert("No Socials", "You have not linked a social media account yet.", "OK");
                    return;
                }

                string url = seller.SocialMediaLink;

                if (!url.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                {
                    url = "https://" + url;
                }

                await Launcher.Default.OpenAsync(new Uri(url));
            }
            catch (Exception ex)
            {
                await DisplayAlert("Oops!", $"Unable to open social link: {ex.Message}", "OK");
            }
        }


    }
}
