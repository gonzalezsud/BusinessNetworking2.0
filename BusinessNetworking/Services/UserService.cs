using BusinessNetworking.Entities;
using Dapper;
using Microsoft.IdentityModel.Tokens;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BusinessNetworking.Services
{
    public interface IUserService
    {
        Task<int> RegisterUser(ClientUser user);
        Task<string> AuthenticateUser(string email, string password);
        Task<ClientUser> GetUserByTypeId(int TypeId);
        Task<ClientUser> GetUserByUserId(int UserId);
        Task<ClientUser> GetUserByUserIdAndTypeId(int UserId, int TypeId);
        Task<ExpertUser> GetExpertByUserId(int UserId);
    }

    public class UserService : IUserService
    {
        private readonly string _connectionString;
        private readonly IConfiguration _configuration;

#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
        public UserService(IConfiguration configuration)
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
        {
#pragma warning disable CS8601 // Posible asignación de referencia nula
            _connectionString = configuration.GetConnectionString("NetworkingConnection");
#pragma warning restore CS8601 // Posible asignación de referencia nula
            _configuration = configuration;
        }

        public async Task<int> RegisterUser(ClientUser user)
        {
            const string sql = @"INSERT INTO dbo.Client (UserName, Name, LastName, Email, PhoneNumber, Password, TermsAccepted, CreatedDate)
                            VALUES (@UserName, @Name, @LastName, @Email, @PhoneNumber, @Password, @TermsAccepted, @CreatedDate);
                            SELECT CAST(SCOPE_IDENTITY() as int)";

            using (var connection = new SqlConnection(_connectionString))
            {
                user.CreatedDate = DateTime.UtcNow;
                var userId = await connection.ExecuteScalarAsync<int>(sql, user);
                return userId;
            }
        }

        public async Task<string> AuthenticateUser(string email, string password)
        {
            var user = await GetUserByEmail(email);
            if (user != null && password == user.Password)
            {
                return GenerateJwtToken(user);
            }
#pragma warning disable CS8603 // Posible tipo de valor devuelto de referencia nulo
            return null;
#pragma warning restore CS8603 // Posible tipo de valor devuelto de referencia nulo
        }

        private async Task<ClientUser> GetUserByEmail(string email)
        {
            const string sql = "SELECT * FROM dbo.Client WHERE Email = @Email";
            using (var connection = new SqlConnection(_connectionString))
            {
#pragma warning disable CS8603 // Posible tipo de valor devuelto de referencia nulo
                return await connection.QuerySingleOrDefaultAsync<ClientUser>(sql, new { Email = email });
#pragma warning restore CS8603 // Posible tipo de valor devuelto de referencia nulo
            }
        }

        public async Task<ClientUser> GetUserByUserId(int UserId)
        {
            const string sql = "SELECT * FROM dbo.Users WHERE UserId = @UserId";
            using (var connection = new SqlConnection(_connectionString))
            {
#pragma warning disable CS8603 // Posible tipo de valor devuelto de referencia nulo
                return await connection.QuerySingleOrDefaultAsync<ClientUser>(sql, new { Email = UserId });
#pragma warning restore CS8603 // Posible tipo de valor devuelto de referencia nulo
            }
        }

        public async Task<ClientUser> GetUserByTypeId(int TypeId)
        {
            const string sql = "SELECT * FROM dbo.Users WHERE TypeId = @TypeId";
            using (var connection = new SqlConnection(_connectionString))
            {
#pragma warning disable CS8603 // Posible tipo de valor devuelto de referencia nulo
                return await connection.QuerySingleOrDefaultAsync<ClientUser>(sql, new { Email = TypeId });
#pragma warning restore CS8603 // Posible tipo de valor devuelto de referencia nulo
            }
        }

        public async Task<ClientUser> GetUserByUserIdAndTypeId(int UserId, int TypeId)
        {
            const string sql = "SELECT * FROM dbo.Users WHERE UserId = @UserId and TypeId = @TypeId";
            using (var connection = new SqlConnection(_connectionString))
            {
#pragma warning disable CS8603 // Posible tipo de valor devuelto de referencia nulo
                return await connection.QuerySingleOrDefaultAsync<ClientUser>(sql, new { UserId = UserId, TypeId = TypeId });
#pragma warning restore CS8603 // Posible tipo de valor devuelto de referencia nulo
            }
        }

        public async Task<ExpertUser> GetExpertByUserId(int UserId)
        {
            int TypeId = (int)UserType.ExpertUser;

            const string sql = "SELECT * FROM dbo.Users WHERE UserId = @UserId and TypeId = @TypeId";
            using (var connection = new SqlConnection(_connectionString))
            {
#pragma warning disable CS8603 // Posible tipo de valor devuelto de referencia nulo
                return await connection.QuerySingleOrDefaultAsync<ExpertUser>(sql, new { UserId = UserId, TypeId = TypeId });
#pragma warning restore CS8603 // Posible tipo de valor devuelto de referencia nulo
            }
        }

        private string GenerateJwtToken(ClientUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
#pragma warning disable CS8604 // Posible argumento de referencia nulo
            var key = Encoding.ASCII.GetBytes(_configuration["JwtConfig:Secret"]);
#pragma warning restore CS8604 // Posible argumento de referencia nulo
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.Email, user.Email)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
