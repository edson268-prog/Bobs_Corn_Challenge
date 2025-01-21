import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { CornStoreComponent } from './components/corn-store/corn-store.component';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { CommonModule } from '@angular/common';
import { MatToolbarModule } from '@angular/material/toolbar';
import { RouterModule, Routes } from '@angular/router';
import { CornHistoryComponent } from './components/corn-history/corn-history.component';

const routes: Routes = [
  { path: 'store', component: CornStoreComponent },
  { path: 'history', component: CornHistoryComponent },
  // { path: '**', component: PageNotFoundComponent } // No time for this page
];

@NgModule({
  declarations: [
    AppComponent,
    CornStoreComponent,
    CornHistoryComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    NoopAnimationsModule,
    CommonModule,
    MatToolbarModule,
    RouterModule.forRoot(routes) // Set the routes
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
