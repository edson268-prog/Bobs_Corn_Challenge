import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ClientIdService {

  generateClientId(): string {
    const id = 'client-' + Math.floor(Math.random() * 1000000).toString(); // Define random ID
    localStorage.setItem('clientId', id);
    return id;
  }
}