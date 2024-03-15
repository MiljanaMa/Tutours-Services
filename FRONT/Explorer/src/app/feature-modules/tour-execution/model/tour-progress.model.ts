import { Tour } from "../../tour-authoring/model/tour.model";
import { TouristPosition } from "./tourist-position.model";

export enum TourExecutionStatus {
    IN_PROGRESS = 'IN_PROGRESS',
    ABANDONED = 'ABANDONED',
    COMPLETED = 'COMPLETED'
}

export interface TourProgress {
    id?: number,
    touristPositionId?: number,
    touristPosition?: TouristPosition,
    tourId?: number,
    tour: Tour,
    status: TourExecutionStatus,
    startTime: Date,
    lastActivity: Date,
    currentKeyPoint: number
}