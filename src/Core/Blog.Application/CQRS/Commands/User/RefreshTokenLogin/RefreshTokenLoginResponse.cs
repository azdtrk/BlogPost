using Blog.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.CQRS.Commands.User.RefreshTokenLogin
{
    public class RefreshTokenLoginResponse
    {
        public TokenDto Token { get; set; }
    }
}
