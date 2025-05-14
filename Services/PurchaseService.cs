using cloud_app_dev_exam_project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloud_app_dev_exam_project.Services
{
    public class PurchaseService
    {
        private readonly DatabaseService _db;
        private readonly BankService _bank;
        //private readonly BankService _bankService;
        //private readonly DatabaseService _dbService;

        public PurchaseService(DatabaseService db, BankService bank)
        {
            _db = db;
            _bank = bank;
        }

        public async Task<bool> BuySingleItemAsync(int itemId, int buyerUserId, int buyerCardId)
        {
            var item = await _db.GetListableItemById(itemId);
            if (item == null || item.AvailableAmount < 1)
                return false;

            var buyerCard = await _bank.GetCardByIdAsync(buyerCardId);
            if (buyerCard == null || buyerCard.UserId != buyerUserId || buyerCard.Balance < item.Price)
                return false;

            if (!int.TryParse(item.ListedByUserId, out int sellerUserId))
                return false;

            var sellerCards = await _bank.GetCardsForUserAsync(sellerUserId);
            if (sellerCards.Count == 0)
                return false;

            var sellerCard = sellerCards.First();

            bool success = _bank.TransferFundsTransactional(buyerCard.Id, sellerCard.Id, item.Price);
            if (!success)
                return false;

            item.AvailableAmount -= 1;
            await _db.UpdateAsync(item);

            var purchase = new PurchaseHistory
            {
                UserId = buyerUserId,
                ItemId = item.ItemId,
                Price = item.Price,
                PurchaseDate = DateTime.UtcNow
            };

            await _db.AddPurchaseHistoryAsync(purchase);

            return true;
        }


        public async Task<PurchaseResult> BuyCartItemsAsync(int userId)
        {
            var cards = await _bank.GetCardsForUserAsync(userId);
            if (cards == null || !cards.Any())
                return new PurchaseResult { Success = false, Message = "No bank card available." };

            var card = cards.First();
            var cartItems = await _db.GetCartItemsAsync(userId);
            var failedItems = new List<PurchaseErrorDetail>();
            decimal totalCost = 0;

            foreach (var cartItem in cartItems)
            {
                var item = await _db.GetListableItemById(cartItem.ListableItemId);
                if (item == null || item.AvailableAmount < cartItem.Quantity)
                {
                    failedItems.Add(new PurchaseErrorDetail
                    {
                        ItemId = cartItem.ListableItemId,
                        Reason = "Not enough stock."
                    });
                    continue;
                }

                totalCost += item.Price * cartItem.Quantity;
            }

            if (card.Balance < totalCost)
                return new PurchaseResult { Success = false, Message = "Insufficient funds." };

            foreach (var cartItem in cartItems)
            {
                var item = await _db.GetListableItemById(cartItem.ListableItemId);
                if (item.AvailableAmount >= cartItem.Quantity)
                {
                    item.AvailableAmount -= cartItem.Quantity;
                    await _db.UpdateAsync(item);
                }
            }

            card.Balance -= totalCost;
            await _db.UpdateAsync(card);

            return new PurchaseResult
            {
                Success = failedItems.Count == 0,
                Message = failedItems.Count == 0 ? "Purchase successful" : "Some items failed",
                FailedItems = failedItems
            };
        }
    }
}
