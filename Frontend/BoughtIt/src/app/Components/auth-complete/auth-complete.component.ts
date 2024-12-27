import { Component, Inject, OnInit, PLATFORM_ID } from '@angular/core';
import { LoadingSpinnerComponent } from '../Common/LoadingSpinner/loading-spinner.component';
import { ActivatedRoute, Router } from '@angular/router';
import { UserService } from '../../Services/UserService/user.service';
import { OAuthService } from 'angular-oauth2-oidc';
import { isPlatformBrowser } from '@angular/common';
import { authConfig } from '../../auth-config';
import { ExternalUserInfo } from '../../Models/ExternalUserInfo';

@Component({
  selector: 'app-auth-complete',
  standalone: true,
  imports: [LoadingSpinnerComponent],
  templateUrl: './auth-complete.component.html',
  styleUrl: './auth-complete.component.css'
})
export class AuthCompleteComponent implements OnInit {
  
  private isBrowser:boolean=false;
  constructor(private route:ActivatedRoute,@Inject(PLATFORM_ID) private platformId: object,
    private router:Router, private oAuthService:OAuthService,private userService:UserService
  ){
    this.isBrowser = isPlatformBrowser(platformId);
  }
  ngOnInit(): void {
    if (this.isBrowser) {
      this.oAuthService.configure(authConfig);
      this.oAuthService.loadDiscoveryDocument().then(() => {
        return this.oAuthService.tryLoginImplicitFlow();
      }).then(() => {
        if (this.oAuthService.hasValidAccessToken()) {
          var data = this.oAuthService.getIdentityClaims();
          var externalUserInfo = new ExternalUserInfo();
          externalUserInfo = {firstName:data["given_name"],lastName:data["family_name"],email:data["email"]};
          this.userService.externalLogin(externalUserInfo).subscribe((response)=>{
            if(response>0)this.router.navigateByUrl("/home");
            else this.router.navigateByUrl("/login");
          })
        } else {
          console.error('No valid access token!');
        }
      }).catch(err => {
        console.error('Error during login flow:', err);
      });
    }
  }
  
}
