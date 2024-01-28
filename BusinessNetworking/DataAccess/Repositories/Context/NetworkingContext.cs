using BusinessNetworking.Entities;
using Microsoft.EntityFrameworkCore;

namespace BusinessNetworking.DataAccess.Repositories.Context
{
    public class NetworkingContext : DbContext
    {
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
        public NetworkingContext(DbContextOptions<NetworkingContext> options) : base(options)
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
        {
        }

        public DbSet<ClientUser> Users { get; set; }
    }
}
