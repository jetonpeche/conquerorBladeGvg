import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Groupe } from '../Types/Groupe';

@Injectable({
  providedIn: 'root'
})
export class GroupeService 
{
  private readonly DOSSIER = "groupe";

  constructor(private http: HttpClient) { }

  Lister(): Observable<Groupe[]>
  {
    return this.http.get<Groupe[]>(`${environment.urlApi}/${this.DOSSIER}/lister`);
  }

  ModifierCompteGroupe(_idCompte: number, _idGvg: number, _idGroupe: number): Observable<boolean>
  {
    const DATA = { IdCompte: _idCompte, IdGvg: _idGvg, IdGroupe: _idGroupe };

    return this.http.put<boolean>(`${environment.urlApi}/${this.DOSSIER}/modifierCompteGroupe`, DATA);
  }
}
