import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';

import * as fromCalendar from "@app/core/store/calendar";
import { JsonApiService } from '@app/core/services';
import { ApiService } from '@app/core/services/api.service';

@Component({
  selector: 'sa-analytics',
  templateUrl: './analytics.component.html',
})
export class AnalyticsComponent implements OnInit {

  public chartjsData: any;
  constructor(private jsonApiService: JsonApiService, private apiService:ApiService) {
    
  }

  ngOnInit() {
     this.apiService.get("Dashboard/UsersByDate").subscribe(ret => {
      let labels = ret.map(a => new Date(a.date).toLocaleDateString());
      let data = ret.map(a => a.numberOfUsers);
      this.chartjsData = {
        "labels": labels,
        "datasets": [
          {
            "label": "My Second dataset",
            "backgroundColor": "rgba(151,187,205,0.5)",
            "borderColor": "rgba(151,187,205,0.8)",
            "hoverBackgroundColor": "rgba(151,187,205,0.75)",
            "hoverBorderColor": "rgba(151,187,205,1)",
            "data": data
          }
    
        ]
      }
      console.log(ret);
    }, err => {
      console.log('Erro ao realizar requisição')
    });
  }

}
