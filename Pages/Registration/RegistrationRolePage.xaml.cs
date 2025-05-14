namespace cloud_app_dev_exam_project.Pages;

public partial class RegistrationRolePage : ContentPage
{
    public RegistrationRolePage()
    {
        InitializeComponent();
    }

    private async void OnCustomerClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CustomerRegistrationPage());
    }

    private async void OnSellerClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SellerRegistrationPage());
    }

    private async void OnSupplierClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SupplierRegistrationPage());
    }
}
