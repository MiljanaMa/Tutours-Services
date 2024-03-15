import { TourIssueComment } from "./tour-issue-comment.model";

export interface TourIssue {
    id?: string,
    category: string,
    priority: string,
    description: string,
    creationDateTime: Date,
    resolveDateTime?: Date,
    isResolved?: boolean,
    tourId?: string,
    comments?: TourIssueComment[],
    userId: string,
}