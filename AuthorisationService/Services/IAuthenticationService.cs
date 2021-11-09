using AuthorisationService.Requests;
using AuthorisationService.Responses;
using System.Threading.Tasks;

namespace AuthorisationService.Services {
    public interface IAuthenticationService {
        Task<AuthenticateResponse> Authenticate(AuthenticateRequest request);
    }
}