using System;
using cloud_app_dev_exam_project.Shell;
using cloud_app_dev_exam_project.Pages;
using cloud_app_dev_exam_project.Pages.SellerPages;
using cloud_app_dev_exam_project.Pages.CustomerPages;
using cloud_app_dev_exam_project.Pages.SupplierPages;
using Microsoft.Maui.Controls;

namespace cloud_app_dev_exam_project.Pages.SupplierPages;

public partial class SupplierMainMenuPage : ContentPage
{
    private int _carouselIndex = 0;
    private IDispatcherTimer _carouselTimer;

    public SupplierMainMenuPage()
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

    private async void OnAddItemClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AddSupplierItemPage("Supplier"));
    }

    private async void OnWarehouseClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SupplierWarehousePage("Supplier"));
    }

    private async void OnBankClicked(object sender, EventArgs e) =>
        await SupplierShell.Current.GoToAsync("BankPage");

    private async void OnStatisticsClicked(object sender, EventArgs e) =>
        await SupplierShell.Current.GoToAsync("Stats");

    private async void OnAnnouncementsClicked(object sender, EventArgs e) =>
        await DisplayAlert("Announcements", "📣 Announcements page is not implemented yet.", "OK");

    private async void OnSettingsClicked(object sender, EventArgs e) =>
        await DisplayAlert("Settings", "⚙️ Settings page is not implemented yet.", "OK");

    private async void OnProfileClicked(object sender, EventArgs e) =>
        await SupplierShell.Current.GoToAsync("ProfilePage");
}
