using Microsoft.AspNetCore.Identity;

namespace SaltStackers.Application.Custom
{
    public class GlobalIdentityErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError DefaultError()
            => new IdentityError { Code = nameof(DefaultError), Description = Resources.Error.UnknownError };

        public override IdentityError ConcurrencyFailure()
            => new IdentityError { Code = nameof(ConcurrencyFailure), Description = Resources.Error.ConcurrencyFailure };

        public override IdentityError PasswordMismatch()
            => new IdentityError { Code = nameof(PasswordMismatch), Description = Resources.Error.PasswordMismatch };

        public override IdentityError InvalidToken()
            => new IdentityError { Code = nameof(InvalidToken), Description = Resources.Error.InvalidToken };

        public override IdentityError LoginAlreadyAssociated()
            => new IdentityError { Code = nameof(LoginAlreadyAssociated), Description = Resources.Error.LoginAlreadyAssociated };

        public override IdentityError InvalidUserName(string userName)
            => new IdentityError { Code = nameof(InvalidUserName), Description = string.Format(Resources.Error.InvalidUserName, userName) };

        public override IdentityError InvalidEmail(string email)
            => new IdentityError { Code = nameof(InvalidEmail), Description = string.Format(Resources.Error.InvalidEmail, email) };

        public override IdentityError DuplicateUserName(string userName)
            => new IdentityError { Code = nameof(DuplicateUserName), Description = string.Format(Resources.Error.DuplicateUserName, userName) };

        public override IdentityError DuplicateEmail(string email)
            => new IdentityError { Code = nameof(DuplicateEmail), Description = string.Format(Resources.Error.DuplicateEmail, email) };

        public override IdentityError InvalidRoleName(string role)
            => new IdentityError { Code = nameof(InvalidRoleName), Description = string.Format(Resources.Error.InvalidRoleName, role) };

        public override IdentityError DuplicateRoleName(string role)
            => new IdentityError { Code = nameof(DuplicateRoleName), Description = string.Format(Resources.Error.DuplicateRoleName, role) };

        public override IdentityError UserAlreadyHasPassword()
            => new IdentityError { Code = nameof(UserAlreadyHasPassword), Description = Resources.Error.UserAlreadyHasPassword };

        public override IdentityError UserLockoutNotEnabled()
            => new IdentityError { Code = nameof(UserLockoutNotEnabled), Description = Resources.Error.UserLockoutNotEnabled };

        public override IdentityError UserAlreadyInRole(string role)
            => new IdentityError { Code = nameof(UserAlreadyInRole), Description = string.Format(Resources.Error.UserAlreadyInRole, role) };

        public override IdentityError UserNotInRole(string role)
            => new IdentityError { Code = nameof(UserNotInRole), Description = string.Format(Resources.Error.UserNotInRole, role) };

        public override IdentityError PasswordTooShort(int length)
            => new IdentityError { Code = nameof(PasswordTooShort), Description = string.Format(Resources.Error.PasswordTooShort, length) };

        public override IdentityError PasswordRequiresNonAlphanumeric()
            => new IdentityError { Code = nameof(PasswordRequiresNonAlphanumeric), Description = Resources.Error.PasswordRequiresNonAlphanumeric };

        public override IdentityError PasswordRequiresDigit()
            => new IdentityError { Code = nameof(PasswordRequiresDigit), Description = Resources.Error.PasswordRequiresDigit };

        public override IdentityError PasswordRequiresLower()
            => new IdentityError { Code = nameof(PasswordRequiresLower), Description = Resources.Error.PasswordRequiresLower };

        public override IdentityError PasswordRequiresUpper()
            => new IdentityError { Code = nameof(PasswordRequiresUpper), Description = Resources.Error.PasswordRequiresUpper };

        public override IdentityError RecoveryCodeRedemptionFailed()
            => new IdentityError { Code = nameof(RecoveryCodeRedemptionFailed), Description = Resources.Error.RecoveryCodeRedemptionFailed };

        public override IdentityError PasswordRequiresUniqueChars(int uniqueChars)
            => new IdentityError { Code = nameof(PasswordRequiresUniqueChars), Description = string.Format(Resources.Error.PasswordRequiresUniqueChars, uniqueChars) };
    }
}
