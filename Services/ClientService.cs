using BaseRMS.Configurations;
using BaseRMS.DTOs;
using BaseRMS.Entities;
using BaseRMS.Entities.AttatchmentClasses;
using BaseRMS.Extensions;
using BaseRMS.Localization;
using BaseRMS.Repositories;
using BaseRMS.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace BaseRMS.Services;

public class ClientService (ClientRepository clientRepository,
    IFileService fileService,
    IStringLocalizer<IdentityErrorMessages> identityLocalizer)
{
    private readonly ClientRepository _clientRepository = clientRepository;
    private readonly IFileService _fileService = fileService;
    private readonly IStringLocalizer<IdentityErrorMessages> _identityLocalizer = identityLocalizer;

    public async Task<Client> CreateClient(ClientCreateDTO clientDto)
    {
        var newClient = await _clientRepository.GetClients().FirstOrDefaultAsync(c =>
            c.Email == clientDto.Email 
            && c.Name == clientDto.Name
            && c.Address == clientDto.Address
        );

        if (newClient != null) { 
            throw new ValidationException(new List<ValidationError>() { 
                new() { Field = "Email", Message = _identityLocalizer["DuplicateEmail"].Value},
                new() { Field = "Name", Message = _identityLocalizer["DuplicateName"].Value},
            });
        }

        if (clientDto.ClientImage.ContentType != "image/jpeg" && clientDto.ClientImage.ContentType != "image/png")
        {
            throw new ValidationException(new List<ValidationError>() {
                new() { Field = "ClientImage", Message = _identityLocalizer["InvalidImageFormat"].Value}
            });
        }

        newClient = new Client
        {
            Name = clientDto.Name,
            Address = clientDto.Address,
            Email = clientDto.Email,
            PhoneNumber = clientDto.PhoneNumber
        };

        await _clientRepository.CreateClientAsync(newClient);
        await UpdateClientObjectImage(newClient, clientDto.ClientImage);
        await AddClientAttatchments(newClient, clientDto.Attachments);
        await _clientRepository.UpdateClientAsync(newClient);
        return newClient;
    }

    private async Task UpdateClientObjectImage(Client client, IFormFile newImage)
    {
        if (client.ClientImagePath != null)
        {
            await _fileService.DeleteFileAsync(client.ClientImagePath);
        }
        var extension = Path.GetExtension(newImage.FileName);
        var filePath = Constants.ClientFolderPath + "\\" + client.Id;
        var imagePath = await _fileService.SaveFileAsync(newImage, filePath, "clientImage" + extension);
        client.ClientImagePath = imagePath;
    }

    private async Task AddClientAttatchments(Client client, ICollection<ApplicationFileDTO>? attachments)
    {
        if (attachments == null || !attachments.Any())
        {
            return;
        }

        foreach (var attachment in attachments)
        {
            var filePath = Constants.ClientFolderPath + "\\" + client.Id;
            filePath = await _fileService.SaveFileAsync(attachment.File, filePath, new Guid() + "_" + attachment.FileName);

            var applicationFile = new ApplicationFile
            {
                Name = attachment.FileName,
                Path = filePath
            };

            // Now create the ClientAttachment with the saved ApplicationFile (which has an ID)
            var clientAttachment = new ClientAttachment
            {
                Entity = client,
                EntityId = client.Id,
                FileId = applicationFile.Id,
                UploadedAt = DateTime.UtcNow,
                File = applicationFile
            };

            client.Attachments.Add(clientAttachment);
        }

    }
}
