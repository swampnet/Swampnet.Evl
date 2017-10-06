import { Injectable, Inject } from '@angular/core';
import { APP_BASE_HREF } from '@angular/common';
import { Observable } from 'rxjs/Observable';
import { Http, Response, Headers, RequestOptions, URLSearchParams } from '@angular/http';
import 'rxjs/Rx';
import 'rxjs/add/operator/toPromise';
import { EventSearchCriteria, Cfg } from '../entities/entities';

@Injectable()
export class Globals {
    headers: Headers;
    options: RequestOptions;
    cfg: any;


    constructor(
        @Inject('BASE_URL') baseUrl: string,
        private http: Http) {

        this.headers = new Headers({
            'Content-Type': 'application/json',
            'Accept': 'q=0.8;application/json;q=0.9'
        });

        this.options = new RequestOptions({ headers: this.headers });

        this.loadCfg(baseUrl);
    }

	public criteria: EventSearchCriteria = {
		category: "Information",
		pageSize: 20,
		page: 0
    };


    private async loadCfg(baseUrl: string) {
        try {
            this.cfg = await this.get(baseUrl + 'home/cfg');
        }
        catch (err) {
            console.log(err);
        }
    }

    private get(resource: string) {
        return new Promise((resolve, reject) => {
            this.http.get(resource)
                .map(res => res.json())
                .catch((error: any) => {
                    console.error(error);
                    reject(error);
                    return Observable.throw(error.json().error || 'Server error');
                })
                .subscribe((data) => {
                    resolve(data);
                });
        });
    }
}