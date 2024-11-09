using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserOrder.Domain.Common.Responses;

namespace UserOrder.Application.Responses
{
    public class AuthResponseDto
    {
        public int UserID { get; set; }
        public TokenDto Tokens { get; set; }
    }
}
