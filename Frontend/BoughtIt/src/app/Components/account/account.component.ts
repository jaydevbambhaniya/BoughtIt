import { Component, OnInit, ViewChild } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, FormsModule, ReactiveFormsModule, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { UserService } from '../../Services/UserService/user.service';
import { CommonModule } from '@angular/common';
import { User } from '../../Models/User';
import { LoadingSpinnerComponent } from "../Common/LoadingSpinner/loading-spinner.component";
import { AlertComponent } from "../Common/Alert/alert.component";
import { title } from 'process';
import { response } from 'express';

@Component({
  selector: 'app-account',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, LoadingSpinnerComponent, AlertComponent],
  templateUrl: './account.component.html',
  styleUrl: './account.component.css'
})
export class AccountComponent implements OnInit {
  @ViewChild('messageBox') messageBox! :AlertComponent;
  public userForm!:FormGroup;
  public isLoading:boolean = false;
  public userPasswordForm!:FormGroup;
  constructor(private userService:UserService){
    this.userForm = new FormGroup({
      userId: new FormControl(-1,[]),
      firstName: new FormControl('',[Validators.required]),
      lastName: new FormControl('',[Validators.required]),
      gender: new FormControl('',[Validators.required]),
      phoneNumber: new FormControl('',[Validators.required]),
      email: new FormControl('',[Validators.required])
    });
    this.userPasswordForm = new FormGroup({
      oldPassword: new FormControl('',[Validators.required,this.passwordValidator]),
      newPassword: new FormControl('',[Validators.required,this.passwordValidator]),
      confirmPassword: new FormControl('',[Validators.required])
    },{validators: this.passwordMatchValidator});

    this.messageBox = new AlertComponent();
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

  ngOnInit(): void {
    this.userService.getUserData().subscribe({
      next:(user:User|null)=>{
        this.userForm.get('userId')?.setValue(user?.id);
        this.userForm.get('firstName')?.setValue(user?.firstName);
        this.userForm.get('lastName')?.setValue(user?.lastName);
        this.userForm.get('email')?.setValue(user?.email);
        this.userForm.get('phoneNumber')?.setValue(user?.phoneNumber);
        this.userForm.get('gender')?.setValue(user?.gender);
      },
      error:(error)=>{
        console.log(error);
      }
    })
  }


  updateUser(){
    var user:User;
    user = this.userForm.value;
    console.log(user);
    this.isLoading=true;
    this.userService.updateUser(user).subscribe({
      next:(response:number)=>{
        console.log(response);
        this.isLoading=false;
        this.messageBox.buttons = [{text:'Okay',action:()=>{this.messageBox.hideAlert()},primary:true}];
        if(response>0)
          this.messageBox.showAlert({title:'User',message:'User Details Updated Successfully!'});
        else if(response<0){
          this.messageBox.showAlert({title:'User',message:'Something went wrong, please try again!'});
        }
      },
      error:(error)=>{
        console.log(error);
        this.isLoading=false;
        this.messageBox.buttons = [{text:'Okay',action:()=>{this.messageBox.hideAlert()},primary:true}];
        this.messageBox.showAlert({title:'User',message:'Error Ocurred, please try again!'});
      }
    });
  }

  updateUserPassword(){
    var userId = this.userForm.get('userId')?.value;
    var oldPassword = this.userPasswordForm.get('oldPassword')?.value;
    var newPassword = this.userPasswordForm.get('newPassword')?.value;
    console.log(userId);
    this.isLoading=true;
    this.userService.updateUserPassword(userId,oldPassword,newPassword).subscribe({
      next:(response:number)=>{
        this.isLoading=false;
        this.messageBox.buttons = [{text:'Okay',action:()=>{this.messageBox.hideAlert()},primary:true}];
        if(response>0){
          this.messageBox.showAlert({title:'User Password',message:'Password changed successfully!'})
        }else if(response==-10){
          this.messageBox.showAlert({title:'User Password',message:'Old password is incorrect!'});
        }else{
          this.messageBox.showAlert({title:'User Password',message:'Something went wrong, please try again!'})
        }
      },
      error:(error)=>{
        this.isLoading=false;
        this.messageBox.buttons = [{text:'Okay',action:()=>{this.messageBox.hideAlert()},primary:true}];
        this.messageBox.showAlert({title:'User Password',message:'Error occurred, please try again!'})
      }
    })
  }
}
