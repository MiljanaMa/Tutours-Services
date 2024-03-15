import {TourDiscount} from "./tour-discount.model";

export interface Discount{
  id?: number;
  startDate: string;
  endDate: string;
  percentage: number;
  userId?: number,
  tourDiscounts?: TourDiscount[]
}
