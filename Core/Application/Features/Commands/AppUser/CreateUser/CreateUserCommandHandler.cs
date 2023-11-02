using Application.Abstractions.Services;
using Application.DTOs.User;
using Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppUserS = Domain.Entities.Identity.AppUser;//Çakışma olduğu için 

namespace Application.Features.Commands.AppUser.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommandRequest, CreateUserCommandResponse>
    {
        readonly IUserService _userService;

        public CreateUserCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<CreateUserCommandResponse> Handle(CreateUserCommandRequest request, CancellationToken cancellationToken)
        {

            CreateUserResponse response = await _userService.CreateAsync(new()
            {
                Email = request.Email,//Burada CreateUserCommandRequest'i CreateAsync'nin istediği tipe yani CreateUser'e (DTO) dönüştürüyoruz
                NameSurname = request.NameSurname,//ve servise öyle gönderiyoruz. 
                Password = request.Password,
                PasswordAgain = request.PasswordAgain,
                UserName = request.UserName
            });//Bize geriye CreateUserResponse (DTO) dönüyor. 

            return new()//Burada CreateUserResponse'yi geri dönüş tipimiz olan CreateUserCommandResponse'ye çeviriyoruz.
            {
                Message = response.Message,
                Succeeded = response.Succeeded
            };//(Bu şekilde dtodan CQRS request-response'sine eçvirmemizin nedeni DTO lar katmanlar arası entity taşımaya yarıyor.)
        }
    }
}
