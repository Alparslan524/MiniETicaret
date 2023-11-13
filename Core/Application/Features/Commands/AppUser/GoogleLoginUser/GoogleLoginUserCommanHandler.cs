using Application.Abstractions.Services;
using Application.Abstractions.Services.Authentication;
using Application.DTOs;
using MediatR;

namespace Application.Features.Commands.AppUser.GoogleLoginUser
{
    public class GoogleLoginUserCommanHandler : IRequestHandler<GoogleLoginUserCommanRequest, GoogleLoginUserCommanResponse>
    {
        readonly IExternalAuthentication _authService;

        public GoogleLoginUserCommanHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<GoogleLoginUserCommanResponse> Handle(GoogleLoginUserCommanRequest request, CancellationToken cancellationToken)
        {
            Token token = await _authService.GoogleLoginAsync(request.IdToken, 900);
            
            return new()
            {
                Token = token
            };

        }
    }
}
