export interface Profile{
    id?: number,
    userId:number,
    name: string, 
    surname: string,
    profileImage: string, 
    biography: string
    quote : string, 
    username: string, 
    password: string, 
    email: string,
    xp: number,
    level: number
    clubId?: number;
}