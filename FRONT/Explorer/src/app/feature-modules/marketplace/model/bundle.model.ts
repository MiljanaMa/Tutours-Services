import { Tour } from "../../tour-authoring/model/tour.model";

export interface Bundle {
    id?: number;
    name: string;
    
    status: string;
    tours?: Tour[]
  }
  