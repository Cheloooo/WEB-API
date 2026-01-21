using AutoMapper;
using WEB_DOMAIN.Config.Authentication;
using WEB_DOMAIN.Entity.Authentication;
using Isopoh.Cryptography.Argon2;
using WEB_UTILITY.Security.ISecurity;

namespace WEB_SERVICES.MappingProfile.Authentication
{
    public class PasswordEncryptionResolver : IValueResolver<AuthDto, Auth, string>
    {
        private readonly IRsaEncryptionService _rsaEncryptionService;
        public PasswordEncryptionResolver(IRsaEncryptionService rsaEncryptionService)
        {
            _rsaEncryptionService = rsaEncryptionService;
        }

        public string Resolve(AuthDto source, Auth destination, string destMember, ResolutionContext context)
        {
           
            string hashedPassword = Argon2.Hash(source.Password);
            return _rsaEncryptionService.Encrypt(hashedPassword);

        }
    }
}
