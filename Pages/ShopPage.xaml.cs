using cloud_app_dev_exam_project.Models;
using cloud_app_dev_exam_project.Pages.SupplierPages;
using cloud_app_dev_exam_project.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace cloud_app_dev_exam_project.Pages;

public partial class ShopPage : ContentPage
{
    private readonly string _role;
    private List<ListableItem> _allItems;
    private string _selectedCategory = string.Empty;
    private bool _isShowingSellerItems = true;

    public ShopPage(string role)
    {
        InitializeComponent();
        _role = role;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        Console.WriteLine($"[ShopPage] Loading data for role: {_role}");

        _allItems = await App.DbService.GetAllListableItemsAsync();
        var role = SessionService.CurrentUserRole;

        Console.WriteLine($"[ShopPage] Role: {role}");

        if (_role == "Seller")
        {
            SellerToggleSection.IsVisible = true;
            _isShowingSellerItems = true;
        }
        else if (_role == "Customer")
        {
            CartButton.IsVisible = true;
            _isShowingSellerItems = true;
        }
        else
        {
            await DisplayAlert("Error", "Unknown role or user not logged in", "OK");
        }

        ApplyFilters();
    }

    private void OnSearchTextChanged(object sender, TextChangedEventArgs e) => ApplyFilters();

    private void OnSellerViewClicked(object sender, EventArgs e)
    {
        _isShowingSellerItems = true;
        ApplyFilters();
    }

    private void OnSupplierViewClicked(object sender, EventArgs e)
    {
        _isShowingSellerItems = false;
        ApplyFilters();
    }

    private async void OnCategoryButtonClicked(object sender, EventArgs e)
    {
        var categories = await CategoryService.GetHardwareCategoriesAsync();
        var actionSheetOptions = categories.Concat(new[] { "Cancel" }).ToArray();
        var selectedCategory = await DisplayActionSheet("Select Category", "Cancel", null, actionSheetOptions);

        if (selectedCategory != "Cancel")
        {
            _selectedCategory = selectedCategory;
            ApplyFilters();
        }
    }

    private void ApplyFilters()
    {
        var items = _isShowingSellerItems
            ? _allItems.Where(i => i.IsListedBySeller)
            : _allItems.Where(i => !i.IsListedBySeller);

        string search = ItemSearchBar.Text?.ToLower() ?? "";
        string category = _selectedCategory;

        var filtered = items
            .Where(i =>
                (string.IsNullOrWhiteSpace(search) || i.Name.ToLower().Contains(search)) &&
                (string.IsNullOrWhiteSpace(category) || i.Category == category)
            )
            .ToList();

        ItemsCollection.ItemsSource = filtered;
    }

    private async void OnItemTapped(object sender, EventArgs e)
    {
        if (sender is Frame frame && frame.BindingContext is ListableItem item)
        {
            Console.WriteLine($"[Tap] Navigating to item: {item.Name} (ID: {item.ItemId})");
            await Navigation.PushAsync(new ItemDetailsPage(item, _role));
        }
    }

    private async void OnButtonClicked_AddItem(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AddSupplierItemPage("Seller"));
    }

    private async void OnButtonClicked_Cart(object sender, EventArgs e)
    {
        await AppShell.Current.GoToAsync("Cart");
    }
}
