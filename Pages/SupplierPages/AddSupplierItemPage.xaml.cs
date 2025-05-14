using cloud_app_dev_exam_project.Models;
using cloud_app_dev_exam_project.Services;
using Newtonsoft.Json;

namespace cloud_app_dev_exam_project.Pages.SupplierPages;

public partial class AddSupplierItemPage : ContentPage
{
    private readonly string _role;
    private string? coverImagePath;
    private List<string> galleryImagePaths = new();

    public AddSupplierItemPage(string role)
    {
        InitializeComponent();
        _role = role;
        LoadCategories();
    }

    private async void LoadCategories()
    {
        var categories = await CategoryService.GetHardwareCategoriesAsync();
        categoryPicker.ItemsSource = categories;
    }

    private async void OnPickCoverImage(object sender, EventArgs e)
    {
        var path = await ImageService.PickAndSaveImageAsync();
        if (path != null)
        {
            coverImagePath = path;
            coverImagePreview.Source = ImageSource.FromFile(path);
            coverImagePreview.IsVisible = true;
        }
    }

    private async void OnPickGalleryImages(object sender, EventArgs e)
    {
        galleryImagePaths.Clear();
        galleryPreview.Children.Clear();

        for (int i = 0; i < 3; i++)
        {
            var path = await ImageService.PickAndSaveImageAsync();
            if (path == null)
                break;

            galleryImagePaths.Add(path);
            var img = new Image { Source = ImageSource.FromFile(path), HeightRequest = 100, WidthRequest = 100 };
            galleryPreview.Children.Add(img);
        }

        galleryPreview.IsVisible = galleryImagePaths.Count > 0;
    }


    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (SessionService.CurrentUserId == 0)
        {
            await DisplayAlert("Error", "User session is invalid. Please log in again.", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(nameEntry.Text) ||
            string.IsNullOrWhiteSpace(priceEntry.Text) ||
            string.IsNullOrWhiteSpace(descriptionEditor.Text) ||
            string.IsNullOrWhiteSpace(amountEntry.Text) ||
            categoryPicker.SelectedItem == null ||
            coverImagePath == null || galleryImagePaths.Count < 3)
        {
            await DisplayAlert("Missing Info", "Please complete all fields and upload images.", "OK");
            return;
        }

        var warehouseName = Preferences.Get("WarehouseName", "Unknown").Trim();

        var item = new ListableItem
        {
            Name = nameEntry.Text,
            Price = decimal.Parse(priceEntry.Text),
            Description = descriptionEditor.Text,
            Specs = specsEditor.Text,
            PublisherName = warehouseName,
            AvailableAmount = int.Parse(amountEntry.Text),
            Category = categoryPicker.SelectedItem.ToString(),
            CoverImagePath = coverImagePath,
            ImagePathsSerialized = JsonConvert.SerializeObject(galleryImagePaths),
            IsListedBySeller = _role == "Seller",
            ListedByUserId = SessionService.CurrentUserId.ToString()
        };

        await App.DbService.AddListableItemAsync(item);
        await DisplayAlert("Success", "Item listed successfully!", "OK");
        await Navigation.PopAsync();

        Console.WriteLine($"[AddItem] Listed: {item.Name}, Publisher: {item.PublisherName}");
    }
}