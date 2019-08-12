import { Routes } from '@angular/router';

import { HomeComponent } from './home/home.component';
import { ListaDeProfessoresComponent } from './professores/lista-de-professores/lista-de-professores.component';
import { NovoProfessorComponent } from './professores/novo-professor/novo-professor.component';
import { ExcluirProfessorComponent } from './professores/excluir-professor/excluir-professor.component';
import { DetalhesProfessorComponent } from './professores/detalhes-professor/detalhes-professor.component';
import { EditarProfessorComponent } from './professores/editar-professor/editar-professor.component';

export const rootRouterConfig: Routes = [
     { path: '', redirectTo:'home' , pathMatch: 'full' },
     { path: 'home', component: HomeComponent },
     { path:'professores' , component:ListaDeProfessoresComponent},
     { path:'professores/novo' , component:NovoProfessorComponent},
     { path:'professores/excluir/:id' , component:ExcluirProfessorComponent},
     { path:'professores/detalhes/:id' , component:DetalhesProfessorComponent},
     { path:'professores/editar/:id' , component:EditarProfessorComponent},
]