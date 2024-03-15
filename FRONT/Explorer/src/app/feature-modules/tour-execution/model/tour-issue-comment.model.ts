export interface TourIssueComment{
    id? : number,
    tourIssueId : number,
    userId : number,
    comment : string,
    creationDateTime : string,
    name?: string,
    surname?: string
}