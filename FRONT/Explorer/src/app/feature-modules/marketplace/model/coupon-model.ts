export interface Coupon {
    id: number,
    code: string,
    discount:number,
    tourId?: number,
    touristId: number,
    authorId: number,
    expiryDate: Date;
}