import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { catchError, throwError } from 'rxjs';

export const concurrencyInterceptor: HttpInterceptorFn = (req, next) => {
  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      if (error.status === 409) {
        // Concurrency conflict
        console.error('Concurrency conflict detected:', error.error);
        // You can dispatch an event or show a notification here
      }
      return throwError(() => error);
    })
  );
};
