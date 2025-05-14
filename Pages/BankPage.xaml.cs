using cloud_app_dev_exam_project.Pages.Bank;
using cloud_app_dev_exam_project.Services;
using cloud_app_dev_exam_project.Models;
using cloud_app_dev_exam_project.Shell;
using static cloud_app_dev_exam_project.App;

namespace cloud_app_dev_exam_project.Pages
{
    public partial class BankPage : ContentPage
    {
        private bool cardExists = false; 
        private DatabaseService _dbService;

        public BankPage()
        {
            InitializeComponent();
            _dbService = AppServices.Database;
            LoadCards();
        }

        private async void LoadCards()
        {
            var userId = SessionService.CurrentUserId;
            var cards = await _dbService.GetBankCardsForUserAsync(userId);
            NoCardLabel.IsVisible = !cards.Any();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadBankCards();
        }

        private async Task LoadBankCards()
        {
            var userId = SessionService.CurrentUserId;
            var cards = await _dbService.GetBankCardsForUserAsync(userId);

            CardCollection.ItemsSource = cards;
            CardCollection.IsVisible = cards.Any();
            NoCardLabel.IsVisible = !cards.Any();
        }

        private async void OnDeleteCardClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var card = button?.CommandParameter as BankCard;

            if (card == null)
                return;

            bool confirmed = await DisplayAlert("Confirm", $"Delete card ending in {card.CardNumber}?", "Yes", "No");
            if (!confirmed)
                return;

            await _dbService.DeleteBankCardAsync(card);
            await LoadBankCards();
        }

        private async void OnAddCardClicked(object sender, EventArgs e)
        {
            await SupplierShell.Current.GoToAsync("addcard");
        }

        private async void OnTopUpClicked(object sender, EventArgs e)
        {
            await SupplierShell.Current.GoToAsync("topup");
        }
    }
}