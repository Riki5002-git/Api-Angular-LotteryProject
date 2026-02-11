import { Routes } from '@angular/router';
import { Person } from '../Components/person/Person';
import { Register } from '../Components/register/register';
import { Present } from '../Components/present/GetAll/present';
import { GetAllDonors } from '../Components/Donor/GetAll/get-all-donors';
import { AddPresent } from '../Components/present/AddPresent/add-present';
import { UpdatePresent } from '../Components/present/update-present/update-present';
import { Login } from '../Components/login/login';
import { AddDonor } from '../Components/Donor/AddDonor/add-donor';
import { UpdateDonor } from '../Components/Donor/update-donor/update-donor';
import { DonorsPresents } from '../Components/Donor/donors-presents/donors-presents';
import { Basket } from '../Components/Basket/basket';
import { Purchase } from '../Components/purchase/purchasesCards/purchase';
import { PurchasesDetails } from '../Components/purchase/purchases-details/purchases-details';
import { Lottery } from '../Components/lottery/lottery';

export const routes: Routes = [
    { path: 'api/person/getAll', component: Person },
    { path: 'api/person/register', component: Register },
    { path: 'api/person/login', component: Login },
    { path: 'api/present/getAll', component: Present },
    { path: 'api/Donor/getAll', component: GetAllDonors },
    { path: 'api/present/add', component: AddPresent },
    { path: 'api/present/update/:id', component: UpdatePresent },
    { path: 'api/donor/add', component: AddDonor },
    { path: 'api/donor/update/:id', component: UpdateDonor },
    { path: 'api/donor/:id/presents', component: DonorsPresents },
    { path: 'api/basket/getBasket', component: Basket },
    { path: 'api/purchase/GetAllPurchasesOfPresent/:id', component: Purchase },
    { path: 'api/purchase/buyers', component: PurchasesDetails },
    { path: 'api/Lottery', component: Lottery },

    { path: '', redirectTo: 'api/person/login', pathMatch: 'full' }
];