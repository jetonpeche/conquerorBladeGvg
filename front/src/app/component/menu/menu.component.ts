import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { AppComponent } from 'src/app/app.component';
import { ModiferInfoCompteComponent } from 'src/app/modal/modifer-info-compte/modifer-info-compte.component';
import { ClasseHerosService } from 'src/app/service/classe-heros.service';
import { CompteService } from 'src/app/service/compte.service';
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
  listeGvg: Gvg[] = [];

  compte: Compte;
  nomImgClasse: string;
  panelOpenState = false;

  constructor(
    private classeHerosServ: ClasseHerosService,
    private compteServ: CompteService,
    private outilServ: OutilService,
    private dialog: MatDialog
    ) { }

  ngOnInit(): void 
  {
    this.compte = VariableStatic.compte;
    this.nomImgClasse = this.compte.NomImgClasse;

    if(this.compte.EstPremiereConnexion == 1)
      this.dialog.open(ModiferInfoCompteComponent);
  }

  GetNomImage(): string
  {
    return this.compte?.NomImgClasse ?? "";
  }

  private ListerGvg(): void
  {

  }
}
