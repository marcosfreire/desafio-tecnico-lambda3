//modules
import { NgModule, LOCALE_ID } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule,ReactiveFormsModule }   from '@angular/forms';

//routes
import { RouterModule } from '@angular/router';
import { rootRouterConfig } from './app.routes'; // created in app.modules

//bootstrap modules
import { AlertModule } from 'ngx-bootstrap';
import { CollapseModule } from 'ngx-bootstrap/collapse';
import { CarouselModule } from 'ngx-bootstrap/carousel';
import { MyDatePickerModule } from 'mydatepicker';

//shared components
import { FooterComponent } from './shared/footer/footer.component';
import { MenuLoginComponent } from './shared/menu-login/menu-login.component';
import { MenuSuperiorComponent } from './shared/menu-superior/menu-superior.component';
import { MainPrincipalComponent } from './shared/main-principal/main-principal.component';

//components
import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { ListaDeProfessoresComponent } from './professores/lista-de-professores/lista-de-professores.component';

//services
import { SeoService } from './services/seo.service';
import { HttpClientModule } from '@angular/common/http';
import { NovoProfessorComponent } from './professores/novo-professor/novo-professor.component';

// Locales
import { registerLocaleData } from '@angular/common';
import localePt from '@angular/common/locales/pt';
import { ExcluirProfessorComponent } from './professores/excluir-professor/excluir-professor.component';
import { DetalhesProfessorComponent } from './professores/detalhes-professor/detalhes-professor.component';
import { EditarProfessorComponent } from './professores/editar-professor/editar-professor.component';

registerLocaleData(localePt);

@NgModule({
  declarations: [
    AppComponent,
    MenuSuperiorComponent,
    FooterComponent,
    MainPrincipalComponent,
    HomeComponent,
    MenuLoginComponent,
    ListaDeProfessoresComponent,
    NovoProfessorComponent,
    ExcluirProfessorComponent,
    DetalhesProfessorComponent,
    EditarProfessorComponent
  ],
  imports: [
    BrowserModule,
    CollapseModule.forRoot(),
    CarouselModule.forRoot(),
    AlertModule.forRoot(),
    BrowserAnimationsModule,
    HttpClientModule,
    MyDatePickerModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forRoot(rootRouterConfig, { useHash: false })
  ],
  providers:
    [
      SeoService,
      {
        provide: LOCALE_ID,
        useValue: 'pt'
      }
    ]
  ,
  bootstrap: [AppComponent]
})
export class AppModule { }