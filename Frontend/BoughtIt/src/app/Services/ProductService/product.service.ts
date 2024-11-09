import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Product } from '../../Models/Product';
import { catchError, map, Observable, of } from 'rxjs';
import { UserService } from '../UserService/user.service';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  private baseUrl='https://localhost:5000/inventory';
  constructor(private httpClient:HttpClient,private userService:UserService) { 
  }
  getProductById(productId: number): Observable<Product | null> {
    return this.httpClient.get<Product>(`${this.baseUrl}/getProduct?productID=${productId}`, {
      observe: 'response'
    }).pipe(
      map((response: HttpResponse<any>) => {
        if (response && response.body) {
          const product = response.body as Product; 
            return product;
        }
        return null;
      }),
      catchError((error) => {
        console.error('Error fetching product by ID:', error);
        return of(null);
      })
    );
  }
  
  getAllProducts():Observable<Product[]|null>{
    return this.httpClient.get(`${this.baseUrl}/getAllProducts`,{observe:'response'})
    .pipe(
      map((response:HttpResponse<any>)=>{
        console.log(response.body);
        var data = response.body as Product[];
        return data;
      }),
      catchError((error)=>{
        console.log(error);
        return of(null);
      })
    );
  }
  updateProduct(product:Product):Observable<boolean>{
    return of(false);
  }
  deleteProduct(productId:number):Observable<boolean>{
    return of(false);
  }
  getFilteredProduct(searchText:string):Observable<Product[]|null>{
    if(searchText=='')return this.getAllProducts();
    return this.httpClient.get(`${this.baseUrl}/getFilteredProducts?SearchTerm=${searchText}`,{observe:'response'})
    .pipe(
      map((response:HttpResponse<any>)=>{
        var data = response.body.data as Product[];
        return data;
      }),
      catchError((error)=>{
        console.log(error);
        return of(null);
      })
    );
  }
}
