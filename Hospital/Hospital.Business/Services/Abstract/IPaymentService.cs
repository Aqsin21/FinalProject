using Hospital.Business.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Business.Services.Abstract
{
    public interface IPaymentService
    {
        // Test ödənişi yaratmaq üçün metod
        Task<string> CreateTestPaymentAsync(decimal amount, string description);
        Task<bool> CheckPaymentAsync(PaymentCheckDto dto);
    }
}
