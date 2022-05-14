import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { MatCheckbox, MatCheckboxChange } from '@angular/material/checkbox';
import { MatDialogRef } from '@angular/material/dialog';
import { ClasseHerosService } from 'src/app/service/classe-heros.service';
import { CompteService } from 'src/app/service/compte.service';
import { OutilService } from 'src/app/service/outil.service';
import { UniteService } from 'src/app/service/unite.service';
import { VariableStatic } from 'src/app/Static/VariableStatic';
import { ClasseHeros } from 'src/app/Types/ClasseHeros';
import { Compte } from 'src/app/Types/Compte';
import { ConfigCompteExport } from 'src/app/Types/export/ConfigCompteExport';
import { MesUnite } from 'src/app/Types/import/MesUnite';
import { Unite } from 'src/app/Types/Unite';

@Component({
  selector: 'app-modifer-info-compte',
  templateUrl: './modifer-info-compte.component.html',
  styleUrls: ['./modifer-info-compte.component.scss']
})
export class ModiferInfoCompteComponent implements OnInit {

  listeClasseHero: ClasseHeros[] = [];
  listeUnite: Unite[] = [];

  compte: Compte;
  idClasseChoisi: number;
  nomImg: string;

  private listeUniteChoisi: MesUnite[] = [];
  private listeUniteClone: Unite[] = [];
  private listeMesUnite: MesUnite[] = [];

  constructor(
    private classeHeroServ: ClasseHerosService, 
    private outilServ: OutilService,
    private uniteServ: UniteService,
    private compteServ: CompteService,
    private dialogRef: MatDialogRef<ModiferInfoCompteComponent>
    ) { }

  ngOnInit(): void 
  {
    this.compte = VariableStatic.compte;
    this.idClasseChoisi = this.compte.IdClasseHeros;

    this.ListerClasse();
    this.ListerUniter();
  }

  ClasseChoisi(_idClasse: number, _nomImage: string): void
  {
    this.idClasseChoisi = _idClasse;
    this.nomImg = _nomImage;
  }

  Filtrer(_idTypeUnite: number = 0, _idCouleurUnite: number = 0, _checkbox: MatCheckbox): void
  {
    if(_idTypeUnite == 0 && _idCouleurUnite == 0)
    {
      this.listeUnite = this.listeUniteClone;
    }
    else if(_idCouleurUnite != 0 && _idTypeUnite != 0)
    {
      this.listeUnite = this.listeUniteClone.filter(u => u.IdTypeUnite == _idTypeUnite && u.IdCouleur == _idCouleurUnite);
    }
    else if(_idCouleurUnite != 0 && _idTypeUnite == 0)
    {
      this.listeUnite = this.listeUniteClone.filter(u => u.IdCouleur == _idCouleurUnite);
    }  
    else if(_idCouleurUnite == 0 && _idTypeUnite != 0)
    {
      this.listeUnite = this.listeUniteClone.filter(u => u.IdTypeUnite == _idTypeUnite);
    }  

    // mes unités
    if(_checkbox.checked)
      this.listeUnite = this.listeUnite.filter(u => u.EstChoisi == 1);
  }

  Recherche(_recherche: string): void
  {
    if(_recherche == "")
    {
      this.listeUnite = this.listeUniteClone;
      return;
    }

    _recherche = _recherche.toLowerCase();

    this.listeUnite = this.listeUniteClone.filter(u => u.Nom.toLowerCase().includes(_recherche) || u.NomTypeUnite.toLowerCase().includes(_recherche))
  }

  Enregistrer(_form: NgForm): void
  {
    const DATA: ConfigCompteExport =
    {
      IdCompte: this.compte.Id,
      IdClasseHeros: this.idClasseChoisi,
      Pseudo: this.compte.Pseudo,
      Influance: _form.value.Influance,
      IdDiscord: _form.value.IdDiscord.toString(),
      ListeUniteNiv: this.listeUniteChoisi
    }

    this.compteServ.Modifier(DATA).subscribe({
      next: () =>
      {
        this.outilServ.ToastOK("Votre a été mise à jour");
        
        this.compte.NomImgClasse = this.nomImg;
        this.compte.IdDiscord = DATA.IdDiscord;
        this.dialogRef.close();
      },
      error: () =>
      {
        this.outilServ.ToastErreurHttp();
      }
    });
  }

  // modifie le niveau avant de choisir OU dialog BDD changer lvl de mon unité
  ModifierNiveau(_idUnite: number, _niveau: string): void
  {
    let unite = this.listeUniteChoisi.find(u => u.Id == _idUnite);
    
    if(unite)
      unite.Niveau = _niveau;
    else
    {
      const INDEX = this.listeMesUnite.findIndex(u => u.Id == _idUnite);

      if(INDEX == -1)
        return;
        
      this.uniteServ.ModifierLvl(this.compte.Id, _idUnite, _niveau).subscribe({
        next: () =>
        {
          this.outilServ.ToastOK("Le niveau a été modifié");
        },
        error: () =>
        {
          this.outilServ.ToastErreurHttp();
        }
      });
    }
  }

  ChoisirSuppUnite(_unite: Unite, _niveau: string, _element: Event): void
  {
    const INDEX = this.listeMesUnite.findIndex(u => u.Id == _unite.Id);

    if(INDEX != -1)
    {
      this.outilServ.ToastInfo("Cette unité ne peux pas être décochée");
      return;
    }

    if(_unite.EstChoisi == 1)
    {
      const INDEX = this.listeUniteChoisi.findIndex(u => u.Id == _unite.Id);
      this.listeUniteChoisi.splice(INDEX, 1);

      _unite.EstChoisi = 0;

      this.outilServ.ToastOK(`${_unite.Nom} plus selectionnée`);
    }
    else
    {
      _unite.EstChoisi = 1;

      this.listeUniteChoisi.push({ Id: _unite.Id, Niveau: _niveau });
      this.outilServ.ToastOK(`${_unite.Nom} selectionnée`);
    }
  }

  GetNiveauMonUnite(_id: number): string
  {
    return this.listeMesUnite.find(u => u.Id == +_id)?.Niveau ?? "";
  }

  private ListerClasse(): void
  {
    this.classeHeroServ.Lister().subscribe({
      next: (liste: ClasseHeros[]) =>
      {        
        this.listeClasseHero = liste;
      },
      error: () =>
      {
        this.outilServ.ToastErreurHttp();
      }
    })
  }

  private ListerUniter(): void
  {
    this.uniteServ.Lister().subscribe({
      next: (liste: Unite[]) =>
      {
        this.listeUniteClone = this.listeUnite = liste;
        this.ListerMesUnite();
      },
      error: () =>
      {
        this.outilServ.ToastErreurHttp();
      }
    });
  }

  private ListerMesUnite(): void
  {
    this.uniteServ.ListerMesUnite(this.compte.Id).subscribe({
      next: (liste: MesUnite[]) =>
      {   
        if(liste.length > 0)
        {
          console.log(liste);
          
          this.listeMesUnite = liste;

          for (const element of this.listeUnite) 
          {
            let unite = liste.find(u => u.Id == element.Id);

            if(unite)
            {
              element.EstChoisi = 1;
              element.Niveau = unite.Niveau;
            }
          }
  
          this.listeUniteClone = this.listeUnite;
        }
      },  
      error: () =>
      {
        this.outilServ.ToastErreurHttp();
      }
    })
  }
}
