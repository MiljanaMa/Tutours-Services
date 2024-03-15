import { Keypoint } from "src/app/feature-modules/tour-authoring/model/keypoint.model";
import { TransportType } from "src/app/feature-modules/tour-authoring/model/tour.model";

export interface RouteQuery{
    keypoints: Keypoint[]; // maybe replace with MarkerPosition for easier transfer of icons
    transportType: TransportType;
    currentKeypointPosition?: number; // used for displaying different icons depending if the tourist passed the keypoint or not 
}