using BL.ForDA.DTO;

namespace BL.ForDA.Interfaces
{
    public interface IUserRepository
    {
        public Task<UserData?> GetUser(UserData user);
        public Task CreateUser(UserData user);
    }
}
