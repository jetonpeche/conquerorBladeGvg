import { Component, OnInit, ViewChild } from '@angular/core';
import { MatListOption } from '@angular/material/list';
import { ActivatedRoute } from '@angular/router';
import { GvgService } from 'src/app/service/gvg.service';
import { OutilService } from 'src/app/service/outil.service';
import { UniteCompteExport } from 'src/app/Types/export/UniteCompteExport';
import { ParticipantGvG, UniteParticipant } from 'src/app/Types/ParticipantGvg';

@Component({
  selector: 'app-parametrer-gvg',
  templateUrl: './parametrer-gvg.component.html',
  styleUrls: ['./parametrer-gvg.component.scss']
})
export class ParametrerGvgComponent implements OnInit 
{
  participant: ParticipantGvG;
  listeUnite: UniteParticipant[] = [];

  influanceMax: number = 0;
  influanceTotal: number = 0;
  dateGvG: string = "";

  private idGvG: number;
  private idCompte: number;
  private listeUniteClone: UniteParticipant[] = [];
  private listeUniteCompteChoisi: UniteCompteExport[] = [];

  constructor(
    private activateRoute: ActivatedRoute,
    private gvgServ: GvgService,
    private outilServ: OutilService
    ) { }

  ngOnInit(): void 
  {
    this.idGvG = +this.activateRoute.snapshot.paramMap.get("id");

    this.ListerParticipantGvG();
  }

  FiltreUniteInfluance(_estCocher: boolean): void
  {
    if(_estCocher)
      this.listeUnite = this.listeUniteClone.filter(u => u.Influance <= (this.influanceMax - this.influanceTotal));
    else
      this.listeUnite = this.listeUniteClone;
  }

  UniteEstChoisi(_idUnite: number): boolean
  {
    const INDEX = this.listeUniteCompteChoisi.findIndex(u => u.IdUnite == _idUnite && u.IdCompte == this.idCompte);

    return INDEX != -1;
  }

  CalculerInfluanceEtChoisiUnite(_option: MatListOption, _checkboxFiltreEstCocher: boolean, _unite: UniteParticipant): void
  {
    if(this.influanceMax - (this.influanceTotal + _unite.Influance) <= 0 && _option.selected)
    { 
      this.outilServ.ToastInfo("Ajout impossible, influance trop grande");
      
      // evite de tout decocher
      if(+_option.value == _unite.Id)
        _option.selected = false;

      return;
    }

    if(_option.selected)
    {
      if(this.UniteEstChoisi(_unite.Id))
        return;

      this.listeUniteCompteChoisi.push({
        IdCompte: this.idCompte,
        IdUnite: _unite.Id,
        IdGvG: this.idGvG
      });

      this.influanceTotal += _unite.Influance;
    }
    else
    {
      const INDEX = this.listeUniteCompteChoisi.findIndex(u => u.IdUnite == _unite.Id && u.IdCompte == this.idCompte);
      this.listeUniteCompteChoisi.splice(INDEX, 1);

      this.influanceTotal -= _unite.Influance;
    }

    this.FiltreUniteInfluance(_checkboxFiltreEstCocher);
  }

  ListerUniteParticipant(_idCompte: number): void
  {
    const COMPTE = this.participant.ListeCompte.find(c => c.Id == +_idCompte);

    const LISTE_UNITE_COMPTE = this.listeUniteCompteChoisi.filter(u => u.IdCompte == COMPTE.Id);

    this.influanceTotal = 0;
    for (const element of LISTE_UNITE_COMPTE) 
    {
      this.influanceTotal += COMPTE.ListeUnite.find(u => u.Id == element.IdUnite).Influance;
    }

    this.idCompte = COMPTE.Id;
    this.influanceMax = COMPTE.Influance;
    this.listeUnite = this.listeUniteClone = COMPTE.ListeUnite;
  }

  private ListerGroupe(): void
  {

  }

  private ListerParticipantGvG(): void
  {
    this.gvgServ.ListerParticipant(this.idGvG).subscribe({
      next: (retour: ParticipantGvG) =>
      {        
        this.participant = retour[0];
        this.dateGvG = this.participant.Date;      
      },
      error: () =>
      {
        this.outilServ.ToastErreurHttp();
      }
    });
  }
}
