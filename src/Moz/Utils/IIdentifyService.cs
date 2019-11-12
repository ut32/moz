namespace Moz.Utils
{
    public interface IIdentifyService
    {
        long GetUnqId();
        
        string GetGuid();

        string GetSnowId();
    }
}