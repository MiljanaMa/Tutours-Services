import { Club } from "./club.model";

export enum ClubChallengeStatus {
    PENDING = 'PENDING',
    ACCEPTED = 'ACCEPTED',
    DECLINED = 'DECLINED'
}

export interface ClubChallengeRequest {
    id?: number,
    challenger?: Club,
    challengerId: number,
    challenged?: Club,
    challengedId: number,
    status: ClubChallengeStatus
}