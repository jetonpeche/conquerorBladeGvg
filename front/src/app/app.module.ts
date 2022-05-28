import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { ToastrModule } from 'ngx-toastr';
import { DatePipe } from '@angular/common';

import { AppRoutingModule } from './app-routing.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppComponent } from './app.component';
import { AccueilComponent } from './component/accueil/accueil.component';
import { MenuComponent } from './component/menu/menu.component';
import { MonCompteComponent } from './component/mon-compte/mon-compte.component';
import { ModiferInfoCompteComponent } from './modal/modifer-info-compte/modifer-info-compte.component';
import { AjouterGvgComponent } from './modal/ajouter-gvg/ajouter-gvg.component';
import { ParametrerGvgComponent } from './component/parametrer-gvg/parametrer-gvg.component';
import { UniteGvgComponent } from './modal/unite-gvg/unite-gvg.component';

import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatMenuModule } from '@angular/material/menu';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatSelectModule } from '@angular/material/select';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatDialogModule } from '@angular/material/dialog';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule, MAT_DATE_LOCALE  } from '@angular/material/core';
import { MatListModule } from '@angular/material/list';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatTableModule } from '@angular/material/table';
import { GvgParametrerComponent } from './component/gvg-parametrer/gvg-parametrer.component';


@NgModule({
  declarations: [
    AppComponent,
    AccueilComponent,
    MenuComponent,
    MonCompteComponent,
    ModiferInfoCompteComponent,
    AjouterGvgComponent,
    ParametrerGvgComponent,
    UniteGvgComponent,
    GvgParametrerComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot({
      timeOut: 3000,
      progressBar: true,
      progressAnimation: 'increasing'
      //positionClass: 
    }),
    MatInputModule,
    MatFormFieldModule,
    MatButtonModule,
    MatCardModule,
    MatMenuModule,
    MatToolbarModule,
    MatSelectModule,
    MatExpansionModule,
    MatDialogModule,
    MatCheckboxModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatListModule,
    MatProgressSpinnerModule,
    MatTableModule
  ],
  providers: [
    DatePipe, MenuComponent,
    { provide: MAT_DATE_LOCALE, useValue: 'fr-FR' }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
