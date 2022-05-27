import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { UniteCompteGvGExport } from '../Types/export/UniteCompteExport';
import { Gvg } from '../Types/Gvg';
import { ParticipantGvG } from '../Types/ParticipantGvg';

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

  ListerParticipant(_idGvg: number): Observable<ParticipantGvG>
  {
    return this.http.get<ParticipantGvG>(`${environment.urlApi}/${this.dossier}/listerParticipant/${_idGvg}`);
  }

  Ajouter(_info): Observable<number[]>
  {
    return this.http.post<number[]>(`${environment.urlApi}/${this.dossier}/ajouter`, _info)
  }

  Parametrer(_info: UniteCompteGvGExport[]): Observable<any>
  {
    return this.http.post<any>(`${environment.urlApi}/${this.dossier}/parametrerGvG`, _info);
  }

  Participer(_idGvg: number, _idCompte: number): Observable<boolean>
  {
    const DATA = { idGvg: _idGvg, idCompte: _idCompte };

    return this.http.post<boolean>(`${environment.urlApi}/${this.dossier}/participer`, DATA);
  }

  Absent(_idGvg: number, _idCompte: number): Observable<boolean>
  {
    const DATA = { idGvg: _idGvg, idCompte: _idCompte };

    return this.http.post<boolean>(`${environment.urlApi}/${this.dossier}/absent`, DATA);
  }
}
