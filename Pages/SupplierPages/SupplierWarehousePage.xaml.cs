using cloud_app_dev_exam_project.Models;
using cloud_app_dev_exam_project.Services;

namespace cloud_app_dev_exam_project.Pages.SupplierPages;

public partial class SupplierWarehousePage : ContentPage
{
    private readonly string _role;

    public SupplierWarehousePage(string role)
    {
        InitializeComponent();
        _role = role;
        Title = role == "Seller" ? "My Store" : "Supplier Warehouse";
        LoadItems();
    }

    private async void LoadItems()
    {
        var allItems = await App.DbService.GetAllListableItemsAsync();
        List<ListableItem> filteredItems;

        Console.WriteLine($"[WarehousePage] Role: {_role}");
        Console.WriteLine($"[WarehousePage] Current User ID: {SessionService.CurrentUserId}");

        foreach (var item in allItems)
        {
            Console.WriteLine($"Item: {item.Name}, Seller? {item.IsListedBySeller}, ListedBy: {item.ListedByUserId}");
        }


        if (_role == "Seller")
        {
            var currentUserId = SessionService.CurrentUserId.ToString();
            filteredItems = allItems
                .Where(item => item.IsListedBySeller && item.ListedByUserId == currentUserId)
                .ToList();
        }
        else
        {
            var currentWarehouseName = Preferences.Get("WarehouseName", "Unknown").Trim().ToLower();

            filteredItems = allItems
                .Where(item => !item.IsListedBySeller &&
                               item.PublisherName?.Trim().ToLower() == currentWarehouseName)
                .ToList();
        }

        itemsCollection.ItemsSource = filteredItems;
    }
    private async void OnItemTapped(object sender, EventArgs e)
    {
        if (sender is Frame frame && frame.BindingContext is ListableItem item)
        {
            Console.WriteLine($"[Tap] Navigating to item: {item.Name} (ID: {item.ItemId})");
            await Navigation.PushAsync(new ItemDetailsPage(item, _role));
        }
    }
}
