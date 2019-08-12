import { Injectable } from '@angular/core';
import { Title } from '@angular/platform-browser';

@Injectable({
  providedIn: 'root'
})
export class SeoService {

  private title: Title;

  constructor(title: Title) {
    this.title = title;
  }

  public atribuirTitle(newTitle: string) {
    this.title.setTitle(newTitle + "- Teste Tecnico Lambda3");
  }
}