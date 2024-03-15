import { FightParticipantInfo } from "./fight-participant-info.model";

export interface ClubFightXPInfo {
  club1ParticipantsInfo: FightParticipantInfo[],
  club2ParticipantsInfo: FightParticipantInfo[],
  club1TotalXp: number,
  club2TotalXp: number
}
