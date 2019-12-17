import { Component, OnInit } from '@angular/core';
import {Router} from "@angular/router";
import { FormGroup } from '@angular/forms';
import { ApiService } from '@app/core/services/api.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html'
})
export class LoginComponent implements OnInit {

  user : any =  {
    email: '',
    password: ''
  }

  constructor(private apiService:ApiService, private router:Router) { }

  ngOnInit() {
  }

  login(event){
    event.preventDefault();
    if(this.user.email !== '' && this.user.password !== '')
    {
        this.apiService.post('Auth/login', { UserName: this.user.email, Password: this.user.password }).subscribe(ret => {
          console.log(ret);
            localStorage.setItem('access_token', ret.auth_token);
            localStorage.setItem('user_id', ret.id);
            localStorage.setItem('expires_in', ret.expires_in);
            this.router.navigate(['/dashboard/analytics']);
        }, err => {
            console.log('Erro ao realizar requisição')
        });
    }
  }

}
