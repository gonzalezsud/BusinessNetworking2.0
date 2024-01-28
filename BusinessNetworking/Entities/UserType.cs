namespace BusinessNetworking.Entities
{
    public class UserType
    {
#pragma warning disable CS0649 // El campo 'UserType.ExpertUser' nunca se asigna y siempre tendrá el valor predeterminado 0
        internal static readonly int ExpertUser;
#pragma warning restore CS0649 // El campo 'UserType.ExpertUser' nunca se asigna y siempre tendrá el valor predeterminado 0

        public int UserTypeId { get; set; }
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
        public string TypeName { get; set; }
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
        public int IdentityIdentity { get; set; }
        public int IdentitySeed { get; set; }
        public int IdentityIncrement { get; set; }
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
        public List<ClientUser> Clients { get; set; }
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
        public List<ExpertUser> Experts { get; set; }
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    }
}




