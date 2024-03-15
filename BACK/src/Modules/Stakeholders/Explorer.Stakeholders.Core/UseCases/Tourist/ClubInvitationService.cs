using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public.Tourist;
using Explorer.Stakeholders.Core.Domain;

namespace Explorer.Stakeholders.Core.UseCases.Tourist;

public class ClubInvitationService : CrudService<ClubInvitationDto, ClubInvitation>, IClubInvitationService
{
    public ClubInvitationService(ICrudRepository<ClubInvitation> crudRepository, IMapper mapper) : base(crudRepository,
        mapper)
    {
    }
}