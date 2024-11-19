using JetBrains.Annotations;

namespace ProjectHr.Roles.Dto;

public class PermissionWithRoleDto
{
    public string Name { get; set; }
        
    public string DisplayName { get; set; }
        
    [CanBeNull] public string Description { get; set; }
        
    [CanBeNull] public string[] RoleNames { get; set; }
}