import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { AjouterGvgComponent } from './modal/ajouter-gvg/ajouter-gvg.component';
import { ModiferInfoCompteComponent } from './modal/modifer-info-compte/modifer-info-compte.component';
import { VariableStatic } from './Static/VariableStatic';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent 
{
  static listeGvgAjouter: any[] = [];

  constructor(private dialog: MatDialog) { }

  EstConnecter(): Boolean
  {
    return VariableStatic.compte != undefined;
  }

  OuvrirModalModifInfoCompte(): void
  {
    this.dialog.open(ModiferInfoCompteComponent);
  }

  OuvrirModalAjouterGvG(): void
  {
    const DIALOG_REF = this.dialog.open(AjouterGvgComponent);

    DIALOG_REF.afterClosed().subscribe({
      next: (listeGvg) =>
      {
        AppComponent.listeGvgAjouter = listeGvg;
      }
    });
  }
}
