using SaltStackers.Application.Interfaces;
using SaltStackers.Application.ViewModels.Base;
using SaltStackers.Application.ViewModels.Membership;
using Microsoft.Extensions.Options;
using OtpNet;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace SaltStackers.Application.Services
{
    public class OtpService : IOtpService
    {
        private Totp _totp;
        private readonly OtpOptions _options;

        public OtpService(IOptions<OtpOptions> options)
        {
            _options = options?.Value ?? new OtpOptions();
        }

        public string GenerateTotp(string secretKey)
        {
            CreateTotp(secretKey);
            return _totp.ComputeTotp();
        }

        public LoginCustomerTempData GetOtpModel(string otp)
        {
            return JsonSerializer.Deserialize<LoginCustomerTempData>(otp!);
        }

        public ServiceResult VerifyOtp(string secretKey, string otpCode)
        {
            CreateTotp(secretKey);

            var isTotpCodeValid = _totp.VerifyTotp(otpCode, out _);
            if (isTotpCodeValid)
            {
                return new ServiceResult(true);
            }

            return new ServiceResult(false, new List<ServiceError>
            {
                new ServiceError
                {
                    Code = "",
                    Description = "کد وارد شده معتبر نیست، لطفا کد جدیدی دریافت کنید."
                }
            });
        }

        private void CreateTotp(string secretKey)
        {
            _totp = new Totp(Encoding.UTF8.GetBytes(secretKey), _options.StepInSeconds);
        }

        public ServiceResult CanSendNewOtp(string otp)
        {
            var result = new ServiceResult { Succeeded = true, Errors = new List<ServiceError>() };
            var otpModel = GetOtpModel(otp);
            if (otpModel.ExpirationTime >= DateTime.UtcNow)
            {
                var differenceInSeconds = (int)(otpModel.ExpirationTime - DateTime.UtcNow).TotalSeconds;
                result.Succeeded = false;
                result.Errors.Add(new ServiceError
                {
                    Description = $"برای ارسال دوباره کد، لطفا {differenceInSeconds} ثانیه صبر کنید."
                });
            }
            return result;
        }

        public ServiceResult IsOtpValid(string otp)
        {
            var result = new ServiceResult { Succeeded = true, Errors = new List<ServiceError>() };
            var otpModel = GetOtpModel(otp);

            if (otpModel.ExpirationTime <= DateTime.UtcNow)
            {
                result.Succeeded = false;
                result.Errors.Add(new ServiceError { Description = "کد ارسال شده منقضی شده، لطفا کد جدیدی دریافت کنید." });
            }

            return result;
        }
    }
}
