using cloud_app_dev_exam_project.Services;
using cloud_app_dev_exam_project.Shell;

namespace cloud_app_dev_exam_project.Pages;

public partial class ProfilePage : ContentPage
{
    public ProfilePage()
    {
        InitializeComponent();
    }

    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        SessionService.LogOut();
        await AppShell.Current.GoToAsync("login");
    }
}