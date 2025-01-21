import { Component, OnInit } from '@angular/core';
import { CornService } from '../../services/corn.service';
import { CornPurchase, ClientStats } from '../../models/corn.model';
import { ClientIdService } from './../../utils/generateClientId';

@Component({
  selector: 'app-corn-history',
  templateUrl: './corn-history.component.html',
  styleUrls: ['./corn-history.component.scss']
})
export class CornHistoryComponent implements OnInit {
  clientId: string = localStorage.getItem('clientId') || this.clientIdService.generateClientId();
  purchaseHistory: CornPurchase[] = [];
  stats: ClientStats | null = null;
  loading = false;
  error: string = '';
  success: string = '';

  constructor(
    private cornService: CornService,
    private clientIdService: ClientIdService) { }

  ngOnInit() {
    this.loadClientData();
  }

  private loadClientData() {
    this.cornService.getPurchaseHistory(this.clientId)
      .subscribe({
        next: (history) => this.purchaseHistory = history,
        error: (error) => this.error = error
      });

    this.cornService.getClientStats(this.clientId)
      .subscribe({
        next: (stats) => this.stats = stats,
        error: (error) => this.error = error
      });
  }
}