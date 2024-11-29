import { Component, OnInit } from '@angular/core';
import { Init } from 'v8';
import { ProductService } from '../../Services/ProductService/product.service';
import { ActivatedRoute } from '@angular/router';
import { LoadingSpinnerComponent } from "../Common/LoadingSpinner/loading-spinner.component";
import { BehaviorSubject, firstValueFrom } from 'rxjs';
import { Product } from '../../Models/Product';
import { CommonModule } from '@angular/common';
import { CartService } from '../../Services/CartService/cart.service';

@Component({
  selector: 'app-product-info',
  standalone: true,
  imports: [LoadingSpinnerComponent,CommonModule],
  templateUrl: './product-info.component.html',
  styleUrl: './product-info.component.css'
})
export class ProductInfoComponent implements OnInit {
  public isLoading:boolean = false;
  public fileBaseUrl = 'https://localhost:5000/static';
  public product$=new BehaviorSubject<((Product & { inWishlist: boolean; inCartlist: boolean })|null)>(null);
  constructor(
    private productService:ProductService,
    private route:ActivatedRoute,
    private cartService: CartService
  ){}

  ngOnInit(): void {
    this.route.queryParams.subscribe({
      next:(params)=>{
        var productId = params["productId"];
        this.loadProductDetails(productId);
      }
    });
  }
  loadProductDetails(productId:number){
    if(productId==null)return;
    this.isLoading = true;
    this.productService.getProductById(productId).subscribe({
      next:async (product)=>{
        if(product==null){
          this.isLoading=false;
          return;
        }
        var [inCartlist,inWishlist] = await Promise.all([
          firstValueFrom(this.cartService.isInCartlist(product?.productId)),
          firstValueFrom(this.cartService.isInWishlist(product.productId))
        ]);

        this.product$.next({...product,inCartlist,inWishlist});
        this.isLoading=false;
      },
      error:(error)=>{
        console.error(error);
        this.isLoading=false;
      }
    });
  }

}
