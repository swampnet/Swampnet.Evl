import { Injectable, Inject } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { Http, Response, Headers, RequestOptions, URLSearchParams } from '@angular/http';
import 'rxjs/Rx';
import 'rxjs/add/operator/toPromise';
import { Globals } from './globals';

@Injectable()
export class ApiService {
    headers: Headers;
    options: RequestOptions;

    constructor(
        private globals: Globals,
        private _http: Http) {

        this.headers = new Headers({
            'Content-Type': 'application/json',
            'Accept': 'q=0.8;application/json;q=0.9'
        });

        this.options = new RequestOptions({ headers: this.headers });
    }


	getProfile() {
		return this.get('profiles/current');
	}

	getRules() {
        return this.get('rules');
    }


	getRule(id: string) {
        return this.get('rules/' + id);
	}

    deleteRule(id: string) {
        return this.delete('rules/' + id);
    }


    createRule(rule: any) {
        return this.post('rules', rule);
    }


    updateRule(rule: any) {
        return this.put('rules/' + rule.id, rule);
    }


    getMetaData() {
        return this.get('meta');
    }

    searchEvents(criteria: any) {
        let params = new URLSearchParams();
        for (let key in criteria) {
            params.set(key, criteria[key])
        }

        return this.get('events?' + params.toString());
    }

    getEvent(id: string) {
        return this.get('events/' + id);
    }

	getSources() {
		return this.get('events/sources');
	}

    getCategories() {
        return this.get('events/categories');
    }

    getStats() {
        return this.get('stats');
    }

    get(resource: string) {
        return new Promise((resolve, reject) => {
            this._http.get(this.globals.cfg.apiRoot + resource)
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

    post(resource: string, o: any) {
        let body = JSON.stringify(o);

        return new Promise((resolve, reject) => {
            this._http.post(this.globals.cfg.apiRoot + resource, body, this.options)
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

    put(resource: string, o: any) {
        let body = JSON.stringify(o);

        return new Promise((resolve, reject) => {
            this._http.put(this.globals.cfg.apiRoot + resource, body, this.options)
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

    delete(resource: string) {
        return new Promise((resolve, reject) => {
            this._http.delete(this.globals.cfg.apiRoot + resource)
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