using cloud_app_dev_exam_project.Models;
using cloud_app_dev_exam_project.Services;
using static cloud_app_dev_exam_project.App;

namespace cloud_app_dev_exam_project.Pages.Bank;

public partial class TopUpPage : ContentPage
{
    private readonly DatabaseService _dbService;

    public TopUpPage()
    {
        InitializeComponent();
        _dbService = AppServices.Database;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadUserCards();
    }

    private async Task LoadUserCards()
    {
        var userId = SessionService.CurrentUserId;
        var cards = await _dbService.GetBankCardsForUserAsync(userId);
        CardPicker.ItemsSource = cards;
    }

    private async void OnTopUpClicked(object sender, EventArgs e)
    {
        if (CardPicker.SelectedItem is not BankCard selectedCard)
        {
            ShowMessage("Please select a card.");
            return;
        }

        if (!decimal.TryParse(AmountEntry.Text, out decimal amount) || amount <= 0)
        {
            ShowMessage("Please enter a valid top-up amount.");
            return;
        }

        bool success = await _dbService.TopUpBankCardAsync(selectedCard.Id, amount);
        if (success)
        {
            await DisplayAlert("Success", $"Card topped up by {amount:C}.", "OK");
            await AppShell.Current.GoToAsync("BankPage");
        }
        else
        {
            ShowMessage("Top-up failed. Try again.");
        }
    }

    private void ShowMessage(string message)
    {
        MessageLabel.Text = message;
        MessageLabel.IsVisible = true;
    }
}