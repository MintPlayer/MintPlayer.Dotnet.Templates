import { HttpInterceptorFn } from '@angular/common/http';

export const xsrfInterceptor: HttpInterceptorFn = (req, next) => {
  // Get XSRF token from cookie
  const token = getCookie('XSRF-TOKEN');

  if (token && !req.headers.has('X-XSRF-TOKEN')) {
    req = req.clone({
      headers: req.headers.set('X-XSRF-TOKEN', token)
    });
  }

  return next(req);
};

function getCookie(name: string): string | null {
  const value = `; ${document.cookie}`;
  const parts = value.split(`; ${name}=`);
  if (parts.length === 2) {
    return parts.pop()?.split(';').shift() || null;
  }
  return null;
}
