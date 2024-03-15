import { Club } from "./club.model";

export interface ClubFight {
  id?: number,
  winnerId?: number,
  startOfFight: Date,
  endOfFight: Date,
  club1: Club,
  club2: Club,
  isInProgress: boolean
}
