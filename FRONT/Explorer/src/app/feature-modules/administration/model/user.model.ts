import { Wallet } from "../../marketplace/model/wallet.model";

export enum UserRole {
    Administrator = 'administrator',
    Author = 'author',
    Tourist = 'tourist',
  }
export interface User {
    id?: number;
    username: string;
    password: string;
    isActive: boolean;
    role: number;
    email: string;
    isBlocked: boolean;
    isEditing: boolean;
    wallet?: Wallet;
}
