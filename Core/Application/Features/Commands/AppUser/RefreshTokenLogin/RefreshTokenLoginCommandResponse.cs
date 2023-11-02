using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Commands.AppUser.RefreshTokenLogin
{
    public class RefreshTokenLoginCommandResponse
    {
        public Token token { get; set; }
    }
}
