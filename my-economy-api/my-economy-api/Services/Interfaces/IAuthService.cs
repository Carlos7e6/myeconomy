namespace my_economy_api.Services.Interfaces
{
    public interface IAuthService
    {
        Task<string?> GetUserIdAsync(string accessToken);
        Task<bool> IsTokenValidAsync(string accessToken);
    }
}
