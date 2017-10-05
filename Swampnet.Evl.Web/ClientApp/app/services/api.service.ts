import { Injectable, Inject } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { Http, Response, Headers, RequestOptions, URLSearchParams } from '@angular/http';
import 'rxjs/Rx';
import 'rxjs/add/operator/toPromise';

@Injectable()
export class ApiService {
    headers: Headers;
    options: RequestOptions;
    private _baseUrl: string;

    constructor(
        private _http: Http) {

        //this._baseUrl = getApiRoot();
        //this._baseUrl = "http://localhost:5001/";
		this._baseUrl = "http://localhost:5000/";
		//this._baseUrl = "http://swampnet-evl-staging.azurewebsites.net/api/";
        
        this.headers = new Headers({
            'Content-Type': 'application/json',
            'Accept': 'q=0.8;application/json;q=0.9'
        });

        this.options = new RequestOptions({ headers: this.headers });
    }


    getRules() {
        return this.get('rules');
        //return new Promise((resolve, reject) => {
        //    this._http.get(this._baseUrl + 'rules')
        //        .map(res => res.json())
        //        .catch((error: any) => {
        //            console.error(error);
        //            reject(error);
        //            return Observable.throw(error.json().error || 'Server error');
        //        })
        //        .subscribe((data) => {
        //            resolve(data);
        //        });
        //});
    }


	getRule(id: string) {
        return this.get('rules/' + id);
		//return new Promise((resolve, reject) => {
		//	this._http.get(this._baseUrl + 'rules/' + id)
		//		.map(res => res.json())
		//		.catch((error: any) => {
		//			console.error(error);
		//			reject(error);
		//			return Observable.throw(error.json().error || 'Server error');
		//		})
		//		.subscribe((data) => {
		//			resolve(data);
		//		});
		//});
	}

    deleteRule(id: string) {
        return this.delete('rules/' + id);
        //return new Promise((resolve, reject) => {
        //    this._http.delete(this._baseUrl + 'rules/' + id)
        //        .catch((error: any) => {
        //            console.error(error);
        //            reject(error);
        //            return Observable.throw(error.json().error || 'Server error');
        //        })
        //        .subscribe((data) => {
        //            resolve(data);
        //        });
        //});
    }


    createRule(rule: any) {
        return this.post('rules', rule);
        //let body = JSON.stringify(rule);

        //return new Promise((resolve, reject) => {
        //    this._http.post(this._baseUrl + 'rules', body, this.options)
        //        .catch((error: any) => {
        //            console.error(error);
        //            reject(error);
        //            return Observable.throw(error.json().error || 'Server error');
        //        })
        //        .subscribe((data) => {
        //            resolve(data);
        //        });
        //});
    }


    updateRule(rule: any) {
        return this.put('rules/' + rule.id, rule);
        //let body = JSON.stringify(rule);

        //return new Promise((resolve, reject) => {
        //    this._http.put(this._baseUrl + 'rules/' + rule.id, body, this.options)
        //        .catch((error: any) => {
        //            console.error(error);
        //            reject(error);
        //            return Observable.throw(error.json().error || 'Server error');
        //        })
        //        .subscribe((data) => {
        //            resolve(data);
        //        });
        //});
    }


    getMetaData() {
        return this.get('meta');
        //return new Promise((resolve, reject) => {
        //    this._http.get(this._baseUrl + 'meta')
        //        .map(res => res.json())
        //        .catch((error: any) => {
        //            console.error(error);
        //            reject(error);
        //            return Observable.throw(error.json().error || 'Server error');
        //        })
        //        .subscribe((data) => {
        //            resolve(data);
        //        });
        //});
    }

    searchEvents(criteria: any) {
        let params = new URLSearchParams();
        for (let key in criteria) {
            params.set(key, criteria[key])
        }

        return this.get('events?' + params.toString());

        //return new Promise((resolve, reject) => {
        //    this._http.get(this._baseUrl + 'events?' + params.toString())
        //        .map(res => res.json())
        //        .catch((error: any) => {
        //            console.error(error);
        //            reject(error);
        //            return Observable.throw(error.json().error || 'Server error');
        //        })
        //        .subscribe((data) => {
        //            resolve(data);
        //        });
        //});
    }

    getEvent(id: string) {
        return this.get('events/' + id);
        //return new Promise((resolve, reject) => {
        //    this._http.get(this._baseUrl + 'events/' + id)
        //        .map(res => res.json())
        //        .catch((error: any) => {
        //            console.error(error);
        //            reject(error);
        //            return Observable.throw(error.json().error || 'Server error');
        //        })
        //        .subscribe((data) => {
        //            resolve(data);
        //        });
        //});
    }

	getSources() {
        return this.get('sources');
	}

    getCategories() {
        return this.get('categories');
    }


    get(resource: string) {
        return new Promise((resolve, reject) => {
            this._http.get(this._baseUrl + resource)
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
            this._http.post(this._baseUrl + resource, body, this.options)
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
            this._http.put(this._baseUrl + resource, body, this.options)
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
            this._http.delete(this._baseUrl + resource)
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