export interface WishListItem{
    id?: number;
    tourId: number;
    userId: number;
    tourName: string
    tourDescription: string;
    tourPrice: number;
    tourType: string
    tourDifficulty: string;
    travelDistance: number;
    tourDuration: number;
}