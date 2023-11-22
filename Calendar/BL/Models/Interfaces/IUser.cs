using BL.ForAPI.DTO;

namespace BL.Models.Interfaces
{
    public interface IUser
    {
        // возвращает bool значение, есть ли пользователь в системе
        public Task<UserData?> GetUser(UserData userData);
        public Task Registration(UserData userData);
    }
}
