export interface TourReviewString {
    id? : number,
    rating: number,
    comment: string,
    visitDate: Date,//string,
    ratingDate: Date,//string,
    imageLinks: string[],
    tourId?: string,
    userId: string
}