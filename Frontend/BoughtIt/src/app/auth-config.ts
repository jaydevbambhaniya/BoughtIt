import { AuthConfig } from 'angular-oauth2-oidc';

export const authConfig: AuthConfig = {

  issuer: 'https://accounts.google.com',

  redirectUri: '',

  clientId: '',

  scope: 'openid profile email',

  responseType:'code',

  strictDiscoveryDocumentValidation: false,

};