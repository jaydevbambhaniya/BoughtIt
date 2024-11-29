import { Component, ChangeDetectorRef } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductService } from '../../Services/ProductService/product.service';
import { CommonModule } from '@angular/common';
import { BrowserStorageService } from '../../Services/BrowserStorage/browserstorage.service';
import { select, Store } from '@ngrx/store';
import { Observable, firstValueFrom, of, forkJoin, BehaviorSubject, first } from 'rxjs';
import { CartService } from '../../Services/CartService/cart.service';
import { LoadingSpinnerComponent } from '../Common/LoadingSpinner/loading-spinner.component';
import { Product } from '../../Models/Product';
import { response } from 'express';
import { ProductCardComponent } from "../product-card/product-card.component";

@Component({
  selector: 'app-product',
  standalone: true,
  imports: [CommonModule, LoadingSpinnerComponent, ProductCardComponent],
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.css']
})
export class ProductComponent {
  public searchText: string = '';
  public productItems$ = new BehaviorSubject<(Product & { inWishlist: boolean; inCartlist: boolean })[]>([]);
  public isLoading:boolean=false;
  public wishlist: number[] = [];
  public fileBaseUrl = 'https://localhost:5000/static';
  public cartlist: number[] = [];
  public cartCount: number = 0;

  constructor(
    private route: ActivatedRoute,
    private productService: ProductService,
    private cartService:CartService,
    private router:Router
  ) {}

  ngOnInit() {
    this.route.queryParams.subscribe(params => {
      this.searchText = params['searchText'] || '';
      this.loadProducts();
    });
  }
  async loadProducts() {
    try {
      this.isLoading=true;
      this.productService.getFilteredProduct(this.searchText).subscribe({
        next:async (products:Product[])=>{
          if(products.length==0){
            this.productItems$.next([]);
            this.isLoading=false;
            return;
          }
          var productPromises = products.map(async (product)=>{
            if(!product)return null;
            const [inWishlist,inCartlist] = await Promise.all([
              firstValueFrom(this.cartService.isInWishlist(product.productId)),
              firstValueFrom(this.cartService.isInCartlist(product.productId))
            ]);
            return {
              ...product,
              inWishlist,
              inCartlist
            }
          });
          var resolvedProducts = await Promise.all(productPromises);
          this.productItems$.next(resolvedProducts.filter(p=>p!=null));
          this.isLoading=false;
        }
      });
    } catch (error) {
      console.error('Error loading products:', error);
    }
  }

  addToWishlist(productId: number, flag: boolean) {
    if (productId == null) return;
    this.cartService.addToWishlist(productId,flag);
    var products = this.productItems$.getValue();
    var updatedItems = products?.map(product=>
      product.productId==productId?{...product,inWishlist:flag}:product
    );
    this.productItems$.next(updatedItems);
  }
  productDetails(productId:number){
    if(productId==null)return;
    this.router.navigateByUrl('/product-info?productId='+productId);
  }
  addToCart(productId: number, flag: boolean) {
    if (productId == null) return;
    this.cartService.addToCart(productId,1,flag);
    var products = this.productItems$.getValue();
    var updatedItems = products?.map(product=>
      product.productId==productId?{...product,inCartlist:flag}:product
    );
    this.productItems$.next(updatedItems);
  }
}
