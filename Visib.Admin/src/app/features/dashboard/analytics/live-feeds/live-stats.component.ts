import { Component, OnInit, Input, OnDestroy, NgZone } from '@angular/core';
import { ApiService } from '../../../../core/services/api.service';
import { JsonApiService } from '@app/core/services';

@Component({
  selector: 'live-stats-feed',
  templateUrl: './live-stats.component.html',
  styles: []
})
export class LiveStatsComponent implements OnInit, OnDestroy {

  public chartjsData: any;

  constructor(private jsonApiService: JsonApiService,private zone: NgZone, private apiService: ApiService) {
  }

  ngOnInit() {    
    this.jsonApiService.fetch( '/graphs/chartjs.json').subscribe((data)=>{
      this.chartjsData = data;
    })
    // this.apiService.get("Dashboard/UsersByDate").subscribe(ret => {
    //   this.chartjsData = {
    //     "labels": ["January", "February", "March", "April", "May", "June", "July"],
    //     "datasets": [
    //       {
    //         "label": "My First dataset",
    //         "backgroundColor": "rgba(220,220,220,0.5)",
    //         "borderColor": "rgba(220,220,220,0.8)",
    //         "hoverBackgroundColor": "rgba(220,220,220,0.75)",
    //         "hoverBorderColor": "rgba(220,220,220,1)",
    //         "data": [65, 59, 80, 81, 56, 55, 40]
    //       },
    //       {
    //         "label": "My Second dataset",
    //         "backgroundColor": "rgba(151,187,205,0.5)",
    //         "borderColor": "rgba(151,187,205,0.8)",
    //         "hoverBackgroundColor": "rgba(151,187,205,0.75)",
    //         "hoverBorderColor": "rgba(151,187,205,1)",
    //         "data": [28, 48, 40, 19, 86, 27, 90]
    //       }
    
    //     ]
    //   }
    //   console.log(ret);
    // }, err => {
    //   console.log('Erro ao realizar requisição')
    // });
  }

  @Input() public liveSwitch = false;




  updateStats() {
   
  }

  private interval;

  toggleSwitch() {

    if (this.liveSwitch) {
      this.interval = setInterval(() => {
        this.updateStats()
      }, 1000)
    } else {
      clearInterval(this.interval);
    }
  }

  ngOnDestroy() {
    this.interval && clearInterval(this.interval);
  }


}



