import { Component, OnInit } from '@angular/core';
import { MatCheckboxChange } from '@angular/material/checkbox';
import { OutilService } from 'src/app/service/outil.service';
import { UniteService } from 'src/app/service/unite.service';
import { Unite } from 'src/app/Types/Unite';

@Component({
  selector: 'app-gestion-unite',
  templateUrl: './gestion-unite.component.html',
  styleUrls: ['./gestion-unite.component.scss']
})
export class GestionUniteComponent implements OnInit 
{
  listeUnite: Unite[] = [];

  private listeUniteClone: Unite[] = [];
  private btnClicker: boolean = false;

  constructor(private uniteServ: UniteService, private outilServ: OutilService) { }

  ngOnInit(): void 
  {
    this.ListerUnite();
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

  FiltrerUniteMeta(_checkbox: MatCheckboxChange): void
  {
    if(_checkbox.checked)
      this.listeUnite = this.listeUniteClone.filter(u => u.EstMeta == true);
    else
      this.listeUnite = this.listeUniteClone;
  }

  UniteMetaPasMeta(_unite: Unite): void
  {
    if(this.btnClicker)
      return;

    this.btnClicker = true;

    this.uniteServ.ModifierMeta(_unite.Id, !_unite.EstMeta).subscribe({
      next: () =>
      {
        _unite.EstMeta = !_unite.EstMeta;
        this.btnClicker = false;
        this.outilServ.ToastOK(`L'unité ${_unite.Nom} ${_unite.EstMeta ? 'est méta' : 'n\'est plus méta'}`);
      },
      error: () =>
      {
        this.btnClicker = false;
      }
    });
  }

  ModifierVisibiliterUnite(_unite: Unite, _event: Event): void
  {
    _event.stopPropagation();

    if(this.btnClicker)
      return;

    this.btnClicker = true;

    this.uniteServ.ModifierVisibiliter(_unite.Id, !_unite.EstVisible).subscribe({
      next: () =>
      {
        _unite.EstVisible = !_unite.EstVisible;
        this.btnClicker = false;
        this.outilServ.ToastOK(`L'unite ${_unite.EstVisible ? 'est visibible' : 'n\'est plus visible'}`);
      },
      error: () =>
      {
        this.btnClicker = false;
      }
    });
  }

  private ListerUnite(): void
  {
    this.uniteServ.Lister().subscribe({
      next: (liste: Unite[]) =>
      {
        this.listeUnite = this.listeUniteClone = liste;
      }
    });
  }
}
