using System;
using cloud_app_dev_exam_project.Models;
using cloud_app_dev_exam_project.Services;
using static cloud_app_dev_exam_project.App;
namespace cloud_app_dev_exam_project.Pages.Bank;

public partial class AddCardPage : ContentPage
{
    private readonly DatabaseService _dbService;

    public AddCardPage()
    {
        InitializeComponent();
        _dbService = AppServices.Database;
    }

    private async void OnAddCardSubmit(object sender, EventArgs e)
    {
        int userId = SessionService.CurrentUserId; 

        if (string.IsNullOrWhiteSpace(BankNameEntry.Text) ||
            string.IsNullOrWhiteSpace(CardNumberEntry.Text) ||
            string.IsNullOrWhiteSpace(ExpiryEntry.Text) ||
            string.IsNullOrWhiteSpace(CVVEntry.Text) ||
            string.IsNullOrWhiteSpace(OwnerNameEntry.Text) ||
            string.IsNullOrWhiteSpace(InitialAmountEntry.Text) ||
            !decimal.TryParse(InitialAmountEntry.Text, out decimal initialAmount))
        {
            await DisplayAlert("Error", "Please fill in all fields correctly.", "OK");
            return;
        }

        var newCard = new BankCard
        {
            UserId = userId,
            BankName = BankNameEntry.Text,
            CardNumber = CardNumberEntry.Text,
            ExpirationDate = ExpiryEntry.Text,
            CVV = CVVEntry.Text,
            OwnerName = OwnerNameEntry.Text,
            Balance = initialAmount
        };

        await _dbService.AddBankCardAsync(newCard);

        await AppShell.Current.GoToAsync("BankPage");
    }
}