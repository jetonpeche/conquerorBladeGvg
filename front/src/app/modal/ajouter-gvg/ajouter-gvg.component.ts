import { DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { switchMap, tap } from 'rxjs';
import { GvgService } from 'src/app/service/gvg.service';
import { OutilService } from 'src/app/service/outil.service';

@Component({
  selector: 'app-ajouter-gvg',
  templateUrl: './ajouter-gvg.component.html',
  styleUrls: ['./ajouter-gvg.component.scss']
})
export class AjouterGvgComponent implements OnInit 
{
  listeInput: any[] = [];
  listeDate: any[] = [];

  formBloquer: boolean = false;

  constructor(
    private datePipe: DatePipe, 
    private dialogRef: MatDialogRef<AjouterGvgComponent>,
    private gvgServ: GvgService,
    private outilServ: OutilService) { }

  ngOnInit(): void {
  }

  GenererInput(_nbInput: number): void
  {
    this.listeInput.length = 0;

    for (let i = 1; i <= _nbInput; i++)
    {
      this.listeInput.push({ name: `Date${i}` });
    }
  }

  Ajouter(_form: NgForm): void
  {
    if(_form.invalid)
    {
      this.outilServ.ToastErreurForm();
      return;
    }
      
    if(this.listeDate.length == 0)
    {
      this.outilServ.ToastAttention(this.listeInput.length > 1 ? "Les dates choisies sont déjà programmées" : "La date choisie est déjà programmée");
      return;
    }

    this.gvgServ.Ajouter(this.listeDate).subscribe({
      next: (listeId: number[]) =>
      {
        for (let i = 0; i < this.listeDate.length; i++) 
        {
          this.listeDate[i].Id = listeId[i];
        }

        this.outilServ.ToastOK(`${ this.listeDate.length > 1 ? 'Les GvGs ont été programmées' : 'La GvG a été programmée' }`);
        this.dialogRef.close(this.listeDate);
      },
      error: () =>
      {
        this.outilServ.ToastErreurHttp();
      }
    });
  }

  async Existe(_date): Promise<void>
  { 
    let date = this.datePipe.transform(_date, "dd/MM/yyyy");
    let retour = await this.gvgServ.Existe(date);

    if(retour)
      this.outilServ.ToastAttention(`La GvG du ${date} est déjà programmée, elle ne sera pas prise en compte`);
    else
      this.listeDate.push({ Date: date });
  }
}
