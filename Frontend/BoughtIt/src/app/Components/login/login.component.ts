import { Component, Inject, OnInit, PLATFORM_ID, ViewChild } from '@angular/core';
import { UserService } from '../../Services/UserService/user.service';
import { FormControl, FormGroup, FormsModule, NgModel, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { LoadingSpinnerComponent } from '../Common/LoadingSpinner/loading-spinner.component';
import { OAuthService } from 'angular-oauth2-oidc';
import { authConfig } from '../../auth-config';
import { isPlatformBrowser } from '@angular/common';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule,CommonModule,ReactiveFormsModule,LoadingSpinnerComponent],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent implements OnInit{
  @ViewChild('loadingSpinner') loadingSpinner:LoadingSpinnerComponent
  public loginForm:FormGroup
  public alert:string;
  private isBrowser:boolean=false;
  constructor(private userService:UserService,private router:Router,private oAuthService:OAuthService,
    @Inject(PLATFORM_ID) private platformId: object
  ){
    this.isBrowser = isPlatformBrowser(platformId);
    this.loginForm = new FormGroup({
      email: new FormControl('',[Validators.required,Validators.email]),
      password: new FormControl('',[Validators.required])
    });
    this.alert='';
    this.loadingSpinner = new LoadingSpinnerComponent();
  }
  ngOnInit(): void {
    if(this.isBrowser){
      this.oAuthService.configure(authConfig);
      this.oAuthService.loadDiscoveryDocumentAndTryLogin().then(() => {
        if (this.oAuthService.hasValidAccessToken()) {
          console.log('already logged in');
        }else console.log('please login');
      });
    }
  }
  onLoginClick(){
    if(this.loginForm.invalid)return;
    var {email,password} = this.loginForm.value;
    this.loadingSpinner.isLoading=true;
    this.userService.userLogin(email,password).subscribe((retVal:number)=>{
      this.loadingSpinner.isLoading=false;
      if(retVal==-1){
        this.alert='User with given username doesn\'t exists!';
      }else if(retVal==-2){
        this.alert='Username or password is incorrect!';
      }else if(retVal>0){
        this.router.navigateByUrl('/home');
      }else{
        
        this.alert='Something went wrong, please try again!';
      }
    });
  }
  googleLogin(){
    if (this.isBrowser) {
      this.oAuthService.initCodeFlow(undefined,{prompt:'select_account'});
    }
  }
}
