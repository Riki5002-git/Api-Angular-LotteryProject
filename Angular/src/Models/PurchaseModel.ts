import { PersonModel } from "./PersonModel";
import { PresentModel } from "./PresentModel";

export interface PurchaseModel {
    id: number;
    presentId: number;
    present?: PresentModel;
    personId: number;
    person?: PersonModel;
    purchaseDate: Date;

    // [key: string]: any;
}