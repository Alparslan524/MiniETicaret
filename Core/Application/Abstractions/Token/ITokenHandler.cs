using Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TOKENs = Application.DTOs.Token;

namespace Application.Abstractions.Token
{
    public interface ITokenHandler
    {
        TOKENs CreateAccessToken(int second, AppUser appUser);
        string CreateRefreshToken();
    }
}
