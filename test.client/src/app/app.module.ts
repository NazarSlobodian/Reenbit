import { NgModule, LOCALE_ID } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { CoreModule } from './core/core.module';
import { registerLocaleData } from '@angular/common';
import localeUk from '@angular/common/locales/uk';

registerLocaleData(localeUk);

@NgModule({
  declarations: [AppComponent],
  imports: [BrowserModule, AppRoutingModule, CoreModule],
  bootstrap: [AppComponent],
  providers: [
    provideHttpClient(withInterceptorsFromDi()),
    { provide: LOCALE_ID, useValue: 'uk-UA' }
  ],
})
export class AppModule { }
