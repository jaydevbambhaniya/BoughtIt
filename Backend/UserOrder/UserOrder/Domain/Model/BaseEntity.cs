using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserOrder.Domain.Model
{
    public class BaseEntity
    {
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public BaseEntity() { 
            CreatedDate = CreatedDate ?? DateTime.Now;
        }
    }
}
