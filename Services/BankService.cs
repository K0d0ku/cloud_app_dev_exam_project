using cloud_app_dev_exam_project.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cloud_app_dev_exam_project.Services
{
    public class BankService
    {
        private readonly SQLiteAsyncConnection _db;
        private readonly SQLiteConnection _syncDb;


        public BankService(string dbPath)
        {
            _db = new SQLiteAsyncConnection(dbPath);
            _db.CreateTableAsync<BankCard>().Wait();

            _syncDb = new SQLiteConnection(dbPath);
            _syncDb.CreateTable<BankCard>(); 
        }

        public Task<List<BankCard>> GetCardsForUserAsync(int userId)
        {
            return _db.Table<BankCard>().Where(c => c.UserId == userId).ToListAsync();
        }

        public Task<BankCard?> GetCardByIdAsync(int cardId)
        {
            return _db.Table<BankCard>().FirstOrDefaultAsync(c => c.Id == cardId);
        }

        public Task<int> AddCardAsync(BankCard card)
        {
            return _db.InsertAsync(card);
        }

        public async Task<bool> TopUpCardAsync(int cardId, decimal amount)
        {
            var card = await GetCardByIdAsync(cardId);
            if (card == null) return false;

            card.Balance += amount;
            await _db.UpdateAsync(card);
            return true;
        }

        public Task<int> DeleteCardAsync(BankCard card)
        {
            return _db.DeleteAsync(card);
        }

        public bool TransferFundsTransactional(int fromCardId, int toCardId, decimal amount)
        {
            try
            {
                _syncDb.RunInTransaction(() =>
                {
                    var fromCard = _syncDb.Table<BankCard>().FirstOrDefault(c => c.Id == fromCardId);
                    var toCard = _syncDb.Table<BankCard>().FirstOrDefault(c => c.Id == toCardId);

                    if (fromCard == null || toCard == null || fromCard.Balance < amount)
                        throw new Exception("Invalid cards or insufficient funds.");

                    fromCard.Balance -= amount;
                    toCard.Balance += amount;

                    _syncDb.Update(fromCard);
                    _syncDb.Update(toCard);
                });

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[BankService] Transfer failed: {ex.Message}");
                return false;
            }
        }
    }
}
