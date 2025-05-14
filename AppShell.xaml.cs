using cloud_app_dev_exam_project.Pages;
using Microsoft.Maui.Controls;

namespace cloud_app_dev_exam_project
{
    public partial class AppShell : Microsoft.Maui.Controls.Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("login", typeof(LoginPage));

            Routing.RegisterRoute("Cart", typeof(CartPage));

            Routing.RegisterRoute("BankPage", typeof(BankPage));
            Routing.RegisterRoute("addcard", typeof(Pages.Bank.AddCardPage));
            Routing.RegisterRoute("topup", typeof(Pages.Bank.TopUpPage));

            Routing.RegisterRoute("ShopPage", typeof(ShopPage));

            Routing.RegisterRoute("Stats", typeof(StatsPage));

            Routing.RegisterRoute("ProfilePage", typeof(ProfilePage));
        }
    }
}
