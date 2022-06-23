import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Observable } from 'rxjs';
import { MenuComponent } from './component/menu/menu.component';
import { ECache } from './enum/ECache';
import { AjouterCompteComponent } from './modal/ajouter-compte/ajouter-compte.component';
import { AjouterGvgComponent } from './modal/ajouter-gvg/ajouter-gvg.component';
import { ModiferInfoCompteComponent } from './modal/modifer-info-compte/modifer-info-compte.component';
import { VariableStatic } from './Static/VariableStatic';
import { Gvg } from './Types/Gvg';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { map, shareReplay } from 'rxjs/operators';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit
{
  isHandset$: Observable<boolean> = this.breakpointObserver.observe(Breakpoints.Handset)
  .pipe(
    map(result => result.matches),
    shareReplay()
  );

  static listeGvgAjouter: any[] = [];

  constructor(private dialog: MatDialog, private breakpointObserver: BreakpointObserver, ) { }

  ngOnInit(): void 
  {
    if(sessionStorage.getItem(ECache.COMPTE))
      VariableStatic.compte = JSON.parse(sessionStorage.getItem(ECache.COMPTE));
  }

  Test(_isHandset$): boolean
  {
    if(!this.EstConnecter())
      return false;
    
    return _isHandset$;
  }

  EstConnecter(): Boolean
  {
    return VariableStatic.compte != undefined;
  }

  EstRoleAdmin(): boolean
  {
    return VariableStatic.compte?.EstAdmin == 1 ?? false;
  }

  FermerApresClick(_sidenav): void
  {
    if(_sidenav.mode == "side")
      return;

    _sidenav.toggle();
  }

  OuvrirModalModifInfoCompte(): void
  {
    this.dialog.open(ModiferInfoCompteComponent);
  }

  OuvrirModalAjouterCompte(): void
  {
    this.dialog.open(AjouterCompteComponent);
  }

  OuvrirModalAjouterGvG(): void
  {
    const DIALOG_REF = this.dialog.open(AjouterGvgComponent);

    DIALOG_REF.afterClosed().subscribe({
      next: (listeGvg: Gvg[]) =>
      {
        for (const element of listeGvg) 
        {
          MenuComponent.listeGvg.push({
            Id: element.Id,
            Date: element.Date,
            NbParticipant: 0
          });
        }
      }
    });
  }
}
