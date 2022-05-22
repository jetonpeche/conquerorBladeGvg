import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ModiferInfoCompteComponent } from 'src/app/modal/modifer-info-compte/modifer-info-compte.component';
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
    private dialog: MatDialog
    ) { }

  ngOnInit(): void 
  {
    this.ListerGvg();

    this.compte = VariableStatic.compte;
    this.nomImgClasse = this.compte.NomImgClasse;

    if(this.compte.EstPremiereConnexion == 1)
      this.dialog.open(ModiferInfoCompteComponent);
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

  Participer(_gvg: Gvg): void
  {
    this.gvgServ.Participer(_gvg.Id, this.compte.Id).subscribe({
      next: () =>
      {
        this.compte.ListeIdGvgParticipe.push(_gvg.Id);
        _gvg.NbParticipant++;   
        
        this.outilServ.ToastOK(`Inscrit à la GvG du: ${_gvg.Date}`);
      },
      error: () =>
      {
        this.outilServ.ToastErreurHttp();
      }
    });
  }

  Absent(_gvg: Gvg): void
  {
    this.gvgServ.Absent(_gvg.Id, this.compte.Id).subscribe({
      next: () =>
      {
        const INDEX = this.compte.ListeIdGvgParticipe.findIndex(g => g == _gvg.Id);
        this.compte.ListeIdGvgParticipe.splice(INDEX, 1);

        _gvg.NbParticipant--;

        if(_gvg.NbParticipant < 0)
          _gvg.NbParticipant = 0

        this.outilServ.ToastOK(`Absent à la GvG du: ${_gvg.Date}`);
      },
      error: () =>
      {
        this.outilServ.ToastErreurHttp();
      }
    });
  }

  private ListerGvg(): void
  {
   this.gvgServ.Lister().subscribe({
     next: (retour: Gvg[]) =>
     {
        MenuComponent.listeGvg = retour;
     },
     error: () =>
     {
        this.outilServ.ToastErreurHttp();
     }
   });
  }
}
