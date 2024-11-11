using SuperKicks.Data.Models;
using SuperKicks.Repo.ViewModels;

namespace SuperKicks.Repo.Repository.Interface
{
    public interface IRoleAndUserRoles
    {
        List<Role> GetRoles();
        string AddUpdRole(RoleViewModel viewModel);
        string ActiveInactiveRole(bool flag, Guid id);
        string AssignUnAssignRolesToUser(UserRoleViewModel viewModel);
    }
}
