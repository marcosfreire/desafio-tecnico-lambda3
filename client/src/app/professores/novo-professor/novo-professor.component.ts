import { Component, OnInit } from '@angular/core';
import { DateUtils } from 'src/app/utils/data-type-utils/date-utils';
import { SeoService } from 'src/app/services/seo.service';
import { HttpbaseService } from 'src/app/services/httpbase.service';
import { Course, ProfessorViewModel } from '../ProfessorViewModel';
import { FormGroup, FormBuilder } from '@angular/forms';

@Component({
  selector: 'app-novo-professor',
  templateUrl: './novo-professor.component.html',
  styleUrls: ['./novo-professor.component.css']
})
export class NovoProfessorComponent implements OnInit {

  public disciplinas: Course[];
  public disciplinasSelecionadas: Course[] = [];
  public exibirTabelaDisciplinas:boolean = false;
  public teacherForm: FormGroup;
  public idCursoSelecionado: number;
  public professor = new ProfessorViewModel;
  public myDatePickerOptions = DateUtils.getMyDatePickerOptions();

  constructor(seoService: SeoService, private httpbaseService: HttpbaseService) {
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

    var itemJaAdicionado = this.disciplinasSelecionadas.findIndex(x => x.id == this.idCursoSelecionado.toString()) !== -1;
    if(itemJaAdicionado)
    {
      return false;
    }

    let course = this.disciplinas.find(a => a.id == this.idCursoSelecionado.toString());
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
    this.httpbaseService.post('api/professores',this.professor).subscribe(
      a => {
        console.log(a);
      });
  }
}
