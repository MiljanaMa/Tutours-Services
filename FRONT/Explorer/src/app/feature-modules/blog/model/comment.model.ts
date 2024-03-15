export interface Comment{
    id? : number,
    blogId? : number,
    userId : number,
    username?: string,
    comment: string,
    postTime: Date,
    lastEditTime? : Date,
    isDeleted: boolean
}