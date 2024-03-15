import { TourDifficulty, TransportType } from "../../tour-authoring/model/tour.model";

export interface TourPreference {
    id?: number;
    userId?: number;
    difficulty: TourDifficulty;
    transportType: TransportType;
    tags: string[];
}