import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { ModiferInfoCompteComponent } from 'src/app/modal/modifer-info-compte/modifer-info-compte.component';
import { UniteGvgComponent } from 'src/app/modal/unite-gvg/unite-gvg.component';
import { GvgService } from 'src/app/service/gvg.service';
import { OutilService } from 'src/app/service/outil.service';
import { VariableStatic } from 'src/app/Static/VariableStatic';
import { ClasseHeros } from 'src/app/Types/ClasseHeros';
import { Compte } from 'src/app/Types/Compte';
import { Gvg } from 'src/app/Types/Gvg';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.scss']
})
export class MenuComponent implements OnInit 
{
  listeClasseHeros: ClasseHeros[] = [];
  static listeGvg: Gvg[] = [];

  compte: Compte;
  nomImgClasse: string;

  constructor(
    private outilServ: OutilService,
    private gvgServ: GvgService,
    private dialog: MatDialog,
    private router: Router
    ) { }

  ngOnInit(): void 
  {
    this.ListerGvg();

    this.compte = VariableStatic.compte;
    this.nomImgClasse = this.compte.NomImgClasse;

    if(this.compte.EstPremiereConnexion == 1)
      this.dialog.open(ModiferInfoCompteComponent);
  }

  EstAdmin(): boolean
  {
    return VariableStatic.compte.EstAdmin == 1;
  }

  Naviguer(_idGvG: number, _date: string): void
  {
    if(this.compte.EstAdmin)
    {
      this.router.navigate([`/parametrer-gvg/${_idGvG}`]);
      return;
    }

    this.dialog.open(UniteGvgComponent, { data: { idGvg: _idGvG, date: _date }});
  }

  GetNomImage(): string
  {
    return this.compte?.NomImgClasse ?? "";
  }

  GetListeGvG(): Gvg[]
  {
    return MenuComponent.listeGvg;
  }

  ParticipeDeja(_idGvg: number): boolean
  {
    if(this.compte == undefined)
      return false;

    const INDEX = this.compte.ListeIdGvgParticipe.findIndex(g => g == _idGvg);

    return INDEX != -1;
  }

  Participer(_gvg: Gvg, _event: Event): void
  {
    _event.stopPropagation();

    this.gvgServ.Participer(_gvg.Id, this.compte.Id).subscribe({
      next: () =>
      {
        this.compte.ListeIdGvgParticipe.push(_gvg.Id);
        _gvg.NbParticipant++;   
        
        this.outilServ.ToastOK(`Inscrit à la GvG du: ${_gvg.Date}`);
      }
    });
  }

  Absent(_gvg: Gvg, _event: Event): void
  {
    _event.stopPropagation();

    this.gvgServ.Absent(_gvg.Id, this.compte.Id).subscribe({
      next: () =>
      {
        const INDEX = this.compte.ListeIdGvgParticipe.findIndex(g => g == _gvg.Id);
        this.compte.ListeIdGvgParticipe.splice(INDEX, 1);

        _gvg.NbParticipant--;

        if(_gvg.NbParticipant < 0)
          _gvg.NbParticipant = 0

        this.outilServ.ToastOK(`Absent à la GvG du: ${_gvg.Date}`);
      }
    });
  }

  ConfirmerSupprimerGvG(_gvg: Gvg, _event: Event, _index: number): void
  {
    _event.stopPropagation();

    const TITRE = "Confirmation suppression GvG";
    const TEXT = `Confirmez-vous le suppression de la GvG du ${_gvg.Date} ?`;

    this.outilServ.OuvrirModalConfirmer(TITRE, TEXT);

    this.outilServ.reponseModalConfirmation.subscribe({
      next: (retour: boolean) =>
      { 
        if(retour)
          this.SupprimerGvG(_gvg.Id, _index);
      }
    });
  }

  private SupprimerGvG(_idGvg: number, _index: number): void
  {
    this.gvgServ.Supprimer(_idGvg).subscribe({
      next: () =>
      {
        MenuComponent.listeGvg.splice(_index, 1);
        this.outilServ.ToastOK("La GvG a été supprimée");
      }
    });
  }

  private ListerGvg(): void
  {
   this.gvgServ.Lister().subscribe({
     next: (retour: Gvg[]) =>
     {
        MenuComponent.listeGvg = retour;
     }
   });
  }
}
