using my_economy_api.Services.Interfaces;
using Supabase;

namespace my_economy_api.Services
{
    public class SupabaseAuthService : IAuthService
    {
        private readonly Client _supabaseClient;

        public SupabaseAuthService(Client supabaseClient)
        {
            _supabaseClient = supabaseClient;
        }

        public async Task<string?> GetUserIdAsync(string accessToken)
        {
            try
            {
                // Obtenemos el usuario directamente de Supabase usando el token
                var user = await _supabaseClient.Auth.GetUser(accessToken);
                return user?.Id; // Devuelve el UUID de Supabase
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> IsTokenValidAsync(string accessToken)
        {
            var userId = await GetUserIdAsync(accessToken);
            return userId != null;
        }
    }
    
}
