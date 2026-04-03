using Microsoft.AspNetCore.Identity;

namespace BaseRMS.Services.Interfaces;

public interface IIdentityErrorLocalizerService
{
    string MapIdentityErrorCodeToField(string errorCode);
    IEnumerable<string> LocalizeErrors(IEnumerable<IdentityError> errors);
    string LocalizeError(IdentityError error);
}
