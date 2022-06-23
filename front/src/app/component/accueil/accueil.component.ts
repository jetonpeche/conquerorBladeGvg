import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { CompteService } from 'src/app/service/compte.service';
import { OutilService } from 'src/app/service/outil.service';
import { VariableStatic } from 'src/app/Static/VariableStatic';
import { Compte } from 'src/app/Types/Compte';

@Component({
  selector: 'app-accueil',
  templateUrl: './accueil.component.html',
  styleUrls: ['./accueil.component.scss']
})
export class AccueilComponent implements OnInit 
{
  btnClicker: boolean = false;

  constructor(
    private compteServ: CompteService, 
    private outilServ: OutilService,
    private router: Router
    ) { }

  ngOnInit(): void 
  {

  }

  Connexion(_form: NgForm): void
  {
    if(_form.invalid)
    {
      this.outilServ.ToastErreurForm();
      return;
    }

    if(this.btnClicker)
      return;

    this.btnClicker = true;

    this.compteServ.Connexion(_form.value.pseudo).subscribe({
      next: (retour: string | Compte) =>
      {
        this.btnClicker = false;

        if(typeof(retour) == "string")
          this.outilServ.ToastAttention(retour);
        else
        {
          VariableStatic.compte = retour as Compte;
          sessionStorage.setItem("compte", JSON.stringify(retour));
          this.router.navigate(["/menu"]);
        }
      },
      error: () =>
      {
        this.btnClicker = false;
      }
    });
  }

}
