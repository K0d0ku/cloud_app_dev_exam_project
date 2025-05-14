using cloud_app_dev_exam_project.Models;
using CloudAppDevExamProject.Models;
using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloud_app_dev_exam_project.Services
{
    public class DatabaseService
    {
        private SQLiteAsyncConnection _db;
        private readonly string _dbPath;

        public DatabaseService(string dbPath)
        {
            _dbPath = dbPath;
            _db = new SQLiteAsyncConnection(_dbPath);
            //InitAsync().Wait();
        }

        /*tables*/
        private async Task InitAsync()
        {
            if (_db == null)
                _db = new SQLiteAsyncConnection(_dbPath);

            await _db.CreateTableAsync<User>();
            await _db.CreateTableAsync<Models.Location>();
            await _db.CreateTableAsync<CustomerProfile>();
            await _db.CreateTableAsync<SellerProfile>();
            await _db.CreateTableAsync<SupplierProfile>();
            await _db.CreateTableAsync<ListableItem>();
            await _db.CreateTableAsync<BankCard>();
            await _db.CreateTableAsync<CartItem>();
            await _db.CreateTableAsync<PurchaseHistory>();

        }

        /*Locations*/
        public async Task<List<Models.Location>> GetAllLocationsAsync()
        {
            await InitAsync();
            return await _db.Table<Models.Location>().ToListAsync();
        }

        public async Task AddLocationAsync(Models.Location location)
        {
            await InitAsync();
            await _db.InsertAsync(location);
        }

        /*user*/
        public async Task<List<User>> GetAllUsersAsync()
        {
            await InitAsync();
            return await _db.Table<User>().ToListAsync();

        }

        public async Task AddUserAsync(User user)
        {
            await InitAsync();
            await _db.InsertAsync(user);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            await InitAsync();
            return await _db.Table<User>()
                            .Where(u => u.Email.ToLower() == email.ToLower())
                            .FirstOrDefaultAsync();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            await InitAsync();
            return await _db.Table<User>()
                            .Where(u => u.Id == id)
                            .FirstOrDefaultAsync();
        }

        /*location*/
        public async Task SeedLocationsAsync()
        {
            await InitAsync();

            var existingLocations = await _db.Table<Models.Location>().ToListAsync();
            if (existingLocations.Any())
                return;

            var locations = new List<Models.Location>
            {
                new Models.Location { CityName = "Almaty" },
                new Models.Location { CityName = "Astana" },
                new Models.Location { CityName = "Shymkent" },
                new Models.Location { CityName = "Aktobe" },
                new Models.Location { CityName = "Karaganda" },
                new Models.Location { CityName = "Taraz" },
                new Models.Location { CityName = "Pavlodar" },
                new Models.Location { CityName = "Kostanay" },
                new Models.Location { CityName = "Kyzylorda" },
                new Models.Location { CityName = "Uralsk" },
                new Models.Location { CityName = "Semey" },
                new Models.Location { CityName = "Turkistan" },
                new Models.Location { CityName = "Atyrau" },
                new Models.Location { CityName = "Petropavl" }
            };

            await _db.InsertAllAsync(locations);
        }
           
        /*litableitem*/
        public async Task<int> AddListableItemAsync(ListableItem item)
        {
            await InitAsync();

            item.ImagePathsSerialized = JsonConvert.SerializeObject(item.ImagePaths);
            item.AvailableLocationsSerialized = JsonConvert.SerializeObject(item.AvailableLocations);

            return await _db.InsertAsync(item);
        }

        public async Task<List<ListableItem>> GetAllListableItemsAsync()
        {
            await InitAsync();

            var items = await _db.Table<ListableItem>().ToListAsync();

            foreach (var item in items)
            {
                item.ImagePaths = !string.IsNullOrEmpty(item.ImagePathsSerialized)
                    ? JsonConvert.DeserializeObject<List<string>>(item.ImagePathsSerialized)
                    : new List<string>();

                item.AvailableLocations = !string.IsNullOrEmpty(item.AvailableLocationsSerialized)
                    ? JsonConvert.DeserializeObject<List<string>>(item.AvailableLocationsSerialized)
                    : new List<string>();
            }

            return items;
        }

        public async Task<ListableItem> GetListableItemById(int itemId)
        {
            await InitAsync();
            return await _db.Table<ListableItem>().FirstOrDefaultAsync(item => item.ItemId == itemId);
        }


        public async Task<SQLiteAsyncConnection> GetConnectionAsync()
        {
            await InitAsync();
            return _db;
        }

        /*cart*/
        public async Task AddToCartAsync(int userId, int itemId, int quantity)
        {
            await InitAsync();
            var existing = await _db.Table<CartItem>()
                .FirstOrDefaultAsync(c => c.UserId == userId && c.ListableItemId == itemId);

            if (existing != null)
            {
                existing.Quantity += quantity;
                await _db.UpdateAsync(existing);
            }
            else
            {
                var newItem = new CartItem
                {
                    UserId = userId,
                    ListableItemId = itemId,
                    Quantity = quantity
                };
                await _db.InsertAsync(newItem);
            }
        }

        public async Task<List<CartItem>> GetCartItemsAsync(int userId)
        {
            await InitAsync();
            return await _db.Table<CartItem>().Where(c => c.UserId == userId).ToListAsync();
        }

        public async Task RemoveFromCartAsync(int cartItemId)
        {
            await InitAsync();
            var item = await _db.Table<CartItem>().FirstOrDefaultAsync(c => c.Id == cartItemId);
            if (item != null)
                await _db.DeleteAsync(item);
        }

        public async Task UpdateCartItemQuantityAsync(int cartItemId, int newQuantity)
        {
            await InitAsync();
            var item = await _db.Table<CartItem>().FirstOrDefaultAsync(c => c.Id == cartItemId);
            if (item != null)
            {
                item.Quantity = newQuantity;
                await _db.UpdateAsync(item);
            }
        }

        /*bank*/
        public async Task<List<BankCard>> GetBankCardsForUserAsync(int userId)
        {
            await InitAsync();
            return await _db.Table<BankCard>().Where(c => c.UserId == userId).ToListAsync();
        }

        public async Task<BankCard?> GetBankCardByIdAsync(int cardId)
        {
            await InitAsync();
            return await _db.Table<BankCard>().FirstOrDefaultAsync(c => c.Id == cardId);
        }

        public async Task<int> AddBankCardAsync(BankCard card)
        {
            await InitAsync();
            return await _db.InsertAsync(card);
        }

        public async Task<bool> TopUpBankCardAsync(int cardId, decimal amount)
        {
            await InitAsync();
            var card = await GetBankCardByIdAsync(cardId);
            if (card == null) return false;

            card.Balance += amount;
            await _db.UpdateAsync(card);
            return true;
        }

        public async Task<int> DeleteBankCardAsync(BankCard card)
        {
            await InitAsync();
            return await _db.DeleteAsync(card);
        }

        /*purchase*/
        public async Task<int> UpdateAsync<T>(T entity) where T : new()
        {
            await InitAsync();
            return await _db.UpdateAsync(entity);
        }

        public async Task AddPurchaseHistoryAsync(PurchaseHistory purchase)
        {
            await InitAsync();
            await _db.InsertAsync(purchase);
        }



        /*register*/
        public async Task AddCustomerProfileAsync(CustomerProfile profile)
        {
            await InitAsync();
            await _db.InsertAsync(profile);
        }

        public async Task AddSellerProfileAsync(SellerProfile sellerProfile)
        {
            await InitAsync();
            await _db.InsertAsync(sellerProfile);
        }

        public async Task AddSupplierProfileAsync(SupplierProfile supplierProfile)
        {
            await InitAsync();
            await _db.InsertAsync(supplierProfile);
        }

        public async Task<List<CustomerProfile>> GetAllCustomerProfilesAsync()
        {
            await InitAsync();
            return await _db.Table<CustomerProfile>().ToListAsync();
        }

        public async Task<List<SellerProfile>> GetAllSellerProfilesAsync()
        {
            await InitAsync();
            return await _db.Table<SellerProfile>().ToListAsync();
        }

        public async Task<List<SupplierProfile>> GetAllSupplierProfilesAsync()
        {
            await InitAsync();
            return await _db.Table<SupplierProfile>().ToListAsync();
        }
    }
}
