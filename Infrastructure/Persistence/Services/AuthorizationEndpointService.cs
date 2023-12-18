using Application.Abstractions.Services;
using Application.Abstractions.Services.Configurations;
using Application.Repositories.EntityRepository.EndpointRepository;
using Application.Repositories.EntityRepository.MenuRepository;
using Domain.Entities;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Services
{
    public class AuthorizationEndpointService : IAuthorizationEndpointService
    {
        readonly IApplicationService _applicationService;
        readonly IEndpointReadRepository _endpointReadRepository;
        readonly IEndpointWriteRepository _endpointWriteRepository;
        readonly IMenuReadRepository _menuReadRepository;
        readonly IMenuWriteRepository _menuWriteRepository;
        readonly RoleManager<AppRole> _roleManager;
        public AuthorizationEndpointService(IEndpointReadRepository endpointReadRepository, IApplicationService applicationService, IEndpointWriteRepository endpointWriteRepository, IMenuWriteRepository menuWriteRepository, IMenuReadRepository menuReadRepository, RoleManager<AppRole> roleManager)
        {
            _endpointReadRepository = endpointReadRepository;
            _applicationService = applicationService;
            _endpointWriteRepository = endpointWriteRepository;
            _menuWriteRepository = menuWriteRepository;
            _menuReadRepository = menuReadRepository;
            _roleManager = roleManager;
        }

        public async Task AssignRoleEndpointAsync(string[] roles, string menu, string code, Type type)
        {
            Menu _menu = await _menuReadRepository.GetSingleAsync(m => m.Name == menu);
            if (_menu == null)
            {
                _menu = new()
                {
                    Name = menu,
                };

                await _menuWriteRepository.AddAsync(_menu);
                await _endpointWriteRepository.SaveAsync();
            }


            Endpoint? endpoint = await _endpointReadRepository.Table.Include(e => e.Menu).Include(e => e.Roles)
                .FirstOrDefaultAsync(e => e.Code == code && e.Menu.Name == menu);

            if (endpoint == null)
            {
                var action = _applicationService.GetAuthorizeDefinitionEndpoints(type)
                    .FirstOrDefault(m => m.Name == menu)
                    ?.Actions.FirstOrDefault(e => e.Code == code);

                endpoint = new()
                {
                    Code = action.Code,
                    ActionType = action.ActionType,
                    HttpType = action.HttpType,
                    Definition = action.Definition,
                    Menu = _menu
                };

                await _endpointWriteRepository.AddAsync(endpoint);
                await _endpointWriteRepository.SaveAsync();
            }

            foreach (var role in endpoint.Roles)
            {
                endpoint.Roles.Remove(role);
            }



            var appRoles = await _roleManager.Roles.Where(r => roles.Contains(r.Name)).ToListAsync();

            foreach (var role in appRoles)
            {
                endpoint.Roles.Add(role);
            }

            await _endpointWriteRepository.SaveAsync();
        }

        public async Task<List<string>> GetRolesToEndpoint(string code, string menu)
        {
            Endpoint? endpoint = await _endpointReadRepository.Table
                .Include(e => e.Roles)
                .Include(e => e.Menu)
                .FirstOrDefaultAsync(e => e.Code == code && e.Menu.Name == menu);
            if (endpoint != null)
                return endpoint.Roles.Select(r => r.Name).ToList();

            return null;
        }
    }
}
