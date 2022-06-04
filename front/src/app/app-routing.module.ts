import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AccueilComponent } from './component/accueil/accueil.component';
import { GvgParametrerComponent } from './component/gvg-parametrer/gvg-parametrer.component';
import { MenuComponent } from './component/menu/menu.component';
import { ParametrerGvgComponent } from './component/parametrer-gvg/parametrer-gvg.component';
import { AdminGuard } from './guard/admin.guard';

const routes: Routes = [
  { path: "", component: AccueilComponent },
  { path: "menu", component: MenuComponent },
  { path: "parametrer-gvg/:id", canActivate: [AdminGuard], component: ParametrerGvgComponent },
  { path: "gvg-parametrer", component: GvgParametrerComponent }

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
