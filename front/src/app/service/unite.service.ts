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

  ListerMesUnite(_idCompte: number): Observable<MesUnite[]>
  {
    return this.http.get<MesUnite[]>(`${environment.urlApi}/${this.dossier}/listerIdMesUnite/${_idCompte}`);
  }

  ModifierLvl(_idCompte: number, _idUnite: number, _niveau: string): Observable<boolean>
  {
    const DATA = { IdCompte: _idCompte, IdUnite: _idUnite, NiveauMaitrise: _niveau };

    return this.http.put<boolean>(`${environment.urlApi}/${this.dossier}/modifierLvl`, DATA);
  }
}
