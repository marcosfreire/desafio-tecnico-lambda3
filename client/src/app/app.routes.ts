import { Routes } from '@angular/router';

import { HomeComponent } from './home/home.component';
import { ListaDeProfessoresComponent } from './professores/lista-de-professores/lista-de-professores.component';
import { NovoProfessorComponent } from './professores/novo-professor/novo-professor.component';


export const rootRouterConfig: Routes = [
     { path: '', redirectTo:'home' , pathMatch: 'full' },
     { path: 'home', component: HomeComponent },
     { path:'professores' , component:ListaDeProfessoresComponent},
     { path:'professores/novo' , component:NovoProfessorComponent}
]