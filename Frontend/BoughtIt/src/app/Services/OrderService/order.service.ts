import { Injectable } from '@angular/core';
import { Order, OrderDto } from '../../Models/Order';
import { HttpClient, HttpHandler, HttpHeaders, HttpResponse } from '@angular/common/http';
import { BrowserStorageService } from '../BrowserStorage/browserstorage.service';
import { catchError, map, Observable, of } from 'rxjs';
import { error } from 'console';

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  private baseUrl:string='https://localhost:5000/order';
  constructor(private httpClient:HttpClient,private browserStorage:BrowserStorageService) { }

  placeOrder(order:Order):Observable<any>{
    return this.httpClient.post(`${this.baseUrl}/placeOrder`,order,{observe:'response'}).pipe(
      map((response:HttpResponse<any>)=>{
        
        return response.body;
      }),
      catchError((error)=>{
        console.log(error);
        return of(false);
      })
    )
  }
  getUserOrders(userId:number,orderId:number):Observable<OrderDto[]>{
    if(orderId==0){
      return this.httpClient.get(`${this.baseUrl}/getUserOrders?UserID=${userId}`,{observe:'response'}).pipe(
        map((response:HttpResponse<any>)=>{
          
          return response.body.data;
        }),
        catchError((error)=>{
          console.log(error);
          return of([]);
        })
      );
    }else{
      return this.httpClient.get(`${this.baseUrl}/getOrder?OrderID=${orderId}`,{observe:'response'}).pipe(
        map((response:HttpResponse<any>)=>{
          
          if(response.body.data!=null){
            return [response.body.data];
          }
          return [];
        }),
        catchError((error)=>{
          console.log(error);
          return of([]);
        })
      )
    }
  }

  cancelOrder(orderID:number):Observable<number>{
    
    return this.httpClient.delete(`${this.baseUrl}/deleteOrder`,{body:{orderID},observe:'response'})
    .pipe(
      map((response:HttpResponse<any>)=>{
        
        return response.body.data || -5;
      }),
      catchError((error)=>{
        console.log(error);
        return of(-5);
      })
    )
  }
}
