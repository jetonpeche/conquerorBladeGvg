import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { ClasseHeros } from '../Types/ClasseHeros';

@Injectable({
  providedIn: 'root'
})
export class ClasseHerosService {

  constructor(private http: HttpClient) { }

  Lister(): Observable<ClasseHeros[]>
  {
    return this.http.get<ClasseHeros[]>(`${environment.urlApi}/classeHeros/lister`);
  }
}
