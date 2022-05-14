import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root'
})
export class OutilService {

  constructor(private toastrServ: ToastrService) { }

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
