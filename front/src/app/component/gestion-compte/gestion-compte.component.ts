import {AfterViewInit, OnInit, Component, ViewChild} from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import {MatPaginator} from '@angular/material/paginator';
import {MatSort} from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ConfirmationComponent } from 'src/app/modal/confirmation/confirmation.component';
import { CompteService } from 'src/app/service/compte.service';
import { OutilService } from 'src/app/service/outil.service';
import { Compte } from 'src/app/Types/Compte';

@Component({
  selector: 'app-gestion-compte',
  templateUrl: './gestion-compte.component.html',
  styleUrls: ['./gestion-compte.component.scss']
})
export class GestionCompteComponent implements OnInit, AfterViewInit 
{
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  displayedColumns: string[] = ['Pseudo', 'NomClasseHeros', 'Influance', 'action'];

  listeCompte: MatTableDataSource<Compte>;
  btnClicker: boolean = false;

  constructor(
    private compteServ: CompteService,
    private outilServ: OutilService,
    private dialog: MatDialog) { }

  ngOnInit(): void 
  {
    this.listeCompte = new MatTableDataSource();
    this.ListerCompte();
  }

  ngAfterViewInit(): void 
  {
    this.listeCompte.sort = this.sort;
    this.listeCompte.paginator = this.paginator;
    this.listeCompte.paginator._intl.itemsPerPageLabel = "Comptes par page";
  }

  OuvrirModalConfirmer(_compte: Compte, _index: number): void
  {
    if(this.btnClicker)
      return;

    const TITRE = "Suppression de compte";
    const MESSAGE = `Veuillez confirmer la suppression du compte de: ${_compte.Pseudo}`;

    this.outilServ.OuvrirModalConfirmer(TITRE, MESSAGE);
    
    this.outilServ.reponseModalConfirmation.subscribe({
      next: (retour: boolean) =>
      {
        if(retour)
          this.SupprimerCompte(_compte.Id, _index);
      }
    });
  }

  Recherche(_event: Event): void
  {
    const filterValue = (_event.target as HTMLInputElement).value;
    this.listeCompte.filter = filterValue.trim().toLowerCase();

    if (this.listeCompte.paginator)
      this.listeCompte.paginator.firstPage();
  }

  private SupprimerCompte(_idCompte: number, _index: number): void
  {
    this.btnClicker = true;

    this.compteServ.Supprimer(_idCompte).subscribe({
      next: (retour: boolean) =>
      {
        console.log(retour);

        this.listeCompte.data.splice(_index, 1);
        this.listeCompte.data = this.listeCompte.data;

        this.btnClicker = false;
        this.outilServ.ToastOK("Le compte a été supprimé");
      },
      error: () =>
      {
        this.outilServ.ToastErreurHttp();
      }
    });
  }

  private ListerCompte(): void
  {
    this.compteServ.Lister().subscribe({
      next: (retour: Compte[]) =>
      {
        this.listeCompte.data = retour;
      },
      error: () =>
      {
        this.outilServ.ToastErreurHttp();
      }
    });
  }

}
