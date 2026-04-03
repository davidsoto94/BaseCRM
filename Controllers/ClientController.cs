using BaseRMS.DTOs;
using BaseRMS.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace BaseRMS.Controllersl;

[Route("api/v1/[controller]")]
[ApiController]
[ExcludeFromCodeCoverage]
public class ClientController (ClientService clientService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Post([FromForm] ClientCreateDTO clientDto)
    {
        var createdClient = await clientService.CreateClient(clientDto);
        return Created((string?)null, createdClient);
    }
}
