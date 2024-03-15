import { Person } from "./person.model";

export interface ChatMessage{
    id?: number,
    senderId?: number,
    receiverId?: number,
    sender: Person,
    receiver: Person,
    content: string,
    creationDateTime: Date,
    isRead?: boolean
}