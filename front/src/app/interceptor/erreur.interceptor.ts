import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { catchError, Observable, throwError } from 'rxjs';
import { OutilService } from '../service/outil.service';

@Injectable()
export class ErreurInterceptor implements HttpInterceptor {

  constructor(private outilServ: OutilService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> 
  {
    return next.handle(request).pipe(
      catchError(
        (erreur) =>
        {
          if(erreur.status == 500)
            this.outilServ.ToastAttention("Erreur interne c'est produite");
          else if(erreur.status == 0)
            this.outilServ.ToastErreurHttp();

          return throwError(() => null);
        }
      )
    );
  }
}
