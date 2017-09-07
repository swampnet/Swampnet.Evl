import { Injectable, Inject } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { Http, Response, Headers, RequestOptions, URLSearchParams } from '@angular/http';
import 'rxjs/Rx';
import 'rxjs/add/operator/toPromise';

@Injectable()
export class ApiService {
    headers: Headers;
    options: RequestOptions;


    constructor(
        private _http: Http,
        @Inject('BASE_URL') private _baseUrl: string) {

        this.headers = new Headers({
            'Content-Type': 'application/json',
            'Accept': 'q=0.8;application/json;q=0.9'
        });

        this.options = new RequestOptions({ headers: this.headers });
    }


    getRules() {

        return new Promise((resolve, reject) => {
            this._http.get(this._baseUrl + 'api/mocked/rules')
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


	getRule(id: string) {

		return new Promise((resolve, reject) => {
			this._http.get(this._baseUrl + 'api/mocked/rules/' + id)
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


    saveRule(rule: any) {
        let body = JSON.stringify(rule);

        return new Promise((resolve, reject) => {
            this._http.post(this._baseUrl + 'api/mocked/rules', body, this.options)
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


    getMetaData() {
        return new Promise((resolve, reject) => {
            this._http.get(this._baseUrl + 'api/mocked/meta')
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

    searchEvents(criteria: any) {
        let params = new URLSearchParams();
        for (let key in criteria) {
            params.set(key, criteria[key])
        }

        return new Promise((resolve, reject) => {
            this._http.get(this._baseUrl + 'api/mocked/events?' + params.toString())
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

    getEvent(id: string) {
        return new Promise((resolve, reject) => {
            this._http.get(this._baseUrl + 'api/mocked/events/' + id)
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