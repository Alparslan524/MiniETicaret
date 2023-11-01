using Application.Abstractions.Token;
using Application.DTOs;
using Domain.Entities.Identity;
using Google.Apis.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APPUser = Domain.Entities.Identity.AppUser;

namespace Application.Features.Commands.AppUser.GoogleLoginUser
{
    public class GoogleLoginUserCommanHandler : IRequestHandler<GoogleLoginUserCommanRequest, GoogleLoginUserCommanResponse>
    {
        readonly UserManager<APPUser> _userManager;
        readonly ITokenHandler _tokenHandler;

        public GoogleLoginUserCommanHandler(UserManager<APPUser> userManager, ITokenHandler tokenHandler)
        {
            _userManager = userManager;
            _tokenHandler = tokenHandler;
        }

        public async Task<GoogleLoginUserCommanResponse> Handle(GoogleLoginUserCommanRequest request, CancellationToken cancellationToken)
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string> { "94058850904-c256r95i1vte512d70use3mlleg0q1n6.apps.googleusercontent.com" }
            };
            var payload = await GoogleJsonWebSignature.ValidateAsync(request.IdToken, settings);

            var info = new UserLoginInfo(request.Provider, payload.Subject, request.Provider);
            APPUser user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

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

            Token token = _tokenHandler.CreateAccessToken(5);

            return new()
            {
                Token = token
            };

        }
    }
}
