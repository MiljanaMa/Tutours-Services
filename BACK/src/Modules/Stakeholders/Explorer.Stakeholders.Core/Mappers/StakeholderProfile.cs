using AutoMapper;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.Core.Domain;

namespace Explorer.Stakeholders.Core.Mappers;

public class StakeholderProfile : Profile
{
    public StakeholderProfile()
    {
        CreateMap<ApplicationRatingDto, ApplicationRating>().ReverseMap();
        CreateMap<ClubJoinRequestDto, ClubJoinRequest>().ReverseMap();
        CreateMap<UserDto, User>().ReverseMap();
        CreateMap<PersonDto, Person>().ReverseMap();
        CreateMap<ClubDto, Club>().ReverseMap();
        CreateMap<ClubInvitationDto, ClubInvitation>().ReverseMap();
        CreateMap<TourIssueDto, TourIssue>().ReverseMap();
        CreateMap<TourIssueCommentDto, TourIssueComment>().ReverseMap();
        CreateMap<ChatMessageDto, ChatMessage>().ReverseMap();
        CreateMap<NotificationDto, Notification>().ReverseMap();
        CreateMap<ClubChallengeRequestDto, ClubChallengeRequest>().ReverseMap();
        CreateMap<ClubFightDto, ClubFight>().ReverseMap();
        CreateMap<AchievementDto, Achievement>().ReverseMap();
        CreateMap<NewsletterPreferenceDto, NewsletterPreference>().ReverseMap();
    }
}