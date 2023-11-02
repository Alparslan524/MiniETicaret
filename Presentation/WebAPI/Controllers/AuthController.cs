using Application.Features.Commands.AppUser.GoogleLoginUser;
using Application.Features.Commands.AppUser.LoginUser;
using Application.Features.Commands.AppUser.RefreshTokenLogin;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login(LoginUserCommandRequest request)
        {
            LoginUserCommandResponse response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> GoogleLogin(GoogleLoginUserCommanRequest request)
        {
            GoogleLoginUserCommanResponse response = await _mediator.Send(request);
            return Ok(response);
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> RefreshTokenLogin([FromBody] RefreshTokenLoginCommandRequest request)
        {
            RefreshTokenLoginCommandResponse response = await _mediator.Send(request);
            return Ok(response);
        }
    }
}
