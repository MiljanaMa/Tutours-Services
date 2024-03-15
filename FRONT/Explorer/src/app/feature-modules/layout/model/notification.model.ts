export interface NotificationModel{
    id: number;
    userId: number;
    type: number;
    content: string;
    actionURL: string;
    creationDateTime: Date;
    isRead: boolean;
}