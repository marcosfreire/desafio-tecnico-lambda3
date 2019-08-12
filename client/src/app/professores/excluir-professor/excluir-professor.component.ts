import { Component, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { ActivatedRoute, Router } from "@angular/router";
import { SeoService } from 'src/app/services/seo.service';
import { HttpbaseService } from 'src/app/services/httpbase.service';
import { ProfessorViewModel } from 'src/app/view-models/ProfessorViewModel';

@Component({
  selector: 'app-excluir-professor',
  templateUrl: './excluir-professor.component.html',
  styleUrls: ['./excluir-professor.component.css']
})
export class ExcluirProfessorComponent implements OnInit {

  public professor: ProfessorViewModel;
  public sub: Subscription;
  public idProfessor: string = "";

  constructor(private route: ActivatedRoute,
    seoService: SeoService, private httpbaseService: HttpbaseService,
    private router: Router) {
      seoService.atribuirTitle('Excluir Professor');
  }

  ngOnInit() {
    this.sub = this.route.params.subscribe(
      params => {
        this.idProfessor = params['id'];
        this.obterProfessor();
      });
  }

  excluirProfessor() {
    this.httpbaseService.delete('api/professores/' + this.idProfessor).subscribe(professor => {
      this.voltar();
    }, responseError => {
      console.log(responseError);
    });
  }

  obterProfessor() {
    this.httpbaseService.get('api/professores/' + this.idProfessor).subscribe(professor => {
      this.professor = professor;
    }, responseError => {
      console.log(responseError);
    });
  }

  voltar(){
    this.router.navigate(['/professores']);
  }
}
