import { Injectable } from '@angular/core';
import { Http, Headers, RequestOptions, Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/Rx';

declare var $: any;

export class Filter {
    public Key: string;
    public Value: any;
    public ComparisonType: FilterComparisonType;
    constructor() {
        this.ComparisonType = FilterComparisonType.Equal;
    }
}

export class Ordering {
    public Field: string;
    public By: any;
}

export enum FilterComparisonType {
    Equal,
    Contains
}

@Injectable()
export class ApiService {

    private options: RequestOptions;

    constructor(private http: Http) { }

    mainUrl: string = 'https://admin.visib.com/api/';
    //mainUrl: string = 'https://localhost:44338/api/';

    get(domain: string, param?: any): Observable<any> {
        
        
        let url = this.mainUrl + domain;
        this.options = this.initializeOptions();
        if (param) {
            url += '/' + param;
        }        

        return this.http.get(url, this.options)
            .map(result => { return this.successResult(result) })
            .catch(error => { return this.errorResult(error) });
    }

    initializeOptions() {
        const headers = new Headers({
          'Content-Type': 'application/json'
        });
        
        return new RequestOptions({ headers: headers });
      }

    post(domain: string, data: any): Observable<any> {
        const url = this.mainUrl + domain;
        this.options = this.initializeOptions();
        return this.http.post(url, JSON.stringify(data), this.options)
          .map(result => { return this.successResult(result) })
          .catch(error => { return this.errorResult(error) });
      }

      

      put(domain: string, data: any): Observable<any> {
        const url = this.mainUrl + domain;
        this.options = this.initializeOptions();
        return this.http.put(url, JSON.stringify(data), this.options)
          .map(result => { return this.successResult(result) })
          .catch(error => { return this.errorResult(error) });
      }

      delete(domain: string, id: string): Observable<any> {
        const url = this.mainUrl + domain + '/' + id;
        this.options = this.initializeOptions();
        return this.http.delete(url)
          .map(result => { return null })
          .catch(error => { return null });
      }

    private successResult(result: Response): Observable<any> {
        return result.json();
    }

    private errorResult(error: Response): Observable<any> {
        console.log(error);
        return error.json();
    }




}
