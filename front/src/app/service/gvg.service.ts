import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { firstValueFrom, Observable } from 'rxjs';
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

  ListerMesUniteGvg(_idGvg: number, _idCompte: number): Observable<any>
  {
    return this.http.get(`${environment.urlApi}/${this.dossier}/listerMesUnites/${_idGvg}/${_idCompte}`);
  }

  ListerParticipant(_idGvg: number): Observable<ParticipantGvG>
  {
    return this.http.get<ParticipantGvG>(`${environment.urlApi}/${this.dossier}/listerParticipant/${_idGvg}`);
  }

  RecupererInfoGvgParametrer()
  {
    return this.http.get<any>(`${environment.urlApi}/${this.dossier}/recupererInfoGvgDuJour`);
  }

  Ajouter(_info): Observable<number[]>
  {
    return this.http.post<number[]>(`${environment.urlApi}/${this.dossier}/ajouter`, _info)
  }

  Parametrer(_listeAjout: UniteCompteGvGExport[], _listeSupprimer: UniteCompteGvGExport[]): Observable<any>
  {
    const DATA = 
    { 
      ListeUniteAjouter: _listeAjout.filter(u => u.EstDejaChoisi == false), 
      ListeUniteAsupprimer: _listeSupprimer 
    };

    return this.http.post<any>(`${environment.urlApi}/${this.dossier}/parametrerGvG`, DATA);
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

  Supprimer(_idGvg: number): Observable<boolean>
  {
    return this.http.post<boolean>(`${environment.urlApi}/${this.dossier}/supprimer/${_idGvg}`, null);
  }

  async Existe(_date: string): Promise<boolean>
  {
    const DATA = { Date: _date };
    let retour$ = this.http.post<boolean>(`${environment.urlApi}/${this.dossier}/existe`, DATA);

    // recupere la 1ere valeur renvoyer
    // convertie en promise
    return await firstValueFrom(retour$);
  }
}
