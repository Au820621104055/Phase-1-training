using Day12api.Model;
using Day12api.Repo;
using Day12api.Services;
using System.Security.Cryptography;
using System.Text;

namespace Day12api
{
    public class UserService: IUserService
    {
        private readonly IUserRepo _repo;

        public UserService(IUserRepo repo)
        {
            _repo = repo;
        }

        public List<UserDTO> GetAllUsers()
        {
            return _repo.GetAllUsers();
        }

        public void AddUser(UserDTO user)
        {
            user.Password = GetHashPassword(user.Password);
            _repo.AddUser(user);
        }

        public UserDTO? ValidateUser(string username, string password)
        {
            string hashed = GetHashPassword(password);
            return _repo.GetUserByNameAndPassword(username, hashed);
        }

        private string GetHashPassword(string password)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] passBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = sha.ComputeHash(passBytes);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }
    }
}
