import { HttpClient, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { environment } from '../../environments/environment.prod';
import { Product } from '../entities/product';

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  constructor(public http: HttpClient) { }

  Get(): Observable<HttpResponse<Product[]>> {
    return this.http.get<Product[]>(`${environment.apiBase}Product`, { observe: 'response' });
  }

}
