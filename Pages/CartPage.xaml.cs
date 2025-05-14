using cloud_app_dev_exam_project.Models;
using cloud_app_dev_exam_project.Services;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace cloud_app_dev_exam_project.Pages
{
    public partial class CartPage : ContentPage, INotifyPropertyChanged
    {
        private DatabaseService db;
        private List<(CartItem Cart, ListableItem ListableItem)> cartData;
        private List<CartDisplayItem> displayData;

        public CartPage()
        {
            InitializeComponent();
            db = new DatabaseService(App.DatabasePath);
            BindingContext = this;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var userId = SessionService.CurrentUserId;
            var cartItems = await db.GetCartItemsAsync(userId);
            var allItems = await db.GetAllListableItemsAsync();

            cartData = cartItems.Select(ci =>
            {
                var item = allItems.FirstOrDefault(i => i.ItemId == ci.ListableItemId);
                return (Cart: ci, ListableItem: item);
            }).Where(x => x.ListableItem != null).ToList();

            displayData = cartData.Select(cd => new CartDisplayItem
            {
                CartItem = cd.Cart,
                ListableItem = cd.ListableItem
            }).ToList();

            foreach (var item in displayData)
            {
                item.SelectionChanged += OnItemSelectionChanged;
            }

            CartCollection.ItemsSource = displayData;

            OnPropertyChanged(nameof(TotalPriceOfSelectedItems)); 
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async void OnBuyClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Coming Soon", "Purchase functionality will be added later 🛒", "OK");
        }

        private async void OnRemoveClicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var context = button.BindingContext;
            var item = (dynamic)context;

            await db.RemoveFromCartAsync(item.CartItem.Id);
            OnAppearing();
        }

        private async void OnQuantityChanged(object sender, ValueChangedEventArgs e)
        {
            var stepper = (Stepper)sender;
            var context = (dynamic)stepper.BindingContext;

            int newQuantity = (int)e.NewValue;

            await db.UpdateCartItemQuantityAsync(context.CartItem.Id, newQuantity);

            OnPropertyChanged(nameof(TotalPriceOfSelectedItems));
        }

        private void OnItemSelected(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var context = button.BindingContext as CartDisplayItem;

            if (context != null)
            {
                context.IsSelected = !context.IsSelected;

                OnPropertyChanged(nameof(TotalPriceOfSelectedItems));
            }
        }

        private void OnSelectAllClicked(object sender, EventArgs e)
        {
            foreach (var item in displayData)
            {
                item.IsSelected = true;
            }

            OnPropertyChanged(nameof(TotalPriceOfSelectedItems));
        }

        private async void OnDeleteSelectedClicked(object sender, EventArgs e)
        {
            var selectedItems = displayData.Where(item => item.IsSelected).ToList();

            foreach (var item in selectedItems)
            {
                await db.RemoveFromCartAsync(item.CartItem.Id);
            }

            displayData.RemoveAll(item => item.IsSelected);
            OnAppearing();
        }

        private async void OnBuySelectedClicked(object sender, EventArgs e)
        {
            var selectedItems = displayData.Where(item => item.IsSelected).ToList();

            foreach (var item in selectedItems)
            {
                await DisplayAlert("Buying", $"Buying {item.ListableItem.Name}...", "OK");
            }
        }

        private void OnItemSelectionChanged()
        {
            OnPropertyChanged(nameof(TotalPriceOfSelectedItems));
        }

        public decimal TotalPriceOfSelectedItems =>
            displayData?.Where(item => item.IsSelected).Sum(item => item.TotalPrice) ?? 0;
    }
}