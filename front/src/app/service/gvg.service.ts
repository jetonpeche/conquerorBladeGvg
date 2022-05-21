import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Gvg } from '../Types/Gvg';

@Injectable({
  providedIn: 'root'
})
export class GvgService 
{
  private dossier: string = "gvg";

  constructor(private http: HttpClient) { }

  Lister(): Observable<Gvg[]>
  {
    return this.http.get<Gvg[]>(`${environment.urlApi}/${this.dossier}/lister`);
  }

  Ajouter(_info): Observable<number[]>
  {
    return this.http.post<number[]>(`${environment.urlApi}/${this.dossier}/ajouter`, _info)
  }
}
