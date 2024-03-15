import { Coupon } from "./coupon-model";

export interface OrderItem {
    id: number,
    userId: number,
    tourId?:number,
    tourName: string,
    tourDescription: string,
    tourPrice:number;
}