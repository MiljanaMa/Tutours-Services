export enum EncounterStatus {
    ACTIVE = "ACTIVE",
    DRAFT = "DRAFT",
    ARCHIVED = "ARCHIVED"
}

export enum EncounterType {
    SOCIAL = "SOCIAL",
    LOCATION = "LOCATION",
    MISC = "MISC"
}

export enum EncounterApprovalStatus {
    PENDING = "PENDING",
    SYSTEM_APPROVED = "SYSTEM_APPROVED",
    ADMIN_APPROVED = "ADMIN_APPROVED",
    DECLINED = "DECLINED"
}

export interface Encounter {
    id?: number,
    userId?: number,
    name: string,
    description: string,
    latitude: number,
    longitude: number,
    xp: number,
    status: EncounterStatus,
    type: EncounterType,
    range: number,
    image?: string,
    peopleCount? : number,
    approvalStatus: EncounterApprovalStatus
    
}
export interface KeypointEncounter {
    id?: number,
    keypointId: number,
    encounterId: number,
    encounter: Encounter,
    isRequired: boolean
}