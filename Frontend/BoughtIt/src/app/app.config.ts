import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideClientHydration } from '@angular/platform-browser';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { BrowserStorageService } from './Services/BrowserStorage/browserstorage.service';
import { provideState, provideStore, StoreModule } from '@ngrx/store';
import { CartWishlistEffects } from './Services/StoreManagement/Effects/CartWishlistEffects';
import { provideEffects } from '@ngrx/effects';
import { cartReducer, wishlistReducer } from './Services/StoreManagement/global.reducer';
import {provideRouterStore, routerReducer} from '@ngrx/router-store'
import { APIInterceptor } from './Services/Interceptor/api.interceptor';
import { provideOAuthClient } from 'angular-oauth2-oidc';

export const appConfig: ApplicationConfig = {
  providers: [provideZoneChangeDetection({ eventCoalescing: true }), provideRouter(routes), provideClientHydration(),
    provideHttpClient(withInterceptors([APIInterceptor])), BrowserStorageService,
     provideStore({
      router:routerReducer,
      wishlist:wishlistReducer
     }), 
     provideState({
      name:'cart',
      reducer:cartReducer
    }),
      provideEffects([CartWishlistEffects]),
      provideRouterStore(),
      provideOAuthClient()
    ]
};
