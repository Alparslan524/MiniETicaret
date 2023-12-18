using Application.Features.Commands.AuthorizationEndpoint.AssignRoleEndpoint;
using Application.Features.Queries.AuthorizationEndpoint.GetRolesToEndpoints;
using Application.Features.Queries.Role.GetRoles;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationEndpointsController : ControllerBase
    {
        readonly IMediator _mediator;

        public AuthorizationEndpointsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> AssingRoleEndpoint(AssignRoleEndpointCommandRequest request)
        {
            request.Type = typeof(Program);
            AssignRoleEndpointCommandResponse response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpPost("get-roles-to-endpoint")]
        public async Task<IActionResult> GetRolesToEndpoints(GetRolesToEndpointQueryRequest request)
        {
            GetRolesToEndpointQueryResponse response = await _mediator.Send(request);
            return Ok(response);
        }
    }
}
