using BL.Models.Interfaces;
using BL.ForDA.Interfaces;
using BL.ForAPI.DTO;
using BL.Converters;

namespace BL.Models.Implementations
{
    public class User : IUser
    {
        private IUserRepository userRepository;
        public User(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<UserData?> GetUser(UserData userData)
        {
            return UserConverter.ConvertFromDAToAPI(await userRepository.GetUser(UserConverter.ConvertFromAPIToDA(userData)));
        }

        public async Task Registration(UserData userData)
        {
            await userRepository.CreateUser(UserConverter.ConvertFromAPIToDA(userData));
        }
    }
}
