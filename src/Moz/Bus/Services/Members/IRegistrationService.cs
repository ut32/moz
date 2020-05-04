using Moz.Bus.Dtos;
using Moz.Bus.Dtos.Request.Members;
using Moz.Bus.Dtos.Result.Members;

namespace Moz.Bus.Services.Members
{
    public interface IRegistrationService
    {
        ServResult<RegistrationResult> Register(ServRequest<ExternalRegistrationRequest> request); 
    }
}