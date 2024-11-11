using SuperKicks.Data.Models;
using SuperKicks.Repo.ViewModels;

namespace SuperKicks.Repo.Repository.Interface
{
    public interface IUserManagerRepository
    {
        List<User> GetUsers();
        string ValidateCredential(string validateBy, string value);
        string ChangePassword(LoginViewModel viewModel);
        string CreateUser(UserViewModel viewModel);
        string Login(LoginViewModel viewModel);
    }
}
