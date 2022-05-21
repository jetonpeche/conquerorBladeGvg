import { DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
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
    let liste: any[] = [];
    
    for (let i = 1; i <= this.listeInput.length; i++) 
    {
      const DATE = this.datePipe.transform(_form.value[`Date${i}`], "dd/MM/yyyy")
      liste.push({ Date: DATE });
    }

    this.gvgServ.Ajouter(liste).subscribe({
      next: (listeId: number[]) =>
      {
        for (let i = 0; i < liste.length; i++) 
        {
          liste[i].Id = listeId[i];     
        }

        this.outilServ.ToastOK(`${ liste.length > 1 ? 'Les GvGs ont été programmées' : 'La GvG a été programmée' }`);
        this.dialogRef.close(liste);
      },
      error: () =>
      {
        this.outilServ.ToastErreurHttp();
      }
    });
  }
}
