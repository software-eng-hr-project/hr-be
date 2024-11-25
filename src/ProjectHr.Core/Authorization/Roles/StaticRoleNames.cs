namespace ProjectHr.Authorization.Roles
{
    public static class StaticRoleNames
    {
        public static class Host
        {
            public const string Admin = "Admin";
        }

        public static class Tenants
        {
            public const string Admin = "Hesap Sahibi";
            public const string Manager = "Yönetici";
            public const string Employee = "Çalışan";
        } 
        public static class DisplayNames
        {
            public const string Admin = "Hesap Sahibi";
            public const string Manager = "Yönetici";
            public const string Employee = "Çalışan";
        }
    }
}
