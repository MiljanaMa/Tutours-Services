export interface TourReviewString {
    id? : number,
    rating: number,
    comment: string,
    visitDate: string,
    ratingDate: string,
    imageLinks: string[],
    tourId?: string,
    userId: string
}