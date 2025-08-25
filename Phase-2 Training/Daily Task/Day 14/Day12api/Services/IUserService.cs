using Day12api.Model;

namespace Day12api.Services
{
    public interface IUserService
    {
        List<UserDTO> GetAllUsers();
        void AddUser(UserDTO user);
        UserDTO? ValidateUser(string username, string password);
    }
}
