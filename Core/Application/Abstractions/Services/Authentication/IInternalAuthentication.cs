using Application.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions.Services.Authentication
{
    public interface IInternalAuthentication
    {
        Task<DTOs.Token> LoginAsync(LoginUserRequest model,int accessTokenLifeTime);
    }
}
