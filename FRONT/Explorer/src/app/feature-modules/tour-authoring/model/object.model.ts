export enum Category { // rename to object category -.-
    RESTAURANT = 'RESTAURANT',
    PARKING = 'PARKING',
    WC = 'WC',
    OTHER = 'OTHER'
}

export enum Status {
    PRIVATE = 'PRIVATE',
    PUBLIC = 'PUBLIC'
}

export interface Object {
        id? : number,
        name: string,
        description: string, 
        longitude: number,
        latitude: number,
        image?: string,
        category: Category,
        status: number
}