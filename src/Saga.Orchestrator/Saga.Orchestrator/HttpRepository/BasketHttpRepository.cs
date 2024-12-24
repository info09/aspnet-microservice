using Saga.Orchestrator.HttpRepository.Interfaces;
using Shared.Dtos.Basket;

namespace Saga.Orchestrator.HttpRepository
{
    public class BasketHttpRepository : IBasketHttpRepository
    {
        private readonly HttpClient _httpClient;

        public BasketHttpRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> DeleteBasket(string userName)
        {
            var response = await _httpClient.DeleteAsync($"baskets/{userName}");
            if (!response.EnsureSuccessStatusCode().IsSuccessStatusCode)
                throw new Exception($"Delete basket for userName: {userName} not success");

            return response.IsSuccessStatusCode;
        }

        public async Task<CartDto?> GetBasket(string userName)
        {
            var cart = await _httpClient.GetFromJsonAsync<CartDto>($"baskets/{userName}");
            if (cart == null || !cart.Items.Any()) return null;

            return cart;
        }
    }
}
