using BaseRMS.DTOs;
using BaseRMS.Entities;
using BaseRMS.Localization;
using BaseRMS.Repositories;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

namespace BaseRMS.Services;

public class EventService(
    EventRepository eventRepository,
    IStringLocalizer<IdentityErrorMessages> identityLocalizer)
{
    private readonly IStringLocalizer<IdentityErrorMessages> _identityLocalizer = identityLocalizer;
    public async Task<Event> CreateEventAsync(EventDTO eventDTO)
    {
        if (eventDTO.UserId == 0)
        {
            throw new ValidationException(_identityLocalizer["UserRequired"]);
        }
        var eventToSave = new Event()
        {
            EventCategoryId = eventDTO.EventCategoryId,
            UserId = eventDTO.UserId,
            ContractId = eventDTO.ContractId,
            EmployeeId = eventDTO.EmployeeId,
            MachineId = eventDTO.MachineId,
            Description = eventDTO.Description,
            StartDate = eventDTO.StartDate,
            EndDate = eventDTO.EndDate,
            Observations = eventDTO.Observations,
            Cost = eventDTO.Cost
        };

        await eventRepository.CreateEventAsync(eventToSave);
        return eventToSave;
    }
}
