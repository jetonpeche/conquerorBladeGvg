import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import {animate, state, style, transition, trigger} from '@angular/animations';
import { GvgService } from 'src/app/service/gvg.service';
import { OutilService } from 'src/app/service/outil.service';
import { VariableStatic } from 'src/app/Static/VariableStatic';

export interface PeriodicElement {
  name: string;
  position: number;
  weight: number;
  symbol: string;
  description: string;
}

@Component({
  selector: 'app-gvg-parametrer',
  templateUrl: './gvg-parametrer.component.html',
  styleUrls: ['./gvg-parametrer.component.scss']
})
export class GvgParametrerComponent implements OnInit 
{
  nomCompte: string;
  dataSource: any[] = [];

  constructor(private gvgServ: GvgService, private outilServ: OutilService) { }

  ngOnInit(): void 
  {
    this.nomCompte = VariableStatic.compte.Pseudo;
    
    this.RecupereGvgParametrer();
  }

  RecupereGvgParametrer(): void
  {
    this.gvgServ.RecupererInfoGvgParametrer().subscribe({
      next: (retour) => 
      {
        this.dataSource = retour;
      }
    })
  }
}