using Core.Entities;
using Core.Interfaces;

namespace Core.Services
{
    public class UserService : IUserService
    {
       
         private readonly IUnitOfWork _unitOfWork;
    
        public UserService(IUnitOfWork unitOfWork)
        {
               _unitOfWork = unitOfWork;
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _unitOfWork.UserRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _unitOfWork.UserRepository.GetAllAsync();
        }


        public async Task UpdateUserAsync(User user)
        {
      
            await  _unitOfWork.UserRepository.UpdateAsync(user);
        }

        public async Task DeleteUserAsync(int id)
        {
            await  _unitOfWork.UserRepository.DeleteAsync(id);
        }

  
        

    }
}
