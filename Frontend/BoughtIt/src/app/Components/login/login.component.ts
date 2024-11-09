import { Component, ViewChild } from '@angular/core';
import { UserService } from '../../Services/UserService/user.service';
import { FormControl, FormGroup, FormsModule, NgModel, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { LoadingSpinnerComponent } from '../Common/LoadingSpinner/loading-spinner.component';
import { ExternalAuthService } from '../../Services/ExternalAuthService/external-auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule,CommonModule,ReactiveFormsModule,LoadingSpinnerComponent],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  @ViewChild('loadingSpinner') loadingSpinner:LoadingSpinnerComponent
  public loginForm:FormGroup
  public alert:string;
  constructor(private userService:UserService,private router:Router,private externalAuthService:ExternalAuthService){
    this.loginForm = new FormGroup({
      email: new FormControl('',[Validators.required,Validators.email]),
      password: new FormControl('',[Validators.required])
    });
    this.alert='';
    this.loadingSpinner = new LoadingSpinnerComponent();
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
        console.log(retVal);
        this.alert='Something went wrong, please try again!';
      }
    });
  }
  googleLogin(){
    this.externalAuthService.login();
  }
  facebookLogin(){

  }
}
