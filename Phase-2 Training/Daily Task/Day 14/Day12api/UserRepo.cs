using Day12api.Context;
using Day12api.Model;
using Day12api.Repo;

namespace Day12api
{
    public class UserRepo:IUserRepo
    {
        private readonly MyAppDbContext _context;

        public UserRepo(MyAppDbContext context)
        {
            _context = context;
        }

        public List<UserDTO> GetAllUsers()
        {
            return _context.Users.ToList();
        }

        public void AddUser(UserDTO user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public UserDTO? GetUserByNameAndPassword(string username, string hashedPassword)
        {
            return _context.Users.FirstOrDefault(u => u.Username == username && u.Password == hashedPassword);
        }
    }
}
