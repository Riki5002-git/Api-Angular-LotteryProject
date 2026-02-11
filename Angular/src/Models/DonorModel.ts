import { PresentModel } from "./PresentModel";

export interface DonorModel {
    id?: number;
    firstName: string;
    lastName: string;
    userName: string;
    password: string;
    email: string;
    phone: string;

    [key: string]: any;
}