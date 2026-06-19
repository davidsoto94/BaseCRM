using BaseRMS.DTOs;
using BaseRMS.Entities;
using BaseRMS.Extensions;
using BaseRMS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace BaseRMS.Controllersl;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
[ExcludeFromCodeCoverage]
public class ClientsController (ClientService clientService) : ControllerBase
{

    [HttpGet]
    [HasPermission(Permissions.Client.List)]
    public async Task<IActionResult> Get()
    {
        var clients = await clientService.GetAllClients();
        var result = clients.Select(c => new
        {
            c.Id,
            c.Name,
            c.Email,
            c.PhoneNumber,
            c.Address
        }).ToList();
        return Ok(result);
    }

    [HttpGet("inactive")]
    [HasPermission(Permissions.Client.List)]
    public async Task<IActionResult> GetInactive()
    {
        var clients = await clientService.GetInactiveClients();
        var result = clients.Select(c => new
        {
            c.Id,
            c.Name,
            c.Email,
            c.PhoneNumber,
            c.Address
        }).ToList();
        return Ok(result);
    }

    [HttpGet("{id}")]
    [HasPermission(Permissions.Client.View)]
    public async Task<IActionResult> Get(int id)
    {
        var client = await clientService.GetClientById(id);
                
        return Ok(client);
    }

    [HttpGet("{id}/image")]
    [ResponseCache(Duration = 3600, Location = ResponseCacheLocation.Any)]
    [HasPermission(Permissions.Client.View)]
    public async Task<IActionResult> GetClientImage(int id)
    {
        var imageData = await clientService.GetClientImage(id);
        return Ok(imageData);
    }

    [HttpPost]
    [HasPermission(Permissions.Client.Create)]
    public async Task<IActionResult> Post([FromForm] ClientCreateDTO clientDto)
    {
        var createdClient = await clientService.CreateClient(clientDto);
        return Created((string?)null, createdClient);
    }

    [HttpPost("{id}/deactivate")]
    [HasPermission(Permissions.Client.Edit)]
    public async Task<IActionResult> Deactivate(int id)
    {
        await clientService.DiactivateClient(id);
        return NoContent();
    }

    [HttpPost("{id}/activate")]
    [HasPermission(Permissions.Client.Edit)]
    public async Task<IActionResult> Activate(int id)
    {
        await clientService.Activate(id);
        return NoContent();
    }
}
