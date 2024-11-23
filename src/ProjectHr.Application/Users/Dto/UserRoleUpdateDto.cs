namespace ProjectHr.Users.Dto;

public class UserRoleUpdateDto
{
    public string[] RoleNames { get; set; }
    
    public void Normalize()
    {
        if (RoleNames == null)
        {
            RoleNames = new string[0];
        }
    }
}