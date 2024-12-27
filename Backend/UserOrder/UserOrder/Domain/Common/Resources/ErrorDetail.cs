using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserOrder.Domain.Common.Resources
{
    public class ErrorDetail
    {
        public int Code { get; set; }
        public string? Message { get; set; }
    }
}
