namespace MBS.Shared.Services.Interfaces
{
    public interface IClaimService
    {
        string GetUserId();

        string GetClaim(string key);
    }
}
