using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using WEB_DOMAIN.Entity.Authentication;
using WEB_DOMAIN.Entity.Generic;
using WEB_DOMAIN.Interface;
using WEB_SERVICES.DTO.Generic;

namespace WEB_SERVICES.Validations.Generic
{
    public class UserDtoValidator : AbstractValidator<UserDto>  
    {
        private readonly IRepository<Auth> _authRepository;
        private readonly IRepository<UserInfo> _userInfoRepository;

        public UserDtoValidator(IRepository<Auth> authRepository, IRepository<UserInfo> userInfoRepository)
        {
            _authRepository = authRepository;
            _userInfoRepository = userInfoRepository;
            RuleFor(x => x.RoleId)
                .NotEmpty().WithMessage("RoleId is required.");
            RuleFor(x => x.Auth.Username)
                .NotEmpty().WithMessage("Username is required.")
                .MustAsync(BeUniqueUsername).WithMessage("Username already exists.")
                .MaximumLength(50).WithMessage("Username cannot exceed 50 characters.");
            RuleFor(x => x.Auth.Password)
                .NotEmpty().WithMessage("Password is required.");
            RuleFor(x => x.UserInfo.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.")
                .MustAsync(BeUniqueEmail).WithMessage("Email already exists.");
        }
        private async Task<bool> BeUniqueUsername(string username, CancellationToken ct)
        {
            var existing = (await _authRepository.GetAllAsync(a => a.Username == username, ct)).Any();
            return existing;
        }

        private async Task<bool> BeUniqueEmail(string email, CancellationToken ct)
        {
            var existing = (await _userInfoRepository.GetAllAsync(u => u.Email == email, ct)).Any();
            return !existing;
        }
    }
}
