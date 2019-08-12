import { Component, OnInit } from '@angular/core';
import { SeoService } from 'src/app/services/seo.service';
import { HttpbaseService } from 'src/app/services/httpbase.service';

@Component({
  selector: 'app-lista-de-professores',
  templateUrl: './lista-de-professores.component.html',
  styleUrls: ['./lista-de-professores.component.css']
})
export class ListaDeProfessoresComponent implements OnInit {

  public professores: any;
  public exibirResultados: boolean;

  constructor(seoService: SeoService ,private httpbaseService:HttpbaseService) {
    seoService.atribuirTitle('Professores');
  }

  ngOnInit() {    
    this.httpbaseService.get("api/professores").subscribe(data => {
      this.professores = data;      
      this.exibirResultados = data && data.length > 0;      
    });    
  }
}
