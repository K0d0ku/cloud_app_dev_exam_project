using cloud_app_dev_exam_project.Models;
using cloud_app_dev_exam_project.Services;
using Newtonsoft.Json;

namespace cloud_app_dev_exam_project.Pages;

public partial class ItemDetailsPage : ContentPage
{
    private ListableItem _item;
    private readonly string _role;
    private readonly string _currentUserId;
    //private readonly BankService _bankService;
    private readonly PurchaseService _purchaseService;

    public ItemDetailsPage(ListableItem item, string role)
    {
        InitializeComponent();
        _item = item;
        _role = role;
        _currentUserId = SessionService.CurrentUserId.ToString();
        //_bankService = App.BankService;
        _purchaseService = App.PurchaseService;

        if (!string.IsNullOrWhiteSpace(_item.ImagePathsSerialized))
            _item.ImagePaths = JsonConvert.DeserializeObject<List<string>>(_item.ImagePathsSerialized);

        if (!string.IsNullOrWhiteSpace(_item.AvailableLocationsSerialized))
            _item.AvailableLocations = JsonConvert.DeserializeObject<List<string>>(_item.AvailableLocationsSerialized);


        SetupPage();
    }

    private void SetupPage()
    {
        titleLabel.Text = _item.Name;
        categoryLabel.Text = $"Category: {_item.Category}";
        priceLabel.Text = $"Price: ₸{_item.Price:F2}";
        specsLabel.Text = $"Specs: {_item.Specs}";
        quantityLabel.Text = $"Available: {_item.AvailableAmount}";
        publisherLabel.Text = $"Publisher: {_item.PublisherName}";
        descriptionLabel.Text = _item.Description;
        timestampLabel.Text = $"Posted on: {_item.CreatedAt:yyyy-MM-dd HH:mm}";

        coverImage.Source = ImageSource.FromFile(_item.CoverImagePath);

        var fullPaths = _item.ImagePaths.Select(filename =>
        ImageService.GetFullPath(filename)).ToList();
        imageCarousel.ItemsSource = fullPaths;


        locationsLabel.Text = _item.AvailableLocations.Any()
            ? $"Available at: {string.Join(", ", _item.AvailableLocations)}"
            : "No available locations listed.";

        bool isOwner = _item.ListedByUserId == _currentUserId;
        bool listedBySeller = _item.IsListedBySeller;

        editButton.IsVisible = (_role == "Supplier" || _role == "Seller") && isOwner;
        deleteButton.IsVisible = editButton.IsVisible;

        buyButton.IsVisible = (_role == "Seller" && !listedBySeller) ||
                              (_role == "Customer" && listedBySeller);

        cartButton.IsVisible = (_role == "Customer" && listedBySeller) || 
                               (_role == "Seller" && !isOwner && !listedBySeller);
        favoriteButton.IsVisible = _role == "Customer";
    }

    private async void OnEditClicked(object sender, EventArgs e)
        => await DisplayAlert("Edit", "Edit item placeholder", "OK");

    private async void OnDeleteClicked(object sender, EventArgs e)
        => await DisplayAlert("Delete", "Delete item placeholder", "OK");

    private async void OnBuyClicked(object sender, EventArgs e)
    {
        int buyerUserId = SessionService.CurrentUserId;

        var cards = await App.DbService.GetBankCardsForUserAsync(buyerUserId);
        if (cards == null || cards.Count == 0)
        {
            await DisplayAlert("No Card", "You need a bank card to buy this item.", "OK");
            return;
        }

        var cardOptions = cards.Select(c => $"**** {c.CardNumber[^4..]} (₸{c.Balance:F2})").ToArray();
        string selected = await DisplayActionSheet("Select Payment Card", "Cancel", null, cardOptions);

        if (selected == "Cancel" || string.IsNullOrWhiteSpace(selected))
            return;

        var selectedCard = cards[Array.IndexOf(cardOptions, selected)];

        bool success = await _purchaseService.BuySingleItemAsync(_item.ItemId, buyerUserId, selectedCard.Id);

        if (success)
        {
            await DisplayAlert("Success", "Purchase completed 🎉", "OK");
            await Navigation.PopAsync();
        }
        else
        {
            await DisplayAlert("Failed", "Purchase failed. Check balance or availability.", "OK");
        }
    }



    private async void OnAddToCartClicked(object sender, EventArgs e)
    {
        var db = App.DbService;

        int userId = SessionService.CurrentUserId;
        int maxQty = _item.AvailableAmount ?? 0;

        if (maxQty <= 0)
        {
            await DisplayAlert("Unavailable", "This item is currently out of stock.", "OK");
            return;
        }

        string qtyInput = await DisplayPromptAsync("Quantity", $"Enter quantity (1 to {maxQty}):",
                                                    initialValue: "1", maxLength: 3, keyboard: Keyboard.Numeric);

        if (string.IsNullOrWhiteSpace(qtyInput) || !int.TryParse(qtyInput, out int qty) || qty < 1 || qty > maxQty)
        {
            await DisplayAlert("Invalid", "Please enter a valid quantity.", "OK");
            return;
        }

        var cartItem = new CartItem
        {
            UserId = userId,
            ListableItemId = _item.ItemId,
            Quantity = qty
        };

        await db.AddToCartAsync(userId, _item.ItemId, qty);

        await DisplayAlert("Success", "Item added to cart 🛒", "OK");
    }


    private async void OnFavoriteClicked(object sender, EventArgs e)
        => await DisplayAlert("Favorite", "Toggle favorite placeholder", "OK");
}