import { Encounter } from "./encounter.model";

export enum EncounterCompletionStatus {
    STARTED = "STARTED",
    FAILED = "FAILED",
    COMPLETED = "COMPLETED",
    PROGRESSING = "PROGRESSING"
}

export interface EncounterCompletion {
    id?: number,
    userId?: number,
    lastUpdatedAt: Date,
    encounterId: number,
    encounter: Encounter,
    xp: number,
    status: EncounterCompletionStatus
}