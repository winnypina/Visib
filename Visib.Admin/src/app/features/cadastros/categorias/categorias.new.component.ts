import { Component, OnInit } from '@angular/core';
import { ApiService } from '@app/core/services/api.service';
import { NotificationService } from '@app/core/services';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
    selector: 'sa-categorias-new',
    templateUrl: './categorias.new.component.html',
})
export class CategoriasNovoComponent implements OnInit {

    categoria: any = {
        id:'',
        name: '',
        nameEnUs: '',
        nameEs: ''
    }

    constructor(private route: ActivatedRoute,private apiService: ApiService, private router: Router, private notificationService: NotificationService) {

    }

    save()
    {
        console.log(this.categoria);
        if(this.categoria.id === '')
        {
            this.categoria.id = Guid.newGuid();
            this.apiService.post('Category', this.categoria).subscribe(ret => {
                console.log(ret);
                this.router.navigate(['/cadastros/categorias']);
              }, err => {
                  console.log('Erro ao realizar requisição')
              });
        } else {
            this.apiService.put('Category', this.categoria).subscribe(ret => {
                console.log(ret);
                this.router.navigate(['/cadastros/categorias']);
              }, err => {
                  console.log('Erro ao realizar requisição')
              });
        }
        
    }

    ngOnInit() {
        let id = this.route.snapshot.paramMap.get('id');
        if(id)
        {
            var self = this;
            this.apiService.get('Category/' + id).subscribe(ret => {
                self.categoria = ret;
              }, err => {
                  console.log('Erro ao realizar requisição')
              });
        }
    }

}

class Guid {
    static newGuid() {
        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function(c) {
            var r = Math.random()*16|0, v = c == 'x' ? r : (r&0x3|0x8);
            return v.toString(16);
        });
    }
}
