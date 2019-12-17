import { Component, OnInit } from '@angular/core';
import { ApiService } from '@app/core/services/api.service';
import { NotificationService } from '@app/core/services';
import { Router, ActivatedRoute } from '@angular/router';
import { HttpRequest, HttpEventType, HttpClient } from '@angular/common/http';

@Component({
    selector: 'sa-textTranslation-new',
    templateUrl: './textTranslation.new.component.html',
})
export class TextTranslationNovoComponent implements OnInit {

    textTranslation: any = {
        id: '',        
        screen: '',
        key: '',
        value: null,
        valueEnUs: '',
        valueEs: ''
    }

    public progress: number;
    public message: string;

    id: string;

    constructor(private http: HttpClient, private route: ActivatedRoute, private apiService: ApiService, private router: Router, private notificationService: NotificationService) {

    }

    save() {
        var self = this;
        if (this.textTranslation.id === '') {
           
            this.textTranslation.id = Guid.newGuid();
           
            this.apiService.post('TextTranslation', this.textTranslation).subscribe(ret => {
                console.log(ret);
                self.textTranslation = ret;
            }, err => {
                console.log('Erro ao realizar requisição')
            });
        } else {
            this.apiService.put('TextTranslation', this.textTranslation).subscribe(ret => {
                console.log(ret);
                this.router.navigate(['/cadastros/textTranslation']);
            }, err => {
                console.log('Erro ao realizar requisição')
            });
        }
    }

    ngOnInit() {
        let id = this.route.snapshot.paramMap.get('id');
        if (id) {
            var self = this;
            this.apiService.get('TextTranslation/' + id).subscribe(ret => {
                console.log(ret);
                self.textTranslation = ret;
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
