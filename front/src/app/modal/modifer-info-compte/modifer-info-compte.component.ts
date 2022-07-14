import { Component, ElementRef, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { MatButton } from '@angular/material/button';
import { MatCheckbox } from '@angular/material/checkbox';
import { MatDialogRef } from '@angular/material/dialog';
import { ECache } from 'src/app/enum/ECache';
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

  btnClicker: boolean = false;

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
    this.nomImg = this.compte.NomImgClasse;
    this.idClasseChoisi = this.compte.IdClasseHeros;

    this.ListerClasse();
    this.ListerUnite();
  }

  ClasseChoisi(_idClasse: number, _nomImage: string): void
  {
    this.idClasseChoisi = _idClasse;
    this.nomImg = _nomImage;
  }

  Filtrer(_idTypeUnite: number = 0, _idCouleurUnite: number = 0, _checkboxMesUnites: MatCheckbox, _checkboxMeta: MatCheckbox, _checkboxPasMesUnite: MatCheckbox): void
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
    if(_checkboxMesUnites.checked && !_checkboxPasMesUnite.checked)
      this.listeUnite = this.listeUnite.filter(u => u.EstChoisi == 1);

    // pas mes unite
    else if(!_checkboxMesUnites.checked && _checkboxPasMesUnite.checked)
      this.listeUnite = this.listeUnite.filter(u => u.EstChoisi != 1);

    // unités méta
      this.listeUnite = this.listeUnite.filter(u => u.EstMeta == _checkboxMeta.checked);
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
    if(_form.invalid || this.btnClicker)
      return;

    this.btnClicker = true;

    const DATA: ConfigCompteExport =
    {
      IdCompte: this.compte.Id,
      IdClasseHeros: this.idClasseChoisi,
      Pseudo: this.compte.Pseudo,
      Influance: _form.value.Influance,
      IdDiscord: this.compte.IdDiscord,
      ListeUniteNiv: this.listeUniteChoisi
    }

    this.compteServ.Modifier(DATA).subscribe({
      next: () =>
      {
        this.outilServ.ToastOK("Votre compte a été mise à jour");
        
        this.compte.NomImgClasse = this.nomImg;
        this.compte.Pseudo = DATA.Pseudo;
        this.compte.IdClasseHeros = DATA.IdClasseHeros;
        this.compte.Influance = DATA.Influance;
        this.compte.EstPremiereConnexion = 0;

        sessionStorage.setItem(ECache.COMPTE, JSON.stringify(this.compte));
        
        this.btnClicker = false;
        this.dialogRef.close();
      },
      error: () =>
      {
        this.btnClicker = false;
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
        }
      });
    }
  }

  DefinirUniteTemporaire(_unite: Unite, _event: Event): void
  {
    _event.stopPropagation();

    let unite = this.listeUniteChoisi.find(u => u.Id == _unite.Id);

    if(unite)
    {
      unite.EstTemporaire = !unite.EstTemporaire;
      _unite.EstTemporaire = unite.EstTemporaire;

      this.outilServ.ToastOK(`${_unite.Nom} ${_unite.EstTemporaire ? 'est' : 'n\'est plus'} temporaire`);
    }
  }

  ChoisirSuppUnite(_unite: Unite, _niveau: string, _matBtn: MatButton, _element: Event): void
  { 
    const INDEX = this.listeMesUnite.findIndex(u => u.Id == _unite.Id);

    if(INDEX != -1)
    {
      this.outilServ.ToastInfo("Cette unité ne peux pas être décochée");
      return;
    }
    
    let estTemporaire: boolean = _matBtn._elementRef.nativeElement.value == "true";

    if(_unite.EstChoisi == 1)
    {
      const INDEX = this.listeUniteChoisi.findIndex(u => u.Id == _unite.Id);
      this.listeUniteChoisi.splice(INDEX, 1);

      _unite.EstChoisi = 0;
      _unite.EstTemporaire = false;

      this.outilServ.ToastOK(`${_unite.Nom} plus selectionnée`);
    }
    else
    {
      _unite.EstChoisi = 1;
      _unite.EstTemporaire = estTemporaire;

      this.listeUniteChoisi.push({ Id: _unite.Id, Niveau: _niveau, EstTemporaire: estTemporaire });
      this.outilServ.ToastOK(`${_unite.Nom} selectionnée`);
    }

    console.log(this.listeUniteChoisi);
    
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
      }
    })
  }

  private ListerUnite(): void
  {
    this.uniteServ.ListerVisible().subscribe({
      next: (liste: Unite[]) =>
      {
        this.listeUniteClone = this.listeUnite = liste;
        this.ListerMesUnite();
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
          this.listeMesUnite = liste;

          for (const element of this.listeUnite) 
          {
            let unite = liste.find(u => u.Id == element.Id);

            if(unite)
            {
              element.EstChoisi = 1;
              element.EstTemporaire = unite.EstTemporaire;
              element.Niveau = unite.Niveau;
            }
          }
  
          this.listeUniteClone = this.listeUnite;          
        }
      }
    })
  }
}
