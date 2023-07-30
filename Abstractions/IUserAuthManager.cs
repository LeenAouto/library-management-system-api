﻿
using Entities.AuthModels;

namespace Abstractions
{
    public interface IUserAuthManager
    {
        Task<AuthModel> RegisterAsync(RegisterModel model); //sign up
        Task<AuthModel> GetTokenAsync(TokenRequestModel model); //sign in
        Task<string> AddRoleAsync(AddRoleModel model); //adding a user to a role
        Task<bool> UserExists(string userId);
    }
}
