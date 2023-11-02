using Application.Abstractions.Services;
using Application.Abstractions.Token;
using Application.DTOs;
using Application.DTOs.User;
using Application.Exceptions;
using Application.Features.Commands.AppUser.LoginUser;
using Azure.Core;
using Domain.Entities.Identity;
using Google.Apis.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Services
{
    public class AuthService : IAuthService
    {
        readonly UserManager<AppUser> _userManager;
        readonly ITokenHandler _tokenHandler;
        readonly IConfiguration _configuration;
        readonly SignInManager<AppUser> _signInManager;
        readonly IUserService _userService;

        public AuthService(UserManager<AppUser> userManager, ITokenHandler tokenHandler, IConfiguration configuration, SignInManager<AppUser> sıgnInManager, IUserService userService)
        {
            _userManager = userManager;
            _tokenHandler = tokenHandler;
            _configuration = configuration;
            _signInManager = sıgnInManager;
            _userService = userService;
        }

        public async Task<Token> GoogleLoginAsync(string idToken, int accessTokenLifeTime)
        {

            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string> { _configuration["ExternalLoginSettings:Google:Client_ID"] }
            };
            var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);

            var info = new UserLoginInfo("GOOGLE", payload.Subject, "GOOGLE");
            AppUser user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

            bool result = user != null;
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(payload.Email);
                if (user == null)
                {
                    user = new()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Email = payload.Email,
                        UserName = payload.Email,
                        NameSurname = payload.Name
                    };
                    var identityResult = await _userManager.CreateAsync(user);
                    result = identityResult.Succeeded;
                }
            }

            if (result)
                await _userManager.AddLoginAsync(user, info);//AspNetUserLogins tablosu
            else
                throw new Exception("Invalid external authentication");

            Token token = _tokenHandler.CreateAccessToken(accessTokenLifeTime);
            await _userService.UpdateRefreshToken(token.RefreshToken, user, token.Expiration, 5);
            return token;
        }

        public async Task<Token> LoginAsync(LoginUserRequest model, int accessTokenLifeTime)
        {
            AppUser user = await _userManager.FindByNameAsync(model.userNameorEmail);
            if (user == null)
                user = await _userManager.FindByEmailAsync(model.userNameorEmail);
            if (user == null)
                throw new NotFoundUserException();

            SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (result.Succeeded)
            {
                Token token = _tokenHandler.CreateAccessToken(accessTokenLifeTime);
                await _userService.UpdateRefreshToken(token.RefreshToken, user, token.Expiration, 15);
                return token;
            }

            throw new AuthenticationErrorException();
        }

        public async Task<Token> RefreshTokenLoginAsync(string refreshToken)
        {
            AppUser? user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
            if (user != null && user?.RefreshTokenEndDate > DateTime.UtcNow)
            {
                Token token = _tokenHandler.CreateAccessToken(15);
                await _userService.UpdateRefreshToken(token.RefreshToken, user, token.Expiration, 15);
                return token;
            }
            else
                throw new NotFoundUserException();
        }
    }
}
