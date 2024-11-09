import { Inject, inject, Injectable, PLATFORM_ID, signal } from '@angular/core';
import { OAuthService } from 'angular-oauth2-oidc';
import { authConfig } from '../../auth-config';
import { Router } from '@angular/router';
import { isPlatformBrowser } from '@angular/common';

@Injectable({
  providedIn: 'root'
})
export class ExternalAuthService {
  private oAuthService = inject(OAuthService);

  private router = inject(Router);
  private isBrowser:boolean;
  profile = signal<any>(null);

  constructor(@Inject(PLATFORM_ID) private platformId: object) {
    this.isBrowser = isPlatformBrowser(platformId);
    if(this.isBrowser)
      this.initConfiguration();

  }

  initConfiguration() {

    this.oAuthService.configure(authConfig);

    this.oAuthService.setupAutomaticSilentRefresh();

    this.oAuthService.loadDiscoveryDocumentAndTryLogin().then(() => {

      if (this.oAuthService.hasValidIdToken()) {

        this.profile.set(this.oAuthService.getIdentityClaims());

      }

    });

  }

  login() {
    if(this.isBrowser)
      this.oAuthService.initImplicitFlow();

  }

  logout() {

    this.oAuthService.revokeTokenAndLogout();

    this.oAuthService.logOut();

    this.profile.set(null);

  }

  getProfile() {

    return this.profile();

  } 
}
