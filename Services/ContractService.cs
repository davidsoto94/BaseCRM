using BaseRMS.Configurations;
using BaseRMS.DTOs;
using BaseRMS.Entities;
using BaseRMS.Entities.AttatchmentClasses;
using BaseRMS.Extensions;
using BaseRMS.Repositories;
using BaseRMS.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BaseRMS.Services;

public class ContractService(ContractRepository contractRepository
    , IFileService fileService)
{
    private readonly ContractRepository _contractRepository = contractRepository;
    private readonly IFileService _fileService = fileService;
    public async Task<List<Contract>> GetAllContracts()
    {
        return await _contractRepository.GetContracts().ToListAsync();
    }

    public async Task<Contract> GetContractById(int id)
    {
        var contract = await _contractRepository.GetContracts().FirstOrDefaultAsync(c => c.Id == id);
        if (contract == null)
        {
            throw new KeyNotFoundException("Contract not found.");
        }
        return contract;
    }

    public async Task CreateContractAsync(ContractDTO contract)
    {
        if (contract.ClientId == default)
        {
            throw new ValidationException([new ValidationError { Field = "ClientId", Message = "No client specified." }]);
        }
        if (contract.ContractTypeId == default)
        {
            throw new ValidationException([new ValidationError { Field = "ContractTypeId", Message = "No contract type specified." }]);
        }
        if (contract.StartDate == default)
        {
            throw new ValidationException([new ValidationError { Field = "StartDate", Message = "No start date specified." }]);
        }
        if (string.IsNullOrEmpty(contract.Address))
        {
            throw new ValidationException([new ValidationError { Field = "Address", Message = "No address specified." }]);
        }
        if(contract.ContractAmount == null && contract.HourAmmount == null)
        {
            throw new ValidationException([
                new ValidationError { 
                    Field = "ContractAmount", 
                    Message = "No contract amount specified." 
                },
                new ValidationError { 
                    Field = "HourAmmount", 
                    Message = "No hour amount specified." 
                }]);
        }

        var newContract = new Contract
        {
            ClientId = contract.ClientId,
            Name = contract.Name,
            ContractTypeId = contract.ContractTypeId,
            StartDate = contract.StartDate,
            Address = contract.Address,
            ContractAmount = contract.ContractAmount,
            HourAmmount = contract.HourAmmount
        };

        await _contractRepository.CreateContractAsync(newContract);
        await AddContractAttachments(newContract, contract.Attachments);
        await _contractRepository.UpdateContractAsync(newContract);

    }

    public async Task CreateContractTypeAsync(ContractTypes contractType)
    {
        if (string.IsNullOrEmpty(contractType.Name))
        {
            throw new ValidationException([new ValidationError { Field = "Name", Message = "No name specified." }]);
        }
        await _contractRepository.CreateContractTypeAsync(contractType);
    }

    public async Task UpdateContractAsync(ContractDTO contract)
    {
        var existingContract = await _contractRepository.GetContracts().FirstOrDefaultAsync(c => c.Id == contract.Id);
        if (existingContract == null)
        {
            throw new ValidationException([new ValidationError { Field = "Id", Message = "No contract ID specified." }]);
        }
        if (contract.ClientId == default)
        {
            throw new ValidationException([new ValidationError { Field = "ClientId", Message = "No client specified." }]);
        }
        if (contract.ContractTypeId == default)
        {
            throw new ValidationException([new ValidationError { Field = "ContractTypeId", Message = "No contract type specified." }]);
        }
        if (contract.StartDate == default)
        {
            throw new ValidationException([new ValidationError { Field = "StartDate", Message = "No start date specified." }]);
        }
        if (string.IsNullOrEmpty(contract.Address))
        {
            throw new ValidationException([new ValidationError { Field = "Address", Message = "No address specified." }]);
        }
        
        existingContract.ClientId = contract.ClientId;
        existingContract.Name = contract.Name;
        existingContract.Address = contract.Address;
        existingContract.ContractTypeId = contract.ContractTypeId;
        existingContract.StartDate = contract.StartDate;
        existingContract.EndDate = contract.EndDate;
        existingContract.ContractAmount = contract.ContractAmount;
        existingContract.HourAmmount = contract.HourAmmount;

        await _contractRepository.UpdateContractAsync(existingContract);
    }

    private async Task AddContractAttachments(Contract contract, ICollection<ApplicationFileDTO>? attachments)
    {
        if (attachments == null || !attachments.Any())
        {
            return;
        }

        foreach (var attachment in attachments)
        {
            var filePath = Constants.ClientFolderPath + "\\" + contract.ClientId + "\\" + contract.Id;
            filePath = await _fileService.SaveFileAsync(attachment.File, filePath, Guid.NewGuid() + "_" + attachment.FileName);

            var applicationFile = new ApplicationFile
            {
                Name = attachment.FileName,
                CurrentVersion = new FileVersion
                {
                    VersionNumber = 1,
                    StoragePath = filePath,
                    CreatedAt = DateTime.UtcNow
                }
            };

            var contractAttachment = new ContractAttachment
            {
                Entity = contract,
                EntityId = contract.Id,
                FileId = applicationFile.Id,
                UploadedAt = DateTime.UtcNow,
                File = applicationFile
            };

            contract.ContractAttachments.Add(contractAttachment);
        }
    }

}
