import { inject, Injectable } from "@angular/core";
import { Router } from "@angular/router";

@Injectable({
  providedIn: "root",
})
export class AuthGuard {
  public canActivate: boolean = false;
}

export const canActivateAuth = () => {
  const canActivate = inject(AuthGuard).canActivate;
  if (canActivate) {
    return true;
  }
  const router = inject(Router);
  return router.navigate(["/settings"]);
};
