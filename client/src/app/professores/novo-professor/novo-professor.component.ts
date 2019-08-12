import { Component, OnInit } from '@angular/core';
import { DateUtils } from 'src/app/utils/data-type-utils/date-utils';
import { SeoService } from 'src/app/services/seo.service';
import { HttpbaseService } from 'src/app/services/httpbase.service';
import { Router } from '@angular/router';
import { ProfessorViewModel } from 'src/app/view-models/ProfessorViewModel';
import { DisciplinaViewModel } from 'src/app/view-models/DisciplinaViewModel';


@Component({
  selector: 'app-novo-professor',
  templateUrl: './novo-professor.component.html',
  styleUrls: ['./novo-professor.component.css']
})
export class NovoProfessorComponent implements OnInit {
  
  public errors:any[] = [];
  public tituloErros:string;
  public exibirErros :boolean = false;

  public professor = new ProfessorViewModel;

  public disciplinas: DisciplinaViewModel[];
  public idDisciplinaSelecionada: number = 0;
  public disciplinasSelecionadas: DisciplinaViewModel[] = [];
  public exibirTabelaDisciplinas:boolean = false;
    
  public myDatePickerOptions = DateUtils.getMyDatePickerOptions();

  constructor(seoService: SeoService, 
              private httpbaseService: HttpbaseService,
              private router: Router) {

    seoService.atribuirTitle('Novo Professor');
  }

  ngOnInit() {    
    this.recuperarDisciplinas();
  }

  recuperarDisciplinas() {
    this.httpbaseService.get('api/disciplinas').subscribe(a => {
      this.disciplinas = a;
    });
  }

  adicionarDisciplina() {    
    var itemJaAdicionado = this.disciplinasSelecionadas.findIndex(x => x.id == this.idDisciplinaSelecionada.toString()) !== -1;
    if(itemJaAdicionado || this.idDisciplinaSelecionada == 0) 
    {
      return false;
    }

    let course = this.disciplinas.find(a => a.id == this.idDisciplinaSelecionada.toString());
    this.disciplinasSelecionadas.push(course);
    this.exibirTabelaDisciplinas = true;
    this.atualisarCursosProfessor();
  }

  removerDisciplina(courseId:number) {
    let itemIndex = this.disciplinasSelecionadas.findIndex(x => x.id == courseId.toString())
    this.disciplinasSelecionadas.splice(itemIndex,1);
    this.exibirTabelaDisciplinas = this.disciplinasSelecionadas != undefined && this.disciplinasSelecionadas.length > 0;
    this.atualisarCursosProfessor();
  }

  atualisarCursosProfessor(){
    this.professor.disciplinas = this.disciplinasSelecionadas;
  }

  adicionarProfessor() {
        
    this.professor.dataNascimento =  DateUtils.getMyDatePickerDate(this.professor.dataNascimento);

    this.httpbaseService.post('api/professores',this.professor).subscribe(a => {
      this.router.navigate(['/professores']);
    }, responseError => 
    {
      if(responseError.status == 400 || responseError.status == 404)
      {
        this.tituloErros = "Ops! Corrija os erros abaixo antes de continuar!";
        this.errors = responseError.error.errors;
        this.exibirErros = responseError.error.errors != undefined && responseError.error.errors.length > 0;
      }
      else{
        this.exibirErros = true;
        this.tituloErros = "Ops! Alguma coisa deu errado. :(";
        this.errors.push('Tente novamente mais tarde.Se o problema persistir, contate o administrador do sistema.');
        console.log(responseError.error.applicationError);

        // navegar para pagina de erro?
      }
    });
  }
}
