using Moz.Bus.Dtos;
using Moz.Bus.Dtos.Members;

namespace Moz.Bus.Services.Members
{
    public interface IRegistrationService
    {
        PublicResult<RegistrationResult> Register(ExternalRegistrationDto request); 
    }
}