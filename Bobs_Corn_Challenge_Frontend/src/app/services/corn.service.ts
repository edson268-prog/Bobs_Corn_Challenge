import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { CornPurchase, PurchaseResponse, ClientStats } from '../models/corn.model';

@Injectable({
  providedIn: 'root'
})
export class CornService {
  private readonly API_URL = 'https://localhost:7272/api/Corns'; // I could move this to a constants file too

  constructor(private http: HttpClient) { }

  purchaseCorn(clientId: string): Observable<PurchaseResponse> {
    return this.http.post<PurchaseResponse>(`${this.API_URL}/purchase`, { clientId })
      .pipe(catchError(this.handleError));
  }

  getPurchaseHistory(clientId: string): Observable<CornPurchase[]> {
    return this.http.get<CornPurchase[]>(`${this.API_URL}/history/${clientId}`)
      .pipe(catchError(this.handleError));
  }

  getClientStats(clientId: string): Observable<ClientStats> {
    return this.http.get<ClientStats>(`${this.API_URL}/stats/${clientId}`)
      .pipe(catchError(this.handleError));
  }

  private handleError(error: HttpErrorResponse) {
    let errorMessage = 'An error has occurred';
    
    if (error.error instanceof ErrorEvent) {
      errorMessage = `Error: ${error.error.message}`;
    } else {
      if (error.status === 429) {
        errorMessage = 'You must wait 1 minute between purchases';
      } else {
        errorMessage = `${error.error.message || 'Something went wrong, try again'}`;
      }
    }

    return throwError(() => errorMessage);
  }
}