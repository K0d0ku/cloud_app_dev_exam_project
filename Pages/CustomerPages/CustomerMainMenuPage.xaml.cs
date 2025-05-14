using System;
using cloud_app_dev_exam_project.Shell;
using Microsoft.Maui.Controls;

namespace cloud_app_dev_exam_project.Pages.CustomerPages
{
    public partial class CustomerMainMenuPage : ContentPage
    {
        private int _carouselIndex = 0;
        private IDispatcherTimer _carouselTimer;

        public CustomerMainMenuPage()
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
            await Navigation.PushAsync(new ShopPage("Customer"));
        }

        private async void OnButtonClicked_Cart(object sender, EventArgs e) =>
            await CustomerShell.Current.GoToAsync("Cart");

        private async void OnButtonClicked_Favourites(object sender, EventArgs e) =>
            await DisplayAlert("Feature Not Available", "❤️ Favourites page is not implemented yet.", "OK");

        private async void OnButtonClicked_Bank(object sender, EventArgs e) =>
            await CustomerShell.Current.GoToAsync("BankPage");

        private async void OnButtonClicked_History(object sender, EventArgs e) =>
            await SupplierShell.Current.GoToAsync("Stats");

        private async void OnButtonClicked_Announcements(object sender, EventArgs e) =>
            await DisplayAlert("Feature Not Available", "📢 Announcements page is not implemented yet.", "OK");

        private async void OnButtonClicked_Settings(object sender, EventArgs e) =>
            await DisplayAlert("Feature Not Available", "⚙️ Settings page is not implemented yet.", "OK");

        private async void OnButtonClicked_Profile(object sender, EventArgs e) =>
            await CustomerShell.Current.GoToAsync("ProfilePage");
    }
}