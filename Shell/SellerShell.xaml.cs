using cloud_app_dev_exam_project.Pages;
using cloud_app_dev_exam_project.Pages.SupplierPages;
using cloud_app_dev_exam_project.Pages.SellerPages;
using cloud_app_dev_exam_project.Pages.CustomerPages;
using cloud_app_dev_exam_project.Services;
using System.Windows.Input;

namespace cloud_app_dev_exam_project.Shell
{
	public partial class SellerShell : Microsoft.Maui.Controls.Shell
	{
        public ICommand GoToShopCommand { get; }
        public ICommand GoToAddItemCommand { get; }
        public ICommand GoToWarehouseItemCommand { get; }
        public SellerShell()
		{
            InitializeComponent();

            Console.WriteLine($"[SellerShell] Role: {SessionService.CurrentUserRole}");

            Routing.RegisterRoute("login", typeof(LoginPage));
            
            Routing.RegisterRoute("Cart", typeof(CartPage));

            Routing.RegisterRoute("BankPage", typeof(BankPage));
            Routing.RegisterRoute("addcard", typeof(Pages.Bank.AddCardPage));
            Routing.RegisterRoute("topup", typeof(Pages.Bank.TopUpPage));

            GoToShopCommand = new Command(OnShopFlyoutTapped);
            GoToAddItemCommand = new Command(OnAddItemFlyoutTapped);
            GoToWarehouseItemCommand = new Command(OnWarehouseFlyoutTapped);
            BindingContext = this;

            Routing.RegisterRoute("ProfilePage", typeof(ProfilePage));

            Routing.RegisterRoute("Stats", typeof(StatsPage));
        }

        private async void OnShopFlyoutTapped()
        {
            await Navigation.PushAsync(new ShopPage("Seller"));
        }

        private async void OnAddItemFlyoutTapped()
        {
            await Navigation.PushAsync(new AddSupplierItemPage("Seller"));
        }
        private async void OnWarehouseFlyoutTapped()
        {
            await Navigation.PushAsync(new SupplierWarehousePage("Seller"));
        }
    }

}
