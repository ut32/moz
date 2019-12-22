namespace Moz.Auth
{
    public interface IJwtService
    {
        string GenerateJwtToken(string memberUId);
    }
}