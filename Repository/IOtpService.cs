using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IOtpService
    {
        void GenerateOtp(string email, out string otp);
        bool ValidateOtp(string email, string otp);
    }
}
