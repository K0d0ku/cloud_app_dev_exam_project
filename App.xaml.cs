using cloud_app_dev_exam_project.Pages;
using cloud_app_dev_exam_project.Services;
using cloud_app_dev_exam_project.Shell;

namespace cloud_app_dev_exam_project
{
    public partial class App : Application
    {
        public static Services.DatabaseService DbService { get; private set; }
        public static string DatabasePath { get; private set; }
        public static Services.BankService BankService { get; private set; }


        // to track the currently logged-in session
        public static object CurrentUser { get; set; } = null;
        public static string CurrentUserRole { get; set; } = string.Empty;
        public static PurchaseService? PurchaseService { get; internal set; }

        public App()
        {
            InitializeComponent();

            DatabasePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "appdata.db3"
            );
            DbService = new Services.DatabaseService(DatabasePath);
            AppServices.Database = DbService;

            /*MainPage = new NavigationPage(new LoginPage());
            InitializeAppAsync();*/

            SeedInitialData();
            if (CurrentUser != null && !string.IsNullOrEmpty(CurrentUserRole))
            {
                switch (CurrentUserRole)
                {
                    case "Customer":
                        MainPage = new CustomerShell();
                        break;
                    case "Seller":
                        MainPage = new SellerShell(); 
                        break;
                    case "Supplier":
                        MainPage = new SupplierShell();
                        break;
                    default:
                        MainPage = new AppShell();
                        break;
                }
            }
            else
            {
                MainPage = new NavigationPage(new LoginPage());
            }
        }

        public static class AppServices
        {
            public static DatabaseService Database { get; set; }
        }
/*        private async void InitializeAppAsync()
        {
            await DbService.GetConnectionAsync();
            await DbService.SeedLocationsAsync();
        }
*/

        private async void SeedInitialData()
        {
            await DbService.SeedLocationsAsync();
        }


    }
}
