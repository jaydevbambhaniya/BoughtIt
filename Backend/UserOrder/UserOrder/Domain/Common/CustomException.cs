using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserOrder.Domain.Common
{
    public class CustomException : Exception
    {
        public int Code { get; }
        public CustomException(int code, string message) : base(message)
        {
            this.Code = code;
        }
    }
}
