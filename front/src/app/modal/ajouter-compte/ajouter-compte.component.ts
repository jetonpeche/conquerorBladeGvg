import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ClasseHerosService } from 'src/app/service/classe-heros.service';
import { CompteService } from 'src/app/service/compte.service';
import { OutilService } from 'src/app/service/outil.service';
import { ClasseHeros } from 'src/app/Types/ClasseHeros';

@Component({
  selector: 'app-ajouter-compte',
  templateUrl: './ajouter-compte.component.html',
  styleUrls: ['./ajouter-compte.component.scss']
})
export class AjouterCompteComponent implements OnInit 
{
  listeClasse: ClasseHeros[] = [];

  constructor(
    private classeHeroServ: ClasseHerosService, 
    private outilServ: OutilService,
    private comptServ: CompteService
    ) { }

  ngOnInit(): void 
  {
    this.ListerClasse();
  }

  AjouterCompte(_form: NgForm): void
  {
    if(_form.invalid)
      return;

    this.comptServ.Ajouter(_form.value).subscribe({
      next: (retour: boolean) =>
      {
        this.outilServ.ToastOK("Le compte à été ajouté");
      },
      error: () =>
      {
        this.outilServ.ToastErreurHttp();
      }
    });
  }

  private ListerClasse(): void
  {
    this.classeHeroServ.Lister().subscribe({
      next: (liste: ClasseHeros[]) =>
      {
        this.listeClasse = liste;
      },
      error: () =>
      {
        this.outilServ.ToastErreurHttp();
      }
    })
  }
}
