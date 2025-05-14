using cloud_app_dev_exam_project.Pages;
using cloud_app_dev_exam_project.Pages.SupplierPages;
using cloud_app_dev_exam_project.Pages.SellerPages;
using cloud_app_dev_exam_project.Pages.CustomerPages;
using cloud_app_dev_exam_project.Services;
using System.Windows.Input;

namespace cloud_app_dev_exam_project.Shell
{
    public partial class CustomerShell : Microsoft.Maui.Controls.Shell
    {
        public ICommand GoToShopCommand { get; }
        public CustomerShell()
        {
            InitializeComponent();

            Console.WriteLine($"[CustomerShell] Role: {SessionService.CurrentUserRole}");

            Routing.RegisterRoute("login", typeof(LoginPage));

            Routing.RegisterRoute("Cart", typeof(CartPage));

            Routing.RegisterRoute("BankPage", typeof(BankPage));
            Routing.RegisterRoute("addcard", typeof(Pages.Bank.AddCardPage));
            Routing.RegisterRoute("topup", typeof(Pages.Bank.TopUpPage));

            GoToShopCommand = new Command(OnShopFlyoutTapped);
            BindingContext = this;

            Routing.RegisterRoute("Stats", typeof(StatsPage));

            Routing.RegisterRoute("ProfilePage", typeof(ProfilePage));
        }

        private async void OnShopFlyoutTapped()
        {
            await Navigation.PushAsync(new ShopPage("Customer"));
        }
    }
}
