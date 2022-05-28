import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { GvgService } from 'src/app/service/gvg.service';
import { OutilService } from 'src/app/service/outil.service';
import { VariableStatic } from 'src/app/Static/VariableStatic';

@Component({
  selector: 'app-unite-gvg',
  templateUrl: './unite-gvg.component.html',
  styleUrls: ['./unite-gvg.component.scss']
})
export class UniteGvgComponent implements OnInit 
{
  idGvg: number;
  date: string = "";
  listeUnite: MatTableDataSource<any>;
  displayedColumns: string[] = ['Nom', 'Influance'];

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any,
    private gvgServ: GvgService,
    private outilServ: OutilService
    ) { }

  ngOnInit(): void 
  {
    this.listeUnite = new MatTableDataSource();

    this.idGvg = this.data.idGvg;
    this.date = this.data.date;

    this.ListerMesUniteGvG();
  }

  InfluanceTotal(): number
  {
    let total: number = 0;

    for (const element of this.listeUnite.data) 
    {
      total += element.Influance;  
    }

    return total;
  }

  private ListerMesUniteGvG(): void
  {
    this.gvgServ.ListerMesUniteGvg(this.idGvg, VariableStatic.compte.Id).subscribe({
      next: (retour) =>
      {
        this.listeUnite.data = retour;
      },
      error: () =>
      {
        this.outilServ.ToastErreurHttp();
      }
    });
  }

}
