import { Component, OnInit } from '@angular/core';
import { ProfessorViewModel } from 'src/app/view-models/ProfessorViewModel';
import { DisciplinaViewModel } from 'src/app/view-models/DisciplinaViewModel';
import { DateUtils } from 'src/app/utils/data-type-utils/date-utils';
import { SeoService } from 'src/app/services/seo.service';
import { HttpbaseService } from 'src/app/services/httpbase.service';
import { Router, ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-editar-professor',
  templateUrl: './editar-professor.component.html',
  styleUrls: ['./editar-professor.component.css']
})
export class EditarProfessorComponent implements OnInit {

  public sub: Subscription;
  public idProfessor: string = "";

  public errors: any[] = [];
  public tituloErros: string;
  public exibirErros: boolean = false;

  public professor = new ProfessorViewModel;

  public disciplinas: DisciplinaViewModel[];
  public idDisciplinaSelecionada: number = 0;
  public disciplinasSelecionadas: DisciplinaViewModel[] = [];
  public exibirTabelaDisciplinas: boolean = false;

  public myDatePickerOptions = DateUtils.getMyDatePickerOptions();

  constructor(private route: ActivatedRoute,
    seoService: SeoService,
    private httpbaseService: HttpbaseService,
    private router: Router) {
    seoService.atribuirTitle('Editar Professor');
  }

  ngOnInit() {

    this.sub = this.route.params.subscribe(
      params => {
        this.idProfessor = params['id'];
        this.obterProfessor();
      });

    this.recuperarDisciplinas();
  }

  obterProfessor() {
    this.httpbaseService.get('api/professores/' + this.idProfessor).subscribe(professor => {
      this.professor = professor;
      this.professor.dataNascimento = DateUtils.setMyDatePickerDate(this.professor.dataNascimento)
      //this.disciplinasSelecionadas = professor.disciplinas;
    }, responseError => {
      console.log(responseError);
    });
  }

  recuperarDisciplinas() {
    this.httpbaseService.get('api/disciplinas').subscribe(a => {
      this.disciplinas = a;
    });
  }

  adicionarDisciplina() {
    var itemJaAdicionado = this.disciplinasSelecionadas.findIndex(x => x.id == this.idDisciplinaSelecionada.toString()) !== -1;
    if (itemJaAdicionado || this.idDisciplinaSelecionada == 0) {
      return false;
    }

    let course = this.disciplinas.find(a => a.id == this.idDisciplinaSelecionada.toString());
    this.disciplinasSelecionadas.push(course);
    this.exibirTabelaDisciplinas = true;
    this.atualisarCursosProfessor();
  }

  removerDisciplina(courseId: number) {
    let itemIndex = this.disciplinasSelecionadas.findIndex(x => x.id == courseId.toString())
    this.disciplinasSelecionadas.splice(itemIndex, 1);
    this.exibirTabelaDisciplinas = this.disciplinasSelecionadas != undefined && this.disciplinasSelecionadas.length > 0;
    this.atualisarCursosProfessor();
  }

  atualisarCursosProfessor() {
    this.professor.disciplinas = this.disciplinasSelecionadas;
  }

  atualizarProfessor () {

    this.professor.dataNascimento = DateUtils.getMyDatePickerDate(this.professor.dataNascimento);

    this.httpbaseService.put('api/professores',this.idProfessor, this.professor).subscribe(a => {
      this.router.navigate(['/professores']);
    }, responseError => {
        if (responseError.status == 400 || responseError.status == 404) {
          this.tituloErros = "Ops! Corrija os erros abaixo antes de continuar!";
          this.errors = responseError.error.errors;
          this.exibirErros = responseError.error.errors != undefined && responseError.error.errors.length > 0;
        }
        else {
          this.exibirErros = true;
          this.tituloErros = "Ops! Alguma coisa deu errado. :(";
          this.errors.push('Tente novamente mais tarde.Se o problema persistir, contate o administrador do sistema.');
          console.log(responseError.error.applicationError);

          // navegar para pagina de erro?
        }
      });
  }
}
