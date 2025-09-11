using Hospital.Business.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Business.Dtos
{
    public class PaymentCheckDto
    {
        public string Token { get; set; } = string.Empty;
        public int ReceptId { get; set; }
        public PaymentStatuses STATUS { get; set; }
    }
}
