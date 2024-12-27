import { AfterViewInit, Component, ViewChild } from '@angular/core';
import { UserService } from '../../Services/UserService/user.service';
import { User } from '../../Models/User';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ReactiveFormsModule, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { LoadingSpinnerComponent } from "../Common/LoadingSpinner/loading-spinner.component";
import { AlertComponent } from "../Common/Alert/alert.component";
import { ErrorCodes } from '../../error-codes';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, LoadingSpinnerComponent, AlertComponent],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent implements AfterViewInit{
  @ViewChild('loadingSpinner') loadingSpinner!: LoadingSpinnerComponent;
  @ViewChild('messageBox') messageBox!: AlertComponent;
  public registerForm:FormGroup
  public alert:string=''
  constructor(private userService:UserService,private router:Router){
    this.registerForm = new FormGroup({
      firstname: new FormControl('',[Validators.required]),
      lastname: new FormControl('',[Validators.required]),
      email: new FormControl('',[Validators.required,Validators.email]),
      password: new FormControl('',[Validators.required,this.passwordValidator]),
      gender: new FormControl('-1',[Validators.required,this.genderValidator]),
      phoneNumber: new FormControl('',[Validators.required]),
      confirmPassword: new FormControl('',[Validators.required])
    },{validators: this.passwordMatchValidator});
  }
  ngAfterViewInit(): void {
    this.loadingSpinner = new LoadingSpinnerComponent();
    this.messageBox = new AlertComponent();
  }
  genderValidator(control:FormControl):ValidationErrors|null{
    return control.value=='-1'?{required:true}:null;
  }
  passwordMatchValidator: ValidatorFn = (group: AbstractControl): {[key: string]: any} | null => {
    const password = group.get('password');
    const confirmPassword = group.get('confirmPassword');
    return password && confirmPassword && password.value !== confirmPassword.value
      ? { 'passwordMismatch': true }
      : null;
  }
  passwordValidator(control:FormControl):ValidationErrors|null{
    const value: string = control.value || '';
    if(value=='')return null;
    const hasDigit = /[0-9]/.test(value);
    const hasLowerCase = /[a-z]/.test(value);
    const hasUpperCase = /[A-Z]/.test(value);
    const hasNonAlphanumeric = /[^a-zA-Z0-9]/.test(value);
    const hasMinLength = value.length >= 6;
    const passwordValid =
      hasDigit &&
      hasLowerCase &&
      hasUpperCase &&
      hasNonAlphanumeric &&
      hasMinLength
    if (!passwordValid) {
      return {
        passwordRequirements: {
          hasDigit,
          hasLowerCase,
          hasUpperCase,
          hasNonAlphanumeric,
          hasMinLength
        },
      };
    }

    return null;
  }
  onSubmitClick(){
    var user:User = this.registerForm.value;
    this.loadingSpinner.isLoading=true;
    this.userService.createUser(user).subscribe((retVal:number)=>{
      this.loadingSpinner.isLoading=false;
      if(retVal>0){
        this.messageBox.buttons = [{ text: 'Login', primary: true, action: () => this.navigateToLogin() }];
        this.messageBox.showAlert({title:"Message",message:"Registered"});
      }else{
        this.alert = ErrorCodes[`${retVal}`].message;
      }
    });
  }
  navigateToLogin(){
    this.messageBox.hideAlert();
    this.router.navigateByUrl("/login");
  }
}
