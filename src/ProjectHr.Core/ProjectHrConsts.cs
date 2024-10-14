using ProjectHr.Debugging;

namespace ProjectHr
{
    public class ProjectHrConsts
    {
        public const string LocalizationSourceName = "ProjectHr";

        public const string ConnectionStringName = "Default";

        public const bool MultiTenancyEnabled = true;


        /// <summary>
        /// Default pass phrase for SimpleStringCipher decrypt/encrypt operations
        /// </summary>
        public static readonly string DefaultPassPhrase =
            DebugHelper.IsDebug ? "gsKxGZ012HLL3MI5" : "84a4c87fca17446b8c8d10142e970698";
    }
}
