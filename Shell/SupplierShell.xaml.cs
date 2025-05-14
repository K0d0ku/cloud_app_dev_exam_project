using cloud_app_dev_exam_project.Pages;
using cloud_app_dev_exam_project.Pages.SupplierPages;
using cloud_app_dev_exam_project.Pages.SellerPages;
using cloud_app_dev_exam_project.Pages.CustomerPages;
using System.Windows.Input;
using cloud_app_dev_exam_project.Services;

namespace cloud_app_dev_exam_project.Shell
{
    public partial class SupplierShell : Microsoft.Maui.Controls.Shell
    {
        public ICommand GoToAddItemCommand { get; }
        public ICommand GoToWarehouseItemCommand { get; }
        public SupplierShell()
        {
            InitializeComponent();

            Console.WriteLine($"[SupplierShell] Role: {SessionService.CurrentUserRole}");

            Routing.RegisterRoute("login", typeof(LoginPage));

            Routing.RegisterRoute("BankPage", typeof(BankPage));
            Routing.RegisterRoute("addcard", typeof(Pages.Bank.AddCardPage));
            Routing.RegisterRoute("topup", typeof(Pages.Bank.TopUpPage));

            Routing.RegisterRoute("ProfilePage", typeof(ProfilePage));

            GoToAddItemCommand = new Command(OnAddItemFlyoutTapped);
            GoToWarehouseItemCommand = new Command(OnWarehouseFlyoutTapped);
            BindingContext = this;

            Routing.RegisterRoute("Stats", typeof(StatsPage));
        }

        private async void OnAddItemFlyoutTapped()
        {
            await Navigation.PushAsync(new AddSupplierItemPage("Supplier"));
        }
        private async void OnWarehouseFlyoutTapped()
        {
            await Navigation.PushAsync(new SupplierWarehousePage("Supplier"));
        }

    }
}
