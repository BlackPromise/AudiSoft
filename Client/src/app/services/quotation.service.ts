import { HttpClient, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

import { Quotation } from '../entities/quotation';

@Injectable({
  providedIn: 'root'
})
export class QuotationService {

  constructor(public http: HttpClient) { }

  Get(): Observable<HttpResponse<Quotation>> {
    return this.http.get<Quotation>(`${environment.apiBase}Quotation`, { observe: 'response' });
  }

  Post(quotation: Quotation) {
    return this.http.post(`${environment.apiBase}Quotation`, quotation, { observe: 'response' });
  }

  Delete() {
    return this.http.delete(`${environment.apiBase}Quotation`, { observe: 'response' });
  }

}
