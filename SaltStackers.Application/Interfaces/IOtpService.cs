using SaltStackers.Application.ViewModels.Base;
using SaltStackers.Application.ViewModels.Membership;

namespace SaltStackers.Application.Interfaces
{
    public interface IOtpService
    {
        string GenerateTotp(string secretKey);

        LoginCustomerTempData GetOtpModel(string otp);

        ServiceResult VerifyOtp(string secretKey, string otpCode);

        ServiceResult CanSendNewOtp(string otp);

        ServiceResult IsOtpValid(string otp);
    }
}
