using Day12api.Model;

namespace Day12api.Repo
{
    public interface IUserRepo
    {
        List<UserDTO> GetAllUsers();
        void AddUser(UserDTO user);
        UserDTO GetUserByNameAndPassword(string username, string hashedPassword);
    }
}
