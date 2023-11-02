using Application.Abstractions.Services.Authentication;
using Application.Abstractions.Token;
using Application.DTOs;
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
        readonly IInternalAuthentication _authService;

        public LoginUserCommandHandler(IInternalAuthentication authService)
        {
            _authService = authService;
        }

        public async Task<LoginUserCommandResponse> Handle(LoginUserCommandRequest request, CancellationToken cancellationToken)
        {
            var token = await _authService.LoginAsync(new()
            {
                userNameorEmail = request.userNameorEmail,
                Password = request.Password
            }, 15);
            return new LoginUserSuccessCommandResponse()
            {
                Token = token
            };
        }
    }
}
