import { Component, OnInit } from '@angular/core';
import { CornService } from '../../services/corn.service';
import { finalize } from 'rxjs/operators';
import { ClientIdService } from './../../utils/generateClientId';
import { CornPurchase } from 'src/app/models/corn.model';

@Component({
  selector: 'app-corn-store',
  templateUrl: './corn-store.component.html',
  styleUrls: ['./corn-store.component.scss']
})
export class CornStoreComponent implements OnInit {
  clientId: string = localStorage.getItem('clientId') || this.clientIdService.generateClientId();
  purchaseHistory: CornPurchase[] = [];
  loading = false;
  error: string = '';
  success: string = '';

  constructor(
    private cornService: CornService,
    private clientIdService: ClientIdService) { }

  ngOnInit() { }

  purchaseCorn() {
    this.loading = true;
    this.error = '';
    this.success = '';

    this.cornService.purchaseCorn(this.clientId)
      .pipe(finalize(() => this.loading = false))
      .subscribe({
        next: (response) => {
          this.success = response.message;
        },
        error: (error) => {
          this.error = error;
        }
      });
  }
}