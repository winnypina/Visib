import { Injectable } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';

@Injectable()
export class AuthService {

  constructor(private router: Router) {

  }

  login(): void {
    this.router.navigate(['/auth/login']);
  }

  logout(): void {
    console.log('aaa');
    localStorage.clear();
    this.login();
  }

  isAuthenticated(queryParams?: any, redirectToLogin: boolean = true): boolean {
  
    var access_token = localStorage.getItem('access_token');
    if (access_token && access_token !== '') {
      return true;
    }
    this.logout();
    return false;

  }

  isAuthorized(roles: string[]): boolean {
    return true;
  }

  authenticateUser(user, password) {

  }

}
