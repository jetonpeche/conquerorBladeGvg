import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Compte } from '../Types/Compte';
import { CompteExport } from '../Types/export/CompteExport';
import { ConfigCompteExport } from '../Types/export/ConfigCompteExport';

@Injectable({
  providedIn: 'root'
})
export class CompteService 
{
  private readonly DOSSIER = "compte";

  constructor(private http: HttpClient) { }

  Connexion(_pseudo: string): Observable<string | Compte>
  {
    return this.http.get<string | Compte>(`${environment.urlApi}/${this.DOSSIER}/connexion/${_pseudo}`);
  }

  Ajouter(_info: CompteExport): Observable<string>
  {
    return this.http.post<string>(`${environment.urlApi}/${this.DOSSIER}/ajouter`, _info);
  }

  Modifier(_info: ConfigCompteExport): Observable<boolean>
  {
    return this.http.put<boolean>(`${environment.urlApi}/${this.DOSSIER}/modifier`, _info);
  }
}
