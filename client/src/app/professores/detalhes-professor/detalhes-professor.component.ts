import { Component, OnInit } from '@angular/core';
import { ProfessorViewModel } from 'src/app/view-models/ProfessorViewModel';
import { Subscription } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { SeoService } from 'src/app/services/seo.service';
import { HttpbaseService } from 'src/app/services/httpbase.service';

@Component({
  selector: 'app-detalhes-professor',
  templateUrl: './detalhes-professor.component.html',
  styleUrls: ['./detalhes-professor.component.css']
})
export class DetalhesProfessorComponent implements OnInit {

  public professor: ProfessorViewModel;
  public sub: Subscription;
  public idProfessor: string = "";
  
  constructor(private route: ActivatedRoute,
              private seoService: SeoService, 
              private httpbaseService: HttpbaseService) {
                  seoService.atribuirTitle('Detalhes do Professor');
              }

  ngOnInit() {
    this.sub = this.route.params.subscribe(
      params => {
        this.idProfessor = params['id'];
        this.obterProfessor();
      });
  }

  obterProfessor() {
    this.httpbaseService.get('api/professores/' + this.idProfessor).subscribe(professor => {
      this.professor = professor;
    }, responseError => {
      console.log(responseError);
    });
  }
}
