import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { of,Observable } from 'rxjs';
import {map,catchError, switchMap} from 'rxjs/operators'
import { User } from '../../Models/User';
import { BrowserStorageService } from '../BrowserStorage/browserstorage.service';
import { Router } from '@angular/router';
import { ExternalUserInfo } from '../../Models/ExternalUserInfo';
@Injectable({
  providedIn: 'root'
})
export class UserService {
  private baseUrl='https://localhost:5000/user/';
  constructor(private httpClient:HttpClient,private browserStorage:BrowserStorageService,private router:Router) { }
  public getUserToken():string|null{
    return this.browserStorage.get('userToken');
  }
  public isAuthenticated():boolean{
    return !!this.browserStorage.get('userToken');
  }
  public logout():void{
    this.browserStorage.remove('userToken');
    
    this.router.navigate(['/login']);
  }
  public userLogin(username:string,password:string):Observable<number>{
    return this.httpClient.post(this.baseUrl + 'login', { username, password }, { observe: 'response' })
    .pipe(
      map((response: HttpResponse<any>) => {
        if(response.body.statusCode>0){
          this.browserStorage.set("userToken", response.body.data.tokens.accessToken);
          this.browserStorage.set("refreshToken", response.body.data.tokens.refreshToken);
          return response.body.data.userID; 
        }else{
          return response.body.data.statusCode;
        }
      }),
      switchMap((userID: number) => {
        return this.getUserData(userID,true).pipe(
          map(() => userID)
        );
      }),
      catchError((error) => {
        console.error(error);
        return of(-10);
      })
    );
  }
  public createUser(user:User):Observable<number>{
    return this.httpClient.post(this.baseUrl+'register',user,{observe:'response'})
    .pipe(
      map((response:HttpResponse<any>)=>{
        
        return response.body.statusCode;
      }),
      catchError((error)=>{
        
        return of(-10);
      })
    )
  }
  public getUserData(userId?:number,fromDb:boolean=false):Observable<User|null>{
    if(this.browserStorage.get("currentUser")!=null && !fromDb){
      return of(JSON.parse(this.browserStorage.get("currentUser")||'') as User);
    }
    return this.httpClient.get(this.baseUrl+`getUserDetails?UserId=${userId}`,{observe:'response'})
    .pipe(
      map((response:HttpResponse<any>)=>{
         var user = response.body.data as User;
         if(user != null){
          this.browserStorage.set("currentUser",JSON.stringify(user));
         }
         return user;
      }),
      catchError((error)=>{
        console.log(error);
        return of(null);
      })
    )
  }

  updateUser(user:User):Observable<number>{
    return this.httpClient.put(`${this.baseUrl}updateUserDetails`,user,{observe:'response'})
    .pipe(
      map((response:HttpResponse<any>)=>{
        if(response.body.data as number==1){
          this.getUserData(user.id,true).subscribe();
        }
        return response.body.data;
      }),
      catchError((error)=>{
        console.log(error);
        return of(-5);
      })
    )
  }

  updateUserPassword(userId:number,oldPassword:string,newPassword:string):Observable<number>{
    return this.httpClient.put(`${this.baseUrl}updateUserPassword`,{userId,oldPassword,password:newPassword},{observe:'response'})
    .pipe(
      map((response:HttpResponse<any>)=>{
        
        return response.body.data;
      }),
      catchError((error)=>{
        console.log(error);
        return of(-5);
      })
    )
  }
  refreshToken():Observable<any>{
    var accessToken = this.browserStorage.get('userToken');
    var refreshToken = this.browserStorage.get('refreshToken');
    return this.httpClient.post(`${this.baseUrl}refreshToken`,{accessToken,refreshToken},{observe:'response'})
    .pipe(
      map((response:HttpResponse<any>)=>{
        
        return response.body.data;
      }),
      catchError((error)=>{
        console.log(error);
        return of({accessToken:'-1'});
      })
    )
  }
  saveAccessToken(token:string){
    this.browserStorage.set('userToken',token);
  }
  externalLogin(externalUserInfo:ExternalUserInfo):Observable<number>{
    return this.httpClient.post(`${this.baseUrl}googleLogin`,externalUserInfo,{observe:'response'})
    .pipe(
      map((response:HttpResponse<any>)=>{
        this.browserStorage.set("userToken", response.body.data.tokens.accessToken);
        this.browserStorage.set("refreshToken", response.body.data.tokens.refreshToken);
        this.getUserData(response.body.data.userID,true).subscribe();
        return response.body.data.userID; 
      }),
      catchError((error)=>{
        console.log(error);
        return of(-10);
      })
    )
  }
}
