using AutoMapper.Configuration.Conventions;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Encounters.API.Dtos;
using Explorer.Encounters.API.Public;
using Explorer.Encounters.Core.Domain.RepositoryInterfaces;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Internal;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Encounters.Core.UseCases
{
    public class XPService : IXPService
    {
        private IEncounterCompletionRepository _encounterCompletionRepository;
        private IInternalClubService _clubService;
        private IInternalClubFightService _clubFightService;
        private IInternalAchievementService _achievementService;

        public XPService(IEncounterCompletionRepository encounterCompletionRepository, IInternalClubService clubService, IInternalClubFightService clubFightService, IInternalAchievementService achievementService)
        {
            _encounterCompletionRepository = encounterCompletionRepository;
            _clubService = clubService;
            _clubFightService = clubFightService;
            _achievementService = achievementService;
        }

        public Result<ClubFightXPInfoDto> GetClubFightXPInfo(int clubFightId)
        {
            ClubFightDto fight = _clubFightService.Get(clubFightId).ValueOrDefault;
            if (fight == null) return Result.Fail(FailureCode.NotFound).WithError("Fight not found");

            ClubDto club1 = _clubService.GetWithMembers(fight.Club1Id).ValueOrDefault;
            ClubDto club2 = _clubService.GetWithMembers(fight.Club2Id).ValueOrDefault;
            if(club1 == null || club2 == null) return Result.Fail(FailureCode.NotFound).WithError("Club not found");

            ClubFightXPInfoDto clubFightXPInfo = new ClubFightXPInfoDto()
            {
                Club1ParticipantsInfo = new List<FightParticipantInfoDto>(),
                Club2ParticipantsInfo = new List<FightParticipantInfoDto>()
            };

            foreach (var member in club1.Members)
            {
                int memberXp = _encounterCompletionRepository.GetTotalXPInDateRangeByUser(member.UserId, fight.StartOfFight, fight.EndOfFight);

                clubFightXPInfo.Club1ParticipantsInfo.Add(ConvertToFightParticipant(member, memberXp));
            }
            clubFightXPInfo.Club1ParticipantsInfo = clubFightXPInfo.Club1ParticipantsInfo.OrderByDescending(pi => pi.XPInFight).ToList();
            clubFightXPInfo.club1TotalXp = clubFightXPInfo.Club1ParticipantsInfo.Select(pi => pi.XPInFight).Sum();

            foreach (var member in club2.Members)
            {
                int memberXp = _encounterCompletionRepository.GetTotalXPInDateRangeByUser(member.UserId, fight.StartOfFight, fight.EndOfFight);

                clubFightXPInfo.Club2ParticipantsInfo.Add(ConvertToFightParticipant(member, memberXp));
            }
            clubFightXPInfo.Club2ParticipantsInfo = clubFightXPInfo.Club2ParticipantsInfo.OrderByDescending(pi => pi.XPInFight).ToList();
            clubFightXPInfo.club2TotalXp = clubFightXPInfo.Club2ParticipantsInfo.Select(pi => pi.XPInFight).Sum();

            return clubFightXPInfo;
        }

        private FightParticipantInfoDto ConvertToFightParticipant(PersonDto person, int xp)
        {
            FightParticipantInfoDto fightParticipantInfoDto = new FightParticipantInfoDto()
            {
                Username = person.Name + " " + person.Surname, // it's just name + surname because I'm lazy to pull username from User table, and person is not connected to user >:(
                ProfileImage = person.ProfileImage,
                Level = person.Level,
                XPInFight = xp
            };

            return fightParticipantInfoDto;
        }

        /* Kinda doesn't belong in XPService */
        public void UpdateFights(bool tricky = false)
        {
            List<ClubFightDto> passedUnfinishedFights;

            if (tricky)
            {
                passedUnfinishedFights = _clubFightService.GetTricky().ValueOrDefault;
            }
            else
            {
                passedUnfinishedFights = _clubFightService.GetPassedUnfinishedFights().ValueOrDefault;
            }
            foreach (var clubFight in passedUnfinishedFights)
            {
                ClubDto winner = DeclareWinner(clubFight);
                winner.FightsWon++;
                clubFight.WinnerId = winner.Id;
                clubFight.IsInProgress = false;
                UpdateWinnerAchievements(winner);

                // clearing loser achievements from memory because EF
                ClubDto loser = winner.Id == clubFight.Club1.Id ? clubFight.Club2 : clubFight.Club1;
                loser.Achievements.Clear();
            }

            _clubFightService.UpdateMultiple(passedUnfinishedFights);
        }

        private void UpdateWinnerAchievements(ClubDto winner)
        {
            AchievementDto achievement = _achievementService.getFightAchievement(winner);
            winner.Achievements.Clear(); // must be here because queries are AsNoTracking, so no duplicates are inserted
            if (achievement != null)
            {
                // _clubService.AddAchievement(winner.Id, achievement.Id); // (: EF and his tracking
                winner.Achievements.Add(_achievementService.GetNoTracking((int)achievement.Id));
            }
        }

        private ClubDto DeclareWinner(ClubFightDto clubFight)
        {
            List<long> club1MemberIds = clubFight.Club1.Members.Select(m => m.Id).ToList();
            List<long> club2MemberIds = clubFight.Club2.Members.Select(m => m.Id).ToList();

            int club1TotalXP = _encounterCompletionRepository.GetTotalXPInDateRangeByUsers(club1MemberIds, clubFight.StartOfFight, clubFight.EndOfFight);
            int club2TotalXP = _encounterCompletionRepository.GetTotalXPInDateRangeByUsers(club2MemberIds, clubFight.StartOfFight, clubFight.EndOfFight);

            return club1TotalXP > club2TotalXP ? clubFight.Club1 : clubFight.Club2;
        }
    }
}
