import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { MesUnite } from '../Types/import/MesUnite';
import { Unite } from '../Types/Unite';

@Injectable({
  providedIn: 'root'
})
export class UniteService 
{
  private dossier: string = "unite";

  constructor(private http: HttpClient) { }

  Lister(): Observable<Unite[]>
  {
    return this.http.get<Unite[]>(`${environment.urlApi}/${this.dossier}/lister`);
  }

  ListerVisible(): Observable<Unite[]>
  {
    return this.http.get<Unite[]>(`${environment.urlApi}/${this.dossier}/listerVisible`);
  }

  ListerMesUnite(_idCompte: number): Observable<MesUnite[]>
  {
    return this.http.get<MesUnite[]>(`${environment.urlApi}/${this.dossier}/listerIdMesUnite/${_idCompte}`);
  }

  ModifierMeta(_idUnite: number, _estMeta: boolean): Observable<boolean>
  {
    const DATA = { Id: _idUnite, EstMeta: _estMeta };
    return this.http.put<boolean>(`${environment.urlApi}/${this.dossier}/modifierMeta`, DATA);
  }

  ModifierVisibiliter(_idUnite: number, _estVisible: boolean): Observable<boolean>
  {
    const DATA = { Id: _idUnite, EstVisible: _estVisible };
    return this.http.put<boolean>(`${environment.urlApi}/${this.dossier}/modifierVisibiliter`, DATA);
  }

  ModifierLvl(_idCompte: number, _idUnite: number, _niveau: string): Observable<boolean>
  {
    const DATA = { IdCompte: _idCompte, IdUnite: _idUnite, NiveauMaitrise: _niveau };

    return this.http.put<boolean>(`${environment.urlApi}/${this.dossier}/modifierLvl`, DATA);
  }
}
