using FoodOrdering.API.Models;

namespace FoodOrdering.API.Services
{
    public interface ITokenService
    {
        string GenerateToken(ApplicationUser user, string role);
    }
}