import { Injectable } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { Subject } from 'rxjs';
import { ConfirmationComponent } from '../modal/confirmation/confirmation.component';

@Injectable({
  providedIn: 'root'
})
export class OutilService 
{
  reponseModalConfirmation: Subject<boolean>;

  constructor(private toastrServ: ToastrService, private dialog: MatDialog) { }

  OuvrirModalConfirmer(_titre: string, _msg: string): void
  {
    this.reponseModalConfirmation = new Subject();

    const DIALOG_REF = this.dialog.open(ConfirmationComponent, { data: { titre: _titre, message: _msg }});
    DIALOG_REF.afterClosed().subscribe({
      next: (retour: boolean) =>
      {
        this.reponseModalConfirmation.next(retour);
        this.reponseModalConfirmation.complete();
      }
    });
  }

  ToastOK(_msg: string): void
  {
    this.toastrServ.success(_msg);
  }

  ToastAttention(_msg: string): void
  {
    this.toastrServ.warning(_msg);
  }

  ToastInfo(_msg: string): void
  {
    this.toastrServ.info(_msg);
  }

  ToastErreurForm(): void
  {
    this.toastrServ.error("Veuillez completer le formulaire");
  }

  ToastErreurHttp(): void
  {
    this.toastrServ.error("Vous n'etes pas connecté a internet");
  }
}
