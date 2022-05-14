import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ModiferInfoCompteComponent } from './modal/modifer-info-compte/modifer-info-compte.component';
import { VariableStatic } from './Static/VariableStatic';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent 
{
  constructor(private dialog: MatDialog) { }

  EstConnecter(): Boolean
  {
    return VariableStatic.compte != undefined;
  }

  OuvrirModalModifInfoCompte(): void
  {
    this.dialog.open(ModiferInfoCompteComponent);
  }
}
