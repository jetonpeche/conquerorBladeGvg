import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AccueilComponent } from './component/accueil/accueil.component';
import { GestionCompteComponent } from './component/gestion-compte/gestion-compte.component';
import { GestionUniteComponent } from './component/gestion-unite/gestion-unite.component';
import { GvgParametrerComponent } from './component/gvg-parametrer/gvg-parametrer.component';
import { MenuComponent } from './component/menu/menu.component';
import { ParametrerGvgComponent } from './component/parametrer-gvg/parametrer-gvg.component';
import { AdminGuard } from './guard/admin.guard';

const routes: Routes = [
  { path: "", component: AccueilComponent },
  { path: "menu", component: MenuComponent },
  { path: "parametrer-gvg/:id", canActivate: [AdminGuard], component: ParametrerGvgComponent, title: "Parametrer gvg" },
  { path: "gvg-parametrer", component: GvgParametrerComponent, title: "gvg du jour" },
  { path: "gestion-unite", canActivate: [AdminGuard], component: GestionUniteComponent, title: "gestion des unités" },
  { path: "gestion-des-comptes", canActivate: [AdminGuard], component: GestionCompteComponent, title: "gestion des comptes" }

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
