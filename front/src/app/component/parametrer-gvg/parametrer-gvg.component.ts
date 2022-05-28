import { Component, OnInit, ViewChild } from '@angular/core';
import { MatListOption } from '@angular/material/list';
import { ActivatedRoute } from '@angular/router';
import { GvgService } from 'src/app/service/gvg.service';
import { OutilService } from 'src/app/service/outil.service';
import { UniteCompteGvGExport } from 'src/app/Types/export/UniteCompteExport';
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

  btnClicker: boolean = false;
  filtreEstActiver: boolean = false;

  private idGvG: number;
  private idCompte: number;
  private listeUniteClone: UniteParticipant[] = [];
  private listeUniteCompteChoisi: UniteCompteGvGExport[] = [];

  private listeUniteDejaChoisiAsupprimer: UniteCompteGvGExport[] = [];

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
    this.filtreEstActiver = _estCocher;

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
        IdGvG: this.idGvG,
        EstDejaChoisi: this.listeUniteClone.find(u => u.Id == _unite.Id).EstDejaChoisi
      });

      if(_unite.EstDejaChoisi)
      {
        const INDEX = this.listeUniteDejaChoisiAsupprimer.findIndex(u => u.IdCompte == this.idCompte && u.IdUnite == _unite.Id);
        this.listeUniteDejaChoisiAsupprimer.splice(INDEX, 1);
      }

      this.influanceTotal += _unite.Influance;
    }
    else
    {
      const INDEX = this.listeUniteCompteChoisi.findIndex(u => u.IdUnite == _unite.Id && u.IdCompte == this.idCompte);
      this.listeUniteCompteChoisi.splice(INDEX, 1);

      if(_unite.EstDejaChoisi)
      {
        this.listeUniteDejaChoisiAsupprimer.push({
          IdCompte: this.idCompte,
          IdUnite: _unite.Id,
          IdGvG: this.idGvG,
          EstDejaChoisi: _unite.EstDejaChoisi
        })
      }

      this.influanceTotal -= _unite.Influance;
    }

    this.FiltreUniteInfluance(_checkboxFiltreEstCocher);
  }

  ListerUniteParticipant(_idCompte: number): void
  {
    const COMPTE = this.participant.ListeCompte.find(c => c.Id == +_idCompte);
    const EXISTE = this.listeUniteCompteChoisi.findIndex(c => c.IdCompte == COMPTE.Id);
    
    // ajout des unités si c la premiere fois qu'on clique sur le compte
    if(EXISTE == -1)
    {
      for (const element of COMPTE.ListeUnite)
      {  
        if(element.EstDejaChoisi)
        {
          this.listeUniteCompteChoisi.push({
            IdCompte: COMPTE.Id,
            IdGvG: this.idGvG,
            IdUnite: element.Id,
            EstDejaChoisi: true
          });
        }
      }
    }
    
    const LISTE_UNITE_COMPTE_CHOISI = this.listeUniteCompteChoisi.filter(u => u.IdCompte == COMPTE.Id);

    // calcul de l'influance total
    this.influanceTotal = 0;
    for (const element of LISTE_UNITE_COMPTE_CHOISI) 
    {
      this.influanceTotal += COMPTE.ListeUnite.find(u => u.Id == element.IdUnite).Influance;
    }

    this.idCompte = COMPTE.Id;
    this.influanceMax = COMPTE.Influance;
    this.listeUnite = this.listeUniteClone = COMPTE.ListeUnite;

    this.FiltreUniteInfluance(this.filtreEstActiver);
  }

  Valider(): void
  {
    if(this.listeUniteCompteChoisi.length == 0 && this.listeUniteDejaChoisiAsupprimer.length == 0)
    {
      this.outilServ.ToastAttention("Au moins une unité doit être choisi");
      return;
    }

    this.btnClicker = true;

    this.gvgServ.Parametrer(this.listeUniteCompteChoisi, this.listeUniteDejaChoisiAsupprimer).subscribe({
      next: () =>
      {
        this.ModifierUniteDejaChoisi(this.listeUniteCompteChoisi, true);
        this.ModifierUniteDejaChoisi(this.listeUniteDejaChoisiAsupprimer, false);
        
        this.listeUniteDejaChoisiAsupprimer.length = 0;

        this.outilServ.ToastOK("Les unités ont été defini");
        this.btnClicker = false;
      },
      error: () =>
      {
        this.btnClicker = false;
        this.outilServ.ToastErreurHttp();
      }
    });
  }

  private ModifierUniteDejaChoisi(_liste: UniteCompteGvGExport[], _estDejaChoisi: boolean): void
  {
    for (let element of _liste) 
    {
      const COMPTE = this.participant.ListeCompte.find(c => c.Id == element.IdCompte);
      let uniteCompte = COMPTE.ListeUnite.find(u => u.Id == element.IdUnite);

      uniteCompte.EstDejaChoisi = _estDejaChoisi;

      if(_estDejaChoisi)
        element.EstDejaChoisi = true;
    }
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
