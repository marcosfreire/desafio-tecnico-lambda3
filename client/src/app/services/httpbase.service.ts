import 'rxjs/Rx';
import { retry } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})

// todo : implement error handling and refresh toquen validations
export class HttpbaseService {

  private headers = new HttpHeaders().set('Content-Type', 'application/json; charset=utf-8');

  constructor(public http: HttpClient) { }

  get(resource: string, headers?: HttpHeaders) {
    const header = headers || this.headers;
    let url = environment.baseUrl + resource;
    return this.http.get(url, { headers: header }).map((response: any) => response).pipe(retry(3));
  }

  post(resource: string, body: any, headers?: HttpHeaders) {
    const header = headers || this.headers;
    let url = environment.baseUrl + resource;    
    return this.http.post(url, body, { headers: header }).map((response: any) => response).pipe();
  }

  put(resource: string, body: any, headers?: HttpHeaders) {
    const header = headers || this.headers;
    let url = environment.baseUrl + resource;
    return this.http.put(url, body, { headers: header }).map((response: any) => response).pipe();
  }

  delete(resource: string, headers?: HttpHeaders) {
    const header = headers || this.headers;
    let url = environment.baseUrl + resource;
    return this.http.delete(url, { headers: header }).map((response: any) => response).pipe();
  }
}