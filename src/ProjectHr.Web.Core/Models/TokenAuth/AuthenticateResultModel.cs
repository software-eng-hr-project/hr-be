using System.Collections.Generic;

namespace ProjectHr.Models.TokenAuth
{
    public class AuthenticateResultModel
    {
        public string AccessToken { get; set; }

        public string EncryptedAccessToken { get; set; }

        public int ExpireInSeconds { get; set; }

        public long UserId { get; set; }
        
        public List<string> GrantedPermissions { get; set; }
    }
}
