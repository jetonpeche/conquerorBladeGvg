import { Component, OnInit } from '@angular/core';
import { MatCheckbox } from '@angular/material/checkbox';
import { ActivatedRoute } from '@angular/router';
import { GroupeService } from 'src/app/service/groupe.service';
import { GvgService } from 'src/app/service/gvg.service';
import { OutilService } from 'src/app/service/outil.service';
import { UniteCompteGvGExport } from 'src/app/Types/export/UniteCompteExport';
import { Groupe } from 'src/app/Types/Groupe';
import { Participant, ParticipantGvG, UniteParticipant } from 'src/app/Types/ParticipantGvg';

@Component({
  selector: 'app-parametrer-gvg',
  templateUrl: './parametrer-gvg.component.html',
  styleUrls: ['./parametrer-gvg.component.scss']
})
export class ParametrerGvgComponent implements OnInit 
{
  participant: ParticipantGvG;
  compte: Participant;
  listeUnite: UniteParticipant[] = [];
  listeGroupe: Groupe[] = [];

  influanceTotal: number = 0;
  dateGvG: string = "";

  btnClicker: boolean = false;
  filtreEstActiver: boolean = false;

  private idGvG: number;
  private listeUniteClone: UniteParticipant[] = [];
  private listeUniteCompteChoisi: UniteCompteGvGExport[] = [];

  private listeUniteDejaChoisiAsupprimer: UniteCompteGvGExport[] = [];

  constructor(
    private activateRoute: ActivatedRoute,
    private gvgServ: GvgService,
    private outilServ: OutilService,
    private grpServ: GroupeService
    ) { }

  ngOnInit(): void 
  {
    this.idGvG = +this.activateRoute.snapshot.paramMap.get("id");

    this.ListerParticipantGvG();
    this.ListerGroupe();
  }

  FiltreUniteInfluance(_estCocher: boolean): void
  {
    this.filtreEstActiver = _estCocher;

    if(_estCocher)
      this.listeUnite = this.listeUniteClone.filter(u => u.Influance <= (this.compte.Influance - this.influanceTotal));
    else
      this.listeUnite = this.listeUniteClone;
  }

  UniteEstChoisi(_idUnite: number): boolean
  {
    const INDEX = this.listeUniteCompteChoisi.findIndex(u => u.IdUnite == _idUnite && u.IdCompte == this.compte.Id);

    return INDEX != -1;
  }

  CalculerInfluanceEtChoisiUnite(_option: MatCheckbox, _checkboxFiltreEstCocher: boolean, _unite: UniteParticipant): void
  {
    if(this.compte.Influance - (this.influanceTotal + _unite.Influance) <= 0 && _option.checked)
    { 
      this.outilServ.ToastInfo("Ajout impossible, influance trop grande");
      
      // evite de tout decocher
      if(+_option.value == _unite.Id)
        _option.checked = false;

      return;
    }

    if(_option.checked)
    {
      if(this.UniteEstChoisi(_unite.Id))
        return;

      this.listeUniteCompteChoisi.push({
        IdCompte: this.compte.Id,
        IdUnite: _unite.Id,
        IdGvG: this.idGvG,
        EstDejaChoisi: this.listeUniteClone.find(u => u.Id == _unite.Id).EstDejaChoisi
      });

      if(_unite.EstDejaChoisi)
      {
        const INDEX = this.listeUniteDejaChoisiAsupprimer.findIndex(u => u.IdCompte == this.compte.Id && u.IdUnite == _unite.Id);
        this.listeUniteDejaChoisiAsupprimer.splice(INDEX, 1);
      }

      this.influanceTotal += _unite.Influance;
    }
    else
    {
      const INDEX = this.listeUniteCompteChoisi.findIndex(u => u.IdUnite == _unite.Id && u.IdCompte == this.compte.Id);
      this.listeUniteCompteChoisi.splice(INDEX, 1);

      if(_unite.EstDejaChoisi)
      {
        this.listeUniteDejaChoisiAsupprimer.push({
          IdCompte: this.compte.Id,
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
    const EXISTE = this.listeUniteCompteChoisi.findIndex(c => c.IdCompte == +_idCompte);

    this.compte = COMPTE;

    // ajout des unités si c la premiere fois qu'on clique sur le compte
    if(EXISTE == -1)
    {
      for (const element of this.compte.ListeUnite)
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
    
    const LISTE_UNITE_COMPTE_CHOISI = this.listeUniteCompteChoisi.filter(u => u.IdCompte == this.compte.Id);

    // calcul de l'influance total
    this.influanceTotal = 0;
    for (const element of LISTE_UNITE_COMPTE_CHOISI) 
    {
      this.influanceTotal += COMPTE.ListeUnite.find(u => u.Id == element.IdUnite).Influance;
    }

    this.listeUnite = this.listeUniteClone = this.compte.ListeUnite;

    this.FiltreUniteInfluance(this.filtreEstActiver);
  }

  ModifierCompteGroupe(_idGroupe: number): void
  {
    this.grpServ.ModifierCompteGroupe(this.compte.Id, this.idGvG, _idGroupe).subscribe({
      next: (retour: boolean) =>
      {
        this.compte.IdGroupe = _idGroupe;
        this.outilServ.ToastOK("Groupe mis à jour");
      },
      error: () =>
      {
        this.outilServ.ToastErreurHttp();
      }
    });
  }

  Valider(): void
  {
    if(this.listeUniteCompteChoisi.length == 0 && this.listeUniteDejaChoisiAsupprimer.length == 0)
    {
      this.outilServ.ToastAttention("Au moins une unité doit être choisie");
      return;
    }

    if(this.compte.IdGroupe == null)
    {
      this.outilServ.ToastInfo("Veuillez choisir un groupe");
      return;
    }

    this.btnClicker = true;

    this.gvgServ.Parametrer(this.listeUniteCompteChoisi, this.listeUniteDejaChoisiAsupprimer).subscribe({
      next: () =>
      {
        this.ModifierUniteDejaChoisi(this.listeUniteCompteChoisi, true);
        this.ModifierUniteDejaChoisi(this.listeUniteDejaChoisiAsupprimer, false);
        
        this.listeUniteDejaChoisiAsupprimer.length = 0;

        this.outilServ.ToastOK("Les unités ont été definies");
        this.btnClicker = false;
      },
      error: () =>
      {
        this.btnClicker = false;
        this.outilServ.ToastErreurHttp();
      }
    });
  }

  private ListerGroupe(): void
  {
    this.grpServ.Lister().subscribe({
      next: (liste: Groupe[]) =>
      {
        this.listeGroupe = liste;
      },
      error: () =>
      {
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
