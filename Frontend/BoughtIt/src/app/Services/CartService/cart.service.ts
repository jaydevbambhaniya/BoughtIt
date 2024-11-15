import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, map, Observable, of } from 'rxjs';
import { CartProductDto } from '../../Models/CartProductDto';
import { Product } from '../../Models/Product';
import { select, Store } from '@ngrx/store';
import { selectIsProductInCart, selectIsProductInWishlist } from '../StoreManagement/global.selectors';
import { updateCart, updateWishlist } from '../StoreManagement/global.actions';
import { User } from '../../Models/User';
import { UserService } from '../UserService/user.service';

@Injectable({
  providedIn: 'root'
})
export class CartService {
  baseUrl:string ='https://localhost:5000/cart';
  public products:Product[] =[];
  public user!:User|null;
  constructor(private httpClient:HttpClient,private userService:UserService,
    private store:Store
  ) {
    this.userService.getUserData().subscribe({
      next:(response)=>{
        this.user = response;
      },
      error:(error)=>{
        return null;
      }
    })
   }
  getCartProducts(userId:number):Observable<CartProductDto[]>{
    if(userId==null){
      return of([]);
    }
    return this.httpClient.get(`${this.baseUrl}/getUserCart?UserId=${userId}`,{observe:'response'})
    .pipe(
      map((response:HttpResponse<any>)=>{
        
        return response.body.data || [] as CartProductDto[];
      }),
      catchError((error)=>{
        console.log(error);
        return of([]);
      })
    );
  }
  getUserWishlist(userId:number):Observable<number[]>{
    if(userId==null){
      return of([]);
    }
    return this.httpClient.get(`${this.baseUrl}/getUserWishlist?UserId=${userId}`,{observe:'response'})
    .pipe(
      map((response:HttpResponse<any>)=>{
        return response.body.data as number[];
      }),
      catchError((error)=>{
        console.log(error);
        return of([]);
      })
    );
  }
  updateUserCart(userId:number,item:CartProductDto,isInCart:boolean):Observable<boolean>{
    if(userId==null)return of(false);
    return this.httpClient.post(`${this.baseUrl}/updateUserCart`,{userId,cartProducts:item,isAdd:isInCart},
      {observe:'response'})
    .pipe(
      map((response:HttpResponse<any>)=>{
        return response.body.data as boolean;
      }),
      catchError((error)=>{
        console.log(error);
        return of(false);
      })
    );
  }
  updateUserWishlist(userId:number,item:number,isInCart:boolean):Observable<boolean>{
    if(userId==null)return of(false);
    return this.httpClient.post(`${this.baseUrl}/updateUserWishlist`,{userId,productId:item,isAdd:isInCart},
      {observe:'response'})
    .pipe(
      map((response:HttpResponse<any>)=>{
        return response.body.data as boolean;
      }),
      catchError((error)=>{
        console.log(error);
        return of(false);
      })
    );
  }
  setProductsToCheckout(products:Product[]){
    
    this.products = products;
  }
  getProductsToBuy():Product[]{
    return this.products;
  }
  isInWishlist(productId: number): Observable<boolean> {
    return this.store.pipe(select(selectIsProductInWishlist(productId)));
  }

  isInCartlist(productId: number): Observable<boolean> {
    return this.store.pipe(select(selectIsProductInCart(productId)));
  }
  addToCart(productId: number,quantity:number, flag: boolean) {
    if (productId == null) return;
    const item = { productId, quantity };
    this.store.dispatch(updateCart({ userId: this.user?.id ||-1, item, isInCart: flag }));
  }
  addToWishlist(productId: number, flag: boolean) {
    if (productId == null) return;
    this.store.dispatch(updateWishlist({ userId:this.user?.id ||-1, item:productId, isInWishlist: flag }));
  }
}
