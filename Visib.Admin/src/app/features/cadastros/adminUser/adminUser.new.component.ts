import { Component, OnInit } from '@angular/core';
import { ApiService } from '@app/core/services/api.service';
import { NotificationService } from '@app/core/services';
import { Router, ActivatedRoute } from '@angular/router';
import { HttpRequest, HttpEventType, HttpClient } from '@angular/common/http';

@Component({
    selector: 'sa-adminUser-new',
    templateUrl: './adminUser.new.component.html',
})
export class AdminUserNovoComponent implements OnInit {

    adminUser: any = {
        id: '',        
        email: '',
        password: ''
    }

    public progress: number;
    public message: string;

    id: string;

    constructor(private http: HttpClient, private route: ActivatedRoute, private apiService: ApiService, private router: Router, private notificationService: NotificationService) {

    }

    save() {
        var self = this;
        if (this.adminUser.id === '') {
           
            this.adminUser.id = Guid.newGuid();
           
            this.apiService.post('Accounts/admin', this.adminUser).subscribe(ret => {
                console.log(ret);
                self.adminUser = ret;
                this.router.navigate(['/cadastros/adminUser']);
            }, err => {
                console.log('Erro ao realizar requisição')
            });
        } else {
            this.apiService.put('Accounts/admin', this.adminUser).subscribe(ret => {
                console.log(ret);
                this.router.navigate(['/cadastros/adminUser']);
            }, err => {
                console.log('Erro ao realizar requisição')
            });
        }
    }

    ngOnInit() {
        let id = this.route.snapshot.paramMap.get('id');
        if (id) {
            var self = this;
            this.apiService.get('Accounts/admin/' + id).subscribe(ret => {
                console.log(ret);
                self.adminUser = ret;
            }, err => {
                console.log('Erro ao realizar requisição')
            });
        }
        
    }

}

class Guid {
    static newGuid() {
        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
            var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
            return v.toString(16);
        });
    }
}
