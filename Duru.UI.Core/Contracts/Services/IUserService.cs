using Duru.UI.Core.Models;

namespace Duru.UI.Core.Contracts.Services;

public interface IUserService
{
    Task<User> GetUserByIdAsync(int id);
    Task<User> GetUserByEmailAsync(string email);
    Task<User> GetUserByPhoneAsync(string phoneNumber);
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<bool> CreateUserAsync(User user);
    Task<bool> UpdateUserAsync(User user);
    Task<bool> DeleteUserAsync(int id);
    Task<bool> AuthenticateUserAsync(string email, string hashedPassword);
}