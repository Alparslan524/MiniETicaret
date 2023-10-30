using Application.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppUsers = Domain.Entities.Identity.AppUser;

namespace Application.Features.Commands.AppUser.LoginUser
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommandRequest, LoginUserCommandResponse>
    {
        readonly UserManager<AppUsers> _userManager;
        readonly SignInManager<AppUsers> _signInManager;

        public LoginUserCommandHandler(UserManager<AppUsers> userManager, SignInManager<AppUsers> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<LoginUserCommandResponse> Handle(LoginUserCommandRequest request, CancellationToken cancellationToken)
        {
            AppUsers user = await _userManager.FindByNameAsync(request.userNameorEmail);
            if (user == null)
                user = await _userManager.FindByEmailAsync(request.userNameorEmail);

            if (user == null)
                throw new NotFoundUserException("Kullanıcı adı veya şifre hatalı...");

            SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (result.Succeeded)
            {
                //yetkiler belirlenecek
            }

            return new();
        }
    }
}
