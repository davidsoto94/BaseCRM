using Microsoft.AspNetCore.Identity;

namespace BaseRMS.Services.Interfaces;

public interface IIdentityErrorLocalizerService
{
    IEnumerable<string> LocalizeErrors(IEnumerable<IdentityError> errors);
    string LocalizeError(IdentityError error);
}
