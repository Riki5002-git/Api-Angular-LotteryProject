import { PersonModel } from "./PersonModel";

export interface PresentModel {
    id?: number;
    name: string;
    description: string;
    donorId: number;
    price?: number;
    pictureUrl: string;
    purchasesAmount?: number;
    categoryId?: number;
    quantity?: number;
    winnerId?: number;
    winner?: PersonModel;

    // [key: string]: any;
}