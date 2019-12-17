import { Component, OnInit, ViewChild } from '@angular/core';
import { ApiService } from '@app/core/services/api.service';
import { NotificationService } from '@app/core/services';
import { DatatableComponent } from '@swimlane/ngx-datatable';
import { Router } from '@angular/router';

@Component({
    selector: 'sa-adminUser-list',
    templateUrl: './adminUser.list.component.html',
    styleUrls: [

        // material theme from ngx-datatable teem
        // '../../../../node_modules/@swimlane/ngx-datatable/release/themes/material.css',
        // '../../../../node_modules/@swimlane/ngx-datatable/release/assets/icons.css',


        './smartadmin-ngx-datatable.css'
    ],
    styles: [`
      @media screen and (max-width: 800px) {
        .desktop-hidden {
          display: initial;
        }
        .mobile-hidden {
          display: none;
        }
      }
      @media screen and (min-width: 800px) {
        .desktop-hidden {
          display: none;
        }
        .mobile-hidden {
          display: initial;
        }
      }
    `]
})
export class AdminUserListComponent implements OnInit {

    rows = [];
    temp = [];
    loadingIndicator: boolean = true;
    currentCustomer: string;

    constructor(private apiService: ApiService, private notificationService: NotificationService, private router: Router) {

    }

    reorderable: boolean = true;

    pageSize: number = 10;

    controls: any = {
        pageSize: 10,
        filter: '',
    }

    columns = [
        { name: 'email' },
    ];

    @ViewChild(DatatableComponent) table: DatatableComponent;

    ngOnInit() {
        this.updateData();

    }

    updateData() {
        this.apiService.get('Accounts/admin').subscribe(data => {

            // cache our list
            this.temp = [...data];

            // push our inital complete list
            this.rows = data;

            this.loadingIndicator = false;
        })
    }

    updateFilter(event) {
        const val = event.target.value.toLowerCase();

        // filter our data
        const temp = this.temp.filter(function (d) {
            return !val || ['name', 'gender', 'company'].some((field: any) => {
                return d[field].toLowerCase().indexOf(val) !== -1
            })
        });

        // update the rows
        this.rows = temp;
        // Whenever the filter changes, always go back to the first page
        this.table.offset = 0;
    }


    updatePageSize(value) {

        if (!this.controls.filter) {
            // update the rows
            this.rows = [...this.temp];
            // Whenever the filter changes, always go back to the first page
            this.table.offset = 0;
        }

        this.controls.pageSize = parseInt(value)

        this.table.limit = this.controls.pageSize;

        window.dispatchEvent(new Event('resize'));

    }

    delete(value) {
        var self = this;
        this.apiService.delete("Accounts/admin", value).subscribe(data => {
            self.updateData();
        })

    }

}
