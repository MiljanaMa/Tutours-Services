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


export interface EncounterMS {
    id: string
}