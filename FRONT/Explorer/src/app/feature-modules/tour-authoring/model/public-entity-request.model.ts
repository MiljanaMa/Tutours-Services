export enum EntityType{
    KEYPOINT = 'KEYPOINT',
    OBJECT = 'OBJECT'
}

export enum Status{
    PENDING = 'PENDING',
    APPROVED = 'APPROVED',
    DECLINED = 'DECLINED'
}

export interface PublicEntityRequest{
    id?: number;
    userId?: number;
    entityId: number;
    entityType: number;
    status: number;
    comment?: string;
}


