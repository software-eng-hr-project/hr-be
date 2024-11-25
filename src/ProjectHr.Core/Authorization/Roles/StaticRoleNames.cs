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
            public const string Admin = "Admin";
            public const string Manager = "Manager";
            public const string Employee = "Employee";
        } 
        public static class DisplayNames
        {
            public const string Admin = "Hesap Sahibi";
            public const string Manager = "Yönetici";
            public const string Employee = "Çalışan";
        }
    }
}
