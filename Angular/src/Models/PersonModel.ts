export interface PersonModel {
    id?: number;
    firstName: string;
    lastName: string;
    userName: string;
    password: string;
    email: string;
    phone: string;

    [key: string]: any;
}