import { TourReview } from "../../tour-execution/model/tour-review.model";
import { Keypoint } from "./keypoint.model";

export enum TourDifficulty {
    EASY = 'EASY',
    MEDIUM = 'MEDIUM',
    HARD = 'HARD',
    EXTREME = 'EXTREME'
}

export enum TransportType {
    WALK = 'WALK',
    BIKE = 'BIKE',
    CAR = 'CAR',
    BOAT = 'BOAT'
}

export enum Status {
    DRAFT = 'DRAFT',
    PUBLISHED = 'PUBLISHED',
    ARCHIVED = 'ARCHIVED',
    DISABLED = 'DISABLED',
    CUSTOM = 'CUSTOM',
    CAMPAIGN = 'CAMPAIGN'
}

export interface Tour {
    id? : number,
    userId: number,
    name: string, 
    description: string,
    duration?: number,
    distance?: number,
    price: number,
    difficulty: TourDifficulty,
    transportType: TransportType,
    status: Status,
    tags?: string[],
    statusUpdateTime?: Date,
    keypoints?: Keypoint[],
    tourReviews?: TourReview[],
}